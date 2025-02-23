using Microsoft.AspNetCore.Identity;
using MongoDB.Bson;

namespace AspNetRazorTodo.WebApp.Identity;

public class SimpleTodoUser : IdentityUser<ObjectId>;