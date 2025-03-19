using Microsoft.AspNetCore.Components;

namespace Fracture.Server.Modules.Shared.ImageChangers;

public interface IBackgroundImageChanger
{
    [Parameter]
    BackgroundImage BackgroundImage { get; set; }

    void change();
}
