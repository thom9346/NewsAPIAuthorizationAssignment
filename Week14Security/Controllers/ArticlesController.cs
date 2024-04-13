using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Week14Security.Data;
using Week14Security.DTOs;
using Week14Security.Models;
using Week14Security.Models.Converters;
using Week14Security.Repositories;
using Week14Security.Services;

namespace Week14Security.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ArticlesController : ControllerBase
    {
        private readonly IRepository<Article> _articlesRepository;
        private readonly IConverter<Article, ArticleDTO> _converter;
        private readonly IUserService _userService;

        public ArticlesController(IConverter<Article, ArticleDTO> converter, IRepository<Article> repository, IUserService userService)
        {
            _articlesRepository = repository;
            _converter = converter;
            _userService = userService;
        }

        [HttpPost]
        [Authorize(Policy = "RequireJournalistRole")]
        public IActionResult CreateArticle([FromBody] ArticleDTO articleDto)
        {
            if(articleDto == null)
            {
                return BadRequest();
            }

            var article = _converter.Convert(articleDto);

            var newArticle = _articlesRepository.Add(article);

            return CreatedAtRoute("GetArticle", new { id = newArticle.ArticleId }, _converter.Convert(newArticle));
        }

        [HttpGet("{id}", Name = "GetArticle")]
        public IActionResult Get(int id)
        {
            var item = _articlesRepository.Get(id);
            if (item == null)
            {
                return NotFound();
            }
            var articleDto = _converter.Convert(item);
            return new ObjectResult(articleDto);
        }
        [HttpGet]
        public IEnumerable<ArticleDTO> GetAll() => _articlesRepository.GetAll().Select(_converter.Convert);


        [Authorize(Policy = "RequireEditorRole")]
        [HttpPut("{id}")]
        public IActionResult EditArticle(int id, [FromBody] ArticleDTO articleDto)
        {
            var article = _articlesRepository.Get(id);
            if (article == null)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            article.Title = articleDto.Title;
            article.Content = articleDto.Content;

            _articlesRepository.Edit(article);

            return NoContent(); 
        }

        [Authorize(Policy = "RequireJournalistRole")]
        [HttpPut("EditOwn/{id}")]
        public IActionResult EditOwnArticle(int id, [FromBody] ArticleDTO articleDto)
        {
            var article = _articlesRepository.Get(id);
            if (article == null)
            {
                return NotFound();
            }

            try
            {
                var userId = _userService.GetCurrentUserId();
                if (article.AuthorId != userId)
                {
                    return Forbid();
                }
            }
            catch (Exception)
            {
                return Forbid();
            }

            article.Title = articleDto.Title;
            article.Content = articleDto.Content;

            _articlesRepository.Edit(article);

            return NoContent(); 
        }

        [Authorize(Policy = "RequireEditorRole")]
        [HttpDelete("{id}")]
        public IActionResult DeleteArticle(int id)
        {
            var article = _articlesRepository.Get(id);
            if (article == null)
            {
                return NotFound();
            }

            _articlesRepository.Remove(id);

            return NoContent();
        }
    }
}
