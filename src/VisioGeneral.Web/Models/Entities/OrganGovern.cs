namespace VisioGeneral.Web.Models.Entities;

/// <summary>
/// Òrgans de govern on es discuteixen les qüestions
/// </summary>
public class OrganGovern
{
    public int Id { get; set; }
    public required string Nom { get; set; }  // Direcció, Junta, Assemblea, Comitè d'Empresa
    public string? Descripcio { get; set; }
    public int Ordre { get; set; }

    // Navegació
    public ICollection<QuestioOrgan> QuestioOrgans { get; set; } = new List<QuestioOrgan>();
}
