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


    public partial class Modulos
    {
        [Key, Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None )]
        [Editable(false)]
        [ScaffoldColumn(false)]
        public int CD_MODULO { get; set; }
        public string DESC_MODULO { get; set; }
        public string TEXTO { get; set; }
    
    }
}
