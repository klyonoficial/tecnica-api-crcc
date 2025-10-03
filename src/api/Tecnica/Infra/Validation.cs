using System.Text;
using System.Text.RegularExpressions;
using System.Globalization;
using System.Xml.Linq;

namespace Tecnica.Infra
{
    public static class Validation
    {

        public static bool IsValidCPF(string cpf)
        {
            if (String.IsNullOrEmpty(cpf))
            {
                return false;
            }

            // Limpa a string
            cpf = cpf.Replace(".", "").Replace("-", "").Replace("/", "").Replace("\\", "").Trim();

            // Se o tamanho for < 11 entao retorna como inv�lido
            if (cpf.Length != 11)
            {
                return false;
            }

            int d1, d2;
            int soma = 0;
            string digitado = "";
            string calculado = "";

            // Pesos para calcular o primeiro digito
            int[] peso1 = new int[] { 10, 9, 8, 7, 6, 5, 4, 3, 2 };

            // Pesos para calcular o segundo digito
            int[] peso2 = new int[] { 11, 10, 9, 8, 7, 6, 5, 4, 3, 2 };

            int[] n = new int[11];

            bool retorno = false;

            // Caso coloque todos os numeros iguais
            switch (cpf)
            {
                case "11111111111":
                    return false;
                case "00000000000":
                    return false;
                case "22222222222":
                    return false;
                case "33333333333":
                    return false;
                case "44444444444":
                    return false;
                case "55555555555":
                    return false;
                case "66666666666":
                    return false;
                case "77777777777":
                    return false;
                case "88888888888":
                    return false;
                case "99999999999":
                    return false;
            }

            try
            {
                // Quebra cada digito do CPF
                n[0] = Convert.ToInt32(cpf.Substring(0, 1));
                n[1] = Convert.ToInt32(cpf.Substring(1, 1));
                n[2] = Convert.ToInt32(cpf.Substring(2, 1));
                n[3] = Convert.ToInt32(cpf.Substring(3, 1));
                n[4] = Convert.ToInt32(cpf.Substring(4, 1));
                n[5] = Convert.ToInt32(cpf.Substring(5, 1));
                n[6] = Convert.ToInt32(cpf.Substring(6, 1));
                n[7] = Convert.ToInt32(cpf.Substring(7, 1));
                n[8] = Convert.ToInt32(cpf.Substring(8, 1));
                n[9] = Convert.ToInt32(cpf.Substring(9, 1));
                n[10] = Convert.ToInt32(cpf.Substring(10, 1));
            }
            catch
            {
                return false;
            }


            // Calcula cada digito com seu respectivo peso
            for (int i = 0; i <= peso1.GetUpperBound(0); i++)
            {
                soma += (peso1[i] * Convert.ToInt32(n[i]));
            }

            // Pega o resto da divisao
            int resto = soma % 11;

            if (resto == 1 || resto == 0)
            {
                d1 = 0;
            }
            else
            {
                d1 = 11 - resto;
            }

            soma = 0;

            // Calcula cada digito com seu respectivo peso
            for (int i = 0; i <= peso2.GetUpperBound(0); i++)
            {
                soma += (peso2[i] * Convert.ToInt32(n[i]));
            }

            // Pega o resto da divisao
            resto = soma % 11;

            if (resto == 1 || resto == 0)
            {
                d2 = 0;
            }
            else
            {
                d2 = 11 - resto;
            }

            calculado = d1.ToString() + d2.ToString();
            digitado = n[9].ToString() + n[10].ToString();

            // Se os ultimos dois digitos calculados bater com
            // os dois ultimos digitos do cpf entao � v�lido
            if (calculado == digitado)
            {
                retorno = true;
            }
            else
            {
                retorno = false;
            }

            return retorno;

        }

        public static bool IsValidCNPJ(string cnpj)
        {
            if (Validation.EstaVazio(cnpj))
            {
                return false;
            }


            cnpj = cnpj.Replace(".", "").Replace("-", "").Replace("/", "").Replace("\\", "").Trim();

            if (cnpj.Length != 14) { return false; }

            if (cnpj == "00000000000000") { return false; }

            string l, inx, dig;
            int s1, s2, i, d1, d2, v, m1, m2;
            inx = cnpj.Substring(12, 2);
            cnpj = cnpj.Substring(0, 12);
            s1 = 0;
            s2 = 0;
            m2 = 2;
            for (i = 11; i >= 0; i--)
            {
                l = cnpj.Substring(i, 1);
                v = Convert.ToInt16(l);
                m1 = m2;
                m2 = m2 < 9 ? m2 + 1 : 2;
                s1 += v * m1;
                s2 += v * m2;
            }
            s1 %= 11;
            d1 = s1 < 2 ? 0 : 11 - s1;
            s2 = (s2 + 2 * d1) % 11;
            d2 = s2 < 2 ? 0 : 11 - s2;
            dig = d1.ToString() + d2.ToString();

            return (inx == dig);

        }

