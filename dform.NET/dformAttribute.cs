// -----------------------------------------------------------------------
// <copyright file="dformAttribute.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace dform.NET
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
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
        public DFormAttribute(HttpMethod method,Uri action )
        {
            this.method = method;
            this.action = action;
        }
        public HttpMethod method;
        public string id;
        public Uri action;
    }
}
