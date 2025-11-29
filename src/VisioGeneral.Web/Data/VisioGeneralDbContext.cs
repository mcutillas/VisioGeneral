using Microsoft.EntityFrameworkCore;
using VisioGeneral.Web.Models.Entities;

namespace VisioGeneral.Web.Data;

public class VisioGeneralDbContext : DbContext
{
    public VisioGeneralDbContext(DbContextOptions<VisioGeneralDbContext> options)
        : base(options)
    {
    }

    // Estructura organitzativa
    public DbSet<Area> Areas => Set<Area>();
    public DbSet<Servei> Serveis => Set<Servei>();
    public DbSet<Programa> Programes => Set<Programa>();
    public DbSet<Director> Directors => Set<Director>();

    // Gestió de qüestions
    public DbSet<EstatQuestio> EstatsQuestio => Set<EstatQuestio>();
    public DbSet<OrganGovern> OrgansGovern => Set<OrganGovern>();
    public DbSet<Questio> Questions => Set<Questio>();
    public DbSet<HistorialQuestio> HistorialQuestio => Set<HistorialQuestio>();
    public DbSet<ComentariQuestio> ComentarisQuestio => Set<ComentariQuestio>();
    public DbSet<QuestioOrgan> QuestioOrgans => Set<QuestioOrgan>();
    public DbSet<QuestioDirector> QuestioDirectors => Set<QuestioDirector>();
    public DbSet<Subtasca> Subtasques => Set<Subtasca>();
    public DbSet<ComentariSubtasca> ComentarisSubtasca => Set<ComentariSubtasca>();

    // Context anual
    public DbSet<ContextAnual> ContextAnual => Set<ContextAnual>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Area
        modelBuilder.Entity<Area>(entity =>
        {
            entity.HasIndex(e => e.Codi).IsUnique();
            entity.Property(e => e.Nom).HasMaxLength(100);
            entity.Property(e => e.Codi).HasMaxLength(20);
            entity.Property(e => e.Color).HasMaxLength(7);
        });

        // Servei
        modelBuilder.Entity<Servei>(entity =>
        {
            entity.Property(e => e.Nom).HasMaxLength(200);
            entity.Property(e => e.Descripcio).HasMaxLength(500);
            entity.HasOne(e => e.Area)
                  .WithMany(a => a.Serveis)
                  .HasForeignKey(e => e.AreaId);
        });

        // Director
        modelBuilder.Entity<Director>(entity =>
        {
            entity.Property(e => e.Nom).HasMaxLength(100);
            entity.Property(e => e.Cognoms).HasMaxLength(150);
            entity.Property(e => e.Email).HasMaxLength(200);
            entity.Property(e => e.Telefon).HasMaxLength(20);
            entity.Property(e => e.Tipus).HasMaxLength(50);
            entity.Property(e => e.UsernameAD).HasMaxLength(100);
        });

        // Programa
        modelBuilder.Entity<Programa>(entity =>
        {
            entity.Property(e => e.Nom).HasMaxLength(200);
            entity.Property(e => e.NomCurt).HasMaxLength(50);
            entity.Property(e => e.Descripcio).HasMaxLength(1000);
            entity.Property(e => e.Estat).HasMaxLength(50);
            entity.HasOne(e => e.Servei)
                  .WithMany(s => s.Programes)
                  .HasForeignKey(e => e.ServeiId);
            entity.HasOne(e => e.Director)
                  .WithMany(d => d.Programes)
                  .HasForeignKey(e => e.DirectorId);
        });

        // EstatQuestio
        modelBuilder.Entity<EstatQuestio>(entity =>
        {
            entity.Property(e => e.Nom).HasMaxLength(100);
            entity.Property(e => e.Descripcio).HasMaxLength(500);
            entity.Property(e => e.Color).HasMaxLength(7);
        });

        // OrganGovern
        modelBuilder.Entity<OrganGovern>(entity =>
        {
            entity.Property(e => e.Nom).HasMaxLength(100);
            entity.Property(e => e.Descripcio).HasMaxLength(500);
        });

        // Questio - mapeja a taula Questions (sense dièresi per compatibilitat SQL)
        modelBuilder.Entity<Questio>(entity =>
        {
            entity.ToTable("Questions");
            entity.Property(e => e.Titol).HasMaxLength(300);
            entity.Property(e => e.Prioritat).HasMaxLength(20);
            entity.Property(e => e.FontExterna).HasMaxLength(200);

            entity.HasOne(e => e.Estat)
                  .WithMany(es => es.Qüestions)
                  .HasForeignKey(e => e.EstatId);

            entity.HasOne(e => e.Programa)
                  .WithMany(p => p.Qüestions)
                  .HasForeignKey(e => e.ProgramaId);

            entity.HasOne(e => e.Area)
                  .WithMany(a => a.Qüestions)
                  .HasForeignKey(e => e.AreaId);

            entity.HasOne(e => e.DirectorResponsable)
                  .WithMany(d => d.QüestionsResponsable)
                  .HasForeignKey(e => e.DirectorResponsableId)
                  .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(e => e.DirectorCreador)
                  .WithMany(d => d.QüestionsCreades)
                  .HasForeignKey(e => e.DirectorCreadorId)
                  .OnDelete(DeleteBehavior.Restrict);

            entity.HasIndex(e => e.EstatId);
            entity.HasIndex(e => e.ProgramaId);
            entity.HasIndex(e => e.AreaId);
            entity.HasIndex(e => e.Prioritat);
            entity.HasIndex(e => e.DataCreacio);
        });

        // HistorialQuestio
        modelBuilder.Entity<HistorialQuestio>(entity =>
        {
            entity.Property(e => e.Comentari).HasMaxLength(1000);

            entity.HasOne(e => e.Questio)
                  .WithMany(q => q.Historial)
                  .HasForeignKey(e => e.QuestioId);

            entity.HasOne(e => e.EstatAnterior)
                  .WithMany()
                  .HasForeignKey(e => e.EstatAnteriorId)
                  .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(e => e.EstatNou)
                  .WithMany()
                  .HasForeignKey(e => e.EstatNouId)
                  .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(e => e.Director)
                  .WithMany(d => d.HistorialCanvis)
                  .HasForeignKey(e => e.DirectorId)
                  .OnDelete(DeleteBehavior.Restrict);

            entity.HasIndex(e => e.QuestioId);
        });

        // ComentariQuestio
        modelBuilder.Entity<ComentariQuestio>(entity =>
        {
            entity.HasOne(e => e.Questio)
                  .WithMany(q => q.Comentaris)
                  .HasForeignKey(e => e.QuestioId);

            entity.HasOne(e => e.Director)
                  .WithMany(d => d.Comentaris)
                  .HasForeignKey(e => e.DirectorId)
                  .OnDelete(DeleteBehavior.Restrict);

            entity.HasIndex(e => e.QuestioId);
        });

        // QuestioOrgan
        modelBuilder.Entity<QuestioOrgan>(entity =>
        {
            entity.Property(e => e.Resultat).HasMaxLength(500);
            entity.Property(e => e.PendentDe).HasMaxLength(200);

            entity.HasOne(e => e.Questio)
                  .WithMany(q => q.Organs)
                  .HasForeignKey(e => e.QuestioId);

            entity.HasOne(e => e.Organ)
                  .WithMany(o => o.QuestioOrgans)
                  .HasForeignKey(e => e.OrganId);
        });

        // QuestioDirector (relació molts-a-molts)
        modelBuilder.Entity<QuestioDirector>(entity =>
        {
            // Clau primària composta
            entity.HasKey(qd => new { qd.QuestioId, qd.DirectorId });

            entity.HasOne(qd => qd.Questio)
                  .WithMany(q => q.DirectorsResponsables)
                  .HasForeignKey(qd => qd.QuestioId)
                  .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(qd => qd.Director)
                  .WithMany(d => d.QuestionsAssignades)
                  .HasForeignKey(qd => qd.DirectorId)
                  .OnDelete(DeleteBehavior.Cascade);
        });

        // Subtasca
        modelBuilder.Entity<Subtasca>(entity =>
        {
            entity.Property(e => e.Titol).HasMaxLength(300);
            entity.Property(e => e.Descripcio).HasMaxLength(1000);
            entity.Property(e => e.Estat).HasConversion<string>().HasMaxLength(20);

            entity.HasOne(e => e.Questio)
                  .WithMany(q => q.Subtasques)
                  .HasForeignKey(e => e.QuestioId)
                  .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(e => e.DirectorAssignat)
                  .WithMany()
                  .HasForeignKey(e => e.DirectorAssignatId)
                  .OnDelete(DeleteBehavior.Restrict);

            entity.HasIndex(e => e.QuestioId);
            entity.HasIndex(e => new { e.QuestioId, e.Ordre });
        });

        // ComentariSubtasca
        modelBuilder.Entity<ComentariSubtasca>(entity =>
        {
            entity.Property(e => e.Text).HasMaxLength(2000);
            entity.Property(e => e.EstatEnComentari).HasConversion<string>().HasMaxLength(20);

            entity.HasOne(e => e.Subtasca)
                  .WithMany(s => s.Comentaris)
                  .HasForeignKey(e => e.SubtascaId)
                  .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(e => e.Director)
                  .WithMany()
                  .HasForeignKey(e => e.DirectorId)
                  .OnDelete(DeleteBehavior.Restrict);

            entity.HasIndex(e => e.SubtascaId);
        });

        // ContextAnual
        modelBuilder.Entity<ContextAnual>(entity =>
        {
            entity.HasIndex(e => e.Any).IsUnique();
        });
    }
}
