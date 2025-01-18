using Domain.DTOs;
using Domain.Shared;

namespace Domain.Interfaces.Service
{
    public interface IAccountService
    {
        Task<ApiResponse<IEnumerable<AccountDTO>>> GetAllAccountsAsync();
        Task<ApiResponse<AccountDTO>> GetAccountByIdAsync(long id);
        Task<ApiResponse<AccountDTO>> CreateAccountAsync(AccountDTO accountDto);
        Task<ApiResponse<string>> UpdateAccountAsync(long id, AccountDTO accountDto);
        Task<ApiResponse<string>> DeleteAccountAsync(long id);
    }
}
