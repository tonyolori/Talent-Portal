//using Infrastructure.Data;
//using Microsoft.EntityFrameworkCore;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace Infrastructure.Migrations;

//public class ContextSeeder : DbContextSeeder<ApplicationDbContext>
//{
//    public override async Task SeedAsync(ApplicationDbContext context)
//    {
//        // Example: Seed a few users
//        if (!context.Users.Any())
//        {
//            context.Users.AddAsync(new User { Username = "admin", Password = "password" });
//            context.Users.AddAsync(new User { Username = "user1", Password = "user123" });
//        }

//        //prog.cs or dependency injection
//        //var host = CreateHostBuilder(args).Build();

//        //using (var scope = host.Services.CreateScope())
//        //{
//        //    var services = scope.ServiceProvider;
//        //    var seeder = services.GetRequiredService<MyContextSeeder>();

//        //    await seeder.SeedAsync(services.GetRequiredService<MyContext>());
//        //}
//        await context.SaveChangesAsync();
//    }
//}
