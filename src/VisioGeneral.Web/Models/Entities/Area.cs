namespace VisioGeneral.Web.Models.Entities;

/// <summary>
/// Àrea de l'entitat (6 àrees segons estructura del GREC)
/// </summary>
public class Area
{
    public int Id { get; set; }
    public required string Nom { get; set; }
    public required string Codi { get; set; }  // COM, INF, INS, SAL, PEN, ADM
    public required string Color { get; set; } // Hexadecimal per visualització
    public int Ordre { get; set; }
    public bool Activa { get; set; } = true;
    public DateTime DataCreacio { get; set; } = DateTime.Now;
    public DateTime? DataModificacio { get; set; }

    // Navegació
    public ICollection<Servei> Serveis { get; set; } = new List<Servei>();
    public ICollection<Questio> Qüestions { get; set; } = new List<Questio>();
}
