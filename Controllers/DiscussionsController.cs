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
        /// <summary>
        /// Hämtar Discussionen från DB. Konverterar dom till JSON och skickar tillbaka requesten.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetDiscussion()
        {
            var listaDiscussion = await _context.Discussion.ToListAsync();
            var converted = JsonConvert.SerializeObject(listaDiscussion);

            return Ok(converted);
        }

        // GET: api/Discussions/5
        /// <summary>
        /// Hämtar information på ett specifikt ID. 
        /// görs om till en DTO och skickar tillbaka med extra information
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetDiscussion(int id)
        {
            var discussion = await _context.Discussion.FindAsync(id);

            if (discussion == null)
            {
                return NotFound();
            }
            var postlist = await _context.Posts.Where(x => x.discussionid == id).ToListAsync();

            // Sätter extra fältet till null för att skapa en DTO
            foreach (var item in postlist)
            {
                item.Discussion = null;
            }

            var dto = new DiscussionDTO(discussion, postlist);

            return Ok(dto);
        }

        // PUT: api/Discussions/5
        /// <summary>
        /// Ändrar en discussion
        /// </summary>
        /// <param name="id"></param>
        /// <param name="discussion"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> PutDiscussion(int id, Discussion discussion)
        {
            if (id != discussion.discussionid)
            {
                return BadRequest();
            }
            if (!DiscussionExists(id))
            {
                return NotFound();
            }

            //hittar gamla texten för att skicka med 
            //och hitta den unika kommentaren i databasen hos de andra användare
            var oldtext = await _context.Discussion.Where(x => x.discussionid == discussion.discussionid)
                                                    .Select(x => x.discussiontext)
                                                    .FirstOrDefaultAsync();
            //skickar ut mail
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
        /// <summary>
        /// Lägger upp en discussion och skickar ut mail
        /// </summary>
        /// <param name="discussion"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> PostDiscussion([FromBody] Discussion discussion)
        {
            discussion.createddate = DateTime.Now;

            //Sätter ID manuellt för att matcha i DB hos alla användare.
            if (_context.Discussion.Any())
            {
                var HighestID =  await _context.Discussion.Select(x => x.discussionid).MaxAsync();
                discussion.discussionid = HighestID + 1;
            }
            else
            {
                discussion.discussionid = 1;
            }

            //skickar ut mail
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
        /// <summary>
        /// Tar bort discussionen med ett specifikt ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDiscussion(int id)
        {
            var discussion = await _context.Discussion.FindAsync(id);
            if (discussion == null)
            {
                return NotFound();
            }
            //skickar ut mail
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
        private bool DiscussionExists(int? id)
        {
            return _context.Discussion.Any(e => e.discussionid == id);
        }
    }
}