        public static bool IsValidCpfCnpj(string documento)
        {
            if (String.IsNullOrEmpty(documento))
                return false;

            documento = documento.Replace(".", "").Replace("-", "").Replace("/", "").Replace("\\", "").Trim();

            if (documento.Length == 11)
                return Validation.IsValidCPF(documento);
            else
                return Validation.IsValidCNPJ(documento);
        }
         

        public static bool IsValidSequenciaNumeros(string sequencia, int quantidade)
        {
            System.Text.RegularExpressions.Regex r = new System.Text.RegularExpressions.Regex("^[0-9]{" + quantidade + "}$");

            return r.IsMatch(sequencia);
        }


        //    '-----------------------------------------------------------------'
        //    '                  Regra de composi��o do CMC7                    '
        //    '                                                                 '
        //    '                                                                 '
        //    ' N�mero composto por 30 d�gitos, descritos da seguinte forma :   '
        //    '                                                                 '
        //    '   |341|0004|8|018|000001|5|0|5000000004|7|                      '
        //    '     1    2  3  4    5    6 7      8     9                       '
        //    '                                                                 '
        //    ' 1 = c�digo compensa��o do banco (3 d�gitos)                     '
        //    ' 2 = n�mero da ag�ncia (4 d�gitos)                               '
        //    ' 3 = d�gito m�dulo 10 dos seguintes campos :                     '
        //    '     c�d.c�mara compensa��o (3 d�gitos)                          '
        //    '     n�m.do cheque (6 d�gitos)                                   '
        //    '     c�d.tipifica��o do documento (1 d�gito)                     '
        //    ' 4 = c�digo da c�mara compensa��o (3 d�gitos)                    '
        //    ' 5 = n�mero do cheque (6 d�gitos)                                '
        //    ' 6 = c�digo tipifica��o do documento (1 d�gito)                  '
        //    '     5 : cheque comum                                            '
        //    '     6 : cheque ordem de pagamento                               '
        //    '     7 : cheque viagem                                           '
        //    '     8 : cheque administrativo                                   '
        //    ' 7 = d�gito m�dulo 10 dos seguintes campos :                     '
        //    '     c�d.banco (3 d�gitos)                                       '
        //    '     c�d.ag�ncia (4 d�gitos)                                     '
        //    ' 8 = n�mero da conta corrente (com d�gito da conta) (10 d�gitos) '
        //    ' 9 = d�gito m�dulo 10 do campo n�mero da cta.corrente            '
        //    '-----------------------------------------------------------------'
        //=============================================================
        public static bool IsValidCMC7(string cmc7)
        {
            if (cmc7.Length != 30)
            {
                return false;
            }

            string numCalc;
            string digRec1;
            string digRec2;
            string digRec3;
            string digCalc1;
            string digCalc2;
            string digCalc3;
            string cmc7Calc;

            numCalc = cmc7.Substring(8, 10);
            digRec1 = cmc7.Substring(7, 1);
            digCalc1 = CalcMod10(numCalc);

            numCalc = "000" + cmc7.Substring(0, 7);
            digRec2 = cmc7.Substring(18, 1);
            digCalc2 = CalcMod10(numCalc);

            numCalc = cmc7.Substring(19, 10);
            digRec3 = cmc7.Substring(29, 1);
            digCalc3 = CalcMod10(numCalc);

            cmc7Calc =
                cmc7.Substring(0, 7)
                + digCalc1
                + cmc7.Substring(8, 10)
                + digCalc2
                + cmc7.Substring(19, 10)
                + digCalc3;

            return (cmc7 == cmc7Calc);
        }

