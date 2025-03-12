using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Servidor
{
    public class Mensagem
    {
        public int IdMsg { get; set; }
        public string NomeSala { get; set; }
        public string User { get; set; }
        public string DataEnvio { get; set; }
        public string Msg { get; set; }
    }
}
