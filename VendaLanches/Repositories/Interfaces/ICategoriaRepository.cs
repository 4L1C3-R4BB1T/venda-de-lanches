using VendaLanches.Models;

namespace VendaLanches.Repositories.Interfaces;

public interface ICategoriaRepository
{
    IEnumerable<Categoria> Categorias { get; }
}
