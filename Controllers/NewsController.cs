using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Practica3.Services;

namespace Practica3.Controllers
{
    
    public class NewsController : Controller
    {
        private readonly NewsService _newsService;

        public NewsController(NewsService newsService)
        {
            _newsService = newsService;
        }

        public async Task<IActionResult> Index()
        {
            var posts = await _newsService.GetPostsAsync();
            return View(posts.Take(10).ToList()); // para limitar cantidad
        }

        public async Task<IActionResult> Detalle(int id)
        {
            var post = (await _newsService.GetPostsAsync()).FirstOrDefault(p => p.Id == id);
            post.Author = await _newsService.GetUserAsync(post.UserId);
            post.Comments = await _newsService.GetCommentsAsync(post.Id);
            return View(post);
        }

        [HttpPost]
        public async Task<IActionResult> EnviarFeedback(int postId, string sentimiento)
        {
            var http = new HttpClient();
            var feedback = new
            {
                PostId = postId,
                Sentimiento = sentimiento,
                Fecha = DateTime.Now
            };

            var res = await http.PostAsJsonAsync("http://localhost:5000/api/feedback", feedback);
            TempData["mensaje"] = res.IsSuccessStatusCode ? "Gracias por tu feedback." : "Ya votaste.";
            return RedirectToAction("Detalle", new { id = postId });
        }
    }
}