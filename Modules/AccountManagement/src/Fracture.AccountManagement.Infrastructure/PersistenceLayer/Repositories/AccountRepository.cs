using Fracture.AccountManagement.Domain.Data.Entities;
using Fracture.AccountManagement.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fracture.AccountManagement.Infrastructure.PersistenceLayer.Repositories
{
    internal class AccountRepository : IAccountRepository
    {
        public IEnumerable<Account> GetAllAccount()
        {
            return new List<Account>()
            {
                new Account { Id = Guid.NewGuid(), Name = "Example1" },
                new Account { Id = Guid.NewGuid(), Name = "Example2" }
            };
        }
    }
}
