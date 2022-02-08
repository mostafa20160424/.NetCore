using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeManagement.Models
{
    public static class ModelBuilderExtension
    {
        public static void Seed(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Employee>().HasData(
                    new Employee
                    {
                        ID = 2,
                        Name = "khaled",
                        Email = "khaled@yahoo.com",
                        Department = Dept.IT
                    },
                    new Employee
                    {
                        ID = 3,
                        Name = "abdalla",
                        Email = "abdalla@yahoo.com",
                        Department = Dept.IT
                    }
                   );
        }
    }
}
