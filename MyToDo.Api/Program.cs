using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using MyToDo.Api;
using MyToDo.Api.Context;
using MyToDo.Api.Context.Repository;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{

    c.SwaggerDoc("v1", new OpenApiInfo { Title = "MyToDo.Api", Version = "v1" });

});
builder.Services.AddDbContext<MyToDoContext>(option =>
{
    var constr = builder.Configuration.GetSection("ConnectionStrings")["ToDoConnection"];
    option.UseSqlite(constr);
}).AddUnitOfWork<MyToDoContext>()
.AddCustomRepository<ToDo, ToDoRepository>()
.AddCustomRepository<Memo, MemoRepository>()
.AddCustomRepository<User, UserRepository>();
var app = builder.Build();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
