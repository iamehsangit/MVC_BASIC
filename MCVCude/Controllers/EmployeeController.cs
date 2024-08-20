using MCVCude.DataAccess;
using MCVCude.Models;
using Microsoft.AspNetCore.Mvc;

namespace MCVCude.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly EmployeeDataAccess _dataAccess;

        public EmployeeController(IConfiguration configuration)
        {
            _dataAccess = new EmployeeDataAccess(configuration);
        }
        public IActionResult Index()
        {
            IEnumerable<Employee> employees = _dataAccess.GetEmployees();
            return View(employees);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Employee employee)
        {
            _dataAccess.AddEmployee(employee);
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Edit(int id)
        {
            Employee employee = _dataAccess.GetEmployeeById(id);
            return View(employee);
        }
        [HttpPost]
        public IActionResult Edit(Employee emp)
        {
           _dataAccess.UpdateEmployee(emp);
            return RedirectToAction(nameof(Index));
        }

    }
}
