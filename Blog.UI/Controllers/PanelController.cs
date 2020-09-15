using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Blog.UI.Controllers.Repository.Interface;
using Blog.UI.Data.FileManager.Interface;
using Blog.UI.Models;
using Blog.UI.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Blog.UI.Controllers
{
    [Authorize(Roles="Admin")]
    public class PanelController : Controller
    {
        private readonly IRepository _repository;
        private readonly IFileManager _fileManager;

        public PanelController(IRepository repository,
                               IFileManager fileManager)
        {
            _repository = repository;
            _fileManager = fileManager;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var posts = _repository.GetAllPosts();
            return View(posts);
        }

        [HttpGet]
        public IActionResult Edit(int? id)
        {
            if (id == null)
                return View(new PostViewModel());
            else
            {
                var post = _repository.GetPost((int)id);
                return View(new PostViewModel 
                {
                    Id = post.Id,
                    Title = post.Title,
                    Body = post.Body,
                    CurrentImage = post.Image
                });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Edit(PostViewModel vm)
        {
            var post = new Post
            {
                Id = vm.Id,
                Title = vm.Title,
                Body = vm.Body,
                
            };

            if (vm.Image == null)
                post.Image = vm.CurrentImage;
            else
                post.Image = await _fileManager.SaveImage(vm.Image);

            if (post.Id > 0)
                _repository.UpdatePost(post);
            else
                _repository.AddPost(post);

            if (await _repository.SaveChangesAsync())
                return RedirectToAction("Index");
            else
                return View(post);
        }

        [HttpGet]
        public async Task<IActionResult> Remove(int id)
        {
            _repository.RemovePost(id);
            await _repository.SaveChangesAsync();
            return RedirectToAction("Index");
        }
    }
}
