using AutoMapper;
using ToDoList;
using ToDoList.Data;
using ToDoList.Extensions;
using ToDoList.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRouting(opts => { opts.LowercaseUrls = true; });
builder.Services.AddSingleton(_ => MapperConfig.Init());
builder.Services.AddControllersWithViews();
builder.Services.AddDbContextFactory<AppDbContext>();
builder.Services.AddScoped<IToDoContext, ToDoContextTest>();
builder.Services.AddScoped<IOffsetTodoItemPagination, ToDoContextTest>();
builder.Services.AddScoped<ITodoItemSorter, ToDoContextTest>();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
	app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();
app.MapControllerRoute(
	name: "default",
	pattern: "{controller=ToDo}/{action=Index}");

#if DEBUG
app.EnsureDatabaseCreated();
#endif

app.Run();

// Span<char> span = stackalloc char[24];
// IOffsetPaginationBuild build = null!;
//
// build.SelectEntity(() =>
// {
// 	var dbContext = app.Services.GetRequiredService<AppDbContext>();
//
// 	return dbContext.ToDoItems;
// }).Skip(5).Take(10).GetResult();