using BackEnd.Mappers.Abstractions;
using BackEnd.Mappers.Implementations;
using BackEnd.Services.Abstractions;
using BackEnd.Services.Implementations;

namespace BackEnd
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddScoped<IMapper, GeminiMapper>();
            builder.Services.AddHttpClient<ILLMService, GeminiService>();
            builder.Services.AddScoped<ILLMResponseAnalyserService, LLMResponseAnalyserService>();
            builder.Services.AddTransient<IFormSubmissionService, FormSubmissionService>();
            
            var configuration = builder.Configuration;           
            var allowedOrigins = configuration["AllowedOrigins"]?.Split(',');

            if (allowedOrigins != null)
                builder.Services.AddCors(options =>
                {
                    options.AddPolicy("CorsPolicy", corsPolicyBuilder =>
                    {
                        corsPolicyBuilder.WithOrigins(allowedOrigins)
                            .AllowAnyMethod()
                            .AllowAnyHeader();
                    });
                });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseCors("CorsPolicy");

            app.UseHttpsRedirection();

            app.UseAuthorization();
                     
            app.MapControllers();

            app.Run();
        }
    }
}
