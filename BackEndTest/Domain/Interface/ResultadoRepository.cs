
using BackEndTest.Domain.Model;
using BackEndTest.Domain.Models;
using BackEndTest.Infra;
using System;
using System.Linq;

namespace BackEndTest.Domain.Interface
{
    public class ResultadoRepository
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="resultado">Retorna a classe resultado com a mensagem gravada no Log</param>
        /// <param name="pMensagem">Mensagem a ser gravada no Log</param>
        /// <param name="pSistema">Nome do sistema origem da mensagem</param>
        /// <param name="pCampo">Nome do campo origem da mensagem</param>
        /// <param name="pMetodo">Nome do Método origem da mensagem</param>
        /// <param name="pPagina">Nome da Classe origem da mensagem</param>
        /// <param name="pGravarSql">Flag para informar se a mensagem será gravada no Database</param>
        /// <param name="pConnectionString">ConnectionString para acessar o Database contendo o log.</param>
        /// <param name="pNameFileLog">Nome do arquivo ".txt" a ser gravado o log.</param>
        public void GravarLog(ref Resultado resultado, String pMensagem, String pSistema, String pCampo, String pMetodo, String pPagina, bool pGravarSql = false, string pConnectionString = "", string pNameFileLog = "")
        {
            resultado.AdicionaLog(pMensagem, pSistema, pCampo, pMetodo, pPagina);

            string sistema = resultado.Sistema;
            GravaLogTxt(pMensagem, sistema, pCampo, pMetodo, pPagina, pGravarSql, pConnectionString, pNameFileLog);
        }

        public void GravarLog(Exception pException, ref Resultado resultado, string pMensagemComplementar = "", string pNameFileLog = "", bool pGravarSql = false, string pConnectionString = "")
        {
            resultado.AdicionaLog(pException, pMensagemComplementar);
            Mensagem mensagem = resultado.Mensagens.First();
            string strMensagem = mensagem.Descricoes.First();

            string sistema = resultado.Sistema;
            GravaLogTxt(strMensagem, sistema, mensagem.Campo, mensagem.Metodo, mensagem.Pagina, pGravarSql, pConnectionString, pNameFileLog);
        }

        private void GravaLogTxt(String pMensagem, String pSistema, String pCampo, String pMetodo, String pPagina, bool pGravarSql = false, string pConnectionString = "", string pNameFileLog = "")
        {
            Util.VerificaArqLog(pNameFileLog);

            Util.GravarLog(pMensagem, pSistema, pNameFileLog);

            //if (pGravarSql && !string.IsNullOrEmpty(pConnectionString))
            //{
            //    Util.GravaErroSql(pConnectionString, pMensagem, pSistema, pCampo, pMetodo, pPagina);
            //}
        }

        ///// <summary>
        ///// Mantido somente para manter compatibilidade com os métodos antigos, para gravar o DbEntityValidationException deve ser utilizado o GravarLog(Exception pException)
        ///// </summary>
        ///// <param name="epException"></param>
        ///// <param name="resultado"></param>
        ///// <param name="pMensagemComplementar"></param>
        ///// <param name="pGravarSql"></param>
        ///// <param name="pConnectionString"></param>
        ///// <param name="pNameFileLog"></param>
        //public void GravarLogDbEntityValidation(DbEntityValidationException epException, ref Resultado resultado, string pMensagemComplementar = "", bool pGravarSql = false, string pConnectionString = "", string pNameFileLog = "")
        //{
        //    GravarLog(epException, ref resultado, pMensagemComplementar, pNameFileLog, pGravarSql, pConnectionString);
        //}


    }
}
