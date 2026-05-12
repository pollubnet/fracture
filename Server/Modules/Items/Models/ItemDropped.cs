using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Fracture.Server.Modules.Items.Models;

public class ItemDropped
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    public int UserId { get; set; }

    public int MapSeed { get; set; }

    public int X { get; set; }
    public int Y { get; set; }
}
