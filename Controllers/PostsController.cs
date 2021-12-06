using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AspNetCore.MariaDB.Models;
using AspNetCore.MariaDB.Persistence;
using Newtonsoft.Json;
using MailKit.Net.Smtp;
using MimeKit;

namespace AspNetCore.MariaDB.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostsController : ControllerBase
    {
        private readonly MariaDbContext _context;

        public PostsController(MariaDbContext context)
        {
            _context = context;
        }

        // GET: api/Posts
        /// <summary>
        /// Hämtar Posten från DB. Konverterar dom till JSON och skickar tillbaka requesten.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetPosts()
        {
            var result = await _context.Posts.ToArrayAsync();
            var converted = JsonConvert.SerializeObject(result);
            
            return Ok(converted);
        }


        // GET: api/Posts/5
        /// <summary>
        /// Hämtar post med ett specifikt ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetPost(int? id)
        {
            var post = await _context.Posts.FindAsync(id);

            if (post == null)
            {
                return NotFound();
            }

            return Ok(post);
        }

        // PUT: api/Posts/5
        /// <summary>
        /// Ändrar en kommentar med ett specifikt ID
        /// </summary>
        /// <param name="id"></param>
        /// <param name="post"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPost(int? id, Post post)
        {
            if (id != post.postid)
            {
                return BadRequest();
            }

            if (!PostExists(id))
            {
                return NotFound();
            }
            //hittar gamla texten för att skicka med 
            //och hitta den unika kommentaren i databasen hos de andra användare
            var oldtext = await _context.Posts.Where(x => x.postid == post.postid).Select(x => x.Text).FirstOrDefaultAsync();

            //skickar ut mail
            try
            {
                foreach (var user in _context.Users)
                {
                    post.EditPost(user.email, oldtext);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return Accepted(post);
        }

        // POST: api/Posts
        /// <summary>
        /// Lägger upp en post och skickar ut mail
        /// </summary>
        /// <param name="post"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> PostPost([FromBody]Post post)
        {
            post.DateTime = DateTime.Now;

            //Sätter ID manuellt för att matcha i DB hos alla användare.
            if (_context.Posts.Any())
            {
                var HighestID = await _context.Posts.Select(x => x.postid).MaxAsync();
                post.postid = HighestID + 1;
            }
            else
            {
                post.postid = 1;
            }
            //skickar ut mail
            try
            {
                foreach (var user in _context.Users)
                {
                    post.SendPost(user.email);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
       

            return CreatedAtAction("GetPost", new { id = post.postid }, post);
        }

        // DELETE: api/Posts/5
        /// <summary>
        /// Deletar en post med specifikt ID och skickar ut mail
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePost(int? id)
        {
            var post = await _context.Posts.FindAsync(id);
            if (post == null)
            {
                return NotFound();
            }
            //skickar ut mail
            try
            {
                foreach (var user in _context.Users)
                {
                    post.DeletePost(user.email);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return Accepted (post);
        }

        private bool PostExists(int? id)
        {
            return _context.Posts.Any(e => e.postid == id);
        } 
    }
}
