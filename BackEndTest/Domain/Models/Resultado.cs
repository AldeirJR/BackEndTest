using BackEndTest.Domain.Models;
using BackEndTest.Infra;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace BackEndTest.Domain.Model
{
    public class Resultado
    {
        #region Propriedades

        /// <summary>
        /// Status que informa se a operação ocorreu com sucesso
        /// </summary>
        public bool Sucesso { get; private set; }
        /// <summary>
        /// Lista dos campos e mensagens de validação
        /// </summary>
        public List<Mensagem> Mensagens { get; private set; }
        /// <summary>
        /// Identificador único dos resultado
        /// </summary>
        public int NumError { get; private set; }
        /// <summary>
        /// Retorna o Id do registro quando no metodo Inclusão a chave é do tipo Identity (sqlServer)
        /// </summary>
        public long Id { get; set; }
        /// <summary>
        /// Nome do método executado, para registro no log
        /// </summary>
        public string Metodo { get; private set; }

        public string Sistema { get; private set; }

        /// <summary>
        /// Login do usuário logado, para registro no Log
        /// </summary>
        public string UserResponsavel { get; set; }

        //public LogStatisticBuilder ChangeLogBuilder { get; set; }

        /// <summary>
        /// Mensagem amigável para ser exibida para o usuário em caso de erros
        /// </summary>
        public string MensagemUsuario { get; private set; }

        #endregion

        #region Construtores
        /// <summary>
        /// Construtor
        /// </summary>
        /// <param name="sucesso">Sucesso.</param>
        /// <param name="mensagens">Mensagens.</param>
        /// <param name="id">Id.</param>
        public Resultado(bool sucesso, List<Mensagem> mensagens, long id)
        {
            this.Sucesso = sucesso;
            this.Mensagens = mensagens;
            this.Id = id;
        }

        /// <summary>
        /// Construtor vazio da classe.
        /// </summary>
        public Resultado(string pUser = "")
            : this(false, new List<Mensagem>(), int.MinValue)
        {
            this.UserResponsavel = pUser;
            this.Sucesso = true;
        }

        /// <summary>
        /// Construtor
        /// </summary>
        /// <param name="sucesso">Sucesso.</param>
        /// <param name="mensagens">Mensagens.</param>
        /// <param name="id">Id.</param>
        public Resultado(bool sucesso, List<Mensagem> mensagens, int id, string pUser = "")
        {
            this.Sucesso = sucesso;
            this.Mensagens = mensagens;
            this.Id = id;
            this.UserResponsavel = pUser;
        }

        #endregion

        #region Métodos 

        public void AdicionaLog(String pMensagem, String pSistema, String pCampo, String pMetodo, String pPagina)
        {
            Mensagem mensagem = new Mensagem();
            mensagem.Campo = pCampo;
            mensagem.Metodo = pMetodo;
            mensagem.Pagina = pPagina;
            mensagem.Descricoes.Add(pMensagem);

            Sucesso = false;
            Mensagens.Add(mensagem);
            Metodo = pMetodo + " - " + pMensagem;
            Sistema = string.Format("{0} | {1} | {2} | {3}", pSistema, pPagina, pMetodo, pCampo);
        }

        public void AdicionaLog(Exception pException, string pMensagemComplementar = "")
        {
            string sistema = string.Empty;
            string pagina = string.Empty;
            string metodo = string.Empty;
            string campo = string.Empty;
            string strMensagem = string.Empty;
            List<string> listaMensagens = new List<string>();

            if (!string.IsNullOrEmpty(pException.Source))
                sistema = pException.Source;

            if (pException.TargetSite != null && pException.TargetSite.ReflectedType != null && !string.IsNullOrEmpty(pException.TargetSite.ReflectedType.FullName))
                pagina = pException.TargetSite.ReflectedType.FullName;

            if (pException.TargetSite != null && pException.TargetSite.ReflectedType != null && !string.IsNullOrEmpty(pException.TargetSite.ReflectedType.Name))
                metodo = string.Format("{0}()", pException.TargetSite.Name);

            var st = new StackTrace(pException, true);
            var frame = st.GetFrame(0);
            var line = (frame != null) ? frame.GetFileLineNumber() : 0;

            if (pException.InnerException != null)
            {
                if (pException.InnerException.InnerException != null)
                    strMensagem = pException.InnerException.InnerException.Message;
                else
                    strMensagem = pException.InnerException.Message;
            }
            else
                strMensagem = pException.Message;

            if (pException.Message.Contains("Erro no documento XML"))
                strMensagem = strMensagem + " - " + pException.Message;
            //strMensagem += string.Format("  Erro na linha :{0}", line.ToString());
            strMensagem = string.Format("Erro na linha :{0} - ({1}) - {2}", line.ToString(), pMensagemComplementar, strMensagem);
            listaMensagens.Add(strMensagem);


            //if (pException is DbEntityValidationException)
            //{
            //    var dbEntityValidation = pException as DbEntityValidationException;

            //    foreach (var eve in dbEntityValidation.EntityValidationErrors)
            //    {
            //        strMensagem = string.Format("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:", eve.Entry.Entity.GetType().Name, eve.Entry.State);
            //        listaMensagens.Add(strMensagem);

            //        foreach (var ve in eve.ValidationErrors)
            //        {
            //            strMensagem = string.Format("- Property: \"{0}\", Error: \"{1}\"", ve.PropertyName, ve.ErrorMessage);
            //            listaMensagens.Add(strMensagem);
            //        }
            //    }
            //}

            if (pException is UserFriendlyException)
            {
                MensagemUsuario = pException.Message;
            }

            if (pException.InnerException != null)
            {
                listaMensagens.Add($"InnerException:  {pException.InnerException.Message} - Source: { pException.InnerException.TargetSite?.ReflectedType?.FullName}.{pException.InnerException?.TargetSite?.Name} - Linha : {new StackTrace(pException.InnerException, true).GetFrame(0).GetFileLineNumber()}");
            }

            strMensagem = string.Join("\r\n", listaMensagens);

            AdicionaLog(strMensagem, sistema, campo, metodo, pagina);

        }

        /// <summary>
        /// Adiciona uma mensagem na classe Resultado e preenche a propriedade MensagemUsuário com a mensagem da exception.
        /// </summary>
        /// <param name="pException"></param>
        /// <param name="pMensagemComplementar"></param>
        public void AdicionaLog(UserFriendlyException pException, string pMensagemComplementar = "")
        {
            AdicionaLog(pException as Exception, pMensagemComplementar);
        }

        #endregion




        #region Métodos Estáticos
        public static Resultado operator +(Resultado value1, Resultado value2)
        {
            Merge(value1, value2);
            return new Resultado((value1.Sucesso && value2.Sucesso), value1.Mensagens, value1.Id);
        }

        private static void Merge(Resultado value1, Resultado value2)
        {
            foreach (Mensagem mensagem in value2.Mensagens)
            {
                if (!value1.Mensagens.Exists(
                    delegate (Mensagem m)
                    {
                        return m.Descricoes.Exists(
                            delegate (string s)
                            {
                                return mensagem.Descricoes.Contains(s);
                            });
                    }))
                {
                    value1.Mensagens.Add(mensagem);
                }
            }
        }
        #endregion
    }
}
