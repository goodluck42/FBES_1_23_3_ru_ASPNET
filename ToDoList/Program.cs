using AutoMapper;
using ToDoList;
using ToDoList.Data;
using ToDoList.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRouting(opts => { opts.LowercaseUrls = true; });
builder.Services.AddKeyedSingleton<IToDoContext, ToDoContextLocal>(ServiceStaticKeys.ToDoService);
builder.Services.AddSingleton(_ => MapperConfig.Init());
builder.Services.AddControllersWithViews();
builder.Services.AddDbContextFactory<AppDbContext>();

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

app.Run();


IOffsetPaginationBuild build = null!;

build.SelectEntity(() =>
{
	var dbContext = app.Services.GetRequiredService<AppDbContext>();

	return dbContext.ToDoItems;
}).Skip(5).Take(10).GetResult();