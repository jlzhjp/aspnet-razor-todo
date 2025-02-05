using AspNetRazorTodo.WebApp.Models;
using AspNetRazorTodo.WebApp.Repositories;

namespace AspNetRazorTodo.WebApp.Services;

public class ApplicationStartupService(ITodoRepository todoRepository) : IHostedService
{
    public Task StartAsync(CancellationToken cancellationToken)
    {
        todoRepository.SaveAsync(new Todo("Study ASP.NET", false));
        todoRepository.SaveAsync(new Todo("Do Laundry", false));
        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
}
