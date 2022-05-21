using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeesYordan.Models
{
    public class Employee
    {
        public int Id { get; set; }

        public int ProjectId { get; set; }

        public int DaysWorking { get; set; }
    }
}
