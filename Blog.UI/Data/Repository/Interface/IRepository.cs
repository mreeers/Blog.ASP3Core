using Blog.UI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Blog.UI.Controllers.Repository.Interface
{
    public interface IRepository
    {
        Post GetPost(int id);
        List<Post> GetAllPosts();
        List<Post> GetAllPosts(string categoty);
        void AddPost(Post post);
        void RemovePost(int id);
        void UpdatePost(Post post);
        Task<bool> SaveChangesAsync();
    }
}
