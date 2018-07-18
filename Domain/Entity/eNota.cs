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


    public partial class eNota
    {

        /// <summary>
        /// Tipo = {int} | Nome {CD_DEPARTAMENTO}
        /// Campo Obrigatorio
        /// </summary>

        [Key, Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Editable(false)]
        [ScaffoldColumn(false)]
        [Required(ErrorMessageResourceType = typeof(strings), ErrorMessageResourceName = "CampoRequerido")]
        [LocalizedDisplayName("NR_NOTA")]
        public int NR_NOTA { get; set; }


        [Required(ErrorMessageResourceType = typeof(strings), ErrorMessageResourceName = "CampoRequerido")]
        [LocalizedDisplayName("CD_CADASTRO")]
        [ForeignKey("Clientes")]
        public int CD_CADASTRO { get; set; }

        [Required(ErrorMessageResourceType = typeof(strings), ErrorMessageResourceName = "CampoRequerido")]
        [LocalizedDisplayName("CD_REGIONAL")]
        [ForeignKey("Regional")]
        public int CD_REGIONAL { get; set; }

        [Required(ErrorMessageResourceType = typeof(strings), ErrorMessageResourceName = "CampoRequerido")]
        [LocalizedDisplayName("CD_TRANSPORTADOR")]
        [ForeignKey("TRANSPORTADOR")]
        public int CD_TRANSPORTADOR { get; set; }
        

        [Required(ErrorMessageResourceType = typeof(strings), ErrorMessageResourceName = "CampoRequerido")]
        [LocalizedDisplayName("DT_EMISSAO")]
        public string DT_EMISSAO { get; set; }
        public string COD_OPER { get; set; }


      

        public virtual Regional Regional { get; set; }
        public virtual Clientes Clientes { get; set; }
        public virtual TRANSPORTADOR TRANSPORTADOR { get; set; }
    
    }
}
