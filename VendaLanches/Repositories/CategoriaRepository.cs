using VendaLanches.Context;
using VendaLanches.Models;
using VendaLanches.Repositories.Interfaces;

namespace VendaLanches.Repositories;

public class CategoriaRepository : ICategoriaRepository
{
    private readonly AppDbContext _context;

    public CategoriaRepository(AppDbContext context)
    {
        _context = context;
    }

    public IEnumerable<Categoria> Categorias => _context.Categorias;
}
