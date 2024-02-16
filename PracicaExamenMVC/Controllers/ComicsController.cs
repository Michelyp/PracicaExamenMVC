using Microsoft.AspNetCore.Mvc;
using PracicaExamenMVC.Models;
using PracicaExamenMVC.Repositories;
using System.Drawing;

namespace PracicaExamenMVC.Controllers
{
    public class ComicsController : Controller
    {
        IRepositoryComic rep;
        public ComicsController(IRepositoryComic rep) {
            this.rep = rep;
        }

        public IActionResult Index()
        {
            List<Comic> comics = this.rep.GetComics();
            return View(comics);
        }

        public IActionResult Create() {
            return View();
        }
        [HttpPost]
        public IActionResult Create(string nombre, string imagen, string descripcion)
        {
            this.rep.InsertComic(nombre, imagen, descripcion);
            return RedirectToAction("Index");
        }
        public IActionResult InsertComicLambda()
        {
            return View();
        }
        [HttpPost]
        public IActionResult InsertComicLambda(string nombre, string imagen, string descripcion)
        {
            this.rep.InsertComicLambda(nombre, imagen, descripcion);
            return RedirectToAction("Index");
        }


        public IActionResult BuscarComic() {
            ViewData["NOMBRE"] = this.rep.GetComicsSelect();
            return View();
        }

        [HttpPost]
        public IActionResult BuscarComic(string comic)
        {
            ViewData["NOMBRE"] = this.rep.GetComicsSelect();
            Comic com = this.rep.FindIdComicNombre(comic);
            return View(com);
        }
        public IActionResult Delete(int id)
        {
            Comic comic = this.rep.FindIdComic(id);
            return View(comic);
        }

        [HttpPost]
        public IActionResult Delete(Comic comic)
        {
           this.rep.DeleteComic(comic.IdComic);
            return RedirectToAction("Index");
        }
    }
}
