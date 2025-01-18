using Domain.DTOs;
using Domain.Interfaces.Service;
using Domain.Shared;
using GeneralStoreManagement.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GeneralStoreManagement.Controllers
{
    [ApiController]
    [Route("v1/api/[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;
        private readonly BinanceService _binanceService;
        public AccountController(IAccountService accountService, BinanceService binance)
        {
            _accountService = accountService;
            _binanceService = binance;
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetAllAccounts()
        {
            var response = await _accountService.GetAllAccountsAsync();
            return Ok(response);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetAccountById(long id)
        {
            var response = await _accountService.GetAccountByIdAsync(id);
            return Ok(response);
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateAccount([FromBody] AccountDTO accountDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ApiResponse<string>(
                    null,
                    false,
                    "Invalid account data.",
                    ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage)).ToList()
                ));
            }

            var response = await _accountService.CreateAccountAsync(accountDto);
            return Ok(response);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAccount(long id, [FromBody] AccountDTO accountDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ApiResponse<string>(
                    null,
                    false,
                    "Invalid account data.",
                    ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage)).ToList()
                ));
            }

            var response = await _accountService.UpdateAccountAsync(id, accountDto);
            return Ok(response);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAccount(long id)
        {
            var response = await _accountService.DeleteAccountAsync(id);
            return Ok(response);
        }
    }
}
