using System;

namespace Fracture.Server.Modules.Shared
{
    public interface INameGenerator
    {
        Task<string> GenerateName();
    }
}
