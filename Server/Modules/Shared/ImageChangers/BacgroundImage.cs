using System.ComponentModel.DataAnnotations;

namespace Fracture.Server.Modules.Shared.ImageChanger;

public class BacgroundImage
{
    [Required]
    public string bgImg { get; set; }

    public BacgroundImage(string bgimg)
    {
        bgImg = bgimg;
    }
}
