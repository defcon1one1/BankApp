using BankApp.Api.Middleware;
using BankApp.Core.Extensions;
using BankApp.Core.Notifications;
using BankApp.Infrastructure.DAL.Seeders;
using BankApp.Infrastructure.Extensions;

WebApplicationBuilder builder = WebApplication.CreateBuilder();

builder.Services.AddCore(builder.Configuration);
builder.Services.AddInfrastructure(builder.Configuration);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

WebApplication app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Perform database seeding
SeedDatabase(app);

app.UseMiddleware<ExceptionHandlingMiddleware>();

app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.UseEndpoints(endpoints =>
{
    endpoints.MapHub<NotificationHub>("/notificationHub");
});

app.UseStaticFiles();

app.Run();


// ensure database is created and seeded before usage
static void SeedDatabase(WebApplication app)
{
    using IServiceScope scope = app.Services.CreateScope();
    IDatabaseSeeder seeder = scope.ServiceProvider.GetRequiredService<IDatabaseSeeder>();
    seeder.SeedDatabase(scope.ServiceProvider);
}
