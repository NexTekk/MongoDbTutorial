using System;
using System.IO;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Tutorial.Data;

namespace MongoDbTutorials
{
    class Program
    {
        private static ServiceProvider _serviceProvider;

        static void Main(string[] args)
        {
            ConfigureDependencies();
             
            var user = AddUser("John", "Smith");

            Console.WriteLine("{0} {1} added with ID: {2}", user.FirstName, user.LastName, user.Id);
        }

        private static UserDocument AddUser(string firstName, string lastName)
        {
            var user = new UserDocument
            {
                Id = Guid.NewGuid(),
                FirstName = firstName,
                LastName = lastName
            };

            var repository = _serviceProvider.GetService<IUserRepository>();

            repository.Add(user);

            return user;
        }

        private static void ConfigureDependencies()
        {
            var currentAssembly = typeof(Program).Assembly;
            var currentDirectory = Path.GetDirectoryName(currentAssembly.Location);
            var configBuilder = new ConfigurationBuilder()
                .SetBasePath(currentDirectory)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
            var configuration = configBuilder.Build();
            var serviceCollection = new ServiceCollection();
            var dbSettings = new DbSettings();

            configuration.GetSection("dbSettings").Bind(dbSettings);

            _serviceProvider = serviceCollection
                                .AddScoped<IUserRepository, UserRepository>()
                                .AddScoped<DbContext>()
                                .AddSingleton<IDbSettings>(dbSettings)
                                .BuildServiceProvider();
        }
    }
}
