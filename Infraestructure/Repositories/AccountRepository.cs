using Domain.Interfaces.Repository;
using Domain.Interfaces.Service;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Infraestructure.Repositories
{
    public class AccountRepository : IRepository<Account>
    {
        private readonly DataContext _dbContext;
        private readonly IEncryptionService _encryptionService;

        public AccountRepository(DataContext context, IEncryptionService encryptionService)
        {
            _dbContext = context;
            _encryptionService = encryptionService;
        }

        public async Task<IEnumerable<Account>> GetAllAsync()
        {
            var accounts = await _dbContext.Accounts.ToListAsync();

            return accounts;
        }

        public async Task<Account?> GetByIdAsync(long id)
        {
            var account = await _dbContext.Accounts.FindAsync(id);

            return account;
        }

        public async Task AddAsync(Account entity)
        {
            entity.EncryptionKey = Convert.ToBase64String(RandomNumberGenerator.GetBytes(32));
            entity.EncryptionIV = Convert.ToBase64String(RandomNumberGenerator.GetBytes(16));

            entity.ApiKey = _encryptionService.Encrypt(entity.ApiKey, entity.EncryptionKey, entity.EncryptionIV);
            entity.SecretKey = _encryptionService.Encrypt(entity.SecretKey, entity.EncryptionKey, entity.EncryptionIV);
            entity.Password = _encryptionService.Encrypt(entity.Password, entity.EncryptionKey, entity.EncryptionIV);

            _dbContext.Accounts.Add(entity);
            await _dbContext.SaveChangesAsync();
        }

        public async Task UpdateAsync(Account entity)
        {
            var existingAccount = await _dbContext.Accounts.FindAsync(entity.Id);
            if (existingAccount == null)
                throw new KeyNotFoundException("Account not found");

            existingAccount.UserName = entity.UserName;

            existingAccount.ApiKey = _encryptionService.Encrypt(entity.ApiKey, existingAccount.EncryptionKey, existingAccount.EncryptionIV);
            existingAccount.SecretKey = _encryptionService.Encrypt(entity.SecretKey, existingAccount.EncryptionKey, existingAccount.EncryptionIV);

            _dbContext.Accounts.Update(existingAccount);
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(long id)
        {
            var account = await _dbContext.Accounts.FindAsync(id);
            if (account == null)
                throw new KeyNotFoundException("Account not found");

            _dbContext.Accounts.Remove(account);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<(IEnumerable<Account>, int)> GetPagedAsync(int pageNumber, int pageSize, Expression<Func<Account, bool>>? filter = null)
        {
            var query = _dbContext.Accounts.AsQueryable();

            if (filter != null)
            {
                query = query.Where(filter);
            }

            var totalRecords = await query.CountAsync();
            var accounts = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            foreach (var account in accounts)
            {
                account.ApiKey = _encryptionService.Decrypt(account.ApiKey, account.EncryptionKey, account.EncryptionIV);
                account.SecretKey = _encryptionService.Decrypt(account.SecretKey, account.EncryptionKey, account.EncryptionIV);
            }

            return (accounts, totalRecords);
        }
    }
}
