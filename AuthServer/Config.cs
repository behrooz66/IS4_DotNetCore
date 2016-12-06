using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IdentityServer4.Models;
using IdentityServer4.Services.InMemory;

namespace AuthServer
{
    public class Config
    {
        public static IEnumerable<Scope> GetScopes()
        {
            return new List<Scope>
            {
                StandardScopes.OfflineAccess,
                new Scope
                {
                    Name = "api1",
                    Description = "My API",
                    IncludeAllClaimsForUser = true
                }
            };
        }

        public static IEnumerable<Client> GetClients()
        {
            return new List<Client>
            {
                new Client
                {
                    ClientId = "google",
                    ClientSecrets =
                    {
                        new Secret("secret".Sha256())
                    },
                    AllowedGrantTypes = GrantTypes.List("googleAuth"),

                    AllowedScopes =
                    {
                       "offline_access",
                        "api1"
                    }
                },
                new Client
                {
                    ClientId = "resourceOwner",
                    ClientSecrets =
                    {
                        new Secret("secret".Sha256())
                    },
                    AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,

                    AllowedScopes =
                    {
                        "offline_access",
                        "api1"
                    }
                }
            };
        }

        public static List<InMemoryUser> GetUsers()
        {
            return new List<InMemoryUser>
            {
                new InMemoryUser
                {
                    Subject = "0001",
                    Username = "behrooz",
                    Password = "mypass",
                    Enabled = true
                },
                new InMemoryUser
                {
                    Subject = "0002",
                    Username = "mahnaz",
                    Password = "anotherpass",
                    Enabled = true
                }
            };
        }
    }
}
