namespace VisioGeneral.Web.Models.Entities;

/// <summary>
/// Director/a de l'equip de direcció
/// </summary>
public class Director
{
    public int Id { get; set; }
    public required string Nom { get; set; }
    public string? Cognoms { get; set; }
    public string? Email { get; set; }
    public string? Telefon { get; set; }
    public required string Tipus { get; set; }  // "Tecnic" o "Gerencia"
    public string? UsernameAD { get; set; }     // Per vincular amb Active Directory
    public bool Actiu { get; set; } = true;
    public DateTime DataCreacio { get; set; } = DateTime.Now;
    public DateTime? DataModificacio { get; set; }

    // Navegació
    public ICollection<Programa> Programes { get; set; } = new List<Programa>();
    public ICollection<Questio> QüestionsResponsable { get; set; } = new List<Questio>();
    public ICollection<Questio> QüestionsCreades { get; set; } = new List<Questio>();
    public ICollection<ComentariQuestio> Comentaris { get; set; } = new List<ComentariQuestio>();
    public ICollection<HistorialQuestio> HistorialCanvis { get; set; } = new List<HistorialQuestio>();
    public ICollection<QuestioDirector> QuestionsAssignades { get; set; } = new List<QuestioDirector>();

    public string NomComplet => string.IsNullOrEmpty(Cognoms) ? Nom : $"{Nom} {Cognoms}";
}
