using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.AccountManagement.Api.Controllers
{
    [ApiController]
    [Route("/[controller]")]
    public class AccountController : ControllerBase
    {
        [HttpGet("")]
        public IActionResult GetAccounts()
        {
            return Ok();
        }
    }
}
