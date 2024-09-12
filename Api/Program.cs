using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Reflection;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var corsOrigins = builder.Configuration["Cors:AllowOrigin"].Split(',', StringSplitOptions.RemoveEmptyEntries);

builder.Services.AddCors(opt =>
{
    opt.AddDefaultPolicy(builder =>
    {
        if (corsOrigins.Contains("*"))
        {
            builder.SetIsOriginAllowed(_ => true);
        }
        else
        {
            builder.WithOrigins(corsOrigins);
        }

        builder.AllowAnyMethod();
        builder.AllowAnyHeader();
        builder.AllowCredentials();
    });
});

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "BeerAfterWork ", Version = "v1" });
    c.SchemaGeneratorOptions = new SchemaGeneratorOptions { SchemaIdSelector = type => type.FullName };
    c.CustomSchemaIds(s => s.FullName.Replace("+", "."));

    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    c.IncludeXmlComments(xmlPath);
});

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "BeerAfterWork v1");
    c.RoutePrefix = string.Empty;
});


app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});


app.Run();
