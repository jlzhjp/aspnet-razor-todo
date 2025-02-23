using AspNetRazorTodo.WebApp.Models;
using MongoDB.Bson;
using MongoDB.Driver;

namespace AspNetRazorTodo.WebApp.Repositories;
public class TodoRepository(IMongoDatabase database) : ITodoRepository
{
    private readonly IMongoCollection<Todo> _todos = database.GetCollection<Todo>("todos");

    public async Task<Todo?> FindTodoByIdAsync(ObjectId id, CancellationToken cancellationToken)
    {
        var cursor = await _todos.FindAsync(
            filter: todo => todo.Id == id,
            cancellationToken: cancellationToken);
        return await cursor.SingleOrDefaultAsync(cancellationToken);
    }

    public async Task<long> DeleteAsync(ObjectId id, CancellationToken cancellationToken)
    {
        var result = await _todos.DeleteOneAsync(
            filter: todo => todo.Id == id,
            cancellationToken: cancellationToken);
        return result.DeletedCount;
    }

    public async Task<List<Todo>> FindTodosByUserIdAsync(ObjectId userId, CancellationToken cancellationToken)
    {
        var cursor = await _todos.FindAsync(
            filter: todo => todo.UserId == userId,
            cancellationToken: cancellationToken);
        return await cursor.ToListAsync(cancellationToken);
    }

    public async Task SaveAsync(Todo todo, CancellationToken cancellationToken)
    {
        await _todos.ReplaceOneAsync(
            filter: x => x.Id == todo.Id,
            replacement: todo,
            options: new ReplaceOptions { IsUpsert = true },
            cancellationToken: cancellationToken);
    }
}
