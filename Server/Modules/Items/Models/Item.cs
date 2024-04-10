using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Fracture.Server.Modules.Users;

namespace Fracture.Server.Modules.Items.Models
{
    public class Item
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string Name { get; set; } = null!;

        public string? History { get; set; } = null!;

        public ItemRarity Rarity { get; set; } = ItemRarity.Common;

        public ItemType Type { get; set; } = ItemType.Helmet;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public bool IsEquipped { get; set; }

        [ForeignKey(nameof(User.Id))]
        public int CreatedById { get; set; }

        [InverseProperty(nameof(ItemStatistics.Item))]
        public virtual ItemStatistics Statistics { get; set; } = null!;

        [InverseProperty(nameof(User.Items))]
        [JsonIgnore]
        public virtual User CreatedBy { get; set; } = null!;
    }
}
