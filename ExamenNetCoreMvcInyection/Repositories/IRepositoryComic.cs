using ExamenNetCoreMvcInyection.Models;

namespace ExamenNetCoreMvcInyection.Repositories
{
    public interface IRepositoryComic
    {
        List<Comic> GetComic();
        void CreateComic(string nombre, string imagen, string descripcion);
    }
}
