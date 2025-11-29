using System.ComponentModel.DataAnnotations;

namespace VisioGeneral.Web.Models.ViewModels;

public class QuestioLlistatViewModel
{
    public int Id { get; set; }
    public string Titol { get; set; } = null!;
    public string Prioritat { get; set; } = null!;
    public string EstatNom { get; set; } = null!;
    public string? EstatColor { get; set; }
    public string? AreaNom { get; set; }
    public string? AreaCodi { get; set; }
    public string? ProgramaNom { get; set; }
    public List<string> DirectorsNoms { get; set; } = new();
    public DateTime DataCreacio { get; set; }
    public DateOnly? DataLimitResposta { get; set; }
    public bool EsEquip { get; set; }

    // Helper per mostrar directors separats per coma
    public string DirectorsText => DirectorsNoms.Any() ? string.Join(", ", DirectorsNoms) : null!;

    public bool EsUrgent => Prioritat == "Urgent";
    public bool TeDataLimitProxima => DataLimitResposta.HasValue &&
        DataLimitResposta.Value <= DateOnly.FromDateTime(DateTime.Now.AddDays(7)) &&
        DataLimitResposta.Value > DateOnly.FromDateTime(DateTime.Now);
    public bool TeDataLimitPassada => DataLimitResposta.HasValue &&
        DataLimitResposta.Value < DateOnly.FromDateTime(DateTime.Now);
}

public class QuestioCreateViewModel
{
    [Required(ErrorMessage = "El títol és obligatori")]
    [StringLength(300, ErrorMessage = "El títol no pot superar els 300 caràcters")]
    [Display(Name = "Títol")]
    public string Titol { get; set; } = null!;

    [Display(Name = "Descripció")]
    public string? Descripcio { get; set; }

    [Required(ErrorMessage = "La prioritat és obligatòria")]
    [Display(Name = "Prioritat")]
    public string Prioritat { get; set; } = "Normal";

    [Display(Name = "Àrea")]
    public int? AreaId { get; set; }

    [Display(Name = "Programa")]
    public int? ProgramaId { get; set; }

    [Display(Name = "Directors responsables")]
    public List<int> DirectorsResponsablesIds { get; set; } = new();

    [Display(Name = "Director/a creador")]
    public int? DirectorCreadorId { get; set; }

    [Display(Name = "Data límit de resposta")]
    [DataType(DataType.Date)]
    public DateOnly? DataLimitResposta { get; set; }

    [Display(Name = "Origen extern?")]
    public bool OrigenExtern { get; set; }

    [Display(Name = "Font externa")]
    [StringLength(200)]
    public string? FontExterna { get; set; }

    [Display(Name = "Qüestió d'equip")]
    public bool EsEquip { get; set; }
}

public class QuestioEditViewModel
{
    public int Id { get; set; }

    [Required(ErrorMessage = "El títol és obligatori")]
    [StringLength(300, ErrorMessage = "El títol no pot superar els 300 caràcters")]
    [Display(Name = "Títol")]
    public string Titol { get; set; } = null!;

    [Display(Name = "Descripció")]
    public string? Descripcio { get; set; }

    [Required(ErrorMessage = "La prioritat és obligatòria")]
    [Display(Name = "Prioritat")]
    public string Prioritat { get; set; } = null!;

    [Display(Name = "Àrea")]
    public int? AreaId { get; set; }

    [Display(Name = "Programa")]
    public int? ProgramaId { get; set; }

    [Display(Name = "Directors responsables")]
    public List<int> DirectorsResponsablesIds { get; set; } = new();

    [Display(Name = "Data límit de resposta")]
    [DataType(DataType.Date)]
    public DateOnly? DataLimitResposta { get; set; }

    [Display(Name = "Origen extern?")]
    public bool OrigenExtern { get; set; }

    [Display(Name = "Font externa")]
    [StringLength(200)]
    public string? FontExterna { get; set; }

    [Display(Name = "Qüestió d'equip")]
    public bool EsEquip { get; set; }
}

public class QuestioHistoricViewModel
{
    public int Id { get; set; }
    public string Titol { get; set; } = null!;
    public string Prioritat { get; set; } = null!;
    public string EstatNom { get; set; } = null!;
    public string? EstatColor { get; set; }
    public string? AreaNom { get; set; }
    public string? AreaCodi { get; set; }
    public string? ProgramaNom { get; set; }
    public DateTime DataCreacio { get; set; }

    // Dades de tancament/eliminació
    public bool Eliminada { get; set; }
    public DateTime? DataEliminacio { get; set; }
    public string? MotiuEliminacio { get; set; }
    public string? DirectorEliminadorNom { get; set; }
    public DateTime? DataResolucio { get; set; }

    public string TipusTancament => Eliminada ? "Eliminada" : "Resolta";
    public DateTime? DataTancament => Eliminada ? DataEliminacio : DataResolucio;
}
