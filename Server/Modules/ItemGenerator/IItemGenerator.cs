using System;
using ItemGeneratorModels;

namespace ExampleItemGenerator.Services.Generators
{
    public interface IItemGenerator
    {
        Task<Item> Generate();
    }
}
