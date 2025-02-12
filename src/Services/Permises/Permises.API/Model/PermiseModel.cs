using System.ComponentModel.DataAnnotations;

namespace Permises.API.Model
{
    public class PermiseModel
    {
        [Key, Required]
        public int PermiseId { get; set; }
        public Guid PermiseGuid { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool Inactive { get; set; }
        public Guid CreatedUserId { get; set; }
        public DateTime CreatedDate { get; set; }
        public Guid UpdatedUserId { get; set; }
        public DateTime UpdatedDate { get; set; }
    }
}