using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using IdentityServer4.Validation;
using static IdentityModel.OidcConstants;
using Newtonsoft.Json;
using AuthServer.Models;

namespace AuthServer
{
    public class GoogleGrant : IExtensionGrantValidator
    {

        public string GrantType
        {
            get
            {
                return "googleAuth";
            }
        }

        private ApplicationDbContext db;

        public GoogleGrant(ApplicationDbContext context)
        {
            db = context;
        }

        public async Task ValidateAsync(ExtensionGrantValidationContext context)
        {
            var userToken = context.Request.Raw.Get("id_token");

            if (string.IsNullOrEmpty(userToken))
            {
                context.Result = new GrantValidationResult(TokenErrors.InvalidGrant, null);
                return;
            }
            HttpClient client = new HttpClient();

            var request = client.GetAsync("https://www.googleapis.com/oauth2/v3/tokeninfo?id_token=" + userToken).Result;

            if (request.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var googleResult = await request.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<GoogleValidationResult>(googleResult);

                if (db.Users.Where(u => u.Email == result.email).Count() == 1)
                {
                    context.Result = new GrantValidationResult(db.Users.Where(u => u.Email == result.email).FirstOrDefault().Id, "google");
                }
                else
                {
                    var user = new User
                    {
                        Email = result.email,
                        Active = true,
                        Password = "",
                        Id = Guid.NewGuid().ToString()
                    };
                    try
                    {
                        db.Users.Add(user);
                        db.SaveChanges();
                        context.Result = new GrantValidationResult(user.Id, "google");
                    }
                    catch (Exception x)
                    {
                        var a = x.Message;
                    }
                }
                return;
            }
            else
            {
                return;
            }
        }
    }

    public class GoogleValidationResult
    {
        public bool email_verified { get; set; }
        public string email { get; set; }
        public string given_name { get; set; }
        public string family_name { get; set; }
    }
}
