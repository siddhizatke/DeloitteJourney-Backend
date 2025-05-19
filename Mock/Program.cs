using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using Microsoft.OpenApi.Models;
using Mock.Data;
using Mock.Repository;

var builder = WebApplication.CreateBuilder(args);

// Add the database context and configure it to use SQL Server with the connection string from appsettings.json
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Register the FileService implementation for the IFileService interface
builder.Services.AddScoped<IFileService, FileService>();

// Add support for controllers
builder.Services.AddControllers();

// Configure Swagger/OpenAPI for API documentation
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "DeloitteTrainingApp API", Version = "v1" });
});

// Configure CORS to allow requests from the Angular application
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngularApp", policy =>
    {
        policy.WithOrigins("http://localhost:4200") // Replace with the actual URL of your Angular app
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    // Enable Swagger UI and developer exception page in development mode
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "DeloitteTrainingApp API");
    });
    app.UseDeveloperExceptionPage();
}

// Enable CORS for the specified policy
app.UseCors("AllowAngularApp");

// Redirect HTTP requests to HTTPS
app.UseHttpsRedirection();

// Enable authorization middleware
app.UseAuthorization();

// Map controller endpoints
app.MapControllers();

// Serve static files from the default wwwroot directory
app.UseStaticFiles();

// Serve static files from the "Photos" directory and map them to the "/Photos" URL path
app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(Path.Combine(builder.Environment.ContentRootPath, "Photos")),
    RequestPath = "/Photos"
});

// Enable directory browsing for the "Photos" directory
app.UseDirectoryBrowser(new DirectoryBrowserOptions
{
    FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "Photos")),
    RequestPath = "/Photos"
});

// Run the application
app.Run();
