using PracicaExamenMVC.Models;

namespace PracicaExamenMVC.Repositories
{
    public interface IRepositoryComic
    {
        List<Comic> GetComics();
        void InsertComic(string nombre, string imagen, string descripcion);
        void InsertComicLambda(string nombre, string imagen, string descripcion);
        Comic FindIdComic(int id);
        Comic FindIdComicNombre(string nombre);
        List<string> GetComicsSelect();
        void DeleteComic(int IdComic);


    }
}
