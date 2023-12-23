using ContactBook.Interfaces;
using ContactBook.Services;
using Microsoft.Extensions.DependencyInjection;

class Program
{
    // This runs and starts the program 
    static void Main()
    {
        var serviceProvider = SetupDependencyInjection();

        serviceProvider.GetRequiredService<IContactMenuService>().ShowMainMenu();
    }

    //This code setups so that the initializations only should be run once 
    private static IServiceProvider SetupDependencyInjection()
    {
        return new ServiceCollection()
            .AddSingleton<IFileService, FileService>()
            .AddSingleton<ICustomerService, CustomerService>()
            .AddSingleton<IContactMenuService, ContactBookMenuService>()
            .BuildServiceProvider();
    }
}
