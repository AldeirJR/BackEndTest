using System.Collections.Generic;

namespace BackEndTest.Domain.Models
{
    public class Mensagem
    {
        #region Propriedades
        /// <summary>
        /// Nome do campo a ser validado
        /// </summary>
        public string Campo { get; set; }

        /// <summary>
        /// Lista de menssagens de validação
        /// </summary>
        public List<string> Descricoes { get; set; }
        /// <summary>
        /// Nome do método que realizaou a chamada
        /// </summary>
        public string Metodo { get; set; }
        /// <summary>
        /// Nome da classe que realizaou a chamada
        /// </summary>
        public string Pagina { get; set; }

        #endregion

        #region Construtores
        /// <summary>
        /// Construtor da classe.
        /// </summary>
        public Mensagem()
        {
            this.Descricoes = new List<string>();
            this.Campo = string.Empty;
        }
        #endregion

        #region Métodos Estáticos
        public static Mensagem Cria(string campo, string descricao)
        {
            Mensagem m = new Mensagem();
            m.Campo = campo;
            m.Descricoes.Add(descricao);

            return m;
        }
        #endregion
    }
}
