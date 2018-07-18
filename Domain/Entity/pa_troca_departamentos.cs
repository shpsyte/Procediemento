using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Services.Functions;

namespace Domain.Entity
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    using IntlTexto;
    using IntlTexto.Intl;


    public partial class pa_troca_departamentos
    {

        [Key, Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Editable(false)]
        [ScaffoldColumn(false)]
        [Required(ErrorMessageResourceType = typeof(strings), ErrorMessageResourceName = "CampoRequerido")]
        [LocalizedDisplayName("NUM_SEQ")]
        public int NUM_SEQ { get; set; }

       
        [LocalizedDisplayName("CD_PROCEDIMENTO")]
        [ForeignKey("ProcedimentoAdm")]
        public int CD_PROCEDIMENTO { get; set; }



        [LocalizedDisplayName("DTA_TROCA")]
        [DataType(DataType.Date)]
        public Nullable<System.DateTime> DTA_TROCA { get; set; }



        [LocalizedDisplayName("DTA_ENTRADA_DEP_NOVA")]
        [DataType(DataType.Date)]
        public Nullable<System.DateTime> DTA_ENTRADA_DEP_NOVA { get; set; }

        
        [LocalizedDisplayName("DTA_SAIDA_DEP_NOVA")]
        [DataType(DataType.Date)]
        public Nullable<System.DateTime> DTA_SAIDA_DEP_NOVA { get; set; }



        [LocalizedDisplayName("COD_USUARIO")]
        [ForeignKey("Usuario")]
        public int CD_USUARIO { get; set; }


        [LocalizedDisplayName("CD_DEPARTAMENTO_ANT")]
        public int CD_DEPARTAMENTO_ANT { get; set; }


        [LocalizedDisplayName("CD_DEPARTAMENTO_NOVA")]
        public int CD_DEPARTAMENTO_NOVA { get; set; }

       
        [LocalizedDisplayName("OBS")]
        [MaxLength(2000)]
        public string OBS
        { get; set; }



        public virtual DEPARTAMENTO DEPANT { get; set; }
        public virtual DEPARTAMENTO DEPNOVA { get; set; }
        
        public virtual Usuario Usuario { get; set; }
        public virtual ProcedimentoAdm ProcedimentoAdm { get; set; }

    }


}
