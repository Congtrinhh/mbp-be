using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Extensions
{
    public static class PropertyExtensions
    {
        public static bool HasValue(this PropertyInfo property, object entity)
        {
            var value = property.GetValue(entity);

            if (value == null)
            {
                return false;
            }

            return true;
        }
    }
}
