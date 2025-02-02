using AutoMapper;
using ToDoAPI;
using ToDoAPI.Data;
using ToDoAPI.Dtos;
using ToDoAPI.Entity;
using ToDoAPI.Extensions;
using ToDoAPI.Services;
using HttpVersion = System.Net.HttpVersion;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRouting(opts => { opts.LowercaseUrls = true; });
builder.Services.AddSingleton(_ => MapperConfig.Init());
builder.Services.AddDbContextFactory<AppDbContext>();
builder.Services.AddScoped<IToDoContext, ToDoContextTest>();
builder.Services.AddScoped<IOffsetTodoItemPagination, ToDoContextTest>();
builder.Services.AddScoped<IToDoItemSorter, ToDoContextTest>();
builder.Services.ConfigureHttpClientDefaults(b =>
{
	b.ConfigureHttpClient(client => { client.DefaultRequestVersion = HttpVersion.Version20; });
});
builder.Services.AddCors();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
	app.UseHsts();
}

app.UseCors(configure => { configure.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin(); });
app.UseHttpsRedirection();

#if DEBUG
app.EnsureDatabaseCreated();
#endif
app.MapEndpoints<ToDoEndpointMapper>();
app.Run();