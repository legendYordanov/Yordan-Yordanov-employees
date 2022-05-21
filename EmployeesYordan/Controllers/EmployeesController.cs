using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using IronXL;
using System.Data;
using System.IO;
using System.Globalization;
using EmployeesYordan.Models;

namespace EmployeesYordan.Controllers
{
    public class EmployeesController : Controller
    {
        public IActionResult Index()
        {
            return View(new List<Employee>());
        }

        [HttpPost]
        public IActionResult Index(IFormFile FileUpload)
        {
            var path = Path.Combine(
               Directory.GetCurrentDirectory(), "wwwroot/employees",
               FileUpload.FileName);

            using (var stream = new FileStream(path, FileMode.Create))
            {
                FileUpload.CopyToAsync(stream);
            }

            var csvFilereader = new DataTable();
            csvFilereader = ReadExcel(path);

            var employees = new List<Employee>();

            for (int i = 0; i < csvFilereader.Rows.Count; i++)
            {
                var empID = int.Parse(csvFilereader.Rows[i][0].ToString());
                var projectID = int.Parse(csvFilereader.Rows[i][1].ToString());
                var dateFrom = DateTime.ParseExact(csvFilereader.Rows[i][2].ToString()
                    , "yyyy-MM-dd", CultureInfo.InvariantCulture);
                var dateTo = csvFilereader.Rows[i][3].ToString();
                DateTime dateToParsed;
                if (dateTo == "NULL")
                {
                    dateToParsed = DateTime.Now;
                }
                else
                {
                    dateToParsed = DateTime.ParseExact(csvFilereader.Rows[i][3].ToString()
                    , "yyyy-MM-dd", CultureInfo.InvariantCulture);
                }

                var daysWorking = (dateToParsed - dateFrom).Days;
                var employee = new Employee();

                employee.Id = empID;
                employee.ProjectId = projectID;
                employee.DaysWorking += daysWorking;

                employees.Add(employee);
            }


            return View(employees.OrderBy(x => x.ProjectId));
        }

        private DataTable ReadExcel(string fileName)
        {
            WorkBook workbook = WorkBook.Load(fileName);
            WorkSheet sheet = workbook.DefaultWorkSheet;
            return sheet.ToDataTable(true);
        }
    }
}