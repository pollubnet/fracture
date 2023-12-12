using Fracture.AccountManagement.Application.Contracts.Queries;
using Fracture.AccountManagement.Domain.Data.Entities;
using Fracture.AccountManagement.Domain.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fracture.AccountManagement.Application.Handlers.Queries
{
    internal class GetAllAccountsHandler
        : IRequestHandler<GetAllAccountsQuery, IEnumerable<Account>>
    {
        private readonly IAccountRepository _accountRepository;

        public GetAllAccountsHandler(IAccountRepository accountRepository)
        {
            _accountRepository = accountRepository;
        }

        public Task<IEnumerable<Account>> Handle(
            GetAllAccountsQuery request,
            CancellationToken cancellationToken
        )
        {
            return Task.FromResult(_accountRepository.GetAllAccount());
        }
    }
}
