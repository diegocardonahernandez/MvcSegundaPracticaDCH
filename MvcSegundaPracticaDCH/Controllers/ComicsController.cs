using Microsoft.AspNetCore.Mvc;
using MvcSegundaPracticaDCH.Models;
using MvcSegundaPracticaDCH.Repositories;

namespace MvcSegundaPracticaDCH.Controllers
{
    public class ComicsController : Controller
    {

        RepositoryComics repo;

        public ComicsController()
        {
            this.repo = new RepositoryComics();
        }

        public IActionResult Index()
        {
            List<Comic> comics = this.repo.getComics();
            return View(comics);
        }

        public IActionResult Details(int id)
        {
            Comic comic = this.repo.GetComicdata(id);
            return View(comic);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]

        public async Task <IActionResult> Create(string nombre, string imagen, string descripcion)
        {
            await this.repo.CrearComic(nombre, imagen, descripcion);
            return RedirectToAction("index");
        }

    }
}
