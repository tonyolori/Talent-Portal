//using Domain.Entities;
//using Domain.Enum;
//using Infrastructure.Migrations;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using System.Web.Helpers;

//namespace Infrastructure.Data;
//public class SeedData
//{
//    public static void Initialize(ApplicationDbContext context)
//    {
//        if (!context.Database.EnsureCreated())
//        {
//            return;
//        }
//        //context.Database.EnsureCreated();

//        // Check if any data already exists
//        if (context.Modules.Any())
//        {
//            return;   // Database has been seeded
//        }

//        // Define sample data for Programmes table
//        List<Programme> programmes =
//            [
//                new Programme { Id = 1, Type = "FrontEnd Development" },
//                new Programme { Id = 2, Type = "BackEnd Development" },
//                new Programme { Id = 3, Type = "ProjectManagement" },
//                new Programme { Id = 4, Type = "UIUXDesign" },

//            ];
//        context.AddRange(programmes);

//        List<Student> students =
//            [
//                new Student()
//                {
//                    // Set properties for the first student
//                    UserStatus = Status.Active,
//                    UserStatusDes = "Active Student",
//                    IsVerified = true,
//                    EnrollmentDate = DateTime.Now,
//                    ProgrammeId = 1, // Assuming the first Programme is Undergraduate
//                    ApplicationType = ApplicationType.BootCamp,
//                    PaymentTypeDes = "Card Payment"
//                },
//            new Student()
//            {
//                UserName = "Student1",
//                Email = "Student1@gmail.com",
//                FirstName = "Student1 FirstName",
//                LastName = "Student1 LastName",
//                EnrollmentDate = DateTime.Today,
//                Role = UserRoles.Student,
//                RoleDesc = UserRoles.Student.ToString(),
//                IsVerified = true,
//                UserStatus = Status.Active,
//                UserStatusDes = Status.Active.ToString(),
//                ProgrammeId = 1,
//                ApplicationType = ApplicationType.BootCamp,
//                PaymentTypeDes = ApplicationType.BootCamp.ToString(),
//                SecurityStamp = Guid.NewGuid().ToString(),
//                LastModifiedDate = DateTime.UtcNow,
//            }

//            ];
//        context.Students.AddRange(students);


//        // Define sample data for Modules table
//        List<Module> modules =
//            [
//                new Module
//                {
//                    Title = "Introduction to Programming",
//                    ModuleImageUrl = "https://example.com/image1.jpg",
//                    Description = "A comprehensive introduction to programming concepts...",
//                    Objectives = "Students will be able to write basic programs in C# by the end of this module.",
//                    FacilitatorName = "John Doe",
//                    FacilitatorId = "12345",
//                    Timeframe = "2 weeks",
//                    Progress = "Not Started",
//                    AdditionalResources = "https://example.com/resources1.pdf",
//                    ProgrammeId = 1
//                },
//                new Module
//                {
//                    Title = "Introduction to Programming 2",
//                    ModuleImageUrl = "https://example.com/image1.jpg",
//                    Description = "A comprehensive introduction to programming concepts...",
//                    Objectives = "Students will be able to write basic programs in C# by the end of this module.",
//                    FacilitatorName = "John Doe",
//                    FacilitatorId = "12345",
//                    Timeframe = "2 weeks",
//                    Progress = "Not Started",
//                    AdditionalResources = "https://example.com/resources1.pdf",
//                    ProgrammeId = 1
//                },
//            ];
//        context.AddRange(modules);

//        var tasks =
//            [///stopped here
                
//            ];
//        // Add the task to your context or object collection
//        context.Tasks.Add(tasks);

//        context.SaveChanges();
//    }
//}