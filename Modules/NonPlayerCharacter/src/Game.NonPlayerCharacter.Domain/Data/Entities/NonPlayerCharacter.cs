using Game.NonPlayerCharacter.Domain.Data.ValueObjects;
using Game.Shared.Core.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.NonPlayerCharacter.Domain.Data.Entities
{
    public sealed class NonPlayerCharacter : Entity
    {
        public required string Name { get; init; }
        public required NonPlayerCharacterStory Story { get; init; }
    }
}
