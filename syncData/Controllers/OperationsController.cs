using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using syncData.Models;
using System.Runtime.CompilerServices;

namespace syncData.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OperationsController : ControllerBase
    {
        private readonly LocalStorageService _localStorage;

        private readonly userContext _context;

        public OperationsController(LocalStorageService localStorage , userContext context)
        {
            _localStorage = localStorage; 
            _context = context;
        }

        [HttpPost]
        public IActionResult AddOperation( string name , string Email)
        {
            if (!IsInternetAvailable())
            {
                _localStorage.StoreOperation(name , Email);
                return StatusCode(StatusCodes.Status503ServiceUnavailable, "تم تخزين العملية محليًا حتى يعود الاتصال.");
            }

            
             AddOperationToSqlServer(name , Email);

            return Ok();
        }

        [HttpGet]
        private void AddOperationToSqlServer(string name, string email)
        {
           
                var operation = new User
                {
                    Name = name,
                    Email = email
                };

                _context.User.Add(operation);
                _context.SaveChanges();
            
        }



        [HttpGet]
        private bool IsInternetAvailable()
        {
            try
            {
                using (var client = new HttpClient())
                {

                    client.Timeout = TimeSpan.FromSeconds(5);


                    var response = client.GetAsync("http://www.google.com").Result;


                    return response.IsSuccessStatusCode;
                   //return false;
                }
            }
            catch
            {
                return false;
            }
        }
    }
}
