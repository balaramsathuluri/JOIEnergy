using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JOIEnergy.Domain.Models;
using JOIEnergy.Repository.Implementations.Utilities;
using JOIEnergy.Repository.Interfaces;

namespace JOIEnergy.Repository.Implementations
{
    public class UserRepository : IUserRepository
    {
        private readonly string _jsonFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources", "users.json");
        private List<User> _users;

        public UserRepository()
        {
            _users = JsonReader.LoadJson<List<User>>(_jsonFilePath) ?? new List<User>();
        }

        public List<User> GetUsers() => _users;
    }
}