        public static string CalcMod10(string num)
        {
            int tam = num.Length;
            int soma = 0;
            int resto = 0;
            int temp = 0;
            bool pos = true;

            for (int i = 0; i < tam; i++)
            {
                temp = int.Parse(num.Substring(i, 1));
                if (pos)
                {
                    pos = false;
                    soma += temp;
                }
                else
                {
                    pos = true;
                    if (temp > 4)
                    {
                        soma += (temp * 2) - 9;
                    }
                    else
                    {
                        soma += (temp * 2);
                    }
                }
            }

            resto = soma % 10;
            if (resto == 0)
            {
                return "0";
            }
            else
            {
                return string.Format("{0}", (10 - resto));
            }
        }

        public static bool IsValidEmail(string email)
        {
            if (String.IsNullOrEmpty(email))
                return false;

            Regex rg = new Regex(@"^[A-Za-z0-9_\+-]+(\.[A-Za-z0-9_\+-]+)*@[A-Za-z0-9-]+(\.[A-Za-z0-9-]+)*\.([A-Za-z]{2,4})$");

            return rg.IsMatch(email);
        }


        public static bool IsValidPis(string pis)
        {
            int[] multiplicador = new int[10] { 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };
            int soma;
            int resto;

            if (pis.Trim().Length != 11)
                return false;

            pis = pis.Trim();
            pis = pis.Replace("-", "").Replace(".", "").PadLeft(11, '0');


            soma = 0;
            for (int i = 0; i < 10; i++)
                soma += int.Parse(pis[i].ToString()) * multiplicador[i];

            resto = soma % 11;

            if (resto < 2)
                resto = 0;
            else
                resto = 11 - resto;

            return pis.EndsWith(resto.ToString());
        }


        /// <summary>
        /// Valida um Cep sem Mascara
        /// </summary>
        /// <param name="cep"></param>
        /// <returns></returns>
        public static bool IsValidCep(string cep)
        {
            if (Validation.EstaVazio(cep))
            {
                return false;
            }

            Regex rg = new Regex(@"^[0-9]{8}$");

            return rg.IsMatch(cep);
        }

        /// <summary>
        /// Valida uma Data no formato dd/MM/yyyy
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static bool IsValidDate(this string data)
        {
            if (string.IsNullOrWhiteSpace(data))
            {
                return false;
            }

            if (!new Regex(@"^[0-9]{2}/[0-9]{2}/[0-9]{4}$").IsMatch(data))
            {
                return false;
            }

            DateTime dataAuxiliar;

            return DateTime.TryParseExact(
                data
                , "dd/MM/yyyy"
                , CultureInfo.GetCultureInfo("pt-BR")
                , DateTimeStyles.None
                , out dataAuxiliar);

        }
        public static bool IsValidDateInvertida(this string data)
        {
            if (string.IsNullOrWhiteSpace(data))
            {
                return false;
            }

            if (!new Regex(@"^[0-9]{4}/[0-9]{2}/[0-9]{2}$").IsMatch(data))
            {
                return false;
            }

            DateTime dataAuxiliar;

            return DateTime.TryParseExact(
                data
                , "yyyy/MM/dd"
                , CultureInfo.GetCultureInfo("pt-BR")
                , DateTimeStyles.None
                , out dataAuxiliar);

        }


        /// <summary>
        /// Se o valor � null ou string.empty
        /// </summary>
        /// <param name="valor"></param>
        /// <param name="valorDefault"></param>
        /// <returns></returns>
        public static string OuDefault(this string valor, string valorDefault)
        {
            return valor.PossuiValor() ? valor : valorDefault;
        }


        public static bool PossuiValor(this string? valor)
        {
            return !string.IsNullOrWhiteSpace(valor);
        }

        public static bool NaoPossuiValor(this string valor)
        {
            return !PossuiValor(valor);
        }

        public static bool EstaVazio(this string valor)
        {
            return !PossuiValor(valor);
        }


        public static bool IsValidPlacaDeAutomovel(this string placa)
        {
            return new Regex("[a-zA-Z]{3}[0-9]{4}").IsMatch(placa);
        }


        public static bool IsValidDecimal(this string valor)
        {
            decimal numero = 0;

            return decimal.TryParse(valor, out numero);
        }

        public static bool EhNumeroInt(this string data)
        {
            int inteiro;

            return int.TryParse(data, out inteiro);
        }




        public static DateTime? ParseData(string data)
        {
            if (Validation.IsValidDate(data))
            {
                return Convert.ToDateTime(data);
            }

            else
            {
                return null;
            }
        }


