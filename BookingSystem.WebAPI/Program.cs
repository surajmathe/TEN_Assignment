using BookingSystem.Core.Features.ImportInventory;
using BookingSystem.DataAccessLayer;
using BookingSystem.DataAccessLayer.Repository;
using Microsoft.EntityFrameworkCore;

namespace BookingSystem.WebAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();

            // Add repository dependencies.
            builder.Services.AddScoped<IMemberRepository, MemberRepository>();
            builder.Services.AddScoped<IInventoryRepository, InventoryRepository>();
            builder.Services.AddScoped<IBookingDetailRepository, BookingDetailRepository>();

            builder.Services.AddMediatR((config) =>
            {
                config.RegisterServicesFromAssemblies(typeof(ImportInventoryRequest).Assembly);
            });

            builder.Services.AddDbContext<AppDbContext>((options) =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("BookingSystem"));
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();
                        
            app.Run();
        }
    }
}
