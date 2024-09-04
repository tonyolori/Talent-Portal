﻿using Application.Interfaces;
using Domain.Common.Entities;
using Domain.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Reflection.Emit;

namespace Infrastructure.Data;
public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : IdentityDbContext<BaseUser>(options), IApplicationDbContext
{
    public DbSet<Student> Students { get; set; }

    public DbSet<Teacher> Teachers { get; set; }

    public DbSet<LearningAdmin> LearningAdmins { get; set; }

    public DbSet<Module> Modules { get; set; }

    public DbSet<ModuleTask> Tasks { get; set; }
    
    public DbSet<Programme> Programmes { get; set; }
    
    public DbSet<Quiz> Quizzes { get; set; }
    
    public DbSet<Question> Questions { get; set; }
    
    public DbSet<Answer> Answers { get; set; }

    public DbSet<Topic> Topics { get; set; }
    public DbSet<SubmissionDetails> SubmissionDetails { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        //builder.Entity<SubmissionDetails>()
        //    .HasKey(t => new { t.TaskId, t.StudentId });
        builder.Entity<SubmissionDetails>()
            .HasIndex(s => new { s.TaskId, s.StudentId })
            .IsUnique(true);
        
  
        // Configure Answer-Question relationship
        builder.Entity<Answer>()
            .HasOne(a => a.Question)
            .WithMany(q => q.Options)
            .HasForeignKey(a => a.QuestionId)
            .OnDelete(DeleteBehavior.Restrict); // or DeleteBehavior.NoAction

        // Configure Question-Quiz relationship (if needed)
        builder.Entity<Question>()
            .HasOne(q => q.Quiz)
            .WithMany(qu => qu.Questions)
            .HasForeignKey(q => q.QuizId)
            .OnDelete(DeleteBehavior.Restrict); 

        base.OnModelCreating(builder);
    }

}
