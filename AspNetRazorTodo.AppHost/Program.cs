var builder = DistributedApplication.CreateBuilder(args);

var mongo = builder.AddMongoDB("mongo")
    .WithMongoExpress()
    .WithLifetime(ContainerLifetime.Persistent);

var todoDb = mongo.AddDatabase("tododb");

builder.AddProject<Projects.AspNetRazorTodo_WebApp>("aspnetrazortodo-webapp")
    .WithReference(todoDb)
    .WaitFor(todoDb);

builder.Build().Run();
