using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BackEndTest.Domain.Entity
{
    public class Empresa
    {

        public int Id { get; set; }

        public string Cnpj { get; set; }

        public string RazaoSocial { get; set; }
        public IList<ClienteEmpresa> ClienteEmpresas { get; set; }
    }
}
