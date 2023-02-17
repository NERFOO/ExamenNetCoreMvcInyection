using ExamenNetCoreMvcInyection.Models;
using ExamenNetCoreMvcInyection.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace ExamenNetCoreMvcInyection.Controllers
{
    public class ComicController : Controller
    {
        IRepositoryComic irepo;

        public ComicController(IRepositoryComic repo)
        {
            this.irepo = repo;
        }
        public IActionResult Index()
        {
            List<Comic> comics = this.irepo.GetComic();
            return View(comics);
        }


        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(string nombre, string imagen, string descripcion)
        {
            this.irepo.CreateComic(nombre, imagen, descripcion);
            return RedirectToAction("Index");
        }
    }
}
