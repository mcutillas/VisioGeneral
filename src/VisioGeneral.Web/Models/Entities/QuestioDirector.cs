namespace VisioGeneral.Web.Models.Entities;

/// <summary>
/// Taula intermèdia per relacionar qüestions amb múltiples directors responsables
/// </summary>
public class QuestioDirector
{
    public int QuestioId { get; set; }
    public int DirectorId { get; set; }

    // Quan es va assignar aquest director
    public DateTime DataAssignacio { get; set; } = DateTime.Now;

    // Navegació
    public Questio Questio { get; set; } = null!;
    public Director Director { get; set; } = null!;
}
