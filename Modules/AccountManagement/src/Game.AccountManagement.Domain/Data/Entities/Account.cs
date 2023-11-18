using Game.Shared.Core.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.AccountManagement.Domain.Data.Entities
{
    public class Account : Entity
    {
        public string Name { get; set; } = null!;
    }
}
