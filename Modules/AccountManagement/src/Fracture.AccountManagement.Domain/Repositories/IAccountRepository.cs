using Fracture.AccountManagement.Domain.Data.Entities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fracture.AccountManagement.Domain.Repositories
{
    public interface IAccountRepository
    {
        public IEnumerable<Account> GetAllAccount();
    }
}
