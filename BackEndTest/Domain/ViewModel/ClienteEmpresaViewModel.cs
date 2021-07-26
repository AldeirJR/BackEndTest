using BackEndTest.Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BackEndTest.Domain.ViewModel
{
    public class ClienteEmpresaViewModel
    {
        public int Id { get; set; }
        public int ClienteId { get; set; }
        public Cliente Cliente { get; set; }

        public int EmpresaId { get; set; }
        public Empresa Empresa { get; set; }


    }
}
