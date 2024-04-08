
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using Week14Security.Data;
using Week14Security.DTOs;
using Week14Security.Models;
using Week14Security.Models.Converters;
using Week14Security.Repositories;
using Week14Security.Services;

namespace Week14Security
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Your API", Version = "v1" });


                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "JWT Authorization header using the Bearer scheme (Example: 'Bearer 12345abcdef')",
                    Name = "Authorization", 
                    In = ParameterLocation.Header, 
                    Type = SecuritySchemeType.ApiKey, 
                    Scheme = "Bearer" 
                });

                //make sure the JWT token is sent with every call
                c.AddSecurityRequirement(new OpenApiSecurityRequirement()
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            },
                            Scheme = "oauth2",
                            Name = "Bearer",
                            In = ParameterLocation.Header,
                        },
                        new List<string>()
                    }
                });
            });


            builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlite("Data Source=newsSite.db"));
            builder.Services.AddTransient<IDbInitializer, DbInitializer>();

            builder.Services.AddControllers();

            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes("ThisIsASuperSecretKeyThatIsLongEnough")),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            });

            builder.Services.AddAuthorization(options =>
            {
                options.AddPolicy("RequireEditorRole", policy => policy.RequireRole("Editor"));
                options.AddPolicy("RequireJournalistRole", policy => policy.RequireRole("Journalist"));
                options.AddPolicy("RequireSubscriberRole", policy => policy.RequireRole("Subscriber"));
            });

            builder.Services.AddSingleton<IConverter<Article, ArticleDTO>, ArticleCoverter>();
            builder.Services.AddSingleton<IConverter<Comment, CommentDTO>, Commentconverter>();
            builder.Services.AddSingleton<IConverter<User, UserDTO>, UserConverter>();

            builder.Services.AddScoped<IRepository<Article>, ArticleRepository>();
            builder.Services.AddScoped<IRepository<Comment>, CommentRepository>();
            builder.Services.AddScoped<IUserRepository, UserRepository>();

            builder.Services.AddHttpContextAccessor();
            builder.Services.AddScoped<IUserService, UserService>();


            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthentication();
            app.UseAuthorization();


            app.MapControllers();

            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var dbContext = services.GetService<ApplicationDbContext>();
                var dbInitializer = services.GetService<IDbInitializer>();
                dbInitializer.Initialize(dbContext);
            }

            app.Run();
        }
    }
}
