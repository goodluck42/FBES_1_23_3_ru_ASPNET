using AutoMapper;
using ToDoList;
using ToDoList.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRouting(opts => { opts.LowercaseUrls = true; });
builder.Services.AddKeyedSingleton<IToDoContext, ToDoContextLocal>(ServiceStaticKeys.ToDoService);
builder.Services.AddSingleton(_ => MapperConfig.Init());
builder.Services.AddControllersWithViews();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
	// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
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