using Domain.DTOs;
using Domain.Interfaces.Repository;
using Domain.Interfaces.Service;
using Domain.Models;
using Domain.Shared;

namespace GeneralStoreManagement.Services
{
    public class AccountService : IAccountService
    {
        private readonly IRepository<Account> _accountRepository;

        public AccountService(IRepository<Account> accountRepository)
        {
            _accountRepository = accountRepository;
        }

        public async Task<ApiResponse<IEnumerable<AccountDTO>>> GetAllAccountsAsync()
        {
            try
            {
                var accounts = await _accountRepository.GetAllAsync();
                var accountDtos = accounts.Select(a => new AccountDTO
                {
                    UserName = a.UserName,
                    Email = a.Email,
                    Phone = a.Phone
                });

                return new ApiResponse<IEnumerable<AccountDTO>>(
                    accountDtos,
                    true,
                    "Accounts retrieved successfully."
                );
            }
            catch (Exception ex)
            {
                return new ApiResponse<IEnumerable<AccountDTO>>(
                    null,
                    false,
                    "Error retrieving accounts.",
                    new List<string> { ex.Message }
                );
            }
        }

        public async Task<ApiResponse<AccountDTO>> GetAccountByIdAsync(long id)
        {
            try
            {
                var account = await _accountRepository.GetByIdAsync(id);
                if (account == null)
                {
                    return new ApiResponse<AccountDTO>(
                        null,
                        false,
                        "Account not found."
                    );
                }

                var accountDto = new AccountDTO
                {
                    UserName = account.UserName,
                    Email = account.Email,
                    Phone = account.Phone
                };

                return new ApiResponse<AccountDTO>(
                    accountDto,
                    true,
                    "Account retrieved successfully."
                );
            }
            catch (Exception ex)
            {
                return new ApiResponse<AccountDTO>(
                    null,
                    false,
                    "Error retrieving account.",
                    new List<string> { ex.Message }
                );
            }
        }

        public async Task<ApiResponse<AccountDTO>> CreateAccountAsync(AccountDTO accountDto)
        {
            try
            {
                var account = new Account
                {
                    UserName = accountDto.UserName,
                    Password = accountDto.Password,
                    Email = accountDto.Email,
                    Phone = accountDto.Phone,
                    ApiKey = accountDto.apiKey,
                    SecretKey = accountDto.apiSecret
                };

                await _accountRepository.AddAsync(account);

                return new ApiResponse<AccountDTO>(
                    null,
                    true,
                    "Account created successfully."
                );
            }
            catch (Exception ex)
            {
                return new ApiResponse<AccountDTO>(
                    null,
                    false,
                    "Error creating account.",
                    new List<string> { ex.Message }
                );
            }
        }

        public async Task<ApiResponse<string>> UpdateAccountAsync(long id, AccountDTO accountDto)
        {
            try
            {
                var account = await _accountRepository.GetByIdAsync(id);
                if (account == null)
                {
                    return new ApiResponse<string>(
                        null,
                        false,
                        "Account not found."
                    );
                }

                account.UserName = accountDto.UserName;
                account.Email = accountDto.Email;
                account.Phone = accountDto.Phone;

                await _accountRepository.UpdateAsync(account);

                return new ApiResponse<string>(
                    null,
                    true,
                    "Account updated successfully."
                );
            }
            catch (Exception ex)
            {
                return new ApiResponse<string>(
                    null,
                    false,
                    "Error updating account.",
                    new List<string> { ex.Message }
                );
            }
        }

        public async Task<ApiResponse<string>> DeleteAccountAsync(long id)
        {
            try
            {
                var account = await _accountRepository.GetByIdAsync(id);
                if (account == null)
                {
                    return new ApiResponse<string>(
                        null,
                        false,
                        "Account not found."
                    );
                }

                await _accountRepository.DeleteAsync(id);

                return new ApiResponse<string>(
                    null,
                    true,
                    "Account deleted successfully."
                );
            }
            catch (Exception ex)
            {
                return new ApiResponse<string>(
                    null,
                    false,
                    "Error deleting account.",
                    new List<string> { ex.Message }
                );
            }
        }
    }
}
