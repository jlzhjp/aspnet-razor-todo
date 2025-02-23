using AspNetRazorTodo.WebApp.Identity;
using AspNetRazorTodo.WebApp.Repositories;

var builder = WebApplication.CreateBuilder(args);

builder.AddMongoDBClient(connectionName: "tododb");

// Add services to the container.
builder.Services
    .AddDefaultIdentity<SimpleTodoUser>()
    .AddUserStore<SimpleTodoUserStore>();

builder.Services.AddRazorPages();
builder.Services.AddSingleton<ITodoRepository, TodoRepository>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapStaticAssets();

app.MapRazorPages().WithStaticAssets();

app.Run();
