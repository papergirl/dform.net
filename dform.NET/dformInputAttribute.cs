// -----------------------------------------------------------------------
// <copyright file="dformInputAttribute.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace dform.NET
{
    using System.Reflection;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class DFormInputAttribute : DFormField
    {
        internal override string AsJSON(object actualField, PropertyInfo field, DFormSerializer serializer)
        {
            return string.Format("{{ \"type\":\"div\", \"class\":\"editor\",\"html\":[{{{0}}}]}}", base.AsJSON(actualField,field, serializer));
        }
    }
}
