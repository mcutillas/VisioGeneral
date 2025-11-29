namespace VisioGeneral.Web.Models.Entities;

/// <summary>
/// Qüestió (el cor del sistema de gestió)
/// </summary>
public class Questio
{
    public int Id { get; set; }
    public required string Titol { get; set; }
    public string? Descripcio { get; set; }

    // Classificació
    public int EstatId { get; set; }
    public required string Prioritat { get; set; } = "Normal";  // Urgent, Alta, Normal, Baixa

    // Relacions opcionals
    public int? ProgramaId { get; set; }
    public int? AreaId { get; set; }

    // Responsable i seguiment
    public int? DirectorResponsableId { get; set; }
    public int? DirectorCreadorId { get; set; }

    // Dates
    public DateTime DataCreacio { get; set; } = DateTime.Now;
    public DateTime? DataModificacio { get; set; }
    public DateOnly? DataLimitResposta { get; set; }
    public DateTime? DataResolucio { get; set; }

    // Context
    public bool OrigenExtern { get; set; }
    public string? FontExterna { get; set; }
    public bool EsEquip { get; set; }  // Qüestió de tota l'entitat/equip

    // Soft Delete
    public bool Eliminada { get; set; }
    public DateTime? DataEliminacio { get; set; }
    public string? MotiuEliminacio { get; set; }
    public int? DirectorEliminadorId { get; set; }
    public Director? DirectorEliminador { get; set; }

    // Navegació
    public EstatQuestio Estat { get; set; } = null!;
    public Programa? Programa { get; set; }
    public Area? Area { get; set; }
    public Director? DirectorResponsable { get; set; }
    public Director? DirectorCreador { get; set; }
    public ICollection<HistorialQuestio> Historial { get; set; } = new List<HistorialQuestio>();
    public ICollection<ComentariQuestio> Comentaris { get; set; } = new List<ComentariQuestio>();
    public ICollection<QuestioOrgan> Organs { get; set; } = new List<QuestioOrgan>();
    public ICollection<QuestioDirector> DirectorsResponsables { get; set; } = new List<QuestioDirector>();
    public ICollection<Subtasca> Subtasques { get; set; } = new List<Subtasca>();

    // Helpers
    public bool EsUrgent => Prioritat == "Urgent";
    public bool EstaResolta => Estat?.EsFinal ?? false;
}
