using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    /// <summary>
    /// Represents a province.
    /// </summary>
    public class Province : BaseEntity
    {
        public string Code { get; set; } = string.Empty;
        public string Name { get; set; }
        public string FullName { get; set; }
        public string FullNameEn { get; set; }
        public string CodeName { get; set; }
        public int SortOrder { get; set; }

        #region Additional fields
        [NotMapped]
        public int? UserId { get; set; }
        #endregion

        public virtual ICollection<User> Users { get; set; } = new List<User>();
    }
}
