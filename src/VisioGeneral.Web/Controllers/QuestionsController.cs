using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using VisioGeneral.Web.Data;
using VisioGeneral.Web.Models.Entities;
using VisioGeneral.Web.Models.ViewModels;

namespace VisioGeneral.Web.Controllers;

public class QüestionsController : Controller
{
    private readonly VisioGeneralDbContext _context;
    private readonly ILogger<QüestionsController> _logger;

    public QüestionsController(VisioGeneralDbContext context, ILogger<QüestionsController> logger)
    {
        _context = context;
        _logger = logger;
    }

    // GET: Qüestions
    public async Task<IActionResult> Index(
        string? cerca,
        int? areaId,
        int? estatId,
        string? prioritat,
        string? ordenar = "recent")
    {
        var query = _context.Questions
            .Include(q => q.Estat)
            .Include(q => q.Area)
            .Include(q => q.Programa)
            .Include(q => q.DirectorsResponsables)
                .ThenInclude(qd => qd.Director)
            .Where(q => !q.Eliminada && !q.Estat.EsFinal)  // Excloure eliminades i resoltes
            .AsQueryable();

        // Filtres - cerca sense distinció d'accents utilitzant EF.Functions
        if (!string.IsNullOrWhiteSpace(cerca))
        {
            query = query.Where(q =>
                EF.Functions.Collate(q.Titol, "SQL_Latin1_General_CP1_CI_AI").Contains(cerca) ||
                (q.Descripcio != null && EF.Functions.Collate(q.Descripcio, "SQL_Latin1_General_CP1_CI_AI").Contains(cerca)));
        }

        if (areaId.HasValue)
        {
            query = query.Where(q => q.AreaId == areaId);
        }

        if (estatId.HasValue)
        {
            query = query.Where(q => q.EstatId == estatId);
        }

        if (!string.IsNullOrWhiteSpace(prioritat))
        {
            query = query.Where(q => q.Prioritat == prioritat);
        }

        // Ordenació
        query = ordenar switch
        {
            "antic" => query.OrderBy(q => q.DataCreacio),
            "prioritat" => query.OrderByDescending(q => q.Prioritat == "Urgent")
                                .ThenByDescending(q => q.Prioritat == "Alta")
                                .ThenByDescending(q => q.DataCreacio),
            "area" => query.OrderBy(q => q.Area!.Nom).ThenByDescending(q => q.DataCreacio),
            _ => query.OrderByDescending(q => q.DataCreacio) // "recent" per defecte
        };

        var questions = await query
            .Select(q => new QuestioLlistatViewModel
            {
                Id = q.Id,
                Titol = q.Titol,
                Prioritat = q.Prioritat,
                EstatNom = q.Estat.Nom,
                EstatColor = q.Estat.Color,
                AreaNom = q.Area != null ? q.Area.Nom : null,
                AreaCodi = q.Area != null ? q.Area.Codi : null,
                ProgramaNom = q.Programa != null ? q.Programa.Nom : null,
                DirectorsNoms = q.DirectorsResponsables.Select(qd => qd.Director.Nom).ToList(),
                DataCreacio = q.DataCreacio,
                DataLimitResposta = q.DataLimitResposta,
                EsEquip = q.EsEquip
            })
            .ToListAsync();

        // Preparar dades per als filtres
        ViewBag.Areas = new SelectList(
            await _context.Areas.Where(a => a.Activa).OrderBy(a => a.Ordre).ToListAsync(),
            "Id", "Nom", areaId);

        ViewBag.Estats = new SelectList(
            await _context.EstatsQuestio.OrderBy(e => e.Ordre).ToListAsync(),
            "Id", "Nom", estatId);

        ViewBag.Prioritats = new SelectList(
            new[] { "Urgent", "Alta", "Normal", "Baixa" },
            prioritat);

        // Guardar filtres actuals per mostrar-los
        ViewBag.CercaActual = cerca;
        ViewBag.AreaIdActual = areaId;
        ViewBag.EstatIdActual = estatId;
        ViewBag.PrioritatActual = prioritat;
        ViewBag.OrdenarActual = ordenar;

        return View(questions);
    }

