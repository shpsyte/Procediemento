using IntlTexto;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entity
{
    public class GarantiaProcedimento
    {
        [Key, Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Editable(false)]
        [ScaffoldColumn(false)]
        [Required(ErrorMessageResourceType = typeof(strings), ErrorMessageResourceName = "CampoRequerido")]
        public int ID { get; set; }

        [Key, Column(Order = 1)]
        public int GARANTIAID { get; set; }
        [Key, Column(Order = 2)]
        public int COD_PROCEDIMENTO { get; set; }

    }
}
