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


    public partial class Tp_Procedimento_Motivos
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int COD_TIPO { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int MOTIVOID { get; set; }
        [Required(ErrorMessageResourceType = typeof(strings), ErrorMessageResourceName = "CampoRequerido")]
        public string DES_NOME { get; set; }

    }


    public partial class tp_procedimento
    {

        private String _DES_TIPO = String.Empty;
        private String _SOL_NF_OBRIGATORIA = String.Empty;
        private String _SOL_NF_CLIENTE_OBRIGATORIA = String.Empty;
        private String _ATIVO = String.Empty;

        /// <summary>
        /// Tipo = {int} | Nome {CD_TIPO}
        /// Campo Obrigatorio
        /// </summary>

        [Key, Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Editable(false)]
        [ScaffoldColumn(false)]
        [Required(ErrorMessageResourceType = typeof(strings), ErrorMessageResourceName = "CampoRequerido")]
        [LocalizedDisplayName("CD_TIPO")]
        public int CD_TIPO { get; set; }

        /// <summary>
        /// Tipo = {string} | Nome {DES_TIPO}
        /// Campo Obrigatorio
        /// </summary>

        [Required(ErrorMessageResourceType = typeof(strings), ErrorMessageResourceName = "CampoRequerido")]
        [LocalizedDisplayName("DES_TIPO")]
        [StringLength(40)]
        public string DES_TIPO
        {


            get
            {
                return _DES_TIPO.FormatToB2y();
            }

            set
            {
                _DES_TIPO = value.FormatToB2y();
            }


        }


        /// <summary>
        /// Tipo = {string} | Nome {SOL_NF_OBRIGATORIA}
        /// Campo Obrigatorio
        /// </summary>
        [LocalizedDisplayName("SOL_NF_OBRIGATORIA")]
        [StringLength(1)]
        public string SOL_NF_OBRIGATORIA
        {


            get
            {
                if (String.IsNullOrEmpty(_SOL_NF_OBRIGATORIA)) { _SOL_NF_OBRIGATORIA = "N"; }
                return _SOL_NF_OBRIGATORIA.FormatToB2y();
            }

            set
            {
                if (String.IsNullOrEmpty(value)) { value = "N"; }
                _SOL_NF_OBRIGATORIA = value.FormatToB2y();
            }

        }


        /// <summary>
        /// Tipo = {string} | Nome {SOL_NF_OBRIGATORIA}
        /// Campo Obrigatorio
        /// </summary>
        [LocalizedDisplayName("SOL_NF_CLIENTE_OBRIGATORIA")]
        [StringLength(1)]
        public string SOL_NF_CLIENTE_OBRIGATORIA
        {


            get
            {
                if (String.IsNullOrEmpty(_SOL_NF_CLIENTE_OBRIGATORIA)) { _SOL_NF_CLIENTE_OBRIGATORIA = "N"; }
                return _SOL_NF_CLIENTE_OBRIGATORIA.FormatToB2y();
            }

            set
            {
                if (String.IsNullOrEmpty(value)) { value = "N"; }
                _SOL_NF_CLIENTE_OBRIGATORIA = value.FormatToB2y();
            }

        }
        /// <summary>
        /// Tipo = {string} | Nome {ATIVO}
        /// Campo Obrigatorio
        /// </summary>
        [LocalizedDisplayName("ATIVO")]
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


        [Required(ErrorMessageResourceType = typeof(strings), ErrorMessageResourceName = "CampoRequerido")]
        [LocalizedDisplayName("TEMPO_PADRAO")]
        public int? TEMPO_PADRAO
        { get; set; }

    }
}
