using Domain.Entities;
using Microsoft.EntityFrameworkCore;


namespace Application.Interfaces
{
    public interface IDataContext
    {
        public DbSet<Student> Students { get; set; }
        //public DbSet<Teacher> Teachers { get; set; }
        //public DbSet<LearningAdmin> LearningAdmins { get; set; }
    }
}