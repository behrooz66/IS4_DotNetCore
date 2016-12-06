using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AuthServer.Models;

namespace AuthServer
{
    public class AuthRepository:IAuthRepository
    {
        private ApplicationDbContext db;

        public AuthRepository(ApplicationDbContext context)
        {
            db = context;
        }

        public User GetUserById(string id)
        {
            var user = db.Users.Where(u => u.Id == id).FirstOrDefault();
            return user;
        }

        public User GetUserByUsername(string username)
        {
            var user = db.Users.Where(u => String.Equals(u.Email, username)).FirstOrDefault();
            return user;
        }

        public bool ValidatePassword(string username, string plainTextPassword)
        {
            var user = db.Users.Where(u => String.Equals(u.Email, username)).FirstOrDefault();
            if (user == null) return false;
            if (String.Equals(plainTextPassword, user.Password)) return true;
            return false;
        }
    }
}
