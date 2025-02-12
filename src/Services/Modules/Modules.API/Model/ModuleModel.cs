using System.ComponentModel.DataAnnotations;

namespace Modules.API.Model
{
    public class ModuleModel
    {
        [Key, Required]
        public int ModuleId { get; set; }
        public Guid ModuleGuid { get; set; }
        public int Cod { get; set; }
        public int Order { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Url { get; set; }
        public bool IsParent { get; set; }
        public Guid ParentModuleId { get; set; }
        public bool Inactive { get; set; }
        public Guid CreatedUserId { get; set; }
        public DateTime CreatedDate { get; set; }
        public Guid UpdatedUserId { get; set; }
        public DateTime UpdatedDate { get; set; }
        public List<ModuleModel> Childs { get; set; }
    }
}