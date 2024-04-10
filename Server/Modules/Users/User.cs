using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Fracture.Server.Modules.Items.Models;

namespace Fracture.Server.Modules.Users
{
    public class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string Username { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [InverseProperty(nameof(Item.CreatedBy))]
        [JsonIgnore]
        public virtual ICollection<Item> Items { get; set; } = null!;
    }
}
