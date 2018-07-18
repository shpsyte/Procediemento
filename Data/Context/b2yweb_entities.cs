using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entity;

namespace Data.Context
{
    public class b2yweb_entities : DbContext
    {
        public b2yweb_entities()
            : base("B2yContext")
        {
        }

        public b2yweb_entities(String strEntity)
            : base("name=" + strEntity + "_entities")
        {
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            base.Configuration.LazyLoadingEnabled = false;

            string shemma = "PATEND"; // "ISCO"; // "PATEND"; // "PATEND_TESTE"; // "ISCO"; //"PATEND"; //"PATEND" //"ISCO"

            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            modelBuilder.Conventions.Remove<ColumnTypeCasingConvention>();

            modelBuilder.Entity<Usuario>().ToTable("USUARIO", shemma);
            modelBuilder.Entity<Usuario>().HasKey(s => new { s.CD_USUARIO });

            modelBuilder.Entity<GUsuario>().ToTable("GUSUARIO", shemma);
            modelBuilder.Entity<GUsuario>().HasKey(s => new { s.CD_GUSUARIO });

            modelBuilder.Entity<tp_procedimento>().ToTable("TP_PROCEDIMENTO", shemma);
            modelBuilder.Entity<tp_procedimento>().HasKey(s => new { s.CD_TIPO });

            modelBuilder.Entity<Tp_Procedimento_Motivos>().ToTable("TP_PROCEDIMENTO_MOTIVOS", shemma);
            modelBuilder.Entity<Tp_Procedimento_Motivos>().HasKey(s => new { s.COD_TIPO, s.MOTIVOID });


        modelBuilder.Entity<Combo>().ToTable("COMBO", shemma);
            modelBuilder.Entity<Combo>().HasKey(s => new { s.ID });

            modelBuilder.Entity<DEPARTAMENTO>().ToTable("DEPARTAMENTO", shemma);
            modelBuilder.Entity<DEPARTAMENTO>().HasKey(s => new { s.CD_DEPARTAMENTO });

            modelBuilder.Entity<DepartamentoUsuario>().ToTable("DEPARTAMENTOUSUARIO", shemma);
            modelBuilder.Entity<DepartamentoUsuario>().HasKey(s => new { s.ID });

            modelBuilder.Entity<UsuarioRegional>().ToTable("USUARIOREGIONAL", shemma);
            modelBuilder.Entity<UsuarioRegional>().HasKey(s => new { s.ID });

            modelBuilder.Entity<Regional>().ToTable("REGIONAL", shemma);
            modelBuilder.Entity<Regional>().HasKey(s => new { s.CD_REGIONAL });

            modelBuilder.Entity<Clientes>().ToTable("CLIENTES", shemma);
            modelBuilder.Entity<Clientes>().HasKey(s => new { s.CD_CADASTRO });

            modelBuilder.Entity<Permissoes>().ToTable("PERMISSOES", shemma);
            modelBuilder.Entity<Permissoes>().HasKey(s => new { s.ID_INSERT });

            modelBuilder.Entity<ProcedimentoAdm>().ToTable("PROCEDIMENTO", shemma);
            modelBuilder.Entity<ProcedimentoAdm>().HasKey(s => new { s.CD_PROCEDIMENTO });

            modelBuilder.Entity<SacProcedimento>().ToTable("SACPROCEDIMENTO", shemma);
            modelBuilder.Entity<SacProcedimento>().HasKey(s => new { s.COD_SAC, s.COD_PROCEDIMENTO });

            modelBuilder.Entity<SacGarantia>().ToTable("SACGARANTIA", shemma);
            modelBuilder.Entity<SacGarantia>().HasKey(s => new { s.COD_SAC, s.GARANTIAID });

            modelBuilder.Entity<Garantia>().ToTable("GARANTIA", shemma);
            modelBuilder.Entity<Garantia>().HasKey(s => new { s.GARANTIAID });



            modelBuilder.Entity<wProcedimento>().ToTable("WPROCEDIMENTO", shemma);
            modelBuilder.Entity<wProcedimento>().HasKey(s => new { s.CD_PROCEDIMENTO });

            modelBuilder.Entity<ProcedimentoAdmArq>().ToTable("PROCEDIMENTOARQ", shemma);
            modelBuilder.Entity<ProcedimentoAdmArq>().HasKey(s => new { s.ID_ARQ });

            modelBuilder.Entity<eNota>().ToTable("ENOTA", shemma);
            modelBuilder.Entity<eNota>().HasKey(s => new { s.NR_NOTA });

            modelBuilder.Entity<Grafico1>().ToTable("GRAFICO1", shemma);
            modelBuilder.Entity<Grafico1>().HasKey(s => new { s.CD_DEPARTAMENTO });

            modelBuilder.Entity<Grafico2>().ToTable("GRAFICO2", shemma);
            modelBuilder.Entity<Grafico2>().HasKey(s => new { s.ID_SITUACAO });

            modelBuilder.Entity<Grafico3>().ToTable("GRAFICO3", shemma);
            modelBuilder.Entity<Grafico3>().HasKey(s => new { s.CD_PROCEDIMENTO });


            modelBuilder.Entity<Situacao>().ToTable("SITUACOESPROCEDIMENTO", shemma);
            modelBuilder.Entity<Situacao>().HasKey(s => new { s.ID_SITUACAO });

            modelBuilder.Entity<Modulos>().ToTable("MODULOS", shemma);
            modelBuilder.Entity<Modulos>().HasKey(s => new { s.CD_MODULO});

            modelBuilder.Entity<pa_troca_departamentos>().ToTable("PA_TROCA_DEPARTAMENTOS", shemma);
            modelBuilder.Entity<pa_troca_departamentos>().HasKey(s => new { s.NUM_SEQ});


            modelBuilder.Entity<pa_troca_departamentos>().HasRequired(a => a.DEPANT).WithMany().HasForeignKey(u => u.CD_DEPARTAMENTO_ANT);
            modelBuilder.Entity<pa_troca_departamentos>().HasRequired(a => a.DEPNOVA).WithMany().HasForeignKey(u => u.CD_DEPARTAMENTO_NOVA);


            modelBuilder.Entity<wpa_troca_departamentos>().ToTable("WPA_TROCA_DEPARTAMENTOS", shemma);
            modelBuilder.Entity<wpa_troca_departamentos>().HasKey(s => new { s.NUM_SEQ });

            modelBuilder.Entity<TRANSPORTADOR>().ToTable("TRANSPORTADOR", shemma);
            modelBuilder.Entity<TRANSPORTADOR>().HasKey(s => new { s.CD_CADASTRO });


            modelBuilder.Entity<wpa_troca_departamentos>().HasRequired(a => a.DEPANT).WithMany().HasForeignKey(u => u.CD_DEPARTAMENTO_ANT);
            modelBuilder.Entity<wpa_troca_departamentos>().HasRequired(a => a.DEPNOVA).WithMany().HasForeignKey(u => u.CD_DEPARTAMENTO_NOVA);


            /* foreign keys */
            /* modelBuilder.Entity<DepartamentoUsuario>().HasRequired(a => a.CD_USUARIO).WithMany().HasForeignKey(u => u.CD_USUARIO);
               modelBuilder.Entity<DepartamentoUsuario>().HasRequired(a => a.CD_DEPARTAMENTO).WithMany().HasForeignKey(u => new { u.CD_DEPARTAMENTO }); */

        }

