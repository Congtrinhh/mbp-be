using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Attributes
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class ChildTableAttribute : Attribute
    {
        public string ForeignKey { get; }

        public ChildTableAttribute(string foreignKey)
        {
            ForeignKey = foreignKey;
        }
    }
}
