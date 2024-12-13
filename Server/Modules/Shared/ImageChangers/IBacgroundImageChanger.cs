using Fracture.Server.Modules.MapGenerator.Models;
using Microsoft.AspNetCore.Components;

namespace Fracture.Server.Modules.Shared.ImageChanger;

public interface IBacgroundImageChanger
{
    [Parameter]
    BacgroundImage _bacgroundImage { get; set; }

    void change();
}
