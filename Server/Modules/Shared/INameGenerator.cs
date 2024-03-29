using System;

namespace Fracture.Server.Modules.Shared
{
    public interface INameGenerator
    {
        /// <summary>
        /// Returns a new generated name
        /// </summary>
        /// <returns></returns>
        Task<string> GenerateNameAsync();
    }
}
