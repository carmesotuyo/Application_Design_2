using BlogsApp.Factory;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//builder.Services.AddDbContext<Context>(); SE SUSTITUYE POR FACTORY PARA DESACOPLAR DE DATA ACCESS
ServiceFactory factory = new ServiceFactory(builder.Services);
factory.AddCustomServices();
factory.AddDbContextService(); //builder.Configuration.GetConnectionString("BlogsAppDBCarme")

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();

