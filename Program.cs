var builder = WebApplication.CreateBuilder(args);
// Add services to the container.
const string nombre = "POLICY";
builder.Services.AddCors(config =>
{
    config.AddPolicy(nombre, policy =>
    {
        policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
    });
});
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();



var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors(nombre);

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
