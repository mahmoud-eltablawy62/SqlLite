using syncData.Models;
using System;

namespace syncData
{
    public class ICheckServices : IHostedService, IDisposable
    {
        private Timer _timer;
       
        private readonly IServiceScopeFactory _serviceScopeFactory;
        

        public ICheckServices(  IServiceScopeFactory serviceScopeFactory) 
        {
            
            _serviceScopeFactory = serviceScopeFactory;
          
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _timer = new Timer(CheckInternetConnection, null, TimeSpan.Zero, TimeSpan.FromMinutes(1));
            return Task.CompletedTask;
        }

        private void CheckInternetConnection(object state)
        {
            using (var scope = _serviceScopeFactory.CreateScope())
            {
                var localStorage = scope.ServiceProvider.GetRequiredService<LocalStorageService>();
                var userContext = scope.ServiceProvider.GetRequiredService<userContext>();
                if (IsInternetAvailable())
                {
                    var operations = localStorage.RetrieveOperations();
                    foreach (var operation in operations)
                    {
                        AddOperationToSqlServer( userContext,operation.Name, operation.Email);
                    }
                    localStorage.ClearStoredOperations();
                }
            }
        }

        private bool IsInternetAvailable()
        {
            try
            {
                using (var client = new HttpClient())
                {
                   
                    client.Timeout = TimeSpan.FromSeconds(5);

                  
                    var response = client.GetAsync("http://www.google.com").Result;

                   
                    return response.IsSuccessStatusCode;
                }
            }
            catch
            {
                return false;
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _timer?.Change(Timeout.Infinite, 0);
            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _timer?.Dispose();
     
        }

        private void AddOperationToSqlServer(userContext _context,string name, string email)
        {
          
                var operation = new User
                {
                    Name = name,
                    Email = email
                };

                _context.User.Add(operation);
                _context.SaveChanges();
            
        }
    }
}
