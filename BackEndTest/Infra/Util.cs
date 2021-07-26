using Microsoft.Extensions.Configuration;
using System;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace BackEndTest.Infra
{
    public class Util
    {
        /// <summary>
        /// Copia os valores das propriedades (de mesmo nome) de um objeto para outro.
        /// </summary>
        /// <param name="source"></param>
        /// <param name="destination"></param>
        public static void CopyPropertyValues(object source, object destination)
        {
            //só realiza a cópia se o source não vier nulo (caso de tabela cujo campo FK não foi preenchido)
            if (source != null)
            {
                var destProperties = destination.GetType().GetProperties();

                foreach (var sourceProperty in source.GetType().GetProperties())
                {
                    foreach (var destProperty in destProperties)
                    {
                        if (destProperty.Name == sourceProperty.Name && destProperty.PropertyType.IsAssignableFrom(sourceProperty.PropertyType))
                        {
                            destProperty.SetValue(destination, sourceProperty.GetValue(source, new object[] { }), new object[] { });
                            break;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Clona um objeto para retirar a referência de memória
        /// </summary>
        /// <param name="objRecebido"></param>
        /// <returns></returns>
        public static object ClonarObjeto(object objRecebido)
        {
            using (var ms = new System.IO.MemoryStream())
            {
                var bf = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();

                bf.Serialize(ms, objRecebido);
                ms.Position = 0;

                object obj = bf.Deserialize(ms);
                ms.Close();

                return obj;
            }
        }

        /// <summary>
        /// Valida o CPF
        /// </summary>
        /// <param name="vrCPF"></param>
        /// <returns></returns>
        public static bool ValidaCPF(string vrCPF)
        {
            string valor = vrCPF.Replace(".", "");
            valor = valor.Replace("-", "");

            if (valor.Length != 11)
                return false;

            bool igual = true;
            for (int i = 1; i < 11 && igual; i++)
                if (valor[i] != valor[0])
                    igual = false;

            if (igual || valor == "12345678901")
                return false;

            int[] numeros = new int[11];
            for (int i = 0; i < 11; i++)
                numeros[i] = int.Parse(
                valor[i].ToString());

            int soma = 0;
            for (int i = 0; i < 9; i++)
                soma += (10 - i) * numeros[i];

            int resultado = soma % 11;
            if (resultado == 1 || resultado == 0)
            {
                if (numeros[9] != 0)
                    return false;
            }
            else if (numeros[9] != 11 - resultado)
                return false;

            soma = 0;
            for (int i = 0; i < 10; i++)
                soma += (11 - i) * numeros[i];

            resultado = soma % 11;

            if (resultado == 1 || resultado == 0)
            {
                if (numeros[10] != 0)
                    return false;

            }
            else
                if (numeros[10] != 11 - resultado)
                return false;
            return true;

        }

        /// <summary>
        /// Valida o CNPJ
        /// </summary>
        /// <param name="vrCNPJ"></param>
        /// <returns></returns>
        public static bool ValidaCNPJ(string vrCNPJ)
        {

            string CNPJ = vrCNPJ.Replace(".", "");
            CNPJ = CNPJ.Replace("/", "");
            CNPJ = CNPJ.Replace("-", "");

            int[] digitos, soma, resultado;
            int nrDig;
            string ftmt;
            bool[] CNPJOk;

            ftmt = "6543298765432";
            digitos = new int[14];
            soma = new int[2];
            soma[0] = 0;
            soma[1] = 0;
            resultado = new int[2];
            resultado[0] = 0;
            resultado[1] = 0;
            CNPJOk = new bool[2];
            CNPJOk[0] = false;
            CNPJOk[1] = false;

            try
            {
                for (nrDig = 0; nrDig < 14; nrDig++)
                {
                    digitos[nrDig] = int.Parse(
                     CNPJ.Substring(nrDig, 1));
                    if (nrDig <= 11)
                        soma[0] += (digitos[nrDig] *
                        int.Parse(ftmt.Substring(
                          nrDig + 1, 1)));
                    if (nrDig <= 12)
                        soma[1] += (digitos[nrDig] *
                        int.Parse(ftmt.Substring(
                          nrDig, 1)));
                }

                for (nrDig = 0; nrDig < 2; nrDig++)
                {
                    resultado[nrDig] = (soma[nrDig] % 11);
                    if ((resultado[nrDig] == 0) || (resultado[nrDig] == 1))
                        CNPJOk[nrDig] = (
                        digitos[12 + nrDig] == 0);

                    else
                        CNPJOk[nrDig] = (
                        digitos[12 + nrDig] == (
                        11 - resultado[nrDig]));

                }

                return (CNPJOk[0] && CNPJOk[1]);
            }
            catch
            {
                return false;
            }

        }

        /// <summary>
        /// Formatar uma string CPF
        /// </summary>
        /// <param name="CPF">string CPF sem formatacao</param>
        /// <returns>string CPF formatada</returns>
        /// <example>Recebe '99999999999' Devolve '999.999.999-99'</example>
        public static string FormataCPFCNPJ(string cpfCnpj)
        {
            if (cpfCnpj.Count() > 11)
                cpfCnpj = FormataCNPJ(cpfCnpj);
            else
                cpfCnpj = FormataCPF(cpfCnpj);

            return cpfCnpj;
        }

        /// <summary>
        /// Formatar uma string CPF
        /// </summary>
        /// <param name="CPF">string CPF sem formatacao</param>
        /// <returns>string CPF formatada</returns>
        /// <example>Recebe '99999999999' Devolve '999.999.999-99'</example>
        public static string FormataCNPJ(string CNPJ)
        {
            string formatado = Convert.ToUInt64(CNPJ).ToString(@"00\.000\.000\/0000\-00");
            return formatado;
        }

        /// <summary>
        /// Formatar uma string CPF
        /// </summary>
        /// <param name="CPF">string CPF sem formatacao</param>
        /// <returns>string CPF formatada</returns>
        /// <example>Recebe '99999999999' Devolve '999.999.999-99'</example>
        public static string FormataCPF(string CPF)
        {
            string formatado = Convert.ToUInt64(CPF).ToString(@"000\.000\.000\-00");
            return formatado;
        }

        /// <summary>
        ///  Valida se o CNPJ-Base (8 primeiros dígitos) é o mesmo que o CNPJ-Base  do certificado
        /// </summary>
        /// <param name="pPath">Caminho do certificado</param>
        /// <param name="pSenha">Senha do certificado</param>
        /// <param name="">retorna true se o cnpj informado for o mesmo cnpj base do certificado</param>
        /// <returns></returns>
        public static bool ValidaCnpjBaseCertificado(string pPath, string pSenha, string pCnpj)
        {
            X509Certificate2 certificadoAtual = new X509Certificate2(pPath, pSenha);
            return ValidaCnpjBaseCertificado(certificadoAtual, pCnpj);
        }

        /// <summary>
        /// Valida se o CNPJ-Base (8 primeiros dígitos) é o mesmo que o CNPJ-Base  do certificado
        /// </summary>
        /// <param name="pCertificado"></param>
        /// <param name="pCNPJ"></param>
        /// <returns>retorna true se o cnpj informado for o mesmo cnpj base do certificado</returns>
        public static bool ValidaCnpjBaseCertificado(X509Certificate2 pCertificado, string pCNPJ)
        {
            string cnpj = string.Empty;
            foreach (X509Extension extension in pCertificado.Extensions)
            {
                string s1 = extension.Format(true);
                string[] lines = s1.Split(new char[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);
                for (int i = 0; i < lines.Length; i++)
                {
                    if (!lines[i].Trim().StartsWith("2.16.76.1.3.3")) continue;
                    string value = lines[i].Substring(lines[i].IndexOf('=') + 1);
                    string[] elements = value.Split(' ');
                    byte[] cnpjBytes = new byte[14];
                    for (int j = 0; j < cnpjBytes.Length; j++)
                        cnpjBytes[j] = Convert.ToByte(elements[j + 2], 16);
                    cnpj = Encoding.UTF8.GetString(cnpjBytes);
                    break;
                }
                if (!string.IsNullOrEmpty(cnpj)) break;
            }
            var isValido = cnpj.Substring(0, 8) == pCNPJ.Substring(0, 8);
            if (!isValido)
                GravarLog($"CNPJ {pCNPJ} inválido para o certificado escolhido. Certificado:  {pCertificado.SubjectName.Name} - CNPJ: {cnpj}", "ResolveNF");

            return isValido;
        }

        /// <summary>
        /// Remove acentos de um texto.
        /// </summary>
        /// <param name="texto"></param>
        /// <returns></returns>
        public static string RemoveAcentos(string texto)
        {
            const string StrComAcentos = "ÄÅÁÂÀÃäáâàãÉÊËÈéêëèÍÎÏÌíîïìÖÓÔÒÕöóôòõÜÚÛüúûùÇç'";
            const string StrSemAcentos = "AAAAAAaaaaaEEEEeeeeIIIIiiiiOOOOOoooooUUUuuuuCc ";
            string result = texto;
            int i = 0;

            foreach (Char c in StrComAcentos)
            {
                result = result.Replace(c.ToString().Trim(), StrSemAcentos[i].ToString().Trim());
                i++;
            }

            return result;
        }

        #region "Controle de Logs"

        /// <summary>
        /// Grava log de na pasta informada no Web.Config (LogPath)
        /// </summary>
        /// <param name="Mensagem"></param>
        /// <param name="Sistema"></param>
        /// <param name="pNomeFileLog"></param>
        public static void GravarLog(String Mensagem, String Sistema, string pNomeFileLog = "")
        {
            //string folderPath = @"C:\Temp\EasySup_Site"; // ConfigurationManager.AppSettings["LogPath"];
            string folderPath = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetSection("AppSettings")["LogPath"];
            string filePath = string.Empty;

            try
            {
                if (!Directory.Exists(folderPath))
                    Directory.CreateDirectory(folderPath);

                filePath = Path.Combine(folderPath, string.Format("{0}_{1}", pNomeFileLog, DateTime.Now.ToString("yyyyMM")) + ".log");

                using (StreamWriter sw = File.AppendText(filePath))
                {
                    sw.WriteLine(DateTime.Now.ToString() + " [" + Sistema + "] " + Mensagem);
                }
            }
            catch
            {
                //Erro ao escrever no log do arquivo filePath.
            }

            finally { }
        }

        /// <summary>
        /// Grava o log em um arquivo "Log.txt" e também em uma lista de Strings
        /// </summary>
        /// <param name="pArqlog">Nome do arquivo log a ser gravado</param>
        /// <param name="pMensagem">Texto de mensagem a ser gravado</param>
        /// <param name="pSistema">Nome do sistema</param>
        /// <param name="pPagina">Nome da classe</param>
        /// <param name="pMetodo">Metodo que executou</param>
        public static void GravarLog(string pArqlog, string pMensagem, string pSistema, string pPagina, string pMetodo)
        {
            string sistema = string.Format("{0} - {1} - {2}", pSistema, pPagina, pMetodo);

            GravarLog(pMensagem, sistema, pArqlog);
        }

        public static void GravarLog(Exception pException, string pArqlog, string pCampo = "", string pMensagemComplementar = "")
        {
            string sistema = string.Empty;
            string pagina = string.Empty;
            string metodo = string.Empty;
            string campo = pCampo;
            string strMensagem = string.Empty;

            if (!string.IsNullOrEmpty(pException.Source))
                sistema = pException.Source;

            if (pException.TargetSite != null && pException.TargetSite.ReflectedType != null && !string.IsNullOrEmpty(pException.TargetSite.ReflectedType.FullName))
                pagina = pException.TargetSite.ReflectedType.FullName;

            if (pException.TargetSite != null && pException.TargetSite.ReflectedType != null && !string.IsNullOrEmpty(pException.TargetSite.ReflectedType.Name))
                metodo = string.Format("{0}()", pException.TargetSite.Name);

            var st = new StackTrace(pException, true);
            var frame = st.GetFrame(0);
            var line = frame.GetFileLineNumber();

            if (pException.InnerException != null)
            {
                if (pException.InnerException.InnerException != null)
                    strMensagem = pException.InnerException.InnerException.Message;
                else
                    strMensagem = pException.Message.Contains("Erro no documento XML") ? string.Format("{0} - {1}", pException.Message, pException.InnerException.Message) : pException.InnerException.Message;
            }
            else
                strMensagem = pException.Message;

            //strMensagem += string.Format("  Erro na linha :{0}", line.ToString());
            strMensagem = string.Format("Erro na linha :{0} - {1} ", line.ToString(), strMensagem);

            if (!string.IsNullOrEmpty(pMensagemComplementar))
            {
                strMensagem = string.Format("{0} \r\n- Mensagem complementar: {1}", strMensagem, pMensagemComplementar);
            }

            sistema = string.Format("{0} | {1} | {2} | {3}", sistema, pagina, metodo, campo);

            Util.GravarLog(strMensagem, sistema, pArqlog);

        }

        //public static void GravaErroSql(string pConnectionString, string pMessageError, string pSistema, string pCampo, string pMetodo, string pPagina)
        //{
        //    string connString = string.Empty;
        //    List<string> parametros = new List<string>();

        //    try
        //    {
        //        pSistema = pSistema.IndexOf("|") > 0 ? pSistema.Substring(0, pSistema.IndexOf("|")) : pSistema.Length > 100 ? pSistema.Substring(0, 100) : pSistema;
        //        pPagina = pPagina.Length > 50 ? pPagina.Substring(pPagina.Length - 50) : pPagina;

        //        //connString = ConfigurationManager.ConnectionStrings["ConnectionStringMaster"].ConnectionString.Replace("=master", string.Format("={0}", "SHARK_EFD_PROC"));
        //        connString = pConnectionString;

        //        //Utilizando o "using" o metodo "dispose" e executado automaticamente no final.
        //        using (AdoHelper db = new AdoHelper(connString))
        //        {
        //            parametros.Clear();
        //            parametros.Add("@ProcID");
        //            parametros.Add("0");
        //            parametros.Add("@NAMEBASE");
        //            parametros.Add(pSistema);
        //            parametros.Add("@PROC_NAME");
        //            parametros.Add(pPagina);
        //            parametros.Add("@NM_CAMPO_ERRO");
        //            parametros.Add("");
        //            parametros.Add("@VL_CAMPO_ERRO");
        //            parametros.Add(pCampo);
        //            parametros.Add("@DS_CHAVE");
        //            parametros.Add("");
        //            parametros.Add("@CODIGO_ERRO");
        //            parametros.Add("0");
        //            parametros.Add("@DS_ERRO");
        //            parametros.Add(pMessageError);
        //            parametros.Add("@DS_ACAO");
        //            parametros.Add(pMetodo);
        //            parametros.Add("@EMAIL");
        //            parametros.Add("N");

        //            db.ExecNonQueryProc("dbo.sp_LogProcessError_Add", parametros.ToArray());

        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        GravarLog(ex, "", string.Format("GravaErroSql()", parametros.ToString()));

        //    }
        //}

        /// <summary>
        /// Verifica tamanho do arquivo Log e limpa para começar novamente.
        /// </summary>
        /// <param name="pArqLog">Path do arquivo de Log</param>
        /// <remarks></remarks>
        public static void VerificaArqLog(string pArqLog)
        {
            System.IO.StreamWriter wfile = null;
            wfile = null;
            string folderPath = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetSection("AppSettings")["LogPath"];
            string filePath = string.Empty;

            try
            {
                filePath = Path.Combine(folderPath, string.Format("{0}_{1}", pArqLog, DateTime.Now.ToString("yyyyMM")) + ".log");

                if (Directory.Exists(folderPath))
                {
                    //Verifia se arquivo existe
                    if (System.IO.File.Exists(filePath))
                    {
                        if (System.IO.File.ReadAllBytes(filePath).Length / 1024 > 10000)
                        {
                            //
                            //Sobrescreve o arquivo com conteúdo vazio
                            wfile = new System.IO.StreamWriter(filePath, false);
                            //
                        }
                    }
                }
                //
            }
            catch (Exception ex)
            {
                GravarLog(ex, "");
            }
            finally
            {
                if ((wfile != null))
                {
                    wfile.Close();
                }
            }
        }

        #endregion "Controle de Logs"

    }
}
