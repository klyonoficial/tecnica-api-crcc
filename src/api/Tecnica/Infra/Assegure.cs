using System.Text.RegularExpressions;

namespace Tecnica.Infra
{
    public class QueSe
    {
        bool expressaoQueSe;

        public QueSe(bool expressaoQueSe)
        {
            this.expressaoQueSe = expressaoQueSe;
        }



        public QueSe Entao(Func<bool> expressao, Func<string> message = null)
        {
            if (expressaoQueSe)
            {
                if (expressao() == false)
                    throw new DomainException(message());
            }

            return this;
        }

        public void EntaoFalhe(Func<string> message = null)
        {
            if (expressaoQueSe)
            {
                throw new DomainException(message());
            }
        }
    }

    public class Falhe
    {
        public static void Se(bool expressao, Func<string> messagem = null)
        {
            if (expressao)
            {
                throw new DomainException(messagem());
            }
        }

        public static void Quando(bool expressao, Func<string> messagem = null)
        {
            if (expressao)
            {
                throw new DomainException(messagem());
            }
        }
    }

    public class Assegure
    {
        public const string MensagemDeErroDefault = "Erro Interno";

        public static void AddContext(Func<object> contexto)
        {
            //var context = System.Web.HttpContext.Current;

            //try
            //{
            //    if (contexto != null && context != null)
            //    {
            //        context.Request.ServerVariables["X-LOG-JSON-DATA"] =

            //     JsonConvert.SerializeObject(contexto()
            //                , Newtonsoft.Json.Formatting.Indented);
            //    }

            //}
            //catch (Exception ex)
            //{

            //    ErrorLog.Default?.Log(new Error(ex));

            //}
        }

        public static QueSe QueSe(bool expressao)
        {
            return new QueSe(expressao);
        }

        public static void Que(bool expressao, Func<List<string>> messages = null)
        {
            if (expressao == false)
            {
                throw new DomainException(messages());
            }
        }
        public static void Que(bool expressao, Func<List<string>> messages = null , Func<List<string>> messages2 = null)
        {
            if (expressao == false)
            {
                throw new DomainException(messages());
            }
        } 

        public static void Que(bool expressao, Func<string> messagem = null, Func<object> contexto = null)
        {
            if (expressao == false)
            {
                AddContext(contexto);

                throw new DomainException(messagem(), contexto?.Invoke());
            }
        }

        public static void Que(bool expressao, string message = null)
        {
            if (expressao == false)
                throw new DomainException(message);
        }
        public static void Que(bool expressao, Func<DomainMessage> messagem = null)
        {
            if (expressao == false)
                throw new DomainException(messagem());
        }

        public static void Que(bool expressao, Func<string> messagem = null)
        {
            if (expressao == false)
            {
                throw new DomainException(messagem());
            }
        }

        public static void QueNao(bool expressao, string message = null)
        {
            if (expressao)
                throw new DomainException(message);
        }





        public static void NaoNulo(object obj, string message = null)
        {
            if (obj is string)
            {
                if (string.IsNullOrWhiteSpace(obj.ToString()))
                    throw new DomainException(message);
            }

            if (obj == null)
                throw new DomainException(message);
        }


        public static void NaoNulo(
              object obj
            , string campo = null
            , string mensagem = null
            , string objetoPai = null
            , int? posicao = null
            )
        {

            string texto = RetornarMensagemValidacao(
                "é obrigatório."
                , campo
                , mensagem
                , objetoPai
                , posicao);



            if (obj == null)
                throw new DomainException(texto);

            if (obj is string && string.IsNullOrWhiteSpace(obj.ToString()))
                throw new DomainException(texto);

        }

        public static void Que(
              object obj
            , string campo = null
            , string mensagem = null
            , string objetoPai = null
            , int? posicao = null
            )
        {

            string texto = RetornarMensagemValidacao(
                "é obrigatório."
                , campo
                , mensagem
                , objetoPai
                , posicao);



            if (obj == null)
                throw new DomainException(texto);

            if (obj is string && string.IsNullOrWhiteSpace(obj.ToString()))
                throw new DomainException(texto);

        }

        private static string RetornarMensagemValidacao(
              string sufixoMensagem
            , string campo
            , string mensagem
            , string objetoPai
            , int? posicao
            )
        {

            sufixoMensagem = sufixoMensagem?.Trim();

            campo = campo?.Trim();

            mensagem = mensagem?.Trim();

            objetoPai = objetoPai?.Trim();


            var result = string.Empty;


            if (objetoPai != null)
                result += objetoPai;

            if ((objetoPai != null) && (posicao != null))
                result += " " + posicao.ToString();

            if (objetoPai != null)
                result += " >";

            if (campo != null)
                result += " " + campo;

            if (sufixoMensagem != null)
                result += " " + sufixoMensagem;

            if (mensagem != null)
                result += " " + mensagem;


            return result.Trim();
        }


        public static void Status(string obj, string message = null)
        {
            if (obj == null)
                throw new DomainException("Status não informado");

            if (obj != "ATIV" && obj != "INAT")
                throw new DomainException("Status inválido: deve ser 'ATIV' ou 'INAT' ");

        }

        public static void NaoNulo(object obj, Func<string> message = null)
        {
            if (obj is string)
            {
                if (string.IsNullOrWhiteSpace(obj.ToString()))
                    throw new DomainException(message());
            }

            if (obj == null)
                throw new DomainException(message());
        }




        public static void Regex(string pattern, string input, string message = null)
        {
            var reg = new Regex(pattern);

            if (!reg.IsMatch(input))
                throw new DomainException(message);
        }


    }
}
