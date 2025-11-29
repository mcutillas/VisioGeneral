using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using VisioGeneral.Web.Data;
using VisioGeneral.Web.Models.Entities;

namespace VisioGeneral.Web.Controllers;

public class ProgramesController : Controller
{
    private readonly VisioGeneralDbContext _context;

    public ProgramesController(VisioGeneralDbContext context)
    {
        _context = context;
    }

    // GET: Programes
    public async Task<IActionResult> Index(int? areaId, int? serveiId)
    {
        var query = _context.Programes
            .Include(p => p.Servei)
                .ThenInclude(s => s.Area)
            .Include(p => p.Director)
            .AsQueryable();

        if (areaId.HasValue)
        {
            query = query.Where(p => p.Servei.AreaId == areaId.Value);
        }

        if (serveiId.HasValue)
        {
            query = query.Where(p => p.ServeiId == serveiId.Value);
        }

        var programes = await query
            .OrderBy(p => p.Servei.Area.Ordre)
            .ThenBy(p => p.Servei.Ordre)
            .ThenBy(p => p.Ordre)
            .ToListAsync();

        ViewBag.Areas = new SelectList(await _context.Areas.OrderBy(a => a.Ordre).ToListAsync(), "Id", "Nom", areaId);
        ViewBag.Serveis = await _context.Serveis
            .Include(s => s.Area)
            .OrderBy(s => s.Area.Ordre)
            .ThenBy(s => s.Ordre)
            .Select(s => new SelectListItem
            {
                Value = s.Id.ToString(),
                Text = $"{s.Area.Codi} - {s.Nom}",
                Selected = s.Id == serveiId
            })
            .ToListAsync();

        ViewBag.AreaIdActual = areaId;
        ViewBag.ServeiIdActual = serveiId;

        return View(programes);
    }

    // GET: Programes/Create
    public async Task<IActionResult> Create(int? serveiId)
    {
        await CarregarLlistesAsync(serveiId: serveiId);

        var maxOrdre = 0;
        if (serveiId.HasValue)
        {
            maxOrdre = await _context.Programes.Where(p => p.ServeiId == serveiId.Value).MaxAsync(p => (int?)p.Ordre) ?? 0;
        }
        ViewBag.SuggeritOrdre = maxOrdre + 1;

        return View(new Programa { ServeiId = serveiId ?? 0, Nom = "", Estat = "Actiu" });
    }

    // POST: Programes/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Programa programa)
    {
        if (ModelState.IsValid)
        {
            programa.DataCreacio = DateTime.Now;
            _context.Add(programa);
            await _context.SaveChangesAsync();
            TempData["Missatge"] = $"Programa '{programa.Nom}' creat correctament.";
            return RedirectToAction(nameof(Index), new { serveiId = programa.ServeiId });
        }

        await CarregarLlistesAsync(serveiId: programa.ServeiId, directorId: programa.DirectorId);
        return View(programa);
    }

    // GET: Programes/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var programa = await _context.Programes.FindAsync(id);
        if (programa == null)
        {
            return NotFound();
        }

        await CarregarLlistesAsync(serveiId: programa.ServeiId, directorId: programa.DirectorId);
        return View(programa);
    }

    // POST: Programes/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, Programa programa)
    {
        if (id != programa.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                programa.DataModificacio = DateTime.Now;
                _context.Update(programa);
                await _context.SaveChangesAsync();
                TempData["Missatge"] = $"Programa '{programa.Nom}' actualitzat correctament.";
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProgramaExists(programa.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return RedirectToAction(nameof(Index), new { serveiId = programa.ServeiId });
        }

        await CarregarLlistesAsync(serveiId: programa.ServeiId, directorId: programa.DirectorId);
        return View(programa);
    }

    // GET: Programes/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var programa = await _context.Programes
            .Include(p => p.Servei)
                .ThenInclude(s => s.Area)
            .Include(p => p.Director)
            .FirstOrDefaultAsync(m => m.Id == id);

        if (programa == null)
        {
            return NotFound();
        }

        // Comprovar dependències
        var numQuestions = await _context.Questions.CountAsync(q => q.ProgramaId == id);
        ViewBag.NumQuestions = numQuestions;
        ViewBag.TeDependencies = numQuestions > 0;

        return View(programa);
    }

    // POST: Programes/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var programa = await _context.Programes.FindAsync(id);
        if (programa != null)
        {
            var teDependencies = await _context.Questions.AnyAsync(q => q.ProgramaId == id);

            if (teDependencies)
            {
                TempData["Error"] = "No es pot eliminar el programa perquè té qüestions associades.";
                return RedirectToAction(nameof(Delete), new { id });
            }

            var serveiId = programa.ServeiId;
            _context.Programes.Remove(programa);
            await _context.SaveChangesAsync();
            TempData["Missatge"] = $"Programa '{programa.Nom}' eliminat correctament.";
            return RedirectToAction(nameof(Index), new { serveiId });
        }

        return RedirectToAction(nameof(Index));
    }

    // POST: Programes/ToggleActiu/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ToggleActiu(int id)
    {
        var programa = await _context.Programes.FindAsync(id);
        if (programa != null)
        {
            programa.Actiu = !programa.Actiu;
            programa.DataModificacio = DateTime.Now;
            await _context.SaveChangesAsync();
            var estat = programa.Actiu ? "activat" : "desactivat";
            TempData["Missatge"] = $"Programa '{programa.Nom}' {estat}.";
            return RedirectToAction(nameof(Index), new { serveiId = programa.ServeiId });
        }
        return RedirectToAction(nameof(Index));
    }

    private async Task CarregarLlistesAsync(int? serveiId = null, int? directorId = null)
    {
        ViewBag.Serveis = await _context.Serveis
            .Include(s => s.Area)
            .Where(s => s.Actiu)
            .OrderBy(s => s.Area.Ordre)
            .ThenBy(s => s.Ordre)
            .Select(s => new SelectListItem
            {
                Value = s.Id.ToString(),
                Text = $"{s.Area.Codi} - {s.Nom}",
                Selected = s.Id == serveiId
            })
            .ToListAsync();

        ViewBag.Directors = new SelectList(
            await _context.Directors.Where(d => d.Actiu).OrderBy(d => d.Nom).ToListAsync(),
            "Id", "NomComplet", directorId);

        ViewBag.Estats = new List<SelectListItem>
        {
            new SelectListItem { Value = "Actiu", Text = "Actiu" },
            new SelectListItem { Value = "Creixement", Text = "En creixement" },
            new SelectListItem { Value = "Parat", Text = "Parat" },
            new SelectListItem { Value = "Finalitzat", Text = "Finalitzat" }
        };
    }

    private bool ProgramaExists(int id)
    {
        return _context.Programes.Any(e => e.Id == id);
    }
}
