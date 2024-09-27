using Application.Interfaces;
using Domain.Common.Entities;
using Domain.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data;
public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : IdentityDbContext<BaseUser>(options), IApplicationDbContext
{
    public DbSet<Student> Students { get; set; }

    public DbSet<Teacher> Teachers { get; set; }

    public DbSet<LearningAdmin> LearningAdmins { get; set; }
    
    public DbSet<Instructor> Instructors { get; set; }

    public DbSet<Module> Modules { get; set; }
    
    public DbSet<StudentModule> StudentModules { get; set; }

    public DbSet<ModuleTask> Tasks { get; set; }
    
    public DbSet<Programme> Programmes { get; set; }
    
    public DbSet<Quiz> Quizzes { get; set; }
    
    public DbSet<Question> Questions { get; set; }
    
    public DbSet<Answer> Answers { get; set; }

    public DbSet<Topic> Topics { get; set; }
    
    public DbSet<Transaction> Transactions { get; set; }
    public DbSet<SubmissionDetails> SubmissionDetails { get; set; }
    public DbSet<Notification> Notifications { get; set; }


    protected override void OnModelCreating(ModelBuilder builder)
    {
        //builder.Entity<SubmissionDetails>()
        //    .HasKey(t => new { t.TaskId, t.StudentId });
        builder.Entity<SubmissionDetails>()
            .HasIndex(s => new { s.TaskId, s.StudentId })
            .IsUnique(true);
        
        builder.Entity<StudentModule>()
            .HasIndex(sm => new { sm.StudentId, sm.ModuleId })
            .IsUnique(true);
        
        // Configure Answer-Question relationship
        builder.Entity<Answer>()
            .HasOne(a => a.Question)
            .WithMany(q => q.Options)
            .HasForeignKey(a => a.QuestionId)
            .OnDelete(DeleteBehavior.Restrict); // or DeleteBehavior.NoAction
        
        // Configuring the relationship between Module and Topic  
        builder.Entity<Topic>()  
            .HasOne(t => t.Module)
            .WithMany(m => m.Topics)  
            .HasForeignKey(t => t.ModuleId) 
            .OnDelete(DeleteBehavior.Cascade); // 

        ////Relationship has been removed, tasks are now related to a program
        //// Configuring the relationship between Module and ModuleTask  
        //builder.Entity<ModuleTask>()  
        //    .HasOne(mt => mt.Module)  
        //    .WithMany(m => m.ModuleTasks)  
        //    .HasForeignKey(mt => mt.ModuleId) 
        //    .OnDelete(DeleteBehavior.Cascade);

        // Configuring the relationship between Module and Quiz  
        builder.Entity<Quiz>()  
            .HasOne(q => q.Module)  
            .WithMany(m => m.Quizzes)  
            .HasForeignKey(q => q.ModuleId) 
            .OnDelete(DeleteBehavior.Cascade); 

        // Configuring the relationship between Module and Programme  
        // builder.Entity<Module>()  
        //     .HasOne(m => m.Programme)  
        //     .WithMany(p => p.Modules)  
        //     .HasForeignKey(m => m.ProgrammeId)  
        //     .OnDelete(DeleteBehavior.Restrict);

        // Configure Question-Quiz relationship 
        builder.Entity<Question>()
            .HasOne(q => q.Quiz)
            .WithMany(qu => qu.Questions)
            .HasForeignKey(q => q.QuizId)
            .OnDelete(DeleteBehavior.Restrict); 
        
        
        // Configuring the relationship between Quiz and Student 
        builder.Entity<Quiz>()
            .HasOne(q => q.Student)
            .WithMany(u => u.Quizzes)
            .HasForeignKey(q => q.StudentId)
            .OnDelete(DeleteBehavior.NoAction);        
        
        builder.Entity<Transaction>(entity =>  
        {  
            entity.Property(e => e.Amount)  
                .HasColumnType("decimal(18, 2)") // Specify decimal type with precision and scale  
                .HasPrecision(18, 2); // You can also use HasPrecision for clarity  
        }); 

        base.OnModelCreating(builder);
    }

}
