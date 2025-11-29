using VisioGeneral.Web.Models.Entities;

namespace VisioGeneral.Web.Models.ViewModels;

public class DashboardViewModel
{
    public int TotalTreballadors { get; set; }
    public int TotalArees { get; set; }
    public int TotalProgrames { get; set; }
    public int QüestionsUrgents { get; set; }
    public int LiniesCreixement { get; set; }

    public List<AreaTreemapViewModel> Areas { get; set; } = new();
    public List<QuestioRecentViewModel> QüestionsRecents { get; set; } = new();
}

public class AreaTreemapViewModel
{
    public int Id { get; set; }
    public string Nom { get; set; } = string.Empty;
    public string Codi { get; set; } = string.Empty;
    public string Color { get; set; } = string.Empty;
    public int TotalTreballadors { get; set; }
    public int NumQüestionsUrgents { get; set; }
    public List<ProgramaTreemapViewModel> Programes { get; set; } = new();
}

public class ProgramaTreemapViewModel
{
    public int Id { get; set; }
    public string Nom { get; set; } = string.Empty;
    public string NomCurt { get; set; } = string.Empty;
    public int NumTreballadors { get; set; }
    public string Estat { get; set; } = string.Empty;
    public bool EsLiniaCreixement { get; set; }
    public bool EsNou { get; set; }
    public bool EsParat { get; set; }
    public int NumQüestionsUrgents { get; set; }
    public string? NomDirector { get; set; }
    public string AreaCodi { get; set; } = string.Empty;
    public string AreaColor { get; set; } = string.Empty;
}

public class QuestioRecentViewModel
{
    public int Id { get; set; }
    public string Titol { get; set; } = string.Empty;
    public string Prioritat { get; set; } = string.Empty;
    public string EstatNom { get; set; } = string.Empty;
    public string EstatColor { get; set; } = string.Empty;
    public string? AreaNom { get; set; }
    public string? ProgramaNom { get; set; }
    public DateTime DataCreacio { get; set; }
}
