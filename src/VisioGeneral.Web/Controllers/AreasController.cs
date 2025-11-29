using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VisioGeneral.Web.Data;
using VisioGeneral.Web.Models.Entities;

namespace VisioGeneral.Web.Controllers;

public class AreasController : Controller
{
    private readonly VisioGeneralDbContext _context;

    public AreasController(VisioGeneralDbContext context)
    {
        _context = context;
    }

    // GET: Areas
    public async Task<IActionResult> Index()
    {
        var areas = await _context.Areas
            .Include(a => a.Serveis)
            .OrderBy(a => a.Ordre)
            .ToListAsync();
        return View(areas);
    }

    // GET: Areas/Create
    public IActionResult Create()
    {
        // Suggerir el següent ordre
        var maxOrdre = _context.Areas.Any() ? _context.Areas.Max(a => a.Ordre) : 0;
        ViewBag.SuggeritOrdre = maxOrdre + 1;
        return View();
    }

    // POST: Areas/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Area area)
    {
        if (ModelState.IsValid)
        {
            // Verificar codi únic
            if (await _context.Areas.AnyAsync(a => a.Codi == area.Codi))
            {
                ModelState.AddModelError("Codi", "Ja existeix una àrea amb aquest codi.");
                return View(area);
            }

            area.DataCreacio = DateTime.Now;
            _context.Add(area);
            await _context.SaveChangesAsync();
            TempData["Missatge"] = $"Àrea '{area.Nom}' creada correctament.";
            return RedirectToAction(nameof(Index));
        }
        return View(area);
    }

    // GET: Areas/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var area = await _context.Areas.FindAsync(id);
        if (area == null)
        {
            return NotFound();
        }
        return View(area);
    }

    // POST: Areas/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, Area area)
    {
        if (id != area.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            // Verificar codi únic (excepte ella mateixa)
            if (await _context.Areas.AnyAsync(a => a.Codi == area.Codi && a.Id != area.Id))
            {
                ModelState.AddModelError("Codi", "Ja existeix una altra àrea amb aquest codi.");
                return View(area);
            }

            try
            {
                area.DataModificacio = DateTime.Now;
                _context.Update(area);
                await _context.SaveChangesAsync();
                TempData["Missatge"] = $"Àrea '{area.Nom}' actualitzada correctament.";
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AreaExists(area.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return RedirectToAction(nameof(Index));
        }
        return View(area);
    }

    // GET: Areas/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var area = await _context.Areas
            .Include(a => a.Serveis)
            .FirstOrDefaultAsync(m => m.Id == id);
        if (area == null)
        {
            return NotFound();
        }

        // Comptar dependències
        var numServeis = area.Serveis.Count;
        var numQuestions = await _context.Questions.CountAsync(q => q.AreaId == id);
        ViewBag.NumServeis = numServeis;
        ViewBag.NumQuestions = numQuestions;
        ViewBag.TeDependencies = numServeis > 0 || numQuestions > 0;

        return View(area);
    }

    // POST: Areas/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var area = await _context.Areas.FindAsync(id);
        if (area != null)
        {
            // Comprovar dependències
            var teDependencies = await _context.Serveis.AnyAsync(s => s.AreaId == id)
                || await _context.Questions.AnyAsync(q => q.AreaId == id);

            if (teDependencies)
            {
                TempData["Error"] = "No es pot eliminar l'àrea perquè té serveis o qüestions associades.";
                return RedirectToAction(nameof(Delete), new { id });
            }

            _context.Areas.Remove(area);
            await _context.SaveChangesAsync();
            TempData["Missatge"] = $"Àrea '{area.Nom}' eliminada correctament.";
        }

        return RedirectToAction(nameof(Index));
    }

    // POST: Areas/ToggleActiva/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ToggleActiva(int id)
    {
        var area = await _context.Areas.FindAsync(id);
        if (area != null)
        {
            area.Activa = !area.Activa;
            area.DataModificacio = DateTime.Now;
            await _context.SaveChangesAsync();
            var estat = area.Activa ? "activada" : "desactivada";
            TempData["Missatge"] = $"Àrea '{area.Nom}' {estat}.";
        }
        return RedirectToAction(nameof(Index));
    }

    private bool AreaExists(int id)
    {
        return _context.Areas.Any(e => e.Id == id);
    }
}
