using ContactBook.Interfaces;
using ContactBook.Services;
using Microsoft.Extensions.DependencyInjection;
using System;

class Program
{
    static void Main()
    {
        // Setup DI (Dependency Injection) container
        var serviceProvider = new ServiceCollection()
            .AddSingleton<IFileService, FileService>()  // Replace YourFileService with your actual implementation
            .AddSingleton<ICustomerService, CustomerService>()
            .AddSingleton<IContactMenuService, ContactBookMenuService>()
            .BuildServiceProvider();

        // Resolve the required service
        var menuService = serviceProvider.GetRequiredService<IContactMenuService>();

        // Call the main menu method
        menuService.ShowMainMenu();
    }
}
