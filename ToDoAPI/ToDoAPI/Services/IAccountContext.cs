using ToDoAPI.Entity;

namespace ToDoAPI.Services;

public interface IAccountContext
{
	Task AddAsync(Account account);
	Task RemoveAsync(Account account) => RemoveAsync(account.Id);
	Task RemoveAsync(int id);
	Task<Account> GetAsync(string login);
	Task<int> CountAsync();
	Task<IEnumerable<Role>> GetRolesAsync(int accountId);
}