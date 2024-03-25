using System;

namespace ExampleItemGenerator.Services.Generators
{
    public interface INameGenerator
    {
        Task<string> GenerateName();
    }
}
