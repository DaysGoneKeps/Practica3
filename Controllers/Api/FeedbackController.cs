using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Practica3.Data;
using Practica3.Models;

namespace Practica3.Controllers.Api
{
    [ApiController]
    [Route("api/[controller]")]
    public class FeedbackController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public FeedbackController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Get() =>
            Ok(await _context.Feedbacks.ToListAsync());

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Feedback feedback)
        {
            var existe = await _context.Feedbacks.AnyAsync(f => f.PostId == feedback.PostId);
            if (existe)
                return BadRequest("Ya diste feedback para este post.");

            feedback.Fecha = DateTime.Now;
            _context.Feedbacks.Add(feedback);
            await _context.SaveChangesAsync();

            return Ok(feedback);
        }
        
    }
}