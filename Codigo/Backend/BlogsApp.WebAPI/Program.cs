//using BlogsApp.DataAccess; // WEBAPI YA NO DEPENDE DE DATA ACCESS, DESACOPLAMOS DE LA IMPLEMENTACION
using BlogsApp.WebAPI;

var builder = WebApplication.CreateBuilder(args);
var startup = new Startup(builder.Configuration);
startup.ConfigureServices(builder.Services);

// COSAS QUE SE PASARON A STARTUP:

// Add services to the container.
//builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
//builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen();

//builder.Services.AddDbContext<Context>();

var app = builder.Build();
startup.Configure(app);

//CREO QUE DEBERIA IR EN STARTUP PERO NO ME ANDA ASI QUE LO DEJO ACA:
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//ESTO ACA ESTA BIEN:

app.UseAuthorization();

app.MapControllers();

app.Run();

