using AspNetCore.MariaDB.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AspNetCore.MariaDB.Persistence
{
    public class PostService : IPostService
    {
        private readonly MariaDbContext _dbContext;

        public PostService(MariaDbContext mariaDbContext)
        {
            _dbContext = mariaDbContext;
        }

        public async Task<int> Delete(int id)
        {
            try
            {
                _dbContext.Posts.Remove(new Post
                {
                    postid = id
                }); 

                return await _dbContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                return 0;
            }
        }
        public async Task<Post> FindOne(int id)
        {
            return await _dbContext.Posts.FirstOrDefaultAsync(x => x.postid == id);
        }

        public async Task<IEnumerable<Post>> FindAll()
        {
            return await _dbContext.Posts.ToListAsync();   
        }

        public async Task<int> Insert(Post post)
        {
            _dbContext.Add(post);
            return await _dbContext.SaveChangesAsync();
        }

        public async Task<int> Update(Post post)
        {
            try
            {
                _dbContext.Update(post);
                return await _dbContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                return 0;
            }
        }

    }
}
