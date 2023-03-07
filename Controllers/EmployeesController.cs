using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MVCCRUD.Data;
using MVCCRUD.Models;
using MVCCRUD.Models.Domain;

namespace MVCCRUD.Controllers
{
    public class EmployeesController : Controller
    {
        private readonly MVCDemoDbContext mvcDemoDbContext;

        public  EmployeesController(MVCDemoDbContext mvcDemoDbContext)
        {
            this.mvcDemoDbContext = mvcDemoDbContext;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var employees =await mvcDemoDbContext.Employees.ToListAsync();
            return View(employees);
        }

        [HttpGet]

        public IActionResult  Add()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Add(AddEmployeeViewModel addEmployeeRequest)
        {
            var employee = new Employee()
            {
                Id = Guid.NewGuid(),
                Name = addEmployeeRequest.Name,
                Email = addEmployeeRequest.Email,
                Salary = addEmployeeRequest.Salary,
                DOB = addEmployeeRequest.DOB,
                Department = addEmployeeRequest.Department,
            };
            await mvcDemoDbContext.Employees.AddAsync(employee);
            await  mvcDemoDbContext.SaveChangesAsync();
            return RedirectToAction("Index");

        }
        [HttpGet]
        public async Task<IActionResult> View(Guid Id)
        {
            var employee = await mvcDemoDbContext.Employees.FirstOrDefaultAsync(x => x.Id == Id);

            if(employee != null)
            {
                var viewModel = new UpdateEmployeeViewModel()
                {
                    Id = employee.Id,
                    Name = employee.Name,
                    Email = employee.Email,
                    Salary = employee.Salary,
                    DOB = employee.DOB,
                    Department = employee.Department,
                };

                return await Task.Run(() =>View("View",viewModel));
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> View (UpdateEmployeeViewModel model)
        {
            var employee = await mvcDemoDbContext.Employees.FindAsync(model.Id);
            if(employee != null)
            {
                employee.Name= model.Name;
                employee.Email= model.Email;
                employee.Salary= model.Salary;
                employee.DOB= model.DOB;
                employee.Department= model.Department;

               await mvcDemoDbContext.SaveChangesAsync();

                return RedirectToAction("Index");
            }

          
            return RedirectToAction("Error");
        }
        [HttpPost]
        public async Task<IActionResult> Delete(UpdateEmployeeViewModel model)
        {
            var employee = await mvcDemoDbContext.Employees.FindAsync(model.Id);

            if (employee != null)
            {
                mvcDemoDbContext.Employees.Remove(employee);
                await mvcDemoDbContext.SaveChangesAsync();

                return RedirectToAction("Index");
            }
            
            return RedirectToAction("Error");
        }

        public IActionResult Error () { 
            
            return Error();
        }
    }
}
