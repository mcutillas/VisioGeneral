namespace VisioGeneral.Web.Models.Entities;

/// <summary>
/// Historial de canvis d'estat de les qüestions
/// </summary>
public class HistorialQuestio
{
    public int Id { get; set; }
    public int QuestioId { get; set; }
    public int? EstatAnteriorId { get; set; }
    public int EstatNouId { get; set; }
    public string? Comentari { get; set; }
    public int? DirectorId { get; set; }
    public DateTime DataCanvi { get; set; } = DateTime.Now;

    // Navegació
    public Questio Questio { get; set; } = null!;
    public EstatQuestio? EstatAnterior { get; set; }
    public EstatQuestio EstatNou { get; set; } = null!;
    public Director? Director { get; set; }
}
