using Infrastructure;
using WebApi;
using Application;
using WebApi.Middlewares;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddDatabase(builder.Configuration);
builder.Services.AddIdentitySettings();
builder.Services.AddApplicationService();
builder.Services.AddJwtAuthentication(builder.Services.GetApplicationSettings(builder.Configuration));
builder.Services.AddIdentityServices();
builder.Services.AddEmployeeServices();
builder.Services.AddInfrastructureDependencies();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.RegisterSwagger();

var app = builder.Build();
app.SeedDatabase();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseMiddleware<ErrorHandlingMiddleware>();
app.MapControllers();

app.Run();
