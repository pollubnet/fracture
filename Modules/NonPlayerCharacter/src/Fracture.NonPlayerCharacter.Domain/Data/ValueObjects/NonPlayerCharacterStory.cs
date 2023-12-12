using Fracture.Shared.Core.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fracture.NonPlayerCharacter.Domain.Data.ValueObjects
{
    public sealed record NonPlayerCharacterStory(string StoryText) : ValueObject;
}
