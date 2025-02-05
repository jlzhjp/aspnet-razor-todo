using AspNetRazorTodo.WebApp.Models;
using MongoDB.Bson;

namespace AspNetRazorTodo.WebApp.Repositories;

public interface ITodoRepository
{
    public Task<List<Todo>> FindTodosAsync();
    public Task<long> DeleteAsync(ObjectId id);
    public Task Save(Todo todo);
}