        public DbSet<Usuario> Usuario { get; set; }
        public DbSet<GUsuario> GUsuario { get; set; }
        public DbSet<Combo> Combo { get; set; }
        public DbSet<tp_procedimento> TP_PROCEDIMENTO { get; set; }
        public DbSet<DEPARTAMENTO> DEPARTAMENTOes { get; set; }
        public DbSet<DepartamentoUsuario> DepartamentoUsuario { get; set; }
        public DbSet<UsuarioRegional> UsuarioRegional { get; set; }
        public DbSet<Regional> Regional { get; set; }
        public DbSet<Clientes> Clientes { get; set; }
        public DbSet<TRANSPORTADOR> TRANSPORTADOR { get; set; }
        public DbSet<ProcedimentoAdm> ProcedimentoAdm { get; set; }
        public DbSet<SacProcedimento> SacProcedimento { get; set; }
        public DbSet<Garantia> Garantia { get; set; }
        
        public DbSet<SacGarantia> SacGarantia { get; set; }
        public DbSet<eNota> eNota { get; set; }
        public DbSet<ProcedimentoAdmArq> ProcedimentoAdmArq { get; set; }
        public DbSet<Situacao> Situacao { get; set; }
        public DbSet<pa_troca_departamentos> pa_troca_departamentos { get; set; }
        public DbSet<wpa_troca_departamentos> wpa_troca_departamentos { get; set; }
        public DbSet<Grafico1> Grafico1 { get; set; }
        public DbSet<Grafico2> Grafico2 { get; set; }
        public DbSet<Grafico3> Grafico3 { get; set; }
        public DbSet<Permissoes> Permissoes { get; set; }
        public DbSet<Modulos> Modulos { get; set; }
        public DbSet<wProcedimento> wProcedimento { get; set; }
        public DbSet<Tp_Procedimento_Motivos> Tp_Procedimento_Motivos { get; set; }

    }

}
