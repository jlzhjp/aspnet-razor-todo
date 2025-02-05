using AspNetRazorTodo.WebApp.Models;
using AspNetRazorTodo.WebApp.Repositories;

namespace AspNetRazorTodo.WebApp.Services;

public class ApplicationStartupService(ITodoRepository todoRepository) : IHostedService
{
    public Task StartAsync(CancellationToken cancellationToken)
    {
        todoRepository.Save(new Todo("Study ASP.NET", false));
        todoRepository.Save(new Todo("Do Laundry", false));
        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
}
