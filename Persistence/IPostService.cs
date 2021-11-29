using AspNetCore.MariaDB.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AspNetCore.MariaDB.Persistence
{
    public interface IPostService
    {
        Task<int> Delete(int id);
        Task<IEnumerable<Post>> FindAll();
        Task<Post> FindOne(int id);
        Task<int> Insert(Post forecast);
        Task<int> Update(Post forecast);

    }
}
