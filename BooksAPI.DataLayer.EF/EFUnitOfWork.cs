using BooksAPI.DataLayer.Abstractions;
using Microsoft.Extensions.Logging;

namespace BooksAPI.DataLayer.EF
{
    public class EFUnitOfWork : IUnitOfWork
    {

        private readonly BooksContext _context;
        private ILogger<EFUnitOfWork> _logger;

        public EFUnitOfWork(BooksContext context, ILogger<EFUnitOfWork> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task Commit()
        {
            try
            {
                if (_context.ChangeTracker.HasChanges())
                {
                    await _context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error on commit");
            }
            
            
        }
    }
}