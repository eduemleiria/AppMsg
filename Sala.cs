using System;

namespace Classes
{
    public class Sala
    {
        public string NomeSala { get; set; }
        public string Descricao { get; set; }
        public string DataCriacao { get; set; }
        public Dictionary<string, string> Membros { get; set; } = new Dictionary<string, string>();
    }
}

