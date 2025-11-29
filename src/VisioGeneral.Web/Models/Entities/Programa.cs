namespace VisioGeneral.Web.Models.Entities;

/// <summary>
/// Programa (la unitat fonamental de treball del GREC)
/// </summary>
public class Programa
{
    public int Id { get; set; }
    public int ServeiId { get; set; }
    public int? DirectorId { get; set; }
    public required string Nom { get; set; }
    public string? NomCurt { get; set; }       // Per espais petits del Treemap
    public string? Descripcio { get; set; }

    // Mètriques
    public int NumTreballadors { get; set; }
    public int? NumUsuaris { get; set; }

    // Estat
    public required string Estat { get; set; } = "Actiu";  // Actiu, Creixement, Parat, Finalitzat
    public bool EsLiniaCreixement { get; set; }
    public bool EsNou { get; set; }

    // Dates
    public DateOnly? DataInici { get; set; }
    public DateOnly? DataFi { get; set; }
    public int Ordre { get; set; }
    public bool Actiu { get; set; } = true;
    public DateTime DataCreacio { get; set; } = DateTime.Now;
    public DateTime? DataModificacio { get; set; }

    // Navegació
    public Servei Servei { get; set; } = null!;
    public Director? Director { get; set; }
    public ICollection<Questio> Qüestions { get; set; } = new List<Questio>();

    // Helpers
    public string NomMostrar => NomCurt ?? Nom;
}
