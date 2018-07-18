using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entity
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using IntlTexto;
    using IntlTexto.Intl;

    public partial class Usuario
    {
        [Key]
        [LocalizedDisplayName("CD_USUARIO")]
        public int CD_USUARIO { get; set; }
        [Required(ErrorMessageResourceType = typeof(strings), ErrorMessageResourceName = "CampoRequerido")]
        [LocalizedDisplayName("NOME")]
        [StringLength(200, ErrorMessage = "Tamanho Inválido")]
        public string NOME { get; set; }
        [LocalizedDisplayName("EMAIL")]
        [StringLength(200, ErrorMessage = "Tamanho Inválido")]
        [Required(ErrorMessageResourceType = typeof(strings), ErrorMessageResourceName = "CampoRequerido")]
        [DataType(DataType.EmailAddress)]
        [EmailAddress()]
        public string EMAIL { get; set; }
        [Required(ErrorMessageResourceType = typeof(strings), ErrorMessageResourceName = "CampoRequerido")]
        [LocalizedDisplayName("LOGIN")]
        [StringLength(60, ErrorMessage="Tamanho Inválido")]
        public string LOGIN { get; set; }
        [LocalizedDisplayName("SENHA")]
        [StringLength(30, ErrorMessage = "Tamanho Inválido")]
        [Required(ErrorMessageResourceType = typeof(strings), ErrorMessageResourceName = "CampoRequerido")]
        [DataType(DataType.Password)]
        public string SENHA { get; set; }
        [LocalizedDisplayName("GRUPO")]
        public short CD_GUSUARIO { get; set; }
        [LocalizedDisplayName("CD_NL")]
        public int CD_NL { get; set; }
        [LocalizedDisplayName("APROVA")]
        [Required(ErrorMessage = "Campo Obrigatorio")]
        public string APROVA { get; set; }
        [LocalizedDisplayName("REPROVA")]
        [Required(ErrorMessage = "Campo Obrigatorio")]
        public string REPROVA { get; set; }
        [Required(ErrorMessage = "Campo Obrigatorio")]
        public string CANCELA { get; set; }
        [LocalizedDisplayName("USUATIVO")]
        [Required(ErrorMessage = "Campo Obrigatorio")]
        public string SITUACAO { get; set; }
        [LocalizedDisplayName("ALT_SENHA")]
        [Required(ErrorMessage = "Campo Obrigatorio")]
        public string ALT_SENHA { get; set; }
        //public virtual GUsuario GUsuario { get; set; }
        
        
    }
}