    // GET: Qüestions/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var questio = await _context.Questions
            .Include(q => q.Estat)
            .Include(q => q.Area)
            .Include(q => q.Programa)
                .ThenInclude(p => p!.Servei)
            .Include(q => q.DirectorsResponsables)
                .ThenInclude(qd => qd.Director)
            .Include(q => q.DirectorCreador)
            .Include(q => q.Historial.OrderByDescending(h => h.DataCanvi))
                .ThenInclude(h => h.EstatAnterior)
            .Include(q => q.Historial)
                .ThenInclude(h => h.EstatNou)
            .Include(q => q.Historial)
                .ThenInclude(h => h.Director)
            .Include(q => q.Comentaris.OrderByDescending(c => c.DataCreacio))
                .ThenInclude(c => c.Director)
            .Include(q => q.Organs)
                .ThenInclude(o => o.Organ)
            .Include(q => q.Subtasques.OrderBy(s => s.Ordre))
                .ThenInclude(s => s.DirectorAssignat)
            .Include(q => q.Subtasques)
                .ThenInclude(s => s.Comentaris.OrderByDescending(c => c.DataCreacio))
                    .ThenInclude(c => c.Director)
            .FirstOrDefaultAsync(q => q.Id == id && !q.Eliminada);

        if (questio == null)
        {
            return NotFound();
        }

        // Preparar llista d'estats per al canvi d'estat
        ViewBag.Estats = new SelectList(
            await _context.EstatsQuestio.OrderBy(e => e.Ordre).ToListAsync(),
            "Id", "Nom", questio.EstatId);

        // Preparar llista de directors
        ViewBag.Directors = await _context.Directors
            .Where(d => d.Actiu)
            .OrderBy(d => d.Nom)
            .ToListAsync();

