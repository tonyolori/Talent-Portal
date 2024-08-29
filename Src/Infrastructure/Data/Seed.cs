//using Domain.Entities;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace Infrastructure.Data;
//public class SeedData
//{
//    public static void Initialize(ApplicationDbContext context)
//    {
//        context.Database.EnsureCreated();

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

//        // Define sample data for Modules table
//        List<ModuleController> modules =
//            [
//                new ModuleController
//                {
//                    Id = 1,
//                    GuId = Guid.NewGuid(),
//                    Title = "Introduction to Programming",
//                    ModuleImageUrl = "https://example.com/image1.jpg",
//                    Topics = ["Variables, Data Types, Operators"],
//                    Description = "A comprehensive introduction to programming concepts...",
//                    Objectives = "Students will be able to write basic programs in C# by the end of this module.",
//                    FacilitatorName = "John Doe",
//                    FacilitatorId = "12345",
//                    StartDate = DateTime.Now,
//                    EndDate = DateTime.Now.AddDays(7),
//                    Timeframe = "2 hours per week",
//                    Progress = "Not Started",
//                    AdditionalResources = "https://example.com/resources1.pdf",
//                    ProgrammeId = 1 // Associate module with a programme
//                },
//                // Add more modules with desired data and programme associations
//            ];
//        context.AddRange(modules);


//var task = new ModuleTask
//{
//    Title = "Create a Basic HTML Page",
//    Description = "Build a simple web page with text and images.",
//    Instructions = "1. Create a new HTML file. 2. Add basic structure (<!DOCTYPE html>, <html>, <head>, <body>). 3. Insert text and images using appropriate tags.",
//    SubmissionLink = "https://example.com/submit-task",
//    FacilitatorFeedBack = null, // Initially empty
//    ModuleId = module.Id // Associate task with the created module
//};

//// Add the task to your context or object collection
//context.ModuleTasks.Add(task);

//var module = new ModuleController
//{
//    Title = "Web Development Fundamentals",
//    ModuleImageUrl = "https://example.com/webdev-image.jpg",
//    Topics = new List<string> { "HTML", "CSS", "JavaScript Basics" },
//    Description = "This module provides a solid foundation in essential web development technologies...",
//    Objectives = "By the end of this module, students will be able to create basic web pages using HTML, CSS, and JavaScript.",
//    FacilitatorName = "Jane Smith",
//    FacilitatorId = "56789",
//    StartDate = DateTime.Now.AddDays(1), // Start date one day from now
//    EndDate = DateTime.Now.AddDays(14), // End date two weeks from now
//    Timeframe = "3 hours per week",
//    ProgrammeId = programme.Id, // Associate module with the created programme
//    Progress = "Not Started",
//    AdditionalResources = "https://example.com/webdev-resources.pdf",
//    ModuleTasks = new List<ModuleTask>() // Empty list for future tasks
//};
//// Add the module to your context or object collection
//context.Modules.Add(module);
//        // Define sample data for other tables (similar to Modules)
//        var roles = new List<AspNetRole>()
//            {
//                new AspNetRole { Id = "1", Name = "Administrator", NormalizedName = "ADMINISTRATOR" },
//                new AspNetRole { Id = "2", Name = "StudentController", NormalizedName = "STUDENT" }
//            };
//        context.AddRange(roles);

//        // Users with assigned roles and programmes (modify as needed)
//        var users = new List<AspNetUser>()
//            {
//                new AspNetUser
//                {
//                    Id = "user1",
//                    FirstName = "John",
//                    LastName = "Doe",
//                    DateOfBirth = DateTime.Parse("1990-01-01"),
//                    Role = 1, // Reference role ID from roles list
//                    RoleDesc = "Administrator",
//                    IsVerified = true,
//                    UserStatus = 1, // Define user status
//                    UserStatusDes = "Active",
//                    ProgrammeID = 1, // Reference programme ID
//                    EnrollmentDate = DateTime.Now,
//                    // ... other user details
//                },
//                new AspNetUser
//                {
//                    Id = "user2",
//                    FirstName = "Jane",
//                    LastName = "Smith",
//                    DateOfBirth = DateTime.Parse("1995-02-15"),
//                    Role = 2, // Reference role ID from roles list
//                    RoleDesc = "StudentController",
//                    IsVerified = true,
//                    UserStatus = 1, // Define user status
//                    UserStatusDes = "Active",
//                    ProgrammeID = 1, // Reference programme ID
//                    EnrollmentDate = DateTime.Now,
//                    // ... other user details
//                }
//            };
//        context.AddRange(users);

//        // Add sample data for other tables like Tasks and Grades (if applicable)

//        context.SaveChanges();
//    }
//}