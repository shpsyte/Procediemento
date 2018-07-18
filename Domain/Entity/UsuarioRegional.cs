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


    public partial class UsuarioRegional
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
        [LocalizedDisplayName("ID")]
        public int ID { get; set; }

        /// <summary>
        /// Tipo = {string} | Nome {DESC_DEPARTAMENTO}
        /// Campo Obrigatorio
        /// </summary>
        [LocalizedDisplayName("CD_REGIONAL")]
        [Required(ErrorMessageResourceType = typeof(strings), ErrorMessageResourceName = "CampoRequerido")]
        [ForeignKey("Regional")]
        public int CD_REGIONAL { get; set; }


        /// <summary>
        /// Tipo = {string} | Nome {DESC_DEPARTAMENTO}
        /// Campo Obrigatorio
        /// </summary>
        [LocalizedDisplayName("cd_usuario")]
        [Required(ErrorMessageResourceType = typeof(strings), ErrorMessageResourceName = "CampoRequerido")]
        [ForeignKey("Usuario")]
        public int CD_USUARIO { get; set; }


        public virtual Usuario Usuario { get; set; }
        public virtual Regional Regional { get; set; }

        
    }
}
