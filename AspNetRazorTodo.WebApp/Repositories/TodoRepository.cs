using AspNetRazorTodo.WebApp.Models;
using MongoDB.Bson;
using MongoDB.Driver;

namespace AspNetRazorTodo.WebApp.Repositories;
public class TodoRepository(IMongoDatabase database) : ITodoRepository
{
    private readonly IMongoCollection<Todo> _todos = database.GetCollection<Todo>("todos");

    public async Task<long> DeleteAsync(ObjectId id)
    {
        var result = await _todos.DeleteOneAsync(Builders<Todo>.Filter.Eq(todo => todo.Id, id));
        return result.DeletedCount;
    }

    public async Task<List<Todo>> FindTodosAsync()
    {
        return await _todos.Find(Builders<Todo>.Filter.Empty).ToListAsync();
    }

    public async Task Save(Todo todo)
    {
        await _todos.InsertOneAsync(todo);
    }
}
