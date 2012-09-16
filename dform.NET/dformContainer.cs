// -----------------------------------------------------------------------
// <copyright file="dformContainer.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace dform.NET
{
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using System.Linq;
    using System.Reflection;
    using System.Text;


    public class DFormContainer : DFormField
    {
        public string[] containedFields = new string[]{};

        public DFormContainer(string[] fields)
        {
            type = DFORM_TYPE.Div;
            if (fields == null)
                containedFields = new string[]{};
            containedFields = fields;
        }

        internal override string AsJSON(object actualField, PropertyInfo field, DFormSerializer serializer)
        {
            Contract.Assume(containedFields != null);
            StringBuilder generatedJSON = new StringBuilder(containedFields.Length);
            foreach (var containedField in containedFields)
            {
                generatedJSON.Append(serializer.serializeNamedField(containedField));
            }

            return string.Format("{{ {0} ,\"html\":[{1}] }}", base.AsJSON(actualField,field, serializer), generatedJSON);
        }
    }
}
