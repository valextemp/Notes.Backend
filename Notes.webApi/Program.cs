using Notes.Persistence;

using AutoMapper;
using Notes.Application.Common.Mappings;
using System.Reflection;
using Notes.Application;
using Notes.Application.Interfaces;
using Notes.webApi.Middleware;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Notes.webApi.Services;
using Serilog;

public class Program
{
    private static void Main(string[] args)
    {
        // Моя добавка для логирования через Serylog
        Log.Logger=new LoggerConfiguration()
            .MinimumLevel.Override("Microsoft", Serilog.Events.LogEventLevel.Information)
            .WriteTo.File("NotesWebAppLog-.txt", rollingInterval: RollingInterval.Day)
            .CreateLogger();

        var builder = WebApplication.CreateBuilder(args);

        //Моя добавка для использования Serilog
        builder.Host.UseSerilog();

        //my code
        // добавление сервисов
        builder.Services.AddAutoMapper(config =>
        {
            config.AddProfile(new AssemblyMappingProfile(Assembly.GetExecutingAssembly()));
            config.AddProfile(new AssemblyMappingProfile(typeof(INotesDbContext).Assembly));
        });
        builder.Services.AddApplication();
        builder.Services.AddPersistence(builder.Configuration);
        builder.Services.AddControllers();

        builder.Services.AddCors(options =>
        {
            options.AddPolicy("AllowAll", policy =>
            {
                policy.AllowAnyHeader();
                policy.AllowAnyMethod();
                policy.AllowAnyOrigin();
            });
        });

        //Мой код добавляю сервис аутентификации
        builder.Services.AddAuthentication(config =>
        {
            config.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            config.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
            .AddJwtBearer("Bearer", options =>
            {
                options.Authority = "https://localhost:44302/";
                options.Audience = "NotesWebAPI";
                options.RequireHttpsMetadata = false;
            });

        //end my code

        //Добавляю Swagger
        builder.Services.AddSwaggerGen();

        // моя добавка для логирования через Serilog
        builder.Services.AddSingleton<ICurrentUserService, CurrentUserService>();
        builder.Services.AddHttpContextAccessor();

        var app = builder.Build();

        //my code
        using (var scope = app.Services.CreateScope())
        {
            var serviceProvider = scope.ServiceProvider;
            try
            {
                var context=serviceProvider.GetRequiredService<NotesDbContext>();
                DbInitializer.Initialize(context);
            }
            catch (Exception exception)
            {
                Log.Fatal(exception, "An error occurred while app initialization");
            }
        }
        //end my code

        //Добавляю Middleware для Swagger
        app.UseSwagger();
        app.UseSwaggerUI();

        //my code
        //Добавление MiddleWare
        app.UseCustomExceptionHundler(); // Это мое MiddleWare
        app.UseRouting();
        app.UseHttpsRedirection();
        app.UseCors("AllowAll");

        //Мой код добавляю аутентификацию и авторизацию
        app.UseAuthentication();
        app.UseAuthorization(); 

        //Так в видео написано
        //app.UseEndpoints(endpoints =>
        //{
        //    endpoints.MapControllers();
        //});

        //Так я переделал
        app.MapControllers();

        //end my code

        //app.MapGet("/", () => "Hello World!");

        app.Run();
    }
}