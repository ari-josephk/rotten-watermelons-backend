using Flix.Settings;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;

services.Configure<FlixDatabaseSettings>(
    builder.Configuration.GetSection(nameof(FlixDatabaseSettings)));

services.AddFlixServices();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseServiceStack(new AppHost(), options => {
    options.MapEndpoints();
});

app.Run();