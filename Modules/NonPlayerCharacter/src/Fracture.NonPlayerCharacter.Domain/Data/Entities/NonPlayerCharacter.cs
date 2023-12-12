using Fracture.NonPlayerCharacter.Domain.Data.ValueObjects;
using Fracture.Shared.Core.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fracture.NonPlayerCharacter.Domain.Data.Entities
{
    public class NonPlayerCharacter : Entity
    {
        public required string Name { get; init; }
        public required NonPlayerCharacterStory Story { get; init; }
    }
}
