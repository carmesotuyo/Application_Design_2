using BlogsApp.Factory;
using BlogsApp.WebAPI.Filters;

var builder = WebApplication.CreateBuilder(args);
var AllowAllOrigins = "_allowAllOrigins";


// Add services to the container.
builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: AllowAllOrigins,
                      policy =>
                      {
                          policy
                          .AllowAnyOrigin()
                          .AllowAnyMethod()
                          .AllowAnyHeader();
                      });
});


ServiceFactory factory = new ServiceFactory(builder.Services);
factory.AddCustomServices();
factory.AddDbContextService();
builder.Services.AddScoped<AuthorizationFilter>();
builder.Services.AddScoped<ExceptionFilter>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.UseCors(AllowAllOrigins);

app.Run();

