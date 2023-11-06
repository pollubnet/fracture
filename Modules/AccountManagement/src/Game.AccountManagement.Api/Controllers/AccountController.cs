using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Game.AccountManagement.Application.Contracts.Queries;
using MediatR;
using Game.AccountManagement.Domain.Data.Entities;

namespace Game.AccountManagement.Api.Controllers
{
    [ApiController]
    [Route("/[controller]")]
    [Area("AccountManagement")]
    public class AccountController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AccountController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("")]
        public async Task<ActionResult<Account>> GetAccounts()
        {
            var getAllAccountsQuery = new GetAllAccountsQuery();
            var accounts = await _mediator.Send(getAllAccountsQuery);
            return Ok(accounts);
        }
    }
}
