using Game.Shared.Core.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.NonPlayerCharacter.Domain.Data.ValueObjects
{
    public sealed record NonPlayerCharacterStory(string StoryText) : ValueObject;
}
