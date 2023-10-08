using Game.AccountManagement.Domain.Data.Entities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.AccountManagement.Domain.Repositories
{
    public interface IAccountRepository
    {
        public IEnumerable<Account> GetAllAccount();
    }
}
