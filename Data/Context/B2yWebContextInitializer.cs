using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Context
{
    class B2yWebContextInitializer : CreateDatabaseIfNotExists<b2yweb_entities>
    {
        protected override void Seed(b2yweb_entities context)
        {
            /* MensagemTopo msg = new MensagemTopo
            {
                id = 1,
                msg = "Gerenciamento de Ticket",
                Tipo = "A"
            };
            context.MensagemTopo.Add(msg);
            context.SaveChanges();
             * */
        }
    }
}
