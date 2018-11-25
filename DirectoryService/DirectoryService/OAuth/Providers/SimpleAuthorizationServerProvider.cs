using Microsoft.Owin.Security.OAuth;
using System.Threading.Tasks;
using System.Security.Claims;

using DirectoryService.Models;
using System.Linq;
using System.Data.Entity;
using System.Data;

namespace DirectoryService.OAuth.Providers
{
    public class SimpleAuthorizationServerProvider : OAuthAuthorizationServerProvider
    {
        // OAuthAuthorizationServerProvider sınıfının client erişimine izin verebilmek için ilgili ValidateClientAuthentication metotunu override ediyoruz.
        public override async System.Threading.Tasks.Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            context.Validated();
        }

        // OAuthAuthorizationServerProvider sınıfının kaynak erişimine izin verebilmek için ilgili GrantResourceOwnerCredentials metotunu override ediyoruz.
        public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {
            // User verify
            ModelContainer _contextModel = new ModelContainer();

            string email = context.UserName.ToString();

            Admin admin = (from c in _contextModel.Admin.AsNoTracking() where c.Email == email select c).FirstOrDefault();

            Company company = (from c in _contextModel.Company.AsNoTracking() where c.Email == email select c).FirstOrDefault();
            CompanyStaff staff = (from c in _contextModel.CompanyStaff.AsNoTracking() where c.Email == email select c).FirstOrDefault();

            if (admin != null)
            {

                if (context.UserName == admin.Email && VerifyPasswordHash(context.Password.ToString(), admin.PasswordHash, admin.PasswordSalt))
                {

                    // CORS ayarlarını set ediyoruz.
                    context.OwinContext.Response.Headers.Add("Access-Control-Allow-Origin", new[] { "*" });

                    // Kullanıcının access_token alabilmesi için gerekli validation işlemlerini yapıyoruz.
                    var identity = new ClaimsIdentity(context.Options.AuthenticationType);

                    identity.AddClaim(new Claim("adminID", admin.ID.ToString()));
                    identity.AddClaim(new Claim("userName", context.UserName));
                    identity.AddClaim(new Claim("role", "admin"));

                    context.Validated(identity);
                }
                else
                {
                    context.SetError("invalid_grant", "The Username or Password is incorrect");
                }

            }
            else if (company != null)
            {

                if (context.UserName == company.Email && VerifyPasswordHash(context.Password.ToString(), company.PasswordHash, company.PasswordSalt))
                {

                    // CORS ayarlarını set ediyoruz.
                    context.OwinContext.Response.Headers.Add("Access-Control-Allow-Origin", new[] { "*" });

                    // Kullanıcının access_token alabilmesi için gerekli validation işlemlerini yapıyoruz.
                    var identity = new ClaimsIdentity(context.Options.AuthenticationType);

                    identity.AddClaim(new Claim("companyID", company.ID.ToString()));
                    identity.AddClaim(new Claim("userName", context.UserName));
                    identity.AddClaim(new Claim("role", "company"));

                    context.Validated(identity);
                }
                else
                {
                    context.SetError("invalid_grant", "The Username or Password is incorrect");
                }
            }
            else if (staff != null)
            {

                if (context.UserName == staff.Email && VerifyPasswordHash(context.Password.ToString(), staff.PasswordHash, staff.PasswordSalt))
                {
                    // CORS ayarlarını set ediyoruz.
                    context.OwinContext.Response.Headers.Add("Access-Control-Allow-Origin", new[] { "*" });

                    // Kullanıcının access_token alabilmesi için gerekli validation işlemlerini yapıyoruz.
                    var identity = new ClaimsIdentity(context.Options.AuthenticationType);

                    identity.AddClaim(new Claim("companyID", staff.CompanyID.ToString()));
                    identity.AddClaim(new Claim("userName", context.UserName));
                    identity.AddClaim(new Claim("role", "staff"));
                    identity.AddClaim(new Claim("staffID", staff.ID.ToString()));

                    context.Validated(identity);
                }
                else
                {
                    context.SetError("invalid_grant", "The Username or Password is incorrect");
                }
            }
            else { context.SetError("invalid_grant", "The Username or Password is incorrect"); }
        }


        private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512(passwordSalt))
            {

                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));

                for (int i = 0; i < computedHash.Length; i++)
                {
                    if (computedHash[i] != passwordHash[i])
                    {
                        return false;
                    }
                }
            }
            return true;
        }
    }
}