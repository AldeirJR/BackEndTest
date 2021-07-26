using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BackEndTest.Controllers
{
    [Authorize]
    public class BaseController : Controller
    {
     //   public async Task<bool> Usuario_Tem_Acesso(int codigoPagina, EasySupDbContext _context)

      //  {
         //   var usuario = User.Identity.Name;
           // var TemAcesso = await (from TP in _context.TipoUsuario
               //                    join AT in _context.AcessoTipoUsuario on TP.Id equals AT.IdTipoUsuario
                      //             join PF in _context.PerfilUsuario on TP.Id equals PF.IdTipoUsuario
                   //                join US in _context.Usuario on PF.UserId equals US.Id
                         //          where AT.Id == codigoPagina && US.Email == usuario
                         //          select new
                          //         {

                              //         TP.Id
                              //     }).AnyAsync();

           // return TemAcesso;
      //  }

        //protected string GetUserIP()
        //{
        //    Resultado resultado = new Resultado();

        //    var ip = (System.Web.HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"] != null
        //          && System.Web.HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"] != "")
        //         ? System.Web.HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"]
        //         : System.Web.HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];

        //    new ResultadoProcess().GravarLog(ref resultado, string.Format("IP com ou sem Proxy: {0}", ip)
        //        , this.GetType().Namespace, "", string.Format("{0}()", System.Reflection.MethodBase.GetCurrentMethod().Name), string.Format("{0}.cs", this.GetType().Name));

        //    if (ip.Contains(","))
        //        ip = ip.Split(',').First();
        //    return ip.Trim();
        //}

    }
}