        return View(questio);
    }

    // GET: Qüestions/Create
    public async Task<IActionResult> Create(int? programaId, int? areaId)
    {
        await CarregarDadesFormulari();

        var viewModel = new QuestioCreateViewModel
        {
            Prioritat = "Normal"
        };

        // Pre-seleccionar si vénen paràmetres
        if (programaId.HasValue)
        {
            viewModel.ProgramaId = programaId;
            var programa = await _context.Programes
                .Include(p => p.Servei)
                .FirstOrDefaultAsync(p => p.Id == programaId);
            if (programa != null)
            {
                viewModel.AreaId = programa.Servei.AreaId;
            }
        }
        else if (areaId.HasValue)
        {
            viewModel.AreaId = areaId;
        }

        return View(viewModel);
    }

    // POST: Qüestions/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(QuestioCreateViewModel viewModel)
    {
        if (ModelState.IsValid)
        {
            // Obtenir l'estat inicial (Pendent)
            var estatPendent = await _context.EstatsQuestio
                .FirstOrDefaultAsync(e => e.Nom == "Pendent");

            var questio = new Questio
            {
                Titol = viewModel.Titol,
                Descripcio = viewModel.Descripcio,
                Prioritat = viewModel.Prioritat,
                AreaId = viewModel.AreaId,
                ProgramaId = viewModel.ProgramaId,
                DirectorCreadorId = viewModel.DirectorCreadorId,
                EstatId = estatPendent?.Id ?? 1,
                DataLimitResposta = viewModel.DataLimitResposta,
                OrigenExtern = viewModel.OrigenExtern,
                FontExterna = viewModel.FontExterna,
                EsEquip = viewModel.EsEquip,
                DataCreacio = DateTime.Now
            };

            _context.Questions.Add(questio);
            await _context.SaveChangesAsync();

            // Afegir directors responsables
            if (viewModel.DirectorsResponsablesIds?.Any() == true)
            {
                foreach (var directorId in viewModel.DirectorsResponsablesIds)
                {
                    _context.QuestioDirectors.Add(new QuestioDirector
                    {
                        QuestioId = questio.Id,
                        DirectorId = directorId,
                        DataAssignacio = DateTime.Now
                    });
                }
                await _context.SaveChangesAsync();
            }

            TempData["Missatge"] = "Qüestió creada correctament.";
            return RedirectToAction(nameof(Details), new { id = questio.Id });
        }

        await CarregarDadesFormulari();
        return View(viewModel);
    }

    // GET: Qüestions/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var questio = await _context.Questions
            .Include(q => q.Programa)
                .ThenInclude(p => p!.Servei)
            .Include(q => q.DirectorsResponsables)
            .FirstOrDefaultAsync(q => q.Id == id && !q.Eliminada);

        if (questio == null)
        {
            return NotFound();
        }

        var viewModel = new QuestioEditViewModel
        {
            Id = questio.Id,
            Titol = questio.Titol,
            Descripcio = questio.Descripcio,
            Prioritat = questio.Prioritat,
            AreaId = questio.AreaId,
            ProgramaId = questio.ProgramaId,
            DirectorsResponsablesIds = questio.DirectorsResponsables.Select(qd => qd.DirectorId).ToList(),
            DataLimitResposta = questio.DataLimitResposta,
            OrigenExtern = questio.OrigenExtern,
            FontExterna = questio.FontExterna,
            EsEquip = questio.EsEquip
        };

        await CarregarDadesFormulari();
        return View(viewModel);
    }

    // POST: Qüestions/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, QuestioEditViewModel viewModel)
    {
        if (id != viewModel.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            var questio = await _context.Questions
                .Include(q => q.DirectorsResponsables)
                .FirstOrDefaultAsync(q => q.Id == id);
            if (questio == null)
            {
                return NotFound();
            }

            questio.Titol = viewModel.Titol;
            questio.Descripcio = viewModel.Descripcio;
            questio.Prioritat = viewModel.Prioritat;
            questio.AreaId = viewModel.AreaId;
            questio.ProgramaId = viewModel.ProgramaId;
            questio.DataLimitResposta = viewModel.DataLimitResposta;
            questio.OrigenExtern = viewModel.OrigenExtern;
            questio.FontExterna = viewModel.FontExterna;
            questio.EsEquip = viewModel.EsEquip;
            questio.DataModificacio = DateTime.Now;

            // Sincronitzar directors responsables
            var directorsActuals = questio.DirectorsResponsables.Select(qd => qd.DirectorId).ToList();
            var directorsNous = viewModel.DirectorsResponsablesIds ?? new List<int>();

            // Eliminar els que ja no estan
            var directorsAEliminar = directorsActuals.Except(directorsNous).ToList();
            foreach (var directorId in directorsAEliminar)
            {
                var qd = questio.DirectorsResponsables.First(x => x.DirectorId == directorId);
                _context.QuestioDirectors.Remove(qd);
            }

            // Afegir els nous
            var directorsAAfegir = directorsNous.Except(directorsActuals).ToList();
            foreach (var directorId in directorsAAfegir)
            {
                _context.QuestioDirectors.Add(new QuestioDirector
                {
                    QuestioId = questio.Id,
                    DirectorId = directorId,
                    DataAssignacio = DateTime.Now
                });
            }

            await _context.SaveChangesAsync();

            TempData["Missatge"] = "Qüestió actualitzada correctament.";
            return RedirectToAction(nameof(Details), new { id = questio.Id });
        }

        await CarregarDadesFormulari();
        return View(viewModel);
    }

    // POST: Qüestions/CanviarEstat
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CanviarEstat(int questioId, int nouEstatId, string? comentari, int? directorId)
    {
        var questio = await _context.Questions.FindAsync(questioId);
        if (questio == null)
        {
            return NotFound();
        }

        var estatAnteriorId = questio.EstatId;

        // Crear registre a l'historial
        var historial = new HistorialQuestio
        {
            QuestioId = questioId,
            EstatAnteriorId = estatAnteriorId,
            EstatNouId = nouEstatId,
            DirectorId = directorId,
            Comentari = comentari,
            DataCanvi = DateTime.Now
        };

        questio.EstatId = nouEstatId;
        questio.DataModificacio = DateTime.Now;

        // Si l'estat és "Resolta", guardar la data de resolució
        var nouEstat = await _context.EstatsQuestio.FindAsync(nouEstatId);
        if (nouEstat?.EsFinal == true)
        {
            questio.DataResolucio = DateTime.Now;
        }

        _context.HistorialQuestio.Add(historial);
        await _context.SaveChangesAsync();

        TempData["Missatge"] = "Estat actualitzat correctament.";
        return RedirectToAction(nameof(Details), new { id = questioId });
    }

    // POST: Qüestions/AfegirComentari
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> AfegirComentari(int questioId, string text, int? directorId)
    {
        if (string.IsNullOrWhiteSpace(text))
        {
            TempData["Error"] = "El comentari no pot estar buit.";
            return RedirectToAction(nameof(Details), new { id = questioId });
        }

        var comentari = new ComentariQuestio
        {
            QuestioId = questioId,
            Text = text,
            DirectorId = directorId,
            DataCreacio = DateTime.Now
        };

        _context.ComentarisQuestio.Add(comentari);
        await _context.SaveChangesAsync();

        TempData["Missatge"] = "Comentari afegit correctament.";
        return RedirectToAction(nameof(Details), new { id = questioId });
    }

    // GET: Qüestions/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var questio = await _context.Questions
            .Include(q => q.Estat)
            .Include(q => q.Area)
            .Include(q => q.Programa)
            .FirstOrDefaultAsync(q => q.Id == id && !q.Eliminada);

        if (questio == null)
        {
            return NotFound();
        }

        ViewBag.Directors = await _context.Directors
            .Where(d => d.Actiu)
            .OrderBy(d => d.Nom)
            .ToListAsync();

        return View(questio);
    }

    // POST: Qüestions/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id, string motiuEliminacio, int? directorId)
    {
        // Validació del motiu
        if (string.IsNullOrWhiteSpace(motiuEliminacio) || motiuEliminacio.Length < 10)
        {
            TempData["Error"] = "El motiu d'eliminació és obligatori i ha de tenir almenys 10 caràcters.";
            return RedirectToAction(nameof(Delete), new { id });
        }

        var questio = await _context.Questions.FindAsync(id);
        if (questio != null && !questio.Eliminada)
        {
            // Soft delete
            questio.Eliminada = true;
            questio.DataEliminacio = DateTime.Now;
            questio.MotiuEliminacio = motiuEliminacio;
            questio.DirectorEliminadorId = directorId;

            await _context.SaveChangesAsync();
            TempData["Missatge"] = "Qüestió eliminada correctament. Quedarà disponible a l'històric.";
        }

        return RedirectToAction(nameof(Index));
    }

    // GET: Qüestions/Historic
    public async Task<IActionResult> Historic(
        string? cerca,
        int? areaId,
        string? tipus,
        string? ordenar = "recent")
    {
        // Qüestions eliminades O resoltes (estat final)
        var query = _context.Questions
            .Include(q => q.Estat)
            .Include(q => q.Area)
            .Include(q => q.Programa)
            .Include(q => q.DirectorEliminador)
            .Where(q => q.Eliminada || q.Estat.EsFinal)
            .AsQueryable();

        // Filtres
        if (!string.IsNullOrWhiteSpace(cerca))
        {
            query = query.Where(q =>
                EF.Functions.Collate(q.Titol, "SQL_Latin1_General_CP1_CI_AI").Contains(cerca) ||
                (q.Descripcio != null && EF.Functions.Collate(q.Descripcio, "SQL_Latin1_General_CP1_CI_AI").Contains(cerca)));
        }

        if (areaId.HasValue)
        {
            query = query.Where(q => q.AreaId == areaId);
        }

        if (!string.IsNullOrWhiteSpace(tipus))
        {
            if (tipus == "eliminades")
            {
                query = query.Where(q => q.Eliminada);
            }
            else if (tipus == "resoltes")
            {
                query = query.Where(q => !q.Eliminada && q.Estat.EsFinal);
            }
        }

        // Ordenació
        query = ordenar switch
        {
            "antic" => query.OrderBy(q => q.Eliminada ? q.DataEliminacio : q.DataResolucio),
            "area" => query.OrderBy(q => q.Area!.Nom)
                          .ThenByDescending(q => q.Eliminada ? q.DataEliminacio : q.DataResolucio),
            _ => query.OrderByDescending(q => q.Eliminada ? q.DataEliminacio : q.DataResolucio) // "recent" per defecte
        };

        var questions = await query
            .Select(q => new QuestioHistoricViewModel
            {
                Id = q.Id,
                Titol = q.Titol,
                Prioritat = q.Prioritat,
                EstatNom = q.Estat.Nom,
                EstatColor = q.Estat.Color,
                AreaNom = q.Area != null ? q.Area.Nom : null,
                AreaCodi = q.Area != null ? q.Area.Codi : null,
                ProgramaNom = q.Programa != null ? q.Programa.Nom : null,
                DataCreacio = q.DataCreacio,
                Eliminada = q.Eliminada,
                DataEliminacio = q.DataEliminacio,
                MotiuEliminacio = q.MotiuEliminacio,
                DirectorEliminadorNom = q.DirectorEliminador != null ? q.DirectorEliminador.Nom : null,
                DataResolucio = q.DataResolucio
            })
            .ToListAsync();

        // Preparar dades per als filtres
        ViewBag.Areas = new SelectList(
            await _context.Areas.Where(a => a.Activa).OrderBy(a => a.Ordre).ToListAsync(),
            "Id", "Nom", areaId);

        ViewBag.CercaActual = cerca;
        ViewBag.AreaIdActual = areaId;
        ViewBag.TipusActual = tipus;
        ViewBag.OrdenarActual = ordenar;

        return View(questions);
    }

    // GET: Qüestions/DetailsHistoric/5
    public async Task<IActionResult> DetailsHistoric(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var questio = await _context.Questions
            .Include(q => q.Estat)
            .Include(q => q.Area)
            .Include(q => q.Programa)
                .ThenInclude(p => p!.Servei)
            .Include(q => q.DirectorsResponsables)
                .ThenInclude(qd => qd.Director)
            .Include(q => q.DirectorCreador)
            .Include(q => q.DirectorEliminador)
            .Include(q => q.Historial.OrderByDescending(h => h.DataCanvi))
                .ThenInclude(h => h.EstatAnterior)
            .Include(q => q.Historial)
                .ThenInclude(h => h.EstatNou)
            .Include(q => q.Historial)
                .ThenInclude(h => h.Director)
            .Include(q => q.Comentaris.OrderByDescending(c => c.DataCreacio))
                .ThenInclude(c => c.Director)
            .Include(q => q.Organs)
                .ThenInclude(o => o.Organ)
            .FirstOrDefaultAsync(q => q.Id == id && (q.Eliminada || q.Estat.EsFinal));

        if (questio == null)
        {
            return NotFound();
        }

        return View(questio);
    }

    // POST: Qüestions/AfegirSubtasca
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> AfegirSubtasca(int questioId, string titol, string? descripcio, int? directorAssignatId, DateOnly? dataLimit)
    {
        if (string.IsNullOrWhiteSpace(titol))
        {
            TempData["Error"] = "El títol de la subtasca és obligatori.";
            return RedirectToAction(nameof(Details), new { id = questioId });
        }

        var questio = await _context.Questions.FindAsync(questioId);
        if (questio == null || questio.Eliminada)
        {
            return NotFound();
        }

        // Obtenir l'ordre màxim actual
        var maxOrdre = await _context.Subtasques
            .Where(s => s.QuestioId == questioId)
            .MaxAsync(s => (int?)s.Ordre) ?? 0;

        var subtasca = new Subtasca
        {
            Titol = titol,
            Descripcio = descripcio,
            QuestioId = questioId,
            DirectorAssignatId = directorAssignatId,
            DataLimit = dataLimit,
            Ordre = maxOrdre + 1,
            DataCreacio = DateTime.Now
        };

        _context.Subtasques.Add(subtasca);
        await _context.SaveChangesAsync();

        TempData["Missatge"] = "Subtasca afegida correctament.";
        return RedirectToAction(nameof(Details), new { id = questioId });
    }

    // POST: Qüestions/CanviarEstatSubtasca
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CanviarEstatSubtasca(int subtascaId, EstatSubtasca nouEstat, string? comentari, int? directorId)
    {
        var subtasca = await _context.Subtasques
            .Include(s => s.Questio)
            .FirstOrDefaultAsync(s => s.Id == subtascaId);

        if (subtasca == null)
        {
            return NotFound();
        }

        subtasca.Estat = nouEstat;
        subtasca.DataCompletada = nouEstat == EstatSubtasca.Completada ? DateTime.Now : null;

        // Si hi ha comentari, afegir-lo
        if (!string.IsNullOrWhiteSpace(comentari))
        {
            var comentariSubtasca = new ComentariSubtasca
            {
                Text = comentari,
                SubtascaId = subtascaId,
                DirectorId = directorId,
                EstatEnComentari = nouEstat,
                DataCreacio = DateTime.Now
            };
            _context.ComentarisSubtasca.Add(comentariSubtasca);
        }

        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(Details), new { id = subtasca.QuestioId });
    }

    // POST: Qüestions/AfegirComentariSubtasca
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> AfegirComentariSubtasca(int subtascaId, string text, int? directorId)
    {
        if (string.IsNullOrWhiteSpace(text))
        {
            TempData["Error"] = "El comentari no pot estar buit.";
            var sub = await _context.Subtasques.FindAsync(subtascaId);
            return RedirectToAction(nameof(Details), new { id = sub?.QuestioId });
        }

        var subtasca = await _context.Subtasques.FindAsync(subtascaId);
        if (subtasca == null)
        {
            return NotFound();
        }

        var comentari = new ComentariSubtasca
        {
            Text = text,
            SubtascaId = subtascaId,
            DirectorId = directorId,
            DataCreacio = DateTime.Now
        };

        _context.ComentarisSubtasca.Add(comentari);
        await _context.SaveChangesAsync();

        TempData["Missatge"] = "Comentari afegit a la subtasca.";
        return RedirectToAction(nameof(Details), new { id = subtasca.QuestioId });
    }

    // POST: Qüestions/EliminarSubtasca
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> EliminarSubtasca(int subtascaId)
    {
        var subtasca = await _context.Subtasques.FindAsync(subtascaId);
        if (subtasca == null)
        {
            return NotFound();
        }

        var questioId = subtasca.QuestioId;
        _context.Subtasques.Remove(subtasca);
        await _context.SaveChangesAsync();

        TempData["Missatge"] = "Subtasca eliminada correctament.";
        return RedirectToAction(nameof(Details), new { id = questioId });
    }

    // Mètode auxiliar per carregar dades dels desplegables
    private async Task CarregarDadesFormulari()
    {
        ViewBag.Areas = new SelectList(
            await _context.Areas.Where(a => a.Activa).OrderBy(a => a.Ordre).ToListAsync(),
            "Id", "Nom");

        ViewBag.Programes = new SelectList(
            await _context.Programes
                .Where(p => p.Actiu)
                .Include(p => p.Servei)
                    .ThenInclude(s => s.Area)
                .OrderBy(p => p.Servei.Area.Ordre)
                .ThenBy(p => p.Nom)
                .Select(p => new { p.Id, Nom = $"{p.Servei.Area.Codi} - {p.Nom}" })
                .ToListAsync(),
            "Id", "Nom");

        // Directors com a llista per fer checkboxes
        ViewBag.DirectorsList = await _context.Directors
            .Where(d => d.Actiu)
            .OrderBy(d => d.Nom)
            .ToListAsync();

        // SelectList per al director creador
        ViewBag.Directors = new SelectList(
            await _context.Directors.Where(d => d.Actiu).OrderBy(d => d.Nom).ToListAsync(),
            "Id", "Nom");

        ViewBag.Prioritats = new SelectList(
            new[] { "Urgent", "Alta", "Normal", "Baixa" });

        ViewBag.Estats = new SelectList(
            await _context.EstatsQuestio.OrderBy(e => e.Ordre).ToListAsync(),
            "Id", "Nom");
    }
}
