using System.Globalization;
using System.Reflection;

namespace Tecnica.Infra
{
    public static class Util
    {
        public static DateTime GetDate(string str)
        {
            str = str.Trim();

            if (str != "00000000" && str != string.Empty)
                return DateTime.ParseExact(str, "ddMMyyyy", new CultureInfo("pt-BR"));
            else
                return DateTime.MinValue;
        }

        public static DateTime GetDate(string str, string mascara)
        {
            if (str != "00000000")
                return DateTime.ParseExact(str, mascara, new CultureInfo("pt-BR"));
            else
                return DateTime.MinValue;
        }

        public static Decimal GetDecimal(string str, int index)
        {
            return Convert.ToDecimal(Convert.ToDecimal(str.Insert(index, ","), new CultureInfo("pt-BR")));
        }

        /// <summary>
        /// Pega uma substring onde o inicio da string é 1 e não zero.
        /// Facilita a utilização de layouts
        /// </summary>
        /// <param name="str"></param>
        /// <param name="start"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public static string Mid(this string str, int start, int? length = null)
        {
            if (!length.HasValue)
                return str.Substring(start - 1);
            else
                return str.Substring(start - 1, length.Value);
        }

        /// <summary>
        /// Sets a _private_ Property Value from a given Object. Uses Reflection.
        /// Throws a ArgumentOutOfRangeException if the Property is not found.
        /// </summary>
        /// <typeparam name="T">Type of the Property</typeparam>
        /// <param name="obj">Object from where the Property Value is set</param>
        /// <param name="propName">Propertyname as string.</param>
        /// <param name="val">Value to set.</param>
        /// <returns>PropertyValue</returns>
        public static void SetPrivatePropertyValue(object obj, string propName, object val)
        {
            Type t = obj.GetType();
            if (t.GetProperty(propName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance) == null)
                throw new ArgumentOutOfRangeException("propName", string.Format("Property {0} was not found in Type {1}", propName, obj.GetType().FullName));
            t.InvokeMember(propName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.SetProperty | BindingFlags.Instance, null, obj, new object[] { val });
        }

        public static void SetPrivateFieldValue(object obj, string fieldName, object val)
        {
            var field = obj.GetType().GetField(fieldName, BindingFlags.NonPublic | BindingFlags.Instance);
            field.SetValue(obj, val);
        }

        public static object GetPrivateFieldValue(object obj, string fieldName)
        {
            var field = obj.GetType().GetField(fieldName, BindingFlags.NonPublic | BindingFlags.Instance);
            return field.GetValue(obj);
        }

    }

    public class DomainMessage
    {
        public string Key { get; set; }

        public int Code { get; set; }

        public string UserMessage { get; set; }

        public string ProgrammerMessage { get; set; }

        public DomainMessage() { }

        public DomainMessage(int code, string programmerMessage, string userMessage = null)
        {
            Code = code;
            ProgrammerMessage = programmerMessage;
            UserMessage = userMessage;
        }


        public DomainMessage(string userMessage)
        {
            this.UserMessage = userMessage;
        }


        public DomainMessage(string key, string userMessage)
        {
            this.Key = key;
            this.UserMessage = userMessage;
        }
    }
}
