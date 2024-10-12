using syncData.Models;
using System;

namespace syncData
{
    public class LocalStorageService
    {
        private readonly UserContextSqlLite _context;

        public LocalStorageService(UserContextSqlLite context)
        {
            _context = context;
            _context.Database.EnsureCreated();
        }

        public void StoreOperation(string data , string data2)
        {
            var operation = new User
            {
                Name  = data,
                Email = data2
            };
            _context.LocalUser.Add(operation);
            _context.SaveChanges();
        }
        public List<User> RetrieveOperations()
        {
            return _context.LocalUser.ToList();
        }

        public void ClearStoredOperations()
        {
            _context.LocalUser.RemoveRange(_context.LocalUser);
            _context.SaveChanges();
        }
    }

}