        /// <summary>
        /// Fonte : http://www.blackbeltcoder.com/Articles/ecommerce/validating-credit-card-numbers
        /// </summary>
        /// <param name="cardNumber"></param>
        /// <returns></returns>
        public static bool IsValidCartaoDeCredito(string cardNumber)
        {

            if (!new Regex(@"^\d+$").IsMatch(cardNumber))
            {
                return false;
            }

            int i, checkSum = 0;

            // Compute checksum of every other digit starting from right-most digit
            for (i = cardNumber.Length - 1; i >= 0; i -= 2)
                checkSum += (cardNumber[i] - '0');

            // Now take digits not included in first checksum, multiple by two,
            // and compute checksum of resulting digits
            for (i = cardNumber.Length - 2; i >= 0; i -= 2)
            {
                int val = ((cardNumber[i] - '0') * 2);
                while (val > 0)
                {
                    checkSum += (val % 10);
                    val /= 10;
                }
            }

            // Number is valid if sum of both checksums MOD 10 equals 0
            return ((checkSum % 10) == 0);
        }


        public static bool IsCpf(this string documento)
        {
            return Validation.IsValidCPF(documento);
        }


        public static bool IsCnpj(this string documento)
        {
            return (documento.Trim().Length == 14);
        }




        /// <summary>
        /// Valida hora e minuto no formato HH:MM .
        /// Ex.: 15:00 ; 04:30
        /// </summary>
        /// <param name="hora"></param>
        /// <returns></returns>
        public static bool IsValidHora(string hora)
        {
            return new Regex("^([0-9]|0[0-9]|1[0-9]|2[0-3]):[0-5][0-9]$").IsMatch(hora);
        }



        public static bool IsValidNumeroTelefone(string numero)
        {
            if (Validation.EstaVazio(numero))
            {
                return false;
            }

            //numero = numero.RemoveMask();

            return numero.Length == 8 || numero.Length == 9;
        }


        public static bool IsValidDDDTelefone(string numero)
        {
            if (Validation.EstaVazio(numero))
            {
                return false;
            }

            //numero = numero.RemoveMask();

            return numero.Length == 2;
        } 



        public static string ToDateTimeInicial(this string s)
        {
            return string.Format("{0} 00:00:00", s);
        }

        public static string ToDateTimeFinal(this string s)
        {
            return string.Format("{0} 23:59:59", s);
        }
         
        /// <summary>
        /// Retorna os ultimos caracteres de uma string
        /// </summary>
        /// <param name="s"></param>
        /// <param name="lenght">Quantidade no final da string</param>
        /// <returns></returns>
        public static string LastString(this string s, int lenght)
        {
            return s.Substring(s.Length - lenght, lenght);
        }

        /// <summary>
        /// Adiciona zero na frente de  um número
        /// </summary>
        /// <param name="s"></param>
        /// <param name="lenghtOfNumbers">quantidade de numeros que a string deverá ter</param>
        /// <returns></returns>
        public static string AddZerosToFront(this string s, int lenghtOfNumbers)
        {
            string zeros = "";

            for (int i = 0; i < lenghtOfNumbers - s.Length; i++)
            {
                zeros += "0";
            }

            return s.Insert(0, zeros);
        }
          

        public static int ToIntMonth(this string i)
        {
            switch (i)
            {
                case "Janeiro":
                    return 1;
                case "Fevereiro":
                    return 2;
                case "Março":
                    return 3;
                case "Abril":
                    return 4;
                case "Maio":
                    return 5;
                case "Junho":
                    return 6;
                case "Julho":
                    return 7;
                case "Agosto":
                    return 8;
                case "Setembro":
                    return 9;
                case "Outubro":
                    return 10;
                case "Novembro":
                    return 11;
                case "Dezembro":
                    return 12;
                default:
                    throw new Exception("Valor inválido");
            }
        }

        public static void ColocarAspas(this List<string> lista)
        {
            for (int i = 0; i < lista.Count; i++)
            {
                lista[i] = string.Format("'{0}'", lista[i]);
            }
        }

        public static string ToMultiLineWeb(this List<string> lista)
        {
            return string.Join("<br/>", lista.ToArray());
        }




