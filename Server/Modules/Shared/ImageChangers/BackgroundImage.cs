using System.ComponentModel.DataAnnotations;

namespace Fracture.Server.Modules.Shared.ImageChangers;

public class BackgroundImage
{
    [Required]
    public string bgImg { get; set; }

    public BackgroundImage(string bgimg)
    {
        bgImg = bgimg;
    }
}
