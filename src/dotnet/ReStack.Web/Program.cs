using Blazored.LocalStorage;
using Blazored.Modal;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.IdentityModel.Tokens;
using ReStack.Application;

namespace ReStack.Web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddHttpClients(builder.Configuration);

            builder.Services.AddRazorPages();
            builder.Services.AddServerSideBlazor();
            builder.Services.AddBlazoredModal();
            builder.Services.AddHealthChecks();
            builder.Services.AddBlazoredLocalStorage();

            builder.Services.AddAuthentication(options =>
            {
                options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
            })
            .AddCookie()
            .AddOpenIdConnect(options =>
            {
                options.SignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.ResponseType = OpenIdConnectResponseType.Code;
                options.Authority = "https://auth.hlann.be/application/o/restack-floris";
                options.ClientId = "LhrhZCzJR1bYF5iiIZD20zw1Te77uSGU0XLKv4qd";
                options.ClientSecret = "oQTnQeOpbuZotMGH2lDTMx8tH1hoolMBmciGx7PDNg0uKA91Ol7rCREy2d1VSkQn0eLjg5mHH8cGx9ZiweygqxpWRaYVBXBQWE7YYpzUfc1cycWq7O7hbJrnGrfVbyRs";
                options.ResponseType = "code";
                options.SaveTokens = true;
                options.GetClaimsFromUserInfoEndpoint = true;
                options.UseTokenLifetime = false;
                options.Scope.Add("openid");
                options.Scope.Add("profile");
                options.Scope.Add("email");
                options.TokenValidationParameters = new()
                {
                    NameClaimType = "name"
                };

                options.Events = new OpenIdConnectEvents
                {
                    OnAccessDenied = context =>
                    {
                        context.HandleResponse();
                        context.Response.Redirect("/");

                        return Task.CompletedTask;
                    },
                    OnTokenValidated = c =>
                    {
                        Console.WriteLine(c);

                        return Task.CompletedTask;
                    }
                };
            });

            var app = builder.Build();

            JsonWebTokenHandler.DefaultInboundClaimTypeMap.Clear();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();

            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapBlazorHub();
            app.MapRazorPages();

            app.MapHealthChecks("/health");

            app.MapFallbackToPage("/_Host");

            app.Run();
        }
    }
}