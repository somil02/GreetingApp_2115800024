using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RepositoryLayer.Entity;
using ModelLayer.Model;



namespace RepositoryLayer.Interface
{
    public interface IGreetingRL
    {
        string Greet();
        GreetingEntity AddGreeting(SaveGreetingModel greetRequest);
        string GetGreetingById(int id);
        List<string> GetAllGreetingMessage();
        GreetingEntity UpdateGreeting(int id,string newMessage);
        bool DeleteGreeting(int id);
    }
}