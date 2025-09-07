using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    /// <summary>
    /// type of MC: event MC/ wedding MC/ ...
    /// </summary>
    public class McType : BaseEntity
    {
        public string Label { get; set; } = string.Empty;
        public string? Description { get; set; }

        #region Additional fields
        [NotMapped]
        public int? McId { get; set; }
        #endregion

        public virtual ICollection<User> Users { get; set; } = new List<User>();
    }
}
