namespace BooksAPI.DataLayer.Abstractions
{
    public interface IUnitOfWork
    {
        Task Commit();
    }
}