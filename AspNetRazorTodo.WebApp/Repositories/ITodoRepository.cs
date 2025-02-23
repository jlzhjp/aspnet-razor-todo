using AspNetRazorTodo.WebApp.Models;
using MongoDB.Bson;

namespace AspNetRazorTodo.WebApp.Repositories;

public interface ITodoRepository
{
    Task<List<Todo>> FindTodosByUserIdAsync(ObjectId userId, CancellationToken cancellationToken);
    Task<List<Todo>> FindTodosByUserIdAsync(ObjectId userId) => FindTodosByUserIdAsync(userId, CancellationToken.None);
    
    Task<Todo?> FindTodoByIdAsync(ObjectId id, CancellationToken cancellationToken);
    Task<Todo?> FindTodoByIdAsync(ObjectId id) => FindTodoByIdAsync(id, CancellationToken.None);

    Task<long> DeleteAsync(ObjectId id, CancellationToken cancellationToken);
    Task<long> DeleteAsync(ObjectId id) => DeleteAsync(id, CancellationToken.None);
    
    Task SaveAsync(Todo todo, CancellationToken cancellationToken);
    Task SaveAsync(Todo todo) => SaveAsync(todo, CancellationToken.None);
}
