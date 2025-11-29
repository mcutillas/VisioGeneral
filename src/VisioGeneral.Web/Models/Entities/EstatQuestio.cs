namespace VisioGeneral.Web.Models.Entities;

/// <summary>
/// Estats possibles de les qüestions
/// </summary>
public class EstatQuestio
{
    public int Id { get; set; }
    public required string Nom { get; set; }
    public string? Descripcio { get; set; }
    public required string Color { get; set; }  // Per badges
    public int Ordre { get; set; }
    public bool EsFinal { get; set; }           // Si és un estat final (Resolta)

    // Navegació
    public ICollection<Questio> Qüestions { get; set; } = new List<Questio>();
}
