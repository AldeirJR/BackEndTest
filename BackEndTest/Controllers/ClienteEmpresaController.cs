using BackEndTest.Domain.Model;
using BackEndTest.Domain.ViewModel;
using BackEndTest.Service.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BackEndTest.Controllers
{
    public class ClienteEmpresaController : BaseController
    {
        public readonly ClienteEmpresaService serviceClienteEmpresa;
        public readonly ResultadoProcess processResultado;
        public ClienteEmpresaController(ClienteEmpresaService pClienteEmpresaService, ResultadoProcess pResultadoProcess)
        {
            serviceClienteEmpresa = pClienteEmpresaService;
            processResultado = pResultadoProcess;

        }
        public IActionResult Index()
        {
            Resultado resultado = new Resultado(User.Identity.Name);
            List<ClienteEmpresaViewModel> lista = serviceClienteEmpresa.Listar(new ClienteEmpresaViewModel(), ref resultado);


            if (!resultado.Sucesso)
                processResultado.GravarLog(ref resultado, string.Format("Falha ao listar Clientes: {0}", resultado.Mensagens[0].Descricoes[0])
                    , this.GetType().Namespace, "", string.Format("{0}()", System.Reflection.MethodBase.GetCurrentMethod().Name), string.Format("{0}.cs", this.GetType().Name));

            return View(lista);
        }

        [HttpPost]
        public IActionResult CadastrarClienteEmpresa(ClienteEmpresaViewModel clientEmpresa)
        {
            Resultado resultado = new Resultado(User.Identity.Name);
            if (ModelState.IsValid)
            {

                serviceClienteEmpresa.Incluir(clientEmpresa, ref resultado);

                if (!resultado.Sucesso)
                    processResultado.GravarLog(ref resultado, string.Format("Falha ao listar Clientes: {0}", resultado.Mensagens[0].Descricoes[0])
                        , this.GetType().Namespace, "", string.Format("{0}()", System.Reflection.MethodBase.GetCurrentMethod().Name), string.Format("{0}.cs", this.GetType().Name));

                return RedirectToAction("CadastrarEmpresa", "Empresa");


            }
            return View(clientEmpresa);

        }
        [HttpGet]
        public IActionResult CadastrarClienteEmpresa()
        {
            Resultado resultado = new Resultado(User.Identity.Name);
            var clienteEmpresa = new ClienteEmpresaViewModel();
            //List<GenericListViewModel> listauf = serviceCliente.ListarUF(ref resultado);

            //ViewBag.UFs = listauf;

            return View(clienteEmpresa);
        }

        [HttpGet]
        public IActionResult EditarClienteEmpresa(int ID)
        {
            Resultado resultado = new Resultado(User.Identity.Name);
            ClienteEmpresaViewModel clientEmpresa = new ClienteEmpresaViewModel();



            clientEmpresa = serviceClienteEmpresa.GetClienteEmpresa(ID, ref resultado);


            if (!resultado.Sucesso)
                processResultado.GravarLog(ref resultado, string.Format("Falha ao listar ClienteEmpresa: {0}", resultado.Mensagens[0].Descricoes[0])
                    , this.GetType().Namespace, "", string.Format("{0}()", System.Reflection.MethodBase.GetCurrentMethod().Name), string.Format("{0}.cs", this.GetType().Name));

            return View(clientEmpresa);
        }

        [HttpPost]
        public IActionResult EditarClienteEmpresa(ClienteEmpresaViewModel clientEmpresa)
        {
            Resultado resultado = new Resultado(User.Identity.Name);

            if (ModelState.IsValid)
            {
                serviceClienteEmpresa.Alterar(clientEmpresa, ref resultado);

                if (!resultado.Sucesso)
                    processResultado.GravarLog(ref resultado, string.Format("Falha ao listar ClienteEmpresa: {0}", resultado.Mensagens[0].Descricoes[0])
                        , this.GetType().Namespace, "", string.Format("{0}()", System.Reflection.MethodBase.GetCurrentMethod().Name), string.Format("{0}.cs", this.GetType().Name));

                return RedirectToAction("Index");
            }
            else
            {

                return View(clientEmpresa);

            }

        }
        [HttpGet]
        public IActionResult DeletarClienteEmpresa(int ID)
        {
            Resultado resultado = new Resultado(User.Identity.Name);

            
            var clienteEmpresa = serviceClienteEmpresa.GetClienteEmpresa(ID, ref resultado);
            return View(clienteEmpresa);
        }

        [HttpPost]
        public IActionResult DeletarClienteEmpresa(ClienteEmpresaViewModel _clienteEmpresa)
        {
            Resultado resultado = new Resultado(User.Identity.Name);

            ClienteViewModel clienteEmpresa = new ClienteViewModel();

            serviceClienteEmpresa.Excluir(_clienteEmpresa.Id, ref resultado);


            return RedirectToAction("Index");
        }
        [HttpGet]
        public IActionResult DetailsClienteEmpresa(int ID)
        {
            Resultado resultado = new Resultado(User.Identity.Name);

            var clienteEmpresa = serviceClienteEmpresa.GetClienteEmpresa(ID, ref resultado);
            return View(clienteEmpresa);
        }

    }
}
