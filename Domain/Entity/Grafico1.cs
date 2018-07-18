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


    public partial class Grafico1
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
        [LocalizedDisplayName("CD_DEPARTAMENTO")]
        public int CD_DEPARTAMENTO { get; set; }


        public string DESC_DEPARTAMENTO { get; set; }

        /// <summary>
        /// Tipo = {string} | Nome {DESC_DEPARTAMENTO}
        /// Campo Obrigatorio
        /// </summary>
        public int? QTDETOTAL { get; set; }
        public int? QTDEAPROVADA { get; set; }
        public int? QTDEREPROVADA { get; set; }
        public int? QTDEATIVAS { get; set; }

    
    }



    public partial class Grafico3
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Editable(false)]
        [ScaffoldColumn(false)]
        [Required(ErrorMessageResourceType = typeof(strings), ErrorMessageResourceName = "CampoRequerido")]
        public int CD_PROCEDIMENTO { get; set; }

        public String CLIENTE { get; set; }
        public string DEPARTAMENTO { get; set; }
        public int HORASPARADAS { get; set; }

    }
        public partial class Grafico2
    {

        /// <summary>
        /// Tipo = {int} | Nome {CD_DEPARTAMENTO}
        /// Campo Obrigatorio
        /// </summary>

        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Editable(false)]
        [ScaffoldColumn(false)]
        [Required(ErrorMessageResourceType = typeof(strings), ErrorMessageResourceName = "CampoRequerido")]
        [LocalizedDisplayName("QTDETOTAL")]
        public int? QTDETOTAL { get; set; }

        public int? ID_SITUACAO { get; set; }
        public string SITUACAO { get; set; }

     


    }
}
