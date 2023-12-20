using ContactBook.Interfaces;
using ContactBook.Services;
using Microsoft.Extensions.DependencyInjection;

class Program
{
    static void Main()
    {
        
            // Setup DI (Dependency Injection) container
            var serviceProvider = SetupDependencyInjection;

            // Resolve the required service
            var menuService = serviceProvider.GetRequiredService<IContactMenuService>();

            // Call the main menu method
            menuService.ShowMainMenu();
        
        
    }

    private static IServiceProvider SetupDependencyInjection
    {
        get
        {
            // Create a collection
            var serviceCollection = new ServiceCollection();

            // Register services
            serviceCollection.AddSingleton<IFileService, FileService>();
            serviceCollection.AddSingleton<ICustomerService, CustomerService>();
            serviceCollection.AddSingleton<IContactMenuService, ContactBookMenuService>();

            // Build the service provider
            return serviceCollection.BuildServiceProvider();
        }
    }
}
