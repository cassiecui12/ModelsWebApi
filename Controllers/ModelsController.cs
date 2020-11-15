using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using ModelsWebAPI.Data;
using ModelsWebAPI.Hubs;
using ModelsWebAPI.Models;

namespace ModelsWebAPI.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ModelsController : ControllerBase, IModelsController
    {
        private readonly IModelDbContext _context;
        private readonly IHubContext<MessageHub> _messageHubContext;

        public ModelsController(IModelDbContext context, IHubContext<MessageHub> messageHubContext)
        {
            _context = context;
            _messageHubContext = messageHubContext;
        }

        // GET: api/Models
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Model>>> GetModels()
        {
            return await _context.Models.ToListAsync();
        }

        // GET: api/Models/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Model>> GetModel(int id)
        {
            Model model = await _context.Models.FindAsync(id);
            if (model == null) return NotFound();
            return model;
        }

        // PUT: api/Models/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutModel(int id, Model model)
        {
            if (id != model.Id) return BadRequest();
            if (!ModelState.IsValid) return BadRequest(ModelState);

            _context.MarkAsModified(model);

            try { await _context.SaveChangesAsync(); }
            catch (DbUpdateConcurrencyException)
            {
                if (!ModelExists(id)) return NotFound();
                else throw;
            }
            await _messageHubContext.Clients.All.SendAsync("send", "update");
            return NoContent();
        }

        // POST: api/Models
        [HttpPost]
        public async Task<ActionResult<Model>> PostModel(Model model)
        {
            _context.Models.Add(model);
            await _context.SaveChangesAsync();
            await _messageHubContext.Clients.All.SendAsync("send", "update");
            return CreatedAtAction("GetModel", new { id = model.Id }, model);
        }

        // DELETE: api/Models/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Model>> DeleteModel(int id)
        {
            var model = await _context.Models.FindAsync(id);
            if (model == null) return NotFound();

            _context.Models.Remove(model);
            await _context.SaveChangesAsync();
            await _messageHubContext.Clients.All.SendAsync("send", "update");
            return model;
        }

        private bool ModelExists(int id)
        {
            return _context.Models.Any(e => e.Id == id);
        }
    }
}