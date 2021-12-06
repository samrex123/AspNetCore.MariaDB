using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AspNetCore.MariaDB.Models;
using AspNetCore.MariaDB.Persistence;
using Newtonsoft.Json;

namespace AspNetCore.MariaDB.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommentsController : ControllerBase
    {
        private readonly MariaDbContext _context;

        public CommentsController(MariaDbContext context)
        {
            _context = context;
        }

        // GET: api/Comments
        /// <summary>
        /// Hämtar kommentarer från DB. Konverterar dom till JSON och skickar tillbaka requesten.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetComments()
        {
            var lista = await _context.Comments.ToListAsync();
            var converted = JsonConvert.SerializeObject(lista);

            return Ok(converted);
        }

        // GET: api/Comments/5
        /// <summary>
        /// Hämtar information på ett specifikt ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetComment(int? id)
        {

            var comment = await _context.Comments.Where(x => x.postid == id).ToListAsync();
            if (comment == null)
            {
                return NotFound();
            }

            var converted = JsonConvert.SerializeObject(comment);

            return Ok(converted);
        }

        // PUT: api/Comments
        /// <summary>
        /// Ändrar en kommentar
        /// </summary>
        /// <param name="id"></param>
        /// <param name="comment"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> PutComment(int id, [FromBody]Comment comment)
        {
            if ( id != comment.commentid)
            {
                return BadRequest();
            }
            if (!CommentExists(id))
            {
                return NotFound();
            }

            //hittar gamla texten för att skicka med 
            //och hitta den unika kommentaren i databasen hos de andra användare
            var oldcommentText = await _context.Comments.Where(x => x.commentid == comment.commentid)
                                                    .Select(x => x.comment_text)
                                                    .FirstOrDefaultAsync();
            //Skickar ut mailen
            try
            {
                foreach (var user in _context.Users)
                {
                    comment.EditComment(user.email, oldcommentText);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return Accepted(comment);
        }

        // POST: api/Comments
        /// <summary>
        /// Lägger upp en kommentar och skickar ut mail
        /// </summary>
        /// <param name="comment"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> PostComment([FromBody] Comment comment)
        {
            comment.date = DateTime.Now;

            //För att matcha ID i alla Databaser så sätts den manuellt.
            if (_context.Comments.Any())
            {
                var HighestID = await _context.Comments.Select(x => x.commentid).MaxAsync();
                comment.commentid = HighestID + 1;
            }
            else
            {
                comment.commentid = 1;
            }
            //skickar ut mail
            try
            {
                foreach (var user in _context.Users)
                {
                    comment.SendComments(user.email);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return CreatedAtAction("GetComment", new { id = comment.commentid }, comment);
        }

        // DELETE: api/Comments/5
        /// <summary>
        /// Tar bort kommentar med ett specifikt ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteComment(int? id)
        {
            var comment = await _context.Comments.FindAsync(id);

            if (comment == null)
            {
                return NotFound();
            }
            //skickar ut mail
            try
            {
                foreach (var user in _context.Users)
                {
                    comment.DeleteComments(user.email);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return Accepted(comment);
        }
        private bool CommentExists(int? id)
        {
            return _context.Comments.Any(e => e.commentid == id);
        }
    }
}