        /// <summary>
        /// Troca acentos por caracteres normais
        /// Troca os caracteres especiais por " " a não ser .;,_- e " " e \n e \r 
        /// </summary>
        /// <param name="self"></param>
        /// <returns></returns>
        public static String RemoverCaracteresEspeciais(this String self)
        {
            if (self == null)
            {
                return null;
            }


            var normalizedString = self;

            normalizedString = Regex.Replace(normalizedString, "[âáàã]", "a");
            normalizedString = Regex.Replace(normalizedString, "[ÂÀÁÃ]", "A");
            normalizedString = Regex.Replace(normalizedString, "[êèé]", "e");
            normalizedString = Regex.Replace(normalizedString, "[ÊÈÉ]", "E");
            normalizedString = Regex.Replace(normalizedString, "[îìí]", "i");
            normalizedString = Regex.Replace(normalizedString, "[ÎÌÍ]", "I");
            normalizedString = Regex.Replace(normalizedString, "[õôòó]", "o");
            normalizedString = Regex.Replace(normalizedString, "[ÕÔÒÓ]", "O");
            normalizedString = Regex.Replace(normalizedString, "[üûúù]", "u");
            normalizedString = Regex.Replace(normalizedString, "[ÜÛÚÙ]", "U");
            normalizedString = Regex.Replace(normalizedString, "[ç]", "c");
            normalizedString = Regex.Replace(normalizedString, "[Ç]", "C");



            // Remove os outros caracteres especiais.
            var caracteresPermitidos = "[^0-9a-zA-Z._,;\n\r -]+?";
            normalizedString = Regex.Replace(normalizedString, caracteresPermitidos, " ");

            return normalizedString.Trim();
        }
       

        public static string MaxString(this string str, int quantidadeMaxima)
        {


            if (str.Length > quantidadeMaxima)
            {
                return str.Substring(0, quantidadeMaxima);
            }
            else
            {
                return str;
            }

        }

        public static string SplitMaiusculas(this string str)
        {
            string resposta = string.Empty;

            var reg = new Regex("[A-Z]");

            for (int i = 0; i < str.Length; i++)
            {
                if (reg.IsMatch(str[i].ToString()) && i != 0)
                {
                    resposta += " " + str[i].ToString();

                }
                else
                {
                    resposta += str[i].ToString();
                }
            }


            return resposta;
        }

        public static Dictionary<string, string> XmlToDictionary(this string xml)
        {

            XElement element = XElement.Load(xml);

            Dictionary<string, string> dicionario = (from no in element.Descendants()
                                                     select new { key = no.Name.LocalName, value = no.Value })
                                                           .ToDictionary(y => y.key, y => y.value);

            return dicionario;

        }


        /// <summary>
        /// Removes control characters and other non-UTF-8 characters
        /// </summary>
        /// <param name="inString">The string to process</param>
        /// <returns>A string with no control characters or entities above 0x00FD</returns>
        public static string RemoveTroublesomeCharacters(string inString)
        {
            if (inString == null) return null;

            StringBuilder newString = new StringBuilder();

            try
            {

                char ch;

                for (int i = 0; i < inString.Length; i++)
                {

                    ch = inString[i];
                    // remove any characters outside the valid UTF-8 range as well as all control characters
                    // except tabs and new lines
                    if ((ch < 0x00FD && ch > 0x001F) || ch == '\t' || ch == '\n' || ch == '\r')
                    {
                        newString.Append(ch);
                    }
                }
                return newString.ToString();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                newString.Clear();
            }


        }



        /// <summary>
        /// Removes control characters and other non-UTF-8 characters
        /// </summary>
        /// <param name="inString">The string to process</param>
        /// <returns>A string with no control characters or entities above 0x00FD</returns>
        public static string ReplaceTroubleCharacters(string inString, char with)
        {
            return Replace(inString, with);
        }


        public static void Clear(this StringBuilder str)
        {
            str.Remove(0, str.Length - 1);
        }

        /// <summary>
        /// Retornar o Valor da String passada se houver valor.
        /// Se não, retorna null
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string Null_IF_Empty(this string str)
        {
            return Validation.PossuiValor(str) ? str : null;
        }

        public static void Add(this List<string> lista, string format, params object[] args)
        {
            lista.Add(string.Format(format, args));
        }

        public static List<int> ToListOfInts(this string value)
        {
            if (!Validation.PossuiValor(value))
            {
                return new List<int>();
            }

            value = value.Trim();




            if (value.Split('\n').Count() > 1)
            {
                return value
              .Split('\n')
              .ToList()
              .ConvertAll<int>(p => Convert.ToInt32(p.Trim()));
            }



            return value
            .Split(',')
            .ToList()
            .ConvertAll<int>(p =>
            {
                int codigo = 0;

                if (!int.TryParse(p.Trim(), out codigo))
                {
                    throw new DomainException(String.Format("O código {0} não está em um formato válido", p));
                }

                return codigo;
            }

                );

        }


