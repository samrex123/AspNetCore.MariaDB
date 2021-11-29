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
        [HttpGet]
        public async Task<string> GetComments()
        {

           
            //var lista = await _context.Comments.Where(x => x.postid == 1).Include(x => x.post).Where(x => x.post.discussionid == 1).Include(x => x.post.Discussion).ToListAsync();
            var lista = await _context.Comments.ToListAsync();
            var converted = JsonConvert.SerializeObject(lista);

            

            return converted;
        }

        // GET: api/Comments/5
        [HttpGet("{id}")]
        public async Task<string> GetComment(int? id)
        {

            var comment = await _context.Comments.Where(x => x.postid == id).ToListAsync();

            var converted = JsonConvert.SerializeObject(comment);

            if (comment == null)
            {
                string notfound = "notfound";
                return notfound;
            }

            return converted;
        }

        // PUT: api/Comments/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutComment([FromBody]Comment comment)
        {
            //Antar att vi får in rätt kommentar

            //if (id != comment.commentid)
            //{
            //    return BadRequest();
            //}

            _context.Entry(comment).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {

                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Comments
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<Comment>> PostComment([FromBody]Comment comment)
        {
            _context.Comments.Add(comment);
            await _context.SaveChangesAsync();

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
        [HttpDelete("{id}")]
        public async Task<ActionResult<Comment>> DeleteComment(int? id)
        {
            var comment = await _context.Comments.FindAsync(id);
            if (comment == null)
            {
                return NotFound();
            }

            _context.Comments.Remove(comment);
            await _context.SaveChangesAsync();

            return comment;
        }

        private bool CommentExists(int? id)
        {
            return _context.Comments.Any(e => e.commentid == id);
        }
    }
}
