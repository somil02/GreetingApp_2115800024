using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ModelLayer.Model;
using RepositoryLayer.Entity;
using RepositoryLayer.Interface;
using RepositoryLayer.Context;

namespace RepositoryLayer.Service
{
    public class GreetingRL : IGreetingRL
    {
        private readonly GreetingDbContext _dbContext;
        public GreetingRL(GreetingDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public string Greet()
        {
            return "Hello World";
        }
        public GreetingEntity AddGreeting(SaveGreetingModel greetRequest)
        {
            var result = _dbContext.Greetings.FirstOrDefault<GreetingEntity>(e => e.GreetingMessage == greetRequest.GreetingMessage);
            if (result == null)
            {
                var greet = new GreetingEntity
                {
                    GreetingMessage = greetRequest.GreetingMessage
                };


                _dbContext.Add(greet);
                _dbContext.SaveChanges();
                return greet;
            }
            return result;
        }
        public string GetGreetingById(int id)
        {
            var result = _dbContext.Greetings.FirstOrDefault<GreetingEntity>(e => e.Id == id);
            if (result != null)
            {
                return result.GreetingMessage;
            }
            return null;
        }
        public List<string> GetAllGreetingMessage()
        {
            var result = _dbContext.Greetings.ToList();
            List<string> greetings = new List<string>();
            foreach (var greet in result)
            {
                greetings.Add(greet.GreetingMessage);
            }
            return greetings;
        }
        public GreetingEntity UpdateGreeting(int id,string newMessage)
        {
            var result = _dbContext.Greetings.FirstOrDefault<GreetingEntity>(e => e.Id == id);
            if (result != null)
            {
                result.GreetingMessage = newMessage;
                _dbContext.SaveChanges();
                return result;
            }
            return null;
        }
    }
}