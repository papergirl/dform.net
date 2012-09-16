namespace dform.NET
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using System.Reflection;
    using System.Linq;
    public sealed class DFormSerializer
    {
        private readonly object _toserial;
        private readonly PropertyInfo[] _serialized = new PropertyInfo[] { };
        private readonly DFormOptions _options;

        private DFormSerializer(object toSerial,DFormOptions options,PropertyInfo[] getFields)
        {
            Contract.Requires(getFields != null);
            _options = options;
            _toserial = toSerial;
            _serialized = getFields;
        }
        public static string serialize(object toSerialize)
        {
            return serialize(toSerialize, new DFormOptions());
        }

        public static string serialize(object toSerialize, DFormOptions opts)
        {
            Contract.Requires(toSerialize != null);
            Contract.Ensures(Contract.Result<string>() != null);

            PropertyInfo[] infos = toSerialize.GetType().GetProperties();
            DFormSerializer serializer = new DFormSerializer(toSerialize, opts = opts ?? new DFormOptions(), infos);
            List<string> fullySerialized = new List<string>();

            
            foreach (var field in infos)
            {
                foreach (var atti in field.GetCustomAttributes(typeof(DFormField), false))
                {
                    var at = (DFormField)atti;
                    if (!at.hasParent) //this will be serialized by anouther..
                        fullySerialized.Add(at.AsJSON(toSerialize,field,serializer));
                }
            }
            if (opts.HaveCancel)
                fullySerialized.Add(string.Format("{{\"type\":\"reset\",\"Value\":\"{0}\" }}", opts.CancelText));
            if (opts.HaveSubmit)
                fullySerialized.Add(string.Format("{{\"type\":\"submit\",\"Value\":\"{0}\" }}",opts.SubmitText));
            

            string toRet;

            var formAttri = toSerialize.GetType().GetCustomAttributes(typeof(DFormAttribute), false);
            if(formAttri.Length > 0)
            {
                var dform = (DFormAttribute)formAttri[0];
                toRet = String.Format(
                    "{{\"action\":\"{0}\",\"method\":\"{1}\",\"html\":[{2}]}}", dform.action, dform.method, fullySerialized);
            }
            else
            {
                toRet = string.Format("[{0}]",fullySerialized.Aggregate((s,i) => s + "," + i) );
            }

            return toRet;

        }

        internal string serializeNamedField(string containedField)
        {
            Contract.Assume(_serialized != null);
            string toRet = "";
            foreach (var fieldInfo in _serialized)
            {
                Contract.Assume(fieldInfo != null);
                if (fieldInfo.Name == containedField)
                {
                    foreach (var field in fieldInfo.GetCustomAttributes(typeof(DFormField), false))
                    {
                        var f = ((DFormField)field);
                        Contract.Assume(f != null);
                        Contract.Assert(f.hasParent);
                        toRet += f.AsJSON(_toserial, fieldInfo, this);
                    }


                }
            }
            return toRet;
        }
    }

    public class DFormOptions
    {
        public bool HaveSubmit = true;

        public string SubmitText = "Save";

        public bool HaveCancel = true;

        public string CancelText = "Cancel";
    }
}