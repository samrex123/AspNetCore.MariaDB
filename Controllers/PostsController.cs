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
        //private readonly List<Users> users;

        public PostsController(MariaDbContext context)
        {
            _context = context;
        }

        // GET: api/Posts
        [HttpGet]
        public async Task<string> GetPosts()
        {
            var result = await _context.Posts.ToArrayAsync();
            var converted = JsonConvert.SerializeObject(result);
            
            
            //return JsonObject object = "hej";
            return converted;
        }
        //[HttpGet]
        //public async Task<ActionResult<IEnumerable<Post>>> GetPosts()
        //{

        //    return JsonObject object = "hej";
        //    //return await _context.Posts.ToArrayAsync();
        //}

        // GET: api/Posts/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Post>> GetPost(int? id)
        {
            var post = await _context.Posts.FindAsync(id);

            if (post == null)
            {
                return NotFound();
            }

            return post;
        }

        // PUT: api/Posts/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPost(int? id, Post post)
        {
            if (id != post.postid)
            {
                return BadRequest();
            }

            
            _context.Entry(post).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
                try
                {
                    foreach (var user in _context.Users)
                    {
                        //post.DateTime = DateTime.Now;
                        post.EditPost(user.email);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PostExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Accepted(post);
        }

        // POST: api/Posts
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<Post>> PostPost([FromBody]Post post)
        {
            DateTime datenow = DateTime.Now;
            post.DateTime = datenow;
            

            _context.Posts.Add(post);
            await _context.SaveChangesAsync();
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
        [HttpDelete("{id}")]
        public async Task<ActionResult<Post>> DeletePost(int? id)
        {
            var post = await _context.Posts.FindAsync(id);
            if (post == null)
            {
                return NotFound();
            }

            _context.Posts.Remove(post);
            await _context.SaveChangesAsync();

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

        //public static void SendEmail(Users toUser, string text)
        //{
        //    //Rad r = new Rad(Tabell, meddelande, toEmail, (int)DateTimeOffset.Now.ToUnixTimeSeconds());

        //    string mailAddress = "mintestmail321@gmail.com";
        //    string passwordMail = "1Kalaskula!";
        //    MimeMessage message = new MimeMessage();
        //    message.From.Add(new MailboxAddress("Sam", mailAddress));
        //    message.To.Add(MailboxAddress.Parse(toUser.email));
        //    message.Subject = "Update";
        //    message.Body = new TextPart("plain")
        //    {
        //        Text = text
        //    };
        //    SmtpClient client = new SmtpClient();
        //    try
        //    {
        //        client.CheckCertificateRevocation = false;
        //        client.Connect("smtp.gmail.com", 465, true);
        //        client.Authenticate(mailAddress, passwordMail);
        //        client.Send(message);
        //        //Console.WriteLine("Email Sent!");
        //        //dh.SendSqlQuery(r.ToSQL());
        //    }
        //    catch (Exception e)
        //    {
        //        Console.WriteLine(e.Message);
        //    }
        //    finally
        //    {
        //        client.Disconnect(true);
        //        client.Dispose();
        //    }
        //}

        
    }
}
