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


    public partial class TRANSPORTADOR
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


        /// <summary>
        /// Tipo = {string} | Nome {DESC_DEPARTAMENTO}
        /// Campo Obrigatorio
        /// </summary>
        [LocalizedDisplayName("RAZAO")]
        [Required(ErrorMessageResourceType = typeof(strings), ErrorMessageResourceName = "CampoRequerido")]
        public string RAZAO { get; set; }

        
        [LocalizedDisplayName("FANTASIA")]
        [Required(ErrorMessageResourceType = typeof(strings), ErrorMessageResourceName = "CampoRequerido")]
        public string FANTASIA { get; set; }

        public string DES_CIDADE { get; set; }
        public string UF { get; set; }

        public string CGC_CPF { get; set; }


    }
}
