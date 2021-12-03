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
using System.Net;
using AspNetCore.MariaDB.Models.Dto;

namespace AspNetCore.MariaDB.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DiscussionsController : ControllerBase
    {
        private readonly MariaDbContext _context;

        public DiscussionsController(MariaDbContext context)
        {
            _context = context;
        }

        // GET: api/Discussions
        [HttpGet]
        public async Task<string> GetDiscussion()

        {

            var lsita = await _context.Discussion.ToListAsync();

            var converted = JsonConvert.SerializeObject(lsita);

            return converted;
        }

        // GET: api/Discussions/5
        [HttpGet("{id}")]
        public async Task<ActionResult<DiscussionDTO>> GetDiscussion(int id)
        {
            var discussion = await _context.Discussion.FindAsync(id);

            if (discussion == null)
            {
                return NotFound();
            }

            var postlist = await _context.Posts.Where(x => x.discussionid == id).ToListAsync();
            foreach (var item in postlist)
            {
                item.Discussion = null;
            }

            var dto = new DiscussionDTO(discussion, postlist);

            //var listDiscussionPosts = new List<object>();
            //listDiscussionPosts.Add(discussion);

            return dto;
        }

        // PUT: api/Discussions/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutDiscussion(int id, Discussion discussion)
        {
            if (id != discussion.discussionid)
            {
                return BadRequest();
            }

            var oldtext = await _context.Discussion.Where(x => x.discussionid == discussion.discussionid).Select(x => x.discussiontext).FirstOrDefaultAsync();


            try
            {
                foreach (var user in _context.Users)
                {
                    discussion.EditDiscussion(user.email, oldtext);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }




            return Accepted(discussion);
        }

        // POST: api/Discussions
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<Discussion>> PostDiscussion([FromBody] Discussion discussion)
        {
            discussion.createddate = DateTime.Now;
            if (_context.Discussion.Any())
            {
                var HighestID = _context.Discussion.Select(x => x.discussionid).Max();
                discussion.discussionid = HighestID + 1;
            }
            else
            {
                discussion.discussionid = 1;
            }


            try
            {
                foreach (var user in _context.Users)
                {
                    discussion.SendDiscussion(user.email);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return CreatedAtAction("GetDiscussion", new { id = discussion.discussionid }, discussion);
        }

        // DELETE: api/Discussions/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Discussion>> DeleteDiscussion(int id)
        {
            var discussion = await _context.Discussion.FindAsync(id);
            if (discussion == null)
            {
                return NotFound();
            }

            //_context.Discussion.Remove(discussion);
            //await _context.SaveChangesAsync();

            try
            {
                foreach (var user in _context.Users)
                {
                    discussion.DeleteDiscussion(user.email);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return Accepted(discussion);
        }

        private bool DiscussionExists(int id)
        {
            return _context.Discussion.Any(e => e.discussionid == id);
        }
    }
}
