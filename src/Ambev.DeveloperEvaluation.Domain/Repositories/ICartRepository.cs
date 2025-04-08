public interface ICartRepository
{
    Task<Cart?> GetByIdAsync(int id);
    Task<List<Cart>> GetPagedAsync(int page, int size, string order);
    Task<int> CountAsync();
    Task AddAsync(Cart cart);
    Task UpdateAsync(Cart cart);
    Task DeleteAsync(int id);
}
