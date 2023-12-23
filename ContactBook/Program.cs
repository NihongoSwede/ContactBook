using ContactBook.Interfaces;
using ContactBook.Services;
using Microsoft.Extensions.DependencyInjection;

class Program
{
    static void Main()
    {
        var serviceProvider = SetupDependencyInjection();

        serviceProvider.GetRequiredService<IContactMenuService>().ShowMainMenu();
    }

    private static IServiceProvider SetupDependencyInjection()
    {
        return new ServiceCollection()
            .AddSingleton<IFileService, FileService>()
            .AddSingleton<ICustomerService, CustomerService>()
            .AddSingleton<IContactMenuService, ContactBookMenuService>()
            .BuildServiceProvider();
    }
}
