using System;
using System.Collections.Generic;
using System.Linq;

namespace dform.NET
{
    using System.Diagnostics.Contracts;
    using System.Reflection;

    public enum DFORM_TYPE
    {
        Text,
        Number,
        Email,
        Div,
        Fieldset,
        Details,
        Telephone,
        Span,
        Checkbox,
        Checkboxes,
        Select,
        RadioButtons,
        Hidden,

        List
    }
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public class DFormField : Attribute
    {
        public string id;
        public string name;
        public string @class;
        protected string value;
        public string caption;
        public bool hasParent;
        public DFORM_TYPE type = DFORM_TYPE.Text;

        public Type Resource;

        [Pure]
        internal virtual string AsJSON(object actualField, PropertyInfo field, DFormSerializer serializer)
        {
            Contract.Requires(serializer != null);
            Contract.Ensures(Contract.Result<string>() != null);

            List<string> fields = new List<string>(3) { string.Format("\"type\":\"{0}\"", this.type.ToString().ToLower()) };
            fields.Add(
                !String.IsNullOrEmpty(this.id)
                    ? string.Format("\"id\":\"{0}\"", this.id)
                    : string.Format("\"id\":\"{0}\"", field.Name));

            var args = field.GetValue(actualField,null);
            fields.Add(string.Format("\"value\":\"{0}\"", args ?? ""));

            fields.Add(
                !String.IsNullOrEmpty(this.name)
                    ? string.Format("\"name\":\"{0}\"", this.name)
                    : string.Format("\"name\":\"{0}\"", actualField.GetType().Name + "." + field.Name));

            if (!String.IsNullOrEmpty(@class))
                fields.Add(string.Format("\"class\":\"{0}\"", @class));

            if (!String.IsNullOrEmpty(caption))
            {
                if (Resource != null)
                {
                    var resProp = Resource.GetProperty(caption);
                    if (resProp != null)
                    {
                        var localProp = resProp.GetValue(null, null);
                        if (localProp != null)
                            fields.Add(string.Format("\"caption\":\"{0}\"", localProp));
                    }
                    else
                    {
                        fields.Add(string.Format("\"caption\":\"{0}\"", caption));
                    }

                }
                else
                    fields.Add(string.Format("\"caption\":\"{0}\"", caption));
            }

            string toRet = fields.Aggregate((a, b) => a + "," + b);
           return toRet ?? "";
        }
    }
}
