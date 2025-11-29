using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using VisioGeneral.Web.Data;
using VisioGeneral.Web.Models.Entities;

namespace VisioGeneral.Web.Controllers;

public class ServeisController : Controller
{
    private readonly VisioGeneralDbContext _context;

    public ServeisController(VisioGeneralDbContext context)
    {
        _context = context;
    }

    // GET: Serveis
    public async Task<IActionResult> Index(int? areaId)
    {
        var query = _context.Serveis
            .Include(s => s.Area)
            .Include(s => s.Programes)
            .AsQueryable();

        if (areaId.HasValue)
        {
            query = query.Where(s => s.AreaId == areaId.Value);
        }

        var serveis = await query
            .OrderBy(s => s.Area.Ordre)
            .ThenBy(s => s.Ordre)
            .ToListAsync();

        ViewBag.Areas = new SelectList(await _context.Areas.OrderBy(a => a.Ordre).ToListAsync(), "Id", "Nom", areaId);
        ViewBag.AreaIdActual = areaId;

        return View(serveis);
    }

    // GET: Serveis/Create
    public async Task<IActionResult> Create(int? areaId)
    {
        ViewBag.Areas = new SelectList(await _context.Areas.Where(a => a.Activa).OrderBy(a => a.Ordre).ToListAsync(), "Id", "Nom", areaId);

        // Suggerir ordre
        var maxOrdre = 0;
        if (areaId.HasValue)
        {
            maxOrdre = await _context.Serveis.Where(s => s.AreaId == areaId.Value).MaxAsync(s => (int?)s.Ordre) ?? 0;
        }
        ViewBag.SuggeritOrdre = maxOrdre + 1;

        return View(new Servei { AreaId = areaId ?? 0, Nom = "" });
    }

    // POST: Serveis/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Servei servei)
    {
        if (ModelState.IsValid)
        {
            servei.DataCreacio = DateTime.Now;
            _context.Add(servei);
            await _context.SaveChangesAsync();
            TempData["Missatge"] = $"Servei '{servei.Nom}' creat correctament.";
            return RedirectToAction(nameof(Index), new { areaId = servei.AreaId });
        }

        ViewBag.Areas = new SelectList(await _context.Areas.Where(a => a.Activa).OrderBy(a => a.Ordre).ToListAsync(), "Id", "Nom", servei.AreaId);
        return View(servei);
    }

    // GET: Serveis/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var servei = await _context.Serveis.FindAsync(id);
        if (servei == null)
        {
            return NotFound();
        }

        ViewBag.Areas = new SelectList(await _context.Areas.OrderBy(a => a.Ordre).ToListAsync(), "Id", "Nom", servei.AreaId);
        return View(servei);
    }

    // POST: Serveis/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, Servei servei)
    {
        if (id != servei.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                servei.DataModificacio = DateTime.Now;
                _context.Update(servei);
                await _context.SaveChangesAsync();
                TempData["Missatge"] = $"Servei '{servei.Nom}' actualitzat correctament.";
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ServeiExists(servei.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return RedirectToAction(nameof(Index), new { areaId = servei.AreaId });
        }

        ViewBag.Areas = new SelectList(await _context.Areas.OrderBy(a => a.Ordre).ToListAsync(), "Id", "Nom", servei.AreaId);
        return View(servei);
    }

    // GET: Serveis/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var servei = await _context.Serveis
            .Include(s => s.Area)
            .Include(s => s.Programes)
            .FirstOrDefaultAsync(m => m.Id == id);

        if (servei == null)
        {
            return NotFound();
        }

        ViewBag.NumProgrames = servei.Programes.Count;
        ViewBag.TeDependencies = servei.Programes.Count > 0;

        return View(servei);
    }

    // POST: Serveis/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var servei = await _context.Serveis.FindAsync(id);
        if (servei != null)
        {
            var teDependencies = await _context.Programes.AnyAsync(p => p.ServeiId == id);

            if (teDependencies)
            {
                TempData["Error"] = "No es pot eliminar el servei perquè té programes associats.";
                return RedirectToAction(nameof(Delete), new { id });
            }

            var areaId = servei.AreaId;
            _context.Serveis.Remove(servei);
            await _context.SaveChangesAsync();
            TempData["Missatge"] = $"Servei '{servei.Nom}' eliminat correctament.";
            return RedirectToAction(nameof(Index), new { areaId });
        }

        return RedirectToAction(nameof(Index));
    }

    // POST: Serveis/ToggleActiu/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ToggleActiu(int id)
    {
        var servei = await _context.Serveis.FindAsync(id);
        if (servei != null)
        {
            servei.Actiu = !servei.Actiu;
            servei.DataModificacio = DateTime.Now;
            await _context.SaveChangesAsync();
            var estat = servei.Actiu ? "activat" : "desactivat";
            TempData["Missatge"] = $"Servei '{servei.Nom}' {estat}.";
            return RedirectToAction(nameof(Index), new { areaId = servei.AreaId });
        }
        return RedirectToAction(nameof(Index));
    }

    private bool ServeiExists(int id)
    {
        return _context.Serveis.Any(e => e.Id == id);
    }
}
