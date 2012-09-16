// -----------------------------------------------------------------------
// <copyright file="dformSelectionAttribute.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace dform.NET
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    /// <summary>
    /// Used to tell the json serializer that this is a field to be serialized to an html select 
    /// </summary>
    public class DFormSelectionAttribute : DFormField
    {
        private readonly string _textField;
        private readonly string _valueField;
        /// <summary>
        /// If the select supports multiple options to be selected at once 
        /// </summary>
        public bool Multiple;
        /// <summary>
        /// The default value to be selected
        /// </summary>
        public object Selected;

        public DFormSelectionAttribute()
            : this(null, null)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="textField">the name of the field on the object to populate the text in the select</param>
        public DFormSelectionAttribute(string textField) : this(textField,textField)
        {
            
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="textField">The name of the field on the object to populate the text in the select</param>
        /// <param name="valueField">The name of the field on the object to populate the value in the select</param>
        public DFormSelectionAttribute(string textField, string valueField)
        {
            type = DFORM_TYPE.Select;
            _textField = textField;
            _valueField = valueField;
        }

        internal override string AsJSON(object actualField, PropertyInfo field, DFormSerializer serializer)
        {
            List<string> json = new List<string>();

            if (!(actualField is IEnumerable))
                throw new InvalidCastException("Cannot use an non enumrable type for a selection list");

            foreach (var option in actualField as IEnumerable)
            {
              
                Type optType = option.GetType();
                string val = option.ToString();
                string text = option.ToString();
                if (_textField != null)
                {
                    try
                    {
                        val = optType.GetProperty(_textField).GetValue(option, null).ToString();
                    }
                    catch (Exception)
                    {
                        val = option.ToString();
                    }
                }
                if (_valueField != null)
                {
                    try
                    {
                        text = optType.GetProperty(_valueField).GetValue(option, null).ToString();
                    }
                    catch (Exception)
                    {
                        text = option.ToString();
                    }
                }
                json.Add(string.Format("\"{0}\":\"{1}\"", text, val));
            }
            return string.Format("{{ {0},{1} \"options\":{{ {2} }} }}", base.AsJSON(actualField,field, serializer), Multiple ? "\"multiple\":\"multiple\"," : ""  
                ,json.Aggregate((s,i) => s + "," + i));
        }
    }
}
