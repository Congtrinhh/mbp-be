using Microsoft.AspNetCore.Mvc;
using Api.Filters;

namespace Api.Attributes
{
    /// <summary>
    /// Ensures that the current user can only access their own resources.
    /// The action method must have a parameter named 'userId' or an entity parameter inheriting from BaseEntity.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = false)]
    public class RequiresSameUserAttribute : TypeFilterAttribute
    {
        public RequiresSameUserAttribute() : base(typeof(RequiresSameUserFilter))
        {
        }
    }
}