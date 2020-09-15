using Blog.UI.Controllers.Repository.Interface;
using Blog.UI.Data.FileManager.Interface;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Blog.UI.Controllers
{
    public class HomeController : Controller
    {
        private readonly IRepository _repository;
        private readonly IFileManager _fileManager;

        public HomeController(IRepository repository, IFileManager fileManager)
        {
            _repository = repository;
            _fileManager = fileManager;
        }

        public IActionResult Index(string categoty)
        {
            var posts = string.IsNullOrEmpty(categoty) ? _repository.GetAllPosts() : _repository.GetAllPosts(categoty);
            return View(posts);
        }

        public IActionResult Post(int id)
        {
            var post = _repository.GetPost(id);
            return View(post);
        }

        [HttpGet("/Image/{image}")]
        public IActionResult Image(string image)
        {
            var mine = image.Substring(image.LastIndexOf('.') + 1);
            return new FileStreamResult(_fileManager.ImageStream(image), $"image/{mine}");
        }
    }
}
