using StaffManage.Data; // Update this namespace to match your project
using StaffManage.Models; // Update this namespace to match your project
using Microsoft.AspNetCore.Mvc;

namespace StaffManage.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly ApplicationDbContext _db;

        public EmployeeController(ApplicationDbContext db)
        {
            _db = db;
        }

        public IActionResult Index()
        {
            List<Employee> employeeList = _db.Employees.ToList();
            return View(employeeList);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Employee employee)
        {
            if (ModelState.IsValid)
            {
                _db.Employees.Add(employee);
                _db.SaveChanges();
                TempData["success"] = "Employee created successfully.";
                return RedirectToAction("Index");
            }
            return View(employee);
        }

        public IActionResult Edit(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            Employee employeeFromDb = _db.Employees.Find(id);

            if (employeeFromDb == null)
            {
                return NotFound();
            }
            return View(employeeFromDb);
        }

        [HttpPost]
        public IActionResult Edit(Employee employee)
        {
            if (ModelState.IsValid)
            {
                _db.Employees.Update(employee);
                _db.SaveChanges();
                TempData["success"] = "Employee updated successfully.";
                return RedirectToAction("Index");
            }
            return View(employee);
        }

        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            Employee employeeFromDb = _db.Employees.Find(id);

            if (employeeFromDb == null)
            {
                return NotFound();
            }
            return View(employeeFromDb);
        }

        [HttpPost, ActionName("Delete")]
        public IActionResult DeletePost(int? id)
        {
            Employee employee = _db.Employees.Find(id);
            if (employee == null)
            {
                return NotFound();
            }
            _db.Employees.Remove(employee);
            _db.SaveChanges();
            TempData["success"] = "Employee deleted successfully.";
            return RedirectToAction("Index");
        }
    }
}
