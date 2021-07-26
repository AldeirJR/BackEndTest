using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BackEndTest.Domain.Entity
{
    public class Cliente
    {

        public int Id { get; set; }

        public string CPF { get; set; }

        public string Nome { get; set; }

        public string Email { get; set; }

        public DateTime DataCriacao { get; set; }

        public IList<ClienteEmpresa> ClienteEmpresas { get; set; }
    }
}
