namespace VisioGeneral.Web.Models.Entities;

/// <summary>
/// Vinculació de qüestions amb òrgans de govern (on s'ha parlat)
/// </summary>
public class QuestioOrgan
{
    public int Id { get; set; }
    public int QuestioId { get; set; }
    public int OrganId { get; set; }
    public DateOnly DataReunio { get; set; }
    public string? Resultat { get; set; }      // Resum del que es va decidir
    public string? PendentDe { get; set; }     // Si queda pendent d'alguna cosa

    // Navegació
    public Questio Questio { get; set; } = null!;
    public OrganGovern Organ { get; set; } = null!;
}
