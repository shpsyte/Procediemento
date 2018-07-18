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


    public partial class Clientes
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
        [LocalizedDisplayName("CD_CADASTRO")]
        public int CD_CADASTRO { get; set; }


        [Required(ErrorMessageResourceType = typeof(strings), ErrorMessageResourceName = "CampoRequerido")]
        [LocalizedDisplayName("CD_REGIONAL")]
        [ForeignKey("Regional")]
        public int CD_REGIONAL { get; set; }


        [LocalizedDisplayName("CD_REPRESENTANTE")]
        public int CD_REPRESENTANTE { get; set; }

        [LocalizedDisplayName("RAZAO_REPRES")]
        public string RAZAO_REPRES { get; set; }


        [LocalizedDisplayName("CD_CIDADE")]
        public int CD_CIDADE { get; set; }

        [LocalizedDisplayName("DES_CIDADE")]
        public string DES_CIDADE { get; set; }

        [LocalizedDisplayName("CD_ESTADO")]
        public string CD_ESTADO { get; set; }
        


        /// <summary>
        /// Tipo = {string} | Nome {DESC_DEPARTAMENTO}
        /// Campo Obrigatorio
        /// </summary>
        [LocalizedDisplayName("RAZAO")]
        [Required(ErrorMessageResourceType = typeof(strings), ErrorMessageResourceName = "CampoRequerido")]
        public string RAZAO { get; set; }


        [LocalizedDisplayName("CGC_CPF")]
        [Required(ErrorMessageResourceType = typeof(strings), ErrorMessageResourceName = "CampoRequerido")]
        public string CGC_CPF { get; set; }


        [LocalizedDisplayName("FANTASIA")]
        [Required(ErrorMessageResourceType = typeof(strings), ErrorMessageResourceName = "CampoRequerido")]
        public string FANTASIA { get; set; }


        [LocalizedDisplayName("ENDERECO")]
        [Required(ErrorMessageResourceType = typeof(strings), ErrorMessageResourceName = "CampoRequerido")]
        public string ENDERECO { get; set; }

        
        [LocalizedDisplayName("BAIRRO")]
        [Required(ErrorMessageResourceType = typeof(strings), ErrorMessageResourceName = "CampoRequerido")]
        public string BAIRRO { get; set; }

        [LocalizedDisplayName("CEP")]
        [Required(ErrorMessageResourceType = typeof(strings), ErrorMessageResourceName = "CampoRequerido")]
        public string CEP { get; set; }


        [LocalizedDisplayName("EMAIL")]
        [Required(ErrorMessageResourceType = typeof(strings), ErrorMessageResourceName = "CampoRequerido")]
        public string EMAIL { get; set; }


        public virtual Regional Regional { get; set; }
    
    }
}
