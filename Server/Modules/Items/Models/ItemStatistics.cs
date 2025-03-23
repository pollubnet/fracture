using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Fracture.Server.Modules.Items.Models;

public class ItemStatistics
{
    [Key]
    [ForeignKey(nameof(Item.Statistics))]
    public int ItemId { get; set; }

    public int Luck { get; set; }

    public int Health { get; set; }

    public int Armor { get; set; }

    public int Power { get; set; }

    [InverseProperty(nameof(Item.Statistics))]
    [JsonIgnore]
    public virtual Item Item { get; set; }

    public int GetStatFromItemStat(ItemStat stat)
    {
        return stat switch
        {
            ItemStat.Luck => Luck,
            ItemStat.Health => Health,
            ItemStat.Armor => Armor,
            ItemStat.Power => Power,
            _ => throw new ArgumentOutOfRangeException(nameof(stat), stat, null),
        };
    }

    public void SetStatFromItemStat(ItemStat stat, int value)
    {
        switch (stat)
        {
            case ItemStat.Luck:
                Luck = value;
                break;
            case ItemStat.Health:
                Health = value;
                break;
            case ItemStat.Armor:
                Armor = value;
                break;
            case ItemStat.Power:
                Power = value;
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(stat), stat, null);
        }
    }
}