        /// <summary>
        /// Tranforma uma string contendo:
        /// , (vírgula)
        /// ; (ponto e vírgula)
        /// \n (quebra de linha)
        /// em um array de strings
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static List<string> ToListOfStrings(this string value)
        {
            if (value.EhVazio())
            {
                return new List<string>();
            }

            value = value
                .Trim()
                .Replace(";", ",")
                .Replace('\n', ',');


            return value
            .Split(',')
            .Select(x => x.Trim())
            .Where(x => x.Length > 0)
            .ToList();

        }


        public static bool EhVazio(this string? str)
        {
            return !Validation.PossuiValor(str);
        }


        public static string? Empty_if_null(this string? str)
        {
            return Validation.PossuiValor(str) ? str : string.Empty;
        }


        public static string? Value_if_null(this string? str, string? value)
        {
            return Validation.PossuiValor(str) ? str : value;
        }

        public static string? ReplaceWithRegex(this string? str, string? pattern, string? with = "")
        {
            return Regex.Replace(str, pattern, with);
        }

        public static string? DeveTerTamanho(this string? str, int tamanhoExato, Func<string?> mensagem = null)
        {
            Assegure.NaoNulo(str, $"{mensagem()}. Valor null");

            Assegure.Que(str.Length == tamanhoExato,
                () => $"{mensagem()}. {str.Length} caracteres, quando deveria ter {tamanhoExato}. Valor: #{str}#");

            return str;
        }


        public static string? Replace(string? inString, char with)
        {
            if (inString == null) return null;

            var newString = new StringBuilder();

            try
            {

                char ch;

                for (int i = 0; i < inString.Length; i++)
                {

                    ch = inString[i];
                    // remove any characters outside the valid UTF-8 range as well as all control characters
                    // except tabs and new lines
                    if ((ch < 0x00FD && ch > 0x001F) || ch == '\t' || ch == '\n' || ch == '\r')
                    {
                        newString.Append(ch);
                    }
                    else
                    {
                        newString.Append(with);
                    }
                }
                return newString.ToString();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                newString.Clear();
            }


        }

        public static bool EhCpf(this string documento)
        {
            return Validation.IsValidCPF(documento);
        }
        public static bool EhCpfCnpj(this string documento)
        {
            return Validation.IsValidCPF(documento) ||
                Validation.IsValidCNPJ(documento)           ;
        }


        public static bool EhCnpj(this string documento)
        {
            return Validation.IsValidCNPJ(documento);
        }


        public static bool EhEmail(this string value)
        {
            return Validation.IsValidEmail(value);
        }


        public static void Obrigatorio(
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

        public static void AssegurarCpfCnpj(
           string documento
           , string campo = null
           , string objetoPai = null
           , int? posicao = null
           )
        {

            string sufixoMensagem = "Inválido.";

            if (documento == null)
                documento = string.Empty;

            if (campo == null)
                campo = "Documento";

            string mensagem = RetornarMensagemValidacao(
           sufixoMensagem
           , campo
           , null
           , objetoPai
           , posicao);


            Validation.AssegurarQue(Validation.IsValidCpfCnpj(documento), mensagem: mensagem);


        }


        public static void AssegurarCNPJ(
           string documento
           , string campo = null
           , string objetoPai = null
           , int? posicao = null
           )
        {

            string sufixoMensagem = "Inválido.";

            if (documento == null)
                documento = string.Empty;

            if (campo == null)
                campo = "Documento";

            string mensagem = RetornarMensagemValidacao(
           sufixoMensagem
           , campo
           , null
           , objetoPai
           , posicao);


            Validation.AssegurarQue(Validation.IsValidCNPJ(documento), mensagem: mensagem);


        }



        public static void AssegurarQue(
            bool condicao
            , string campo = null
            , string mensagem = null
            , string objetoPai = null
            , int? posicao = null
            )
        {

            string texto = RetornarMensagemValidacao(
              null
              , campo
              , mensagem
              , objetoPai
              , posicao);


            if (condicao == false)
                throw new DomainException(texto);


        }

    }
}