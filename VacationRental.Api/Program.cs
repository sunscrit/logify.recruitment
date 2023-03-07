using Microsoft.OpenApi.Models;
 
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddSwaggerGen(opts => opts.SwaggerDoc("v1", new OpenApiInfo { Title = "Vacation rental information", Version = "v1" }));

builder.Services.AddInfrastructureServices();
builder.Services.AddApplicationServices();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});

app.UseSwagger();
app.UseSwaggerUI(opts => opts.SwaggerEndpoint("/swagger/v1/swagger.json", "VacationRental v1"));

app.Run();

public partial class Program { }