// -----------------------------------------------------------------------
// <copyright file="dformAttribute.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace dform.NET
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using System.Linq;
    using System.Reflection;
    using System.Text;

    public enum HttpMethod
    {
        GET,
        POST
    }


    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class DFormAttribute : Attribute
    {
        public const string ENC_FORM_URL_ENCODED = "application/x-www-form-urlencoded";

        public const string ENC_FORM_DATA = "multipart/form-data";

        public const string ENC_PLAIN = "text/plain";

        public DFormAttribute(HttpMethod method,string action )
        {
            this.method = method;
            this.action = new Uri(action);
        }
        public HttpMethod method;
        public string id;
        public Uri action;
        public string @class;
        public string name;
        public string accept_charset;

        public bool autocomplete = true;
        public string enc_type = ENC_FORM_URL_ENCODED;

        public bool noValidate = false;
        public string target;

        public string title;
        public string lang;
        public string style;

        [Pure]
        internal virtual string AsJSON(object actualField, DFormSerializer serializer)
        {
            Contract.Requires(serializer != null);
            Contract.Ensures(Contract.Result<string>() != null);

            List<string> fields = new List<string>(3);
            fields.Add(
                !String.IsNullOrEmpty(this.id)
                    ? string.Format("\"id\":\"{0}\"", this.id)
                    : string.Format("\"id\":\"{0}\"", actualField.GetType().Name));

            fields.Add(
                !String.IsNullOrEmpty(this.name)
                    ? string.Format("\"name\":\"{0}\"", this.name)
                    : string.Format("\"name\":\"{0}\"", actualField.GetType().Name));

            if (!String.IsNullOrEmpty(@class))
                fields.Add(string.Format("\"class\":\"{0}\"", @class));

            if (!String.IsNullOrEmpty(style))
                fields.Add(string.Format("\"style\":\"{0}\"", style));

            if (!String.IsNullOrEmpty(lang))
                fields.Add(string.Format("\"lang\":\"{0}\"", lang));

            if (!String.IsNullOrEmpty(title))
                fields.Add(string.Format("\"title\":\"{0}\"", title));

            if (autocomplete)
                fields.Add(string.Format("\"autocomplete\":\"on\""));
            else
                fields.Add(string.Format("\"autocomplete\":\"off\""));

            if (noValidate)
                fields.Add(string.Format("\"novaildate\":\"novaildate\""));

            if (!String.IsNullOrEmpty(enc_type))
                fields.Add(string.Format("\"enc-type\":\"{0}\"", enc_type));

            if (!String.IsNullOrEmpty(accept_charset))
                fields.Add(string.Format("\"accept-charset\":\"{0}\"", accept_charset));

            if (!String.IsNullOrEmpty(target))
                fields.Add(string.Format("\"target\":\"{0}\"", target));

            string toRet = fields.Aggregate((a, b) => a + "," + b);
            return toRet ?? "";
        }
    }
}
