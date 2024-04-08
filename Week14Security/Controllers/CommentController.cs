using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Week14Security.DTOs;
using Week14Security.Models.Converters;
using Week14Security.Models;
using Week14Security.Repositories;
using Week14Security.Services;
using Microsoft.AspNetCore.Authorization;

namespace Week14Security.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommentController : ControllerBase
    {
        private readonly IRepository<Comment> _commentRepository;
        private readonly IConverter<Comment, CommentDTO> _converter;

        public CommentController(IConverter<Comment, CommentDTO> converter, IRepository<Comment> repository)
        {
            _commentRepository = repository;
            _converter = converter;
        }
        [HttpPost]
        [Authorize(Policy = "RequireSubscriberRole")]
        public IActionResult CreateComment([FromBody] CommentDTO commenDTO)
        {
            if (commenDTO == null)
            {
                return BadRequest();
            }

            var comment = _converter.Convert(commenDTO);
            var newComment = _commentRepository.Add(comment);

            return CreatedAtRoute("GetComment", new { id = newComment.ArticleId }, _converter.Convert(newComment));
        }
        [HttpGet("{id}", Name = "GetComment")]
        public IActionResult Get(int id)
        {
            var item = _commentRepository.Get(id);
            if (item == null)
            {
                return NotFound();
            }
            var commentDto = _converter.Convert(item);
            return new ObjectResult(commentDto);
        }
        [HttpGet]
        public IEnumerable<CommentDTO> GetAll()
        {
            var commentDtoList = new List<CommentDTO>();

            foreach (var article in _commentRepository.GetAll())
            {
                var commentDto = _converter.Convert(article);
                commentDtoList.Add(commentDto);
            }
            return commentDtoList;
        }
    }
}
