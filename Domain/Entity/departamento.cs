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


    public partial class DEPARTAMENTO
    {

        private String _DESC_DEPARTAMENTO = String.Empty;

        private String _ENVIA_EMAIL = String.Empty;

        private String _ATIVO = String.Empty;
        private String _NIVEL_SERVICO = String.Empty;

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

        /// <summary>
        /// Tipo = {string} | Nome {DESC_DEPARTAMENTO}
        /// Campo Obrigatorio
        /// </summary>
        [LocalizedDisplayName("DESC_DEPARTAMENTO")]
        [StringLength(40)]
        [Required(ErrorMessageResourceType = typeof(strings), ErrorMessageResourceName = "CampoRequerido")]
        public string DESC_DEPARTAMENTO
        {


            get
            {
                return _DESC_DEPARTAMENTO.FormatToB2y();
            }

            set
            {
                _DESC_DEPARTAMENTO = value.FormatToB2y();
            }


        }


        /// <summary>
        /// Tipo = {Nullable<int>} | Nome {TEMPO_PADRAO}
        /// Campo Obrigatorio
        /// </summary>
        [LocalizedDisplayName("TEMPO_PADRAO")]
        [Required(ErrorMessageResourceType = typeof(strings), ErrorMessageResourceName = "CampoRequerido")]
        public Nullable<decimal> TEMPO_PADRAO { get; set; }

        /// <summary>
        /// Tipo = {string} | Nome {ENVIA_EMAIL}
        /// Campo Obrigatorio
        /// </summary>
        [LocalizedDisplayName("ENVIA_EMAIL")]
        [StringLength(1)]
        public string ENVIA_EMAIL
        {


            get
            {
                if (String.IsNullOrEmpty(_ENVIA_EMAIL)) { _ENVIA_EMAIL = "N"; }
                return _ENVIA_EMAIL.FormatToB2y();
            }

            set
            {
                if (String.IsNullOrEmpty(value)) { value = "N"; }
                _ENVIA_EMAIL = value.FormatToB2y();
            }

        }


        /// <summary>
        /// Tipo = {string} | Nome {ATIVO}
        /// Campo Obrigatorio
        /// </summary>
        [LocalizedDisplayName("DEPATIVO")]
        [StringLength(1)]
        public string ATIVO
        {


            get
            {
                if (String.IsNullOrEmpty(_ATIVO)) { _ATIVO = "N"; }
                return _ATIVO.FormatToB2y();
            }

            set
            {
                if (String.IsNullOrEmpty(value)) { value = "N"; }
                _ATIVO = value.FormatToB2y();
            }

        }

        [LocalizedDisplayName("NIVEL_SERVICO")]
        [StringLength(1)]
        public string NIVEL_SERVICO
        {


            get
            {
                if (String.IsNullOrEmpty(_NIVEL_SERVICO)) { _NIVEL_SERVICO = "N"; }
                return _NIVEL_SERVICO.FormatToB2y();
            }

            set
            {
                if (String.IsNullOrEmpty(value)) { value = "N"; }
                _NIVEL_SERVICO = value.FormatToB2y();
            }

        }
    }
}
