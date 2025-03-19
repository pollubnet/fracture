using System.ComponentModel.DataAnnotations;

namespace Fracture.Server.Modules.Shared.ImageChangers;

public class BackgroundImage
{
    [Required]
    public string ImagePath { get; set; }

    public BackgroundImage(string path)
    {
        ImagePath = path;
    }
}
