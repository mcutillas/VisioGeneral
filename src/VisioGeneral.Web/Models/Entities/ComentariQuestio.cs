namespace VisioGeneral.Web.Models.Entities;

/// <summary>
/// Comentaris a les qüestions (amb suport per respostes niuades)
/// </summary>
public class ComentariQuestio
{
    public int Id { get; set; }
    public int QuestioId { get; set; }
    public int? DirectorId { get; set; }
    public required string Text { get; set; }
    public DateTime DataCreacio { get; set; } = DateTime.Now;
    public DateTime? DataModificacio { get; set; }

    // Comentari pare per a respostes niuades (null si és comentari arrel)
    public int? ComentariPareId { get; set; }

    // Navegació
    public Questio Questio { get; set; } = null!;
    public Director? Director { get; set; }
    public ComentariQuestio? ComentariPare { get; set; }
    public ICollection<ComentariQuestio> Respostes { get; set; } = new List<ComentariQuestio>();
}
