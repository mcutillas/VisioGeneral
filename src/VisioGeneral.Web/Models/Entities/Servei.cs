namespace VisioGeneral.Web.Models.Entities;

/// <summary>
/// Servei dins d'una àrea
/// </summary>
public class Servei
{
    public int Id { get; set; }
    public int AreaId { get; set; }
    public required string Nom { get; set; }
    public string? Descripcio { get; set; }
    public int Ordre { get; set; }
    public bool Actiu { get; set; } = true;
    public DateTime DataCreacio { get; set; } = DateTime.Now;
    public DateTime? DataModificacio { get; set; }

    // Navegació
    public Area Area { get; set; } = null!;
    public ICollection<Programa> Programes { get; set; } = new List<Programa>();
}
