using System;
using Fracture.Server.Modules.Items.Models;

namespace Fracture.Server.Modules.Items.Services
{
    public interface IItemGenerator
    {
        Task<Item> Generate();
    }
}
