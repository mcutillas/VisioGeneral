namespace VisioGeneral.Web.Models.Entities;

/// <summary>
/// Estat d'una subtasca
/// </summary>
public enum EstatSubtasca
{
    Pendent,
    EnCurs,
    Completada
}

/// <summary>
/// Subtasca dins d'una qüestió - permet desglossar tasques concretes amb responsables individuals
/// </summary>
public class Subtasca
{
    public int Id { get; set; }

    public required string Titol { get; set; }
    public string? Descripcio { get; set; }

    // Relació amb la qüestió pare
    public int QuestioId { get; set; }

    // Responsable individual de la subtasca (opcional)
    public int? DirectorAssignatId { get; set; }

    // Estat de la subtasca (Pendent, EnCurs, Completada)
    public EstatSubtasca Estat { get; set; } = EstatSubtasca.Pendent;
    public DateTime? DataCompletada { get; set; }

    // Dates
    public DateTime DataCreacio { get; set; } = DateTime.Now;
    public DateOnly? DataLimit { get; set; }

    // Per ordenar les subtasques dins d'una qüestió
    public int Ordre { get; set; }

    // Navegació
    public Questio Questio { get; set; } = null!;
    public Director? DirectorAssignat { get; set; }
    public ICollection<ComentariSubtasca> Comentaris { get; set; } = new List<ComentariSubtasca>();

    // Helpers
    public bool EstaCompletada => Estat == EstatSubtasca.Completada;
}
