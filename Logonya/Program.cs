using Domain.Account.Port;
using Domain.Common;
using Infrastructure.Persistence;
using Infrastructure.Persistence.Repository.Account;
using Microsoft.EntityFrameworkCore;
using Presentation.Extensions;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddRepositoriesAndUnitOfWork();


app.MapGet("/", async (IUnitOfWork unitOfWork) =>
{
    var repository = unitOfWork.GetRepository<UserRepositoryPort>();
    await repository.AddAsync(new Domain.Account.Model.User("fatih", "white", "sdada"));
    await unitOfWork.CompleteAsync();
});

app.Run();
