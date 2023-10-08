using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Game.AccountManagement.Domain.Data.Entities;
using MediatR;
namespace Game.AccountManagement.Application.Contracts.Queries
{
    public record GetAllAccountsQuery : IRequest<IEnumerable<Account>>{} 
}
