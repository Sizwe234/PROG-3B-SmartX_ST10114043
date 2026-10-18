namespace SmartX.Api
{
	public class Program
	{
		public static void Main(string[] args)
		{
			var builder = WebApplication.CreateBuilder(args);

			// Add services to the container.
			builder.Services.AddControllers();
			builder.Services.AddEndpointsApiExplorer();
			builder.Services.AddSwaggerGen();

			// Add CORS policy to allow Dashboard (http://localhost:5260)
			builder.Services.AddCors(options =>
			{
				options.AddPolicy("AllowDashboard",
					policy => policy.WithOrigins("http://localhost:5260")
									.AllowAnyHeader()
									.AllowAnyMethod());
			});

			var app = builder.Build();

			// Configure the HTTP request pipeline.
			if (app.Environment.IsDevelopment())
			{
				app.UseSwagger();
				app.UseSwaggerUI();
			}

			app.UseHttpsRedirection();

			// Enable CORS before authorization
			app.UseCors("AllowDashboard");

			app.UseAuthorization();

			app.MapControllers();

			app.MapGet("/", () => "SmartX API is running. Try /WeatherForecast for data.");

			app.Run();
		}
	}
}
