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


    public partial class DataEntregraExpedicao
    {

        public DateTime? dta_entrega { get; set; }
        public DateTime? dta_expedicao { get; set; }


    }
    public partial class Garantia
    {
        public int GARANTIAID { get; set; }
        public int COD_PROCEDIMENTO_VINCULADO { get; set; }
        public int COD_PROCEDIMENTO_FINAL { get; set; }

    }



    public partial class SacProcedimento
    {
        public int COD_SAC { get; set; }
        public int COD_PROCEDIMENTO { get; set; }
    }


    public partial class SacGarantia
    {
        public int COD_SAC { get; set; }
        public int GARANTIAID { get; set; }
    }

    
    public partial class ProcedimentoAdm
    {

        private String _OBS = String.Empty;

        private String _SITUACAO = String.Empty;

        /// <summary>
        /// Tipo = {int} | Nome {CD_PROCEDIMENTO}
        /// Campo Obrigatorio
        /// </summary>

        [Key, Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Editable(false)]
        [ScaffoldColumn(false)]
        [Required(ErrorMessageResourceType = typeof(strings), ErrorMessageResourceName = "CampoRequerido")]
        [LocalizedDisplayName("CD_PROCEDIMENTO")]
        public int CD_PROCEDIMENTO { get; set; }

        /// <summary>
        /// Tipo = {Nullable<int>} | Nome {CD_CADASTRO}
        /// Campo Obrigatorio
        /// </summary>
        [LocalizedDisplayName("CD_CADASTRO")]
        [ForeignKey("Clientes")]
        [Required(ErrorMessageResourceType = typeof(strings), ErrorMessageResourceName = "CampoRequerido")]
        public Nullable<int> CD_CADASTRO { get; set; }
        public virtual Clientes Clientes { get; set; }
        /// <summary>
        /// Tipo = {Nullable<int>} | Nome {CD_USUARIO}
        /// Campo Obrigatorio
        /// </summary>
        [LocalizedDisplayName("CD_USUARIO")]
        [ForeignKey("Usuario")]
        public int CD_USUARIO { get; set; }
        public virtual Usuario Usuario { get; set; }


        /// <summary>
        /// Tipo = {Nullable<int>} | Nome {CD_REGIONAL}
        /// Campo Obrigatorio
        /// </summary>
        [LocalizedDisplayName("CD_REGIONAL")]
        [ForeignKey("Regional")]
        public int CD_REGIONAL { get; set; }
        public virtual Regional Regional { get; set; }


        /// <summary>
        /// Tipo = {Nullable<int>} | Nome {CD_REGIONAL}
        /// Campo Obrigatorio
        /// </summary>
        [LocalizedDisplayName("CD_DEPARTAMENTO")]
        [ForeignKey("DEPARTAMENTO")]
        public int CD_DEPARTAMENTO { get; set; }
        public virtual DEPARTAMENTO DEPARTAMENTO { get; set; }



        [LocalizedDisplayName("CD_TRANSPORTADOR")]
        [ForeignKey("TRANSPORTADOR")]
        [Required(ErrorMessageResourceType = typeof(strings), ErrorMessageResourceName = "CampoRequerido")]
        public int CD_TRANSPORTADOR { get; set; }
        public virtual TRANSPORTADOR TRANSPORTADOR { get; set; }

        /// <summary>
        /// Tipo = {Nullable<int>} | Nome {CD_TIPO}
        /// Campo Obrigatorio
        /// </summary>
        [LocalizedDisplayName("CD_TIPO")]
        [ForeignKey("tp_procedimento")]
        [Required(ErrorMessageResourceType = typeof(strings), ErrorMessageResourceName = "CampoRequerido")]
        public int CD_TIPO { get; set; }
        public virtual tp_procedimento tp_procedimento { get; set; }

        /// <summary>
        /// Tipo = {Nullable<int>} | Nome {CD_ANEXO}
        /// Campo Obrigatorio
        /// </summary>
        [LocalizedDisplayName("CD_ANEXO")]
        public Nullable<int> CD_ANEXO { get; set; }

        /// <summary>
        /// Tipo = {Nullable<System.DateTime>} | Nome {DTA_ABERTURA}
        /// Campo Obrigatorio
        /// </summary>
        [LocalizedDisplayName("DTA_ABERTURA")]
        [DataType(DataType.Date)]
        public Nullable<System.DateTime> DTA_ABERTURA { get; set; }

        /// <summary>
        /// Tipo = {Nullable<System.DateTime>} | Nome {DTA_FECHAMENTO}
        /// Campo Obrigatorio
        /// </summary>
        [LocalizedDisplayName("DTA_FECHAMENTO")]
        [DataType(DataType.Date)]
        public Nullable<System.DateTime> DTA_FECHAMENTO { get; set; }

        /// <summary>
        /// Tipo = {Nullable<int>} | Nome {NF_FOX}
        /// Campo Obrigatorio
        /// </summary>
        [LocalizedDisplayName("NF_FOX")]
        //[Required(ErrorMessageResourceType = typeof(strings), ErrorMessageResourceName = "CampoRequerido")]
        public Nullable<int> NF_FOX { get; set; }


        [LocalizedDisplayName("DTA_NF_FOX")]
        public string DTA_NF_FOX { get; set; }

        public string COD_OPER { get; set; }

        /// <summary>
        /// Tipo = {Nullable<int>} | Nome {NF_CLIENTE}
        /// Campo Obrigatorio
        /// </summary>
        [LocalizedDisplayName("NF_CLIENTE")]
        //[Required(ErrorMessageResourceType = typeof(strings), ErrorMessageResourceName = "CampoRequerido")]
        public Nullable<int> NF_CLIENTE { get; set; }

        /// <summary>
        /// Tipo = {Nullable<decimal>} | Nome {VL_TRANSPORTADORA}
        /// Campo Obrigatorio
        /// </summary>
        [LocalizedDisplayName("VL_TRANSPORTADORA")]
        [DataType(DataType.Currency)] 
        public Nullable<decimal> VL_TRANSPORTADORA { get; set; }

        /// <summary>
        /// Tipo = {Nullable<decimal>} | Nome {VL_REPRESENTANTE}
        /// Campo Obrigatorio
        /// </summary>
        [LocalizedDisplayName("VL_REPRESENTANTE")]
        [DataType(DataType.Currency)]
        public Nullable<decimal> VL_REPRESENTANTE { get; set; }

        /// <summary>
        /// Tipo = {Nullable<decimal>} | Nome {VL_FOXLUX}
        /// Campo Obrigatorio
        /// </summary>
        [LocalizedDisplayName("VL_FOXLUX")]
        [DataType(DataType.Currency)]
        public Nullable<decimal> VL_FOXLUX { get; set; }

        /// <summary>
        /// Tipo = {Nullable<decimal>} | Nome {VL_CLIENTE}
        /// Campo Obrigatorio
        /// </summary>
        [LocalizedDisplayName("VL_CLIENTE")]
        [DataType(DataType.Currency)]
        public Nullable<decimal> VL_CLIENTE { get; set; }


        [LocalizedDisplayName("VL_DCLIENTE")]
        [DataType(DataType.Currency)]
        public Nullable<decimal> VL_DCLIENTE { get; set; }

        [LocalizedDisplayName("CD_TIPO")]
        //[ForeignKey("Tp_Procedimento_Motivos")]
        [Required(ErrorMessageResourceType = typeof(strings), ErrorMessageResourceName = "CampoRequerido")]

        public int? MOTIVOID { get; set; }


        public int? CD_DEPARTAMENTO_DEBITO { get; set; }



        /// <summary>
        /// Tipo = {string} | Nome {OBS}
        /// Campo Obrigatorio
        /// </summary>
        [LocalizedDisplayName("OBS")]
        [Required(ErrorMessageResourceType = typeof(strings), ErrorMessageResourceName = "CampoRequerido")]
        [MaxLength(2000)]
        public string OBS
        {


            get
            {
                return _OBS.FormatToB2y();
            }

            set
            {
                _OBS = value.FormatToB2y();
            }


        }


        /// <summary>
        /// Tipo = {string} | Nome {SITUACAO}
        /// Campo Obrigatorio
        /// </summary>
        [LocalizedDisplayName("SITUACAO")]
        [ForeignKey("Situacao")]
        public int ID_SITUACAO { get; set; }
        public virtual Situacao Situacao { get; set; }



        public int CD_USUARIO_ALTERACAO
        { get; set; }


        [LocalizedDisplayName("OBS")]
        [MaxLength(2000)]
        public string OBSATENDIMENTO
        { get; set; }



       

    }
}
