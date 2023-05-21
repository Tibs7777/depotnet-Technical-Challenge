using Repository.Contracts;

namespace Repository
{


    public class RepositoryManager : IRepositoryManager
    {
        private readonly RepositoryContext _repositoryContext;
        private IOrderRepository _orderRepository;

        public RepositoryManager(RepositoryContext repositoryContext)
        {
            _repositoryContext = repositoryContext;
            _orderRepository = new OrderRepository(_repositoryContext);
        }

        //Todo: Lazy loading
        public IOrderRepository OrderRepository { get { return _orderRepository; } }

        public async Task SaveAsync()
        {
            await _repositoryContext.SaveChangesAsync();
        }
    }
}
