using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ReflectionIT.Mvc.Paging;
using VendaLanches.Context;
using VendaLanches.Models;

namespace VendaLanches.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Roles = "Admin")]
public class AdminLanchesController : Controller
{
    private readonly AppDbContext _context;

    public AdminLanchesController(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index(string filter, int pageindex = 1, string sort = "Nome")
    {
        var resultado = _context.Lanches.Include(l => l.Categoria).AsQueryable();

        if (!string.IsNullOrWhiteSpace(filter))
            resultado = resultado.Where(p => p.Nome.Contains(filter));

        var model = await PagingList.CreateAsync(resultado, 5, pageindex, sort, "Nome");
        model.RouteValue = new RouteValueDictionary { { "filter", filter } };

        return View(model);
    }

    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
            return NotFound();

        var lanche = await _context.Lanches.Include(l => l.Categoria)
            .FirstOrDefaultAsync(m => m.LancheId == id);

        if (lanche == null)
            return NotFound();

        return View(lanche);
    }

    public IActionResult Create()
    {
        ViewData["CategoriaId"] = new SelectList(_context.Categorias, "CategoriaId", "CategoriaNome");
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("LancheId,Nome,DescricaoCurta,DescricaoDetalhada,Preco,ImagemUrl,ImagemThumbnailUrl,IsLanchePreferido,EmEstoque,CategoriaId")] Lanche lanche)
    {
        if (ModelState.IsValid)
        {
            _context.Add(lanche);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        ViewData["CategoriaId"] = new SelectList(_context.Categorias, "CategoriaId", "CategoriaNome", lanche.CategoriaId);
        return View(lanche);
    }

    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
            return NotFound();

        var lanche = await _context.Lanches.FindAsync(id);

        if (lanche == null)
            return NotFound();

        ViewData["CategoriaId"] = new SelectList(_context.Categorias, "CategoriaId", "CategoriaNome", lanche.CategoriaId);
        return View(lanche);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("LancheId,Nome,DescricaoCurta,DescricaoDetalhada,Preco,ImagemUrl,ImagemThumbnailUrl,IsLanchePreferido,EmEstoque,CategoriaId")] Lanche lanche)
    {
        if (id != lanche.LancheId)
            return NotFound();

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(lanche);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!LancheExists(lanche.LancheId))
                    return NotFound();
                else
                    throw;
            }
            return RedirectToAction(nameof(Index));
        }
        ViewData["CategoriaId"] = new SelectList(_context.Categorias, "CategoriaId", "CategoriaNome", lanche.CategoriaId);
        return View(lanche);
    }

    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
            return NotFound();

        var lanche = await _context.Lanches.Include(l => l.Categoria)
            .FirstOrDefaultAsync(m => m.LancheId == id);

        if (lanche == null)
            return NotFound();

        return View(lanche);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var lanche = await _context.Lanches.FindAsync(id);
        _context.Lanches.Remove(lanche);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool LancheExists(int id)
    {
        return _context.Lanches.Any(e => e.LancheId == id);
    }
}
