using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeManagement.Models
{
    public class MockEmployeeRepository : IEmployeeRepository
    {
        private List<Employee> _employee_list;

        public MockEmployeeRepository()
        {
            _employee_list = new List<Employee>() {
                new Employee { ID = 1, Name = "Mostafa", Email="mostafa@yahoo.com", Department=Dept.HR },
                new Employee { ID = 2, Name = "Abdalla", Email="abdalla@yahoo.com", Department=Dept.IT }
            };

        }

        public Employee Add(Employee employee)
        {
            employee.ID = _employee_list.Max(e => e.ID) + 1;
            _employee_list.Add(employee);
            return employee;
        }

        public Employee Delete(int id)
        {
            Employee employee = _employee_list.FirstOrDefault(e => e.ID == id);
            if (employee != null)
            {
                _employee_list.Remove(employee);
            }
            return employee;
        }

        public Employee GetEmployee(int id)
        {
            return _employee_list.FirstOrDefault(row => row.ID == id);
        }

        public IEnumerable<Employee> GetEmployees()
        {
            return _employee_list;
        }

        public Employee Update(Employee employeeChanges)
        {
            Employee employee = _employee_list.FirstOrDefault(e => e.ID == employeeChanges.ID);
            if (employee != null)
            {
                employee.Name = employeeChanges.Name;
                employee.Email = employeeChanges.Email;
                employee.Department = employeeChanges.Department;
            }
            return employee;
        }
    }
}
