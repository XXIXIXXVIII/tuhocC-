using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MVCCRUD.Data;
using MVCCRUD.Models;
using MVCCRUD.Models.Domain;

namespace MVCCRUD.Controllers
{
    public class EmployeesController : Controller
    {
        private readonly MvcDbContext mvcDbContext;

        public EmployeesController(MvcDbContext mvcDbContext)
        {
            this.mvcDbContext = mvcDbContext;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
          var employees =  await mvcDbContext.Employees.ToListAsync();
            return View(employees);
        }

        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Add(AddEmployeeViewModel req)
        {
            var employee = new Employee()
            {
                Id = Guid.NewGuid(),
                Name = req.Name,
                Email = req.Email,
                Salary = req.Salary,
                DateOfBirth = req.DateOfBirth,
                Department = req.Department,
            };
            await mvcDbContext.Employees.AddAsync(employee);
            await mvcDbContext.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> View(Guid id)
        {
            var employee = await mvcDbContext.Employees.FirstOrDefaultAsync(x => x.Id == id);

            if(employee != null)
            {
                var viewModel = new UpdateEmployeeViewModel()
                {
                    Id = employee.Id,
                    Name = employee.Name,
                    Email = employee.Email,
                    Salary = employee.Salary,
                    DateOfBirth = employee.DateOfBirth,
                    Department = employee.Department,
                };
                
                return await Task.Run(()=>View("View", viewModel));

            }



            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> View(UpdateEmployeeViewModel viewModel)
        {
            var employee = await mvcDbContext.Employees.FindAsync(viewModel.Id);
            if(employee != null)
            {
                employee.Name = viewModel.Name;
                employee.Email = viewModel.Email;
                employee.Salary = viewModel.Salary;
                employee.DateOfBirth = viewModel.DateOfBirth;
                employee.Department = viewModel.Department;

                await mvcDbContext.SaveChangesAsync();

        
            }
            return RedirectToAction("Index");
        }
    }
}
