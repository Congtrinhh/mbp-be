using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Attributes
{
    /// <summary>
    ///  config repo, service của 1 trường trong entity
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class LayerAttribute : Attribute
    {
        public string Repository { get; }
        public string Service { get; }

        public LayerAttribute(string repository, string service)
        {
            Repository = repository;
            Service = service;
        }
    }
}