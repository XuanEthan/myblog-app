using Dtos.Post;
using Humanizer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using MyBlogAdminService.Data;
using MyBlogAdminService.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace MyBlogAdminService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize]
    public class PostsController : ControllerBase
    {
        private readonly IHostEnvironment _hostEnvironment;
        private readonly MyBlogAdminDbContext _context;
        private readonly string UploadsFolder = "Uploads";

        public PostsController(MyBlogAdminDbContext context, IHostEnvironment hostEnvironment)
        {
            _context = context;
            this._hostEnvironment = hostEnvironment;
        }

        // GET: api/Posts
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Post>>> GetPosts([FromQuery] string? searchKey)
        {
            var query = _context.Posts
                                    .Include(p => p.Categories)
                                    .Include(p => p.Tags)
                                    .AsQueryable();

            if (!string.IsNullOrEmpty(searchKey))
                query = query.Where(p => (p.Title ?? "").Contains(searchKey));

            return await query.ToListAsync();
        }

        // GET: api/Posts/5
        [HttpGet("{id}")]
        public async Task<ActionResult<PostResponseDto>> GetPost(int id)
        {
            var post = await _context.Posts
                .Include(p => p.Categories)
                .Include(p => p.Tags)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (post == null)
                return NotFound();

            var result = new PostResponseDto
            {
                Id = post.Id,
                ImagePath = post.ImagePath,
                Title = post.Title,
                Content = post.Content,
                CategoryIds = post.Categories?.Select(c => c.Id).ToList(),
                TagIds = post.Tags?.Select(t => t.Id).ToList()
            };

            return Ok(result);
        }
        private async Task<string> SaveImageAsync(IFormFile ImageFile)
        {
            var fileName = ImageFile.FileName;
            var uploadsFolderPath = Path.Combine(_hostEnvironment.ContentRootPath, UploadsFolder);
            var imagePath = Path.Combine(uploadsFolderPath, fileName);

            using (var stream = new FileStream(imagePath, FileMode.Create))
            {
                await ImageFile.CopyToAsync(stream);
            }
            return $"Uploads/{fileName}";
        }

        private void DeleteImageFile(string? path)
        {
            if (System.IO.File.Exists($"{_hostEnvironment.ContentRootPath + path}"))
                System.IO.File.Delete($"{_hostEnvironment.ContentRootPath + path}");
        }

        // PUT: api/Posts/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPost([FromForm] PostUpdateDto dto)
        {
            if (dto == null)
                return BadRequest("Form data is invalid or missing.");

            var post = await _context.Posts
                .Include(p => p.Categories)
                .Include(p => p.Tags)
                .FirstOrDefaultAsync(p => p.Id == dto.Id);

            if (post == null)
                return NotFound($"Post with id {dto.Id} not found.");

            if (dto.ImageFile != null)
            {
                var imagePath = await SaveImageAsync(dto.ImageFile);
                post.ImagePath = imagePath;
            }

            var foundCategories = await getCategoriesByIds(dto.CategoryIds);
            var foundTags = await getTagsByIds(dto.TagIds);

            if (foundCategories.Count != (dto.CategoryIds?.Count ?? 0) ||
                foundTags.Count != (dto.TagIds?.Count ?? 0))
            {
                return BadRequest("Invalid categories or tags.");
            }

            post.Title = dto.Title;
            post.Content = dto.Content;

            post.Categories ??= new List<Category>();
            post.Tags ??= new List<Tag>();

            post.Categories.Clear();
            foreach (var c in foundCategories)
                post.Categories.Add(c);

            post.Tags.Clear();
            foreach (var t in foundTags)
                post.Tags.Add(t);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PostExists(dto.Id))
                    return NotFound("Post not found or deleted.");
                else
                    throw;
            }

            return NoContent();
        }


        // POST: api/Posts
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Post>> PostPost([FromForm] PostCreateDto dto)
        {
            if (dto == null)
            {
                return BadRequest("Form data is missing.");
            }

            var imagePath = "";
            if (dto.ImageFile != null)
            {
                imagePath = await SaveImageAsync(dto.ImageFile);
            }

            var foundCategories = await getCategoriesByIds(dto.CategoryIds);
            var foundTags = await getTagsByIds(dto.TagIds);

            if (!foundCategories.Count.Equals(dto.CategoryIds?.Count) || !foundTags.Count.Equals(dto.TagIds?.Count))
            {
                return BadRequest();
            }

            // Tạo Post mới
            var post = new Post
            {
                ImagePath = imagePath,
                Title = dto.Title,
                Content = dto.Content,
                Created = DateTime.Now,
                Categories = foundCategories,
                Tags = foundTags
            };

            _context.Posts.Add(post);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetPost), new { id = post.Id }, post);
        }



        // DELETE: api/Posts/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePost(int id)
        {
            var post = await _context.Posts.FindAsync(id);
            if (post == null)
            {
                return NotFound();
            }

            DeleteImageFile(post.ImagePath);

            _context.Posts.Remove(post);
            await _context.SaveChangesAsync();

            return NoContent();
        }
        private async Task<ICollection<Category>> getCategoriesByIds(ICollection<int>? CategoryIds)
        {
            if (CategoryIds == null || !CategoryIds.Any())
                return new List<Category>();

            return await _context.Categories
                .Where(c => CategoryIds.Contains(c.Id))
                .ToListAsync();
        }

        private async Task<ICollection<Tag>> getTagsByIds(ICollection<int>? TagIds)
        {
            if (TagIds == null || !TagIds.Any())
                return new List<Tag>();

            return await _context.Tags
                .Where(t => TagIds.Contains(t.Id))
                .ToListAsync();
        }

        private bool PostExists(int id)
        {
            return _context.Posts.Any(e => e.Id == id);
        }
    }
}
