using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Servidor
{
    public class Sala
    {
        public int IdSala { get; set; }
        public string NomeSala { get; set; }
        public string Descricao { get; set; }
        public string DataCriacao { get; set; }
        public Dictionary<string, string> Membros { get; set; } = new Dictionary<string, string>();
    }
}
