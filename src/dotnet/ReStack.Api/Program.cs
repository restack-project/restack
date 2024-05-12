
using PipShell;
using ReStack.Application;
using ReStack.Application.Notifications.Channels;
using ReStack.Application.Notifications.Hubs;
using ReStack.Common.Validators;
using ReStack.Persistence;

namespace ReStack.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args); 

            // Add services to the container.

            builder.Services.AddApplication(builder.Configuration);
            builder.Services.AddPersistence(builder.Configuration);

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddSignalR();
            builder.Services.AddHealthChecks();
            
            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MigrateDatabase(seedData: app.Environment.IsDevelopment());

            app.GenerateSshKey();

            //app.UpdatePip();

            app.MapControllers();

            app.MapHub<StackHub>("/hub/stack");

            app.MapHealthChecks("/health");

            app.Run();
        }
    }
}