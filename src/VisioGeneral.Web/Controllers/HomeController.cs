using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VisioGeneral.Web.Data;
using VisioGeneral.Web.Models;
using VisioGeneral.Web.Models.ViewModels;

namespace VisioGeneral.Web.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly VisioGeneralDbContext _context;

    public HomeController(ILogger<HomeController> logger, VisioGeneralDbContext context)
    {
        _logger = logger;
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        var currentYear = DateTime.Now.Year;

        // Obtenir totes les àrees amb els seus programes
        var areas = await _context.Areas
            .Where(a => a.Activa)
            .OrderBy(a => a.Ordre)
            .Include(a => a.Serveis)
                .ThenInclude(s => s.Programes.Where(p => p.Actiu))
                    .ThenInclude(p => p.Director)
            .Include(a => a.Qüestions.Where(q => q.Prioritat == "Urgent" && !q.Estat.EsFinal))
            .ToListAsync();

        // Obtenir qüestions urgents per programa
        var questionsUrgentsPerPrograma = await _context.Questions
            .Where(q => q.Prioritat == "Urgent" && !q.Estat.EsFinal && q.ProgramaId != null)
            .GroupBy(q => q.ProgramaId)
            .Select(g => new { ProgramaId = g.Key, Count = g.Count() })
            .ToDictionaryAsync(x => x.ProgramaId!.Value, x => x.Count);

        // Construir el ViewModel
        var viewModel = new DashboardViewModel
        {
            TotalArees = areas.Count,
            Areas = areas.Select(a => new AreaTreemapViewModel
            {
                Id = a.Id,
                Nom = a.Nom,
                Codi = a.Codi,
                Color = a.Color,
                TotalTreballadors = a.Serveis.SelectMany(s => s.Programes).Sum(p => p.NumTreballadors),
                NumQüestionsUrgents = a.Qüestions.Count,
                Programes = a.Serveis.SelectMany(s => s.Programes)
                    .OrderByDescending(p => p.NumTreballadors)
                    .Select(p => new ProgramaTreemapViewModel
                    {
                        Id = p.Id,
                        Nom = p.Nom,
                        NomCurt = p.NomCurt ?? p.Nom,
                        NumTreballadors = p.NumTreballadors,
                        Estat = p.Estat,
                        EsLiniaCreixement = p.EsLiniaCreixement,
                        EsNou = p.EsNou,
                        EsParat = p.Estat == "Parat",
                        NumQüestionsUrgents = questionsUrgentsPerPrograma.GetValueOrDefault(p.Id, 0),
                        NomDirector = p.Director?.Nom,
                        AreaCodi = a.Codi,
                        AreaColor = a.Color
                    }).ToList()
            }).ToList()
        };

        // Calcular totals
        viewModel.TotalTreballadors = viewModel.Areas.Sum(a => a.TotalTreballadors);
        viewModel.TotalProgrames = viewModel.Areas.Sum(a => a.Programes.Count);
        viewModel.QüestionsUrgents = await _context.Questions
            .CountAsync(q => q.Prioritat == "Urgent" && !q.Estat.EsFinal);
        viewModel.LiniesCreixement = viewModel.Areas
            .SelectMany(a => a.Programes)
            .Count(p => p.EsLiniaCreixement);

        // Qüestions recents
        viewModel.QüestionsRecents = await _context.Questions
            .Include(q => q.Estat)
            .Include(q => q.Area)
            .Include(q => q.Programa)
            .OrderByDescending(q => q.DataCreacio)
            .Take(5)
            .Select(q => new QuestioRecentViewModel
            {
                Id = q.Id,
                Titol = q.Titol,
                Prioritat = q.Prioritat,
                EstatNom = q.Estat.Nom,
                EstatColor = q.Estat.Color,
                AreaNom = q.Area != null ? q.Area.Nom : null,
                ProgramaNom = q.Programa != null ? q.Programa.Nom : null,
                DataCreacio = q.DataCreacio
            })
            .ToListAsync();

        return View(viewModel);
    }

    [HttpGet]
    public async Task<IActionResult> GetProgramaDetails(int id)
    {
        var programa = await _context.Programes
            .Include(p => p.Director)
            .Include(p => p.Servei)
                .ThenInclude(s => s.Area)
            .FirstOrDefaultAsync(p => p.Id == id);

        if (programa == null)
        {
            return NotFound();
        }

        return Json(new
        {
            id = programa.Id,
            nom = programa.Nom,
            numTreballadors = programa.NumTreballadors,
            numUsuaris = programa.NumUsuaris,
            director = programa.Director?.Nom,
            areaNom = programa.Servei.Area.Nom,
            serveiNom = programa.Servei.Nom,
            estat = programa.Estat,
            esLiniaCreixement = programa.EsLiniaCreixement,
            esNou = programa.EsNou
        });
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
