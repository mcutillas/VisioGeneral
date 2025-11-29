namespace VisioGeneral.Web.Models.Entities;

/// <summary>
/// Comentaris a les qüestions
/// </summary>
public class ComentariQuestio
{
    public int Id { get; set; }
    public int QuestioId { get; set; }
    public int? DirectorId { get; set; }
    public required string Text { get; set; }
    public DateTime DataCreacio { get; set; } = DateTime.Now;
    public DateTime? DataModificacio { get; set; }

    // Navegació
    public Questio Questio { get; set; } = null!;
    public Director? Director { get; set; }
}
