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

    public partial class GUsuario
    {
        private String _NOME = String.Empty;
        //public GUsuario()
        //{
        //    this.Usuario = new HashSet<Usuario>();
        //}

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int CD_GUSUARIO { get; set; }

        [Required(ErrorMessageResourceType = typeof(strings), ErrorMessageResourceName = "CampoRequerido")]
        [LocalizedDisplayName("NOME")]
        [StringLength(40)]
        public string NOME
        {


            get
            {
                return _NOME.FormatToB2y();
            }

            set
            {
                _NOME = value.FormatToB2y();
            }


        }


        [LocalizedDisplayName("CD_DEPARTAMENTO")]
        public Nullable<int> CD_DEPARTAMENTO
        { get; set; }

        [LocalizedDisplayName("CD_DEPARTAMENTO_DEFAULT")]
        public Nullable<int> CD_DEPARTAMENTO_DEFAULT
        { get; set; }


        [LocalizedDisplayName("CD_PAGINA")]
        public Nullable<int> CD_PAGINA
        { get; set; }

        [LocalizedDisplayName("TMP_MESES_PESQUISA")]
        public int TMP_MESES_PESQUISA
        { get; set; }

//        public virtual DEPARTAMENTO DEPARTAMENTO { get; set; }

        //public virtual ICollection<Usuario> Usuario { get; set; }
    }
}
