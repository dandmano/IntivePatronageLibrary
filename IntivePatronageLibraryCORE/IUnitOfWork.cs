using IntivePatronageLibraryCORE.Repositories;

namespace IntivePatronageLibraryCORE
{
    public interface IUnitOfWork : IDisposable
    {
        IAuthorRepository Authors { get; }
        IBookRepository Books { get; }
        Task<int> CommitAsync();
    }
}