using System.ComponentModel.DataAnnotations;

namespace Fracture.Server.Modules.Shared.ImageChangers;

public class BackgroundImage
{
    public BackgroundImage(string path)
    {
        ImagePath = path;
    }

    [Required]
    public string ImagePath { get; set; }
}
