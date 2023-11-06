using Game.AccountManagement.Application.Contracts.Queries;
using Game.AccountManagement.Domain.Data.Entities;
using Game.AccountManagement.Domain.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.AccountManagement.Application.Handlers.Queries
{
    internal class GetAllAccountsHandler
        : IRequestHandler<GetAllAccountsQuery, IEnumerable<Account>>
    {
        private readonly IAccountRepository _accountRepository;

        public GetAllAccountsHandler(IAccountRepository accountRepository)
        {
            _accountRepository = accountRepository;
        }

        public async Task<IEnumerable<Account>> Handle(
            GetAllAccountsQuery request,
            CancellationToken cancellationToken
        )
        {
            return _accountRepository.GetAllAccount();
        }
    }
}
