using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Fracture.Server.Modules.Users.Models;

namespace Fracture.Server.Modules.Items.Models;

public class ItemDropped
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [ForeignKey(nameof(User))]
    public int UserId { get; set; }

    [ForeignKey(nameof(Item))]
    public int? ItemId { get; set; }

    public int MapSeed { get; set; }

    public int X { get; set; }
    public int Y { get; set; }

    public virtual User User { get; set; } = null!;
    public virtual Item? Item { get; set; }
}
