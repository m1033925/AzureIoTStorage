using AzureStorage.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddScoped<ITableStorageService, TableStorageService>();
builder.Services.AddScoped<IQueueStorageService, QueueCRUD>();
builder.Services.AddScoped<IFileStorage, FileShareStorage>();
builder.Services.AddScoped<IBlobStorage, BlobStorage>();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    //app.UseSwagger();
    //app.UseSwaggerUI();
}

app.UseSwagger();
app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "SandhyaAzureStorageAPI V1"));

//app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
