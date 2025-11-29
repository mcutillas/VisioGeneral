namespace VisioGeneral.Web.Models.Entities;

/// <summary>
/// Context anual de l'entitat (per l'informe)
/// </summary>
public class ContextAnual
{
    public int Id { get; set; }
    public int Any { get; set; }

    // Dades globals
    public int? TotalTreballadors { get; set; }
    public int? TotalUsuaris { get; set; }
    public int? TotalProgrames { get; set; }

    // Reflexions de direcció
    public string? ReflexioGeneral { get; set; }
    public string? ReptesPrincipals { get; set; }
    public string? ExitsPrincipals { get; set; }

    public DateTime DataCreacio { get; set; } = DateTime.Now;
    public DateTime? DataModificacio { get; set; }
}
