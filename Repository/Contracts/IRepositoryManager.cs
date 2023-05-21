namespace Repository.Contracts
{
    public interface IRepositoryManager
    {
        IOrderRepository OrderRepository { get; }

        Task SaveAsync();
    }
}
