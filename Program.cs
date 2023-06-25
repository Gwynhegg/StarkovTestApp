using System.Text.Json.Serialization;
using StarkovTestApp.DbLayer;
using StarkovTestApp.Services;
using StarkovTestApp.Services.Interfaces;
using StarkovTestApp.Utilities;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// регистрируем DbContext
builder.Services.AddDbContext<DbLayerContext>(ServiceLifetime.Scoped);

// регистрируем сервисы
builder.Services.AddScoped<ILinkService, LinkService>();
builder.Services.AddScoped<IValidateService, ValidateService>();
builder.Services.AddScoped<IUploadService, UploadService>();
builder.Services.AddScoped<IOrganizationService, OrganizationService>();
builder.Services.AddScoped<IResultBuilder, ResultBuilder>();
builder.Services.AddScoped<ITableService, TableService>();
builder.Services.AddScoped<IErrorService, ErrorService>();

builder.Services.AddControllers()
    .AddJsonOptions(options => options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.OperationFilter<FileUploadOperation>();
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseDeveloperExceptionPage();
}

//app.UseAuthorization();

app.MapControllers();


app.Run();
