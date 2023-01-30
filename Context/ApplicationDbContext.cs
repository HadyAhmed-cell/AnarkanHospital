using AnarkanHospital.Models;
using Microsoft.EntityFrameworkCore;

namespace AnarkanHospital.Context
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }

        public DbSet<Doctor> Doctors { get; set; }

        public DbSet<Patient> Patients { get; set; }
    }
}
