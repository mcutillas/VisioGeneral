namespace VisioGeneral.Web.Models.Entities;

/// <summary>
/// Comentari de seguiment d'una subtasca
/// </summary>
public class ComentariSubtasca
{
    public int Id { get; set; }

    public required string Text { get; set; }

    // Relació amb la subtasca
    public int SubtascaId { get; set; }

    // Qui fa el comentari (opcional, per si no hi ha autenticació)
    public int? DirectorId { get; set; }

    // Quan es va fer el comentari
    public DateTime DataCreacio { get; set; } = DateTime.Now;

    // Si el comentari es va fer en un canvi d'estat, guardem quin estat
    public EstatSubtasca? EstatEnComentari { get; set; }

    // Navegació
    public Subtasca Subtasca { get; set; } = null!;
    public Director? Director { get; set; }
}
