using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entity
{
    public partial class Combo
    {
        public int ID { get; set; }
        public string VALUE { get; set; }
        public string TEXT { get; set; }
        public int TIPO { get; set; }
    }
}
