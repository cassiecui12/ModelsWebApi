using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using ModelsWebAPI.Models;

namespace ModelsWebAPI.Controllers
{
    public interface IModelsController
    {
        Task<ActionResult<Model>> GetModel(int id);
        Task<ActionResult<IEnumerable<Model>>> GetModels();
        Task<ActionResult<Model>> PostModel(Model model);
        Task<IActionResult> PutModel(int id, Model model);
        Task<ActionResult<Model>> DeleteModel(int id);
    }
}