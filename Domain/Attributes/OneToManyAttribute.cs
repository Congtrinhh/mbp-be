using System;

namespace Domain.Attributes
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class OneToManyAttribute : Attribute
    {
        public string Table { get; }
        public string ForeignKey { get; }

        public OneToManyAttribute(string table, string foreignKey)
        {
            Table = table;
            ForeignKey = foreignKey;
        }
    }
}