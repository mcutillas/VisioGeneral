using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VisioGeneral.Web.Data;
using VisioGeneral.Web.Models.Entities;

namespace VisioGeneral.Web.Controllers;

public class DirectorsController : Controller
{
    private readonly VisioGeneralDbContext _context;

    public DirectorsController(VisioGeneralDbContext context)
    {
        _context = context;
    }

    // GET: Directors
    public async Task<IActionResult> Index()
    {
        var directors = await _context.Directors
            .OrderBy(d => d.Nom)
            .ThenBy(d => d.Cognoms)
            .ToListAsync();
        return View(directors);
    }

    // GET: Directors/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: Directors/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Director director)
    {
        if (ModelState.IsValid)
        {
            director.DataCreacio = DateTime.Now;
            _context.Add(director);
            await _context.SaveChangesAsync();
            TempData["Missatge"] = $"Director/a {director.NomComplet} creat correctament.";
            return RedirectToAction(nameof(Index));
        }
        return View(director);
    }

    // GET: Directors/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var director = await _context.Directors.FindAsync(id);
        if (director == null)
        {
            return NotFound();
        }
        return View(director);
    }

    // POST: Directors/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, Director director)
    {
        if (id != director.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                director.DataModificacio = DateTime.Now;
                _context.Update(director);
                await _context.SaveChangesAsync();
                TempData["Missatge"] = $"Director/a {director.NomComplet} actualitzat correctament.";
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DirectorExists(director.Id))
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
        return View(director);
    }

    // GET: Directors/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var director = await _context.Directors
            .FirstOrDefaultAsync(m => m.Id == id);
        if (director == null)
        {
            return NotFound();
        }

        // Comptar dependències
        var numProgrames = await _context.Programes.CountAsync(p => p.DirectorId == id);
        var numQuestionsAssignades = await _context.QuestioDirectors.CountAsync(qd => qd.DirectorId == id);
        ViewBag.NumProgrames = numProgrames;
        ViewBag.NumQuestionsAssignades = numQuestionsAssignades;
        ViewBag.TeDependencies = numProgrames > 0 || numQuestionsAssignades > 0;

        return View(director);
    }

    // POST: Directors/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var director = await _context.Directors.FindAsync(id);
        if (director != null)
        {
            // Comprovar dependències
            var teDependencies = await _context.Programes.AnyAsync(p => p.DirectorId == id)
                || await _context.QuestioDirectors.AnyAsync(qd => qd.DirectorId == id);

            if (teDependencies)
            {
                TempData["Error"] = "No es pot eliminar el director/a perquè té programes o qüestions assignades.";
                return RedirectToAction(nameof(Delete), new { id });
            }

            _context.Directors.Remove(director);
            await _context.SaveChangesAsync();
            TempData["Missatge"] = $"Director/a {director.NomComplet} eliminat correctament.";
        }

        return RedirectToAction(nameof(Index));
    }

    // POST: Directors/ToggleActiu/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ToggleActiu(int id)
    {
        var director = await _context.Directors.FindAsync(id);
        if (director != null)
        {
            director.Actiu = !director.Actiu;
            director.DataModificacio = DateTime.Now;
            await _context.SaveChangesAsync();
            var estat = director.Actiu ? "activat" : "desactivat";
            TempData["Missatge"] = $"Director/a {director.NomComplet} {estat}.";
        }
        return RedirectToAction(nameof(Index));
    }

    private bool DirectorExists(int id)
    {
        return _context.Directors.Any(e => e.Id == id);
    }
}
