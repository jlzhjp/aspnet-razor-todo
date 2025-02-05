using AspNetRazorTodo.WebApp.Models;
using MongoDB.Bson;
using MongoDB.Driver;

namespace AspNetRazorTodo.WebApp.Repositories;
public class TodoRepository(IMongoDatabase database) : ITodoRepository
{
    private readonly IMongoCollection<Todo> _todos = database.GetCollection<Todo>("todos");

    public async Task<Todo?> FindTodoByIdAsync(ObjectId id)
    {
        var cursor = await _todos.FindAsync(
            Builders<Todo>.Filter.Eq(todo => todo.Id, id));
        return await cursor.SingleOrDefaultAsync();
    }

    public async Task<long> DeleteAsync(ObjectId id)
    {
        var result = await _todos.DeleteOneAsync(
            Builders<Todo>.Filter.Eq(todo => todo.Id, id));
        return result.DeletedCount;
    }

    public async Task<List<Todo>> FindTodosAsync()
    {
        var cursor = await _todos.FindAsync(
            Builders<Todo>.Filter.Empty);
        return await cursor.ToListAsync();
    }

    public async Task SaveAsync(Todo todo)
    {
        await _todos.ReplaceOneAsync(
            Builders<Todo>.Filter.Eq(x => x.Id, todo.Id),
            todo,
            new ReplaceOptions { IsUpsert = true });
    }
}
