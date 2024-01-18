using IdentityServer4;
using IdentityServer4.Models;
using IdentityServer4.Test;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.Security.Cryptography;

var builder = WebApplication.CreateBuilder(args);
var isBuilder = builder.Services.AddIdentityServer();
//isBuilder.AddInMemoryClients(new List<Client>
//{
//    new Client
//    {
//        ClientId = "client",
//        AllowedGrantTypes = GrantTypes.ClientCredentials,
//        ClientSecrets =
//        {
//            new Secret("secret".Sha256())
//        },
//        AllowedScopes = {"api1"}
//    }
//});

//isBuilder.AddInMemoryApiScopes(new List<ApiScope>
//  {
//      new ApiScope("api1", "My API")
//  });

//isBuilder.AddInMemoryApiResources(new List<ApiResource>
//  {
//      new ApiResource("myResource")
//      {
//          Scopes = { "api1" },
//          UserClaims = { "myClaim" }
//      }
//  });

//isBuilder.AddInMemoryIdentityResources(new List<IdentityResource>
//  {
//      new IdentityResources.OpenId()
//  });

//var rsa = RSA.Create();
//var rsaKey = new RsaSecurityKey(rsa);
//isBuilder.AddSigningCredential(rsaKey, IdentityServerConstants.RsaSigningAlgorithm.RS256);
//isBuilder.AddAspNetIdentity<IdentityUser>();

//// Add services to the container.
//// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
//builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen();

//var app = builder.Build();

//// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
//    app.UseSwagger();
//    app.UseSwaggerUI();
//}

//app.UseIdentityServer();

//app.Run();

isBuilder.AddInMemoryClients(new List<Client>
{
    new Client
    {
        ClientId = "machine2machine",
        ClientSecrets = { new Secret("secret".Sha256()) },
        AllowedGrantTypes = GrantTypes.ClientCredentials,
        AllowedScopes = { "app" }
    },
    new Client
    {
        ClientId = "userInteractive",
        ClientSecrets = { new Secret("secret".Sha256()) },
        AllowedGrantTypes = GrantTypes.Code,
        AllowedScopes =
        {
            IdentityServerConstants.StandardScopes.OpenId,
            IdentityServerConstants.StandardScopes.Profile,
            "user"
        },
        RedirectUris =
        {
            "https://localhost:4002/signin-oidc"
        }
    }
});

// scopes available for clients to request
isBuilder.AddInMemoryApiScopes(new List<ApiScope>
{
    new ApiScope("app", "app"),
    new ApiScope("user", "user")
});

// user information that can be requested by the client
isBuilder.AddInMemoryIdentityResources(new List<IdentityResource>
{
    new IdentityResources.OpenId(),
    new IdentityResources.Profile()
});

// Add some users 
isBuilder.AddTestUsers(new List<TestUser>
{
    new TestUser
    {
        Username = "bob",
        Password = "password",
        SubjectId = "4145314a-06b3-4aa8-853b-666719d916c7",
        IsActive = true
    }
});

// Private/Public key pair required to sign JWTs
isBuilder.AddDeveloperSigningCredential();

//I've broken down the IS4 configuration to showcase individual configurations,
//but each of the methods above return the IS builder so a fluent syntax would chain them toghether
// builder.Services.AddIdentityServer(...)
//                 .AddInMemoryClients(...)
//                 .AddInMemoryApiScopes(...)
//                 .AddInMemoryIdentityResources(...);

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseIdentityServer();
app.UseStaticFiles();
app.UseRouting();
app.MapControllers();
app.MapDefaultControllerRoute();

app.Run();

