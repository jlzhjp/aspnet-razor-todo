using AspNetRazorTodo.WebApp.Models;
using MongoDB.Bson;

namespace AspNetRazorTodo.WebApp.Repositories;

public interface ITodoRepository
{
    Task<List<Todo>> FindTodosAsync();
    Task<Todo?> FindTodoByIdAsync(ObjectId id);
    Task<long> DeleteAsync(ObjectId id);
    Task SaveAsync(Todo todo);
}
