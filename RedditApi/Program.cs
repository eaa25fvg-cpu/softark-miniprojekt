using Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();

builder.Services.AddDbContext<PostContext>(options =>
    options.UseSqlite(
        builder.Configuration.GetConnectionString("ContextSQLite")));

var app = builder.Build();