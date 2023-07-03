using CoreVentas.Models;
using Microsoft.EntityFrameworkCore;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//JERM
builder.Services.AddDbContext<DbreactVentaContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("CoreVentas")));
//JERM
builder.Services.AddControllersWithViews()
    .AddNewtonsoftJson(options =>
    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
);
var app = builder.Build();


//JERM

app.UseCors(Policy => Policy.AllowAnyHeader()
                            .AllowAnyMethod()
                            .AllowAnyOrigin()
                            .SetIsOriginAllowed(origin => true));
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
