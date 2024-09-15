using StaffManage.Data; // Update this namespace to match your project
using StaffManage.Models; // Update this namespace to match your project
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Reflection;

namespace StaffManage.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly ApplicationDbContext _db;

        public EmployeeController(ApplicationDbContext db)
        {
            _db = db;
        }

        // Index Action - Display all employees with search, sort, and paging

        public IActionResult Index(string sortOrder, string NameFilter, string DOBFilter, string EmailFilter, string MobileFilter, int page = 1, int pageSize = 10)
        {
            ViewData["NameSortParam"] = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewData["EmailSortParam"] = sortOrder == "Email" ? "email_desc" : "Email";
            ViewData["MobileSortParam"] = sortOrder == "Mobile" ? "mobile_desc" : "Mobile";
            ViewData["DOBSortParam"] = sortOrder == "DOB" ? "dob_desc" : "DOB";

            // Initialize the query
            var employees = from e in _db.Employees
                            select e;

            // Apply filters
            if (!String.IsNullOrEmpty(NameFilter))
            {
                employees = employees.Where(e => (e.FirstName + " " + e.LastName).Contains(NameFilter));
            }

            if (!String.IsNullOrEmpty(DOBFilter) && DateTime.TryParse(DOBFilter, out DateTime dob))
            {
                employees = employees.Where(e => e.DateOfBirth.Date == dob.Date);
            }

            if (!String.IsNullOrEmpty(EmailFilter))
            {
                employees = employees.Where(e => e.Email.Contains(EmailFilter));
            }

            if (!String.IsNullOrEmpty(MobileFilter))
            {
                employees = employees.Where(e => e.Mobile.Contains(MobileFilter));
            }

            // Sorting
            switch (sortOrder)
            {
                case "name_desc":
                    employees = employees.OrderByDescending(e => e.FirstName + " " + e.LastName);
                    break;
                case "Email":
                    employees = employees.OrderBy(e => e.Email);
                    break;
                case "email_desc":
                    employees = employees.OrderByDescending(e => e.Email);
                    break;
                case "Mobile":
                    employees = employees.OrderBy(e => e.Mobile);
                    break;
                case "mobile_desc":
                    employees = employees.OrderByDescending(e => e.Mobile);
                    break;
                case "DOB":
                    employees = employees.OrderBy(e => e.DateOfBirth);
                    break;
                case "dob_desc":
                    employees = employees.OrderByDescending(e => e.DateOfBirth);
                    break;
                default:
                    employees = employees.OrderBy(e => e.FirstName + " " + e.LastName);
                    break;
            }

            // Paging
            int totalItems = employees.Count();
            employees = employees.Skip((page - 1) * pageSize).Take(pageSize);

            ViewData["TotalItems"] = totalItems;
            ViewData["Page"] = page;
            ViewData["PageSize"] = pageSize;

            return View(employees.ToList());
        }
        // Create //
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Employee obj)
        {
            if (ModelState.IsValid)
            {
                _db.Employees.Add(obj);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View();
        }

        // Upload Image //
        [HttpPost]
        [Route("api/upload/photo")]
        public async Task<IActionResult> UploadImage(IFormFile croppedImage)
        {
            if (croppedImage != null && croppedImage.Length > 0)
            {
                // Save the image to the server (e.g., in a directory on the server)
                var uploadPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads");
                if (!Directory.Exists(uploadPath))
                {
                    Directory.CreateDirectory(uploadPath);
                }

                var fileName = Path.GetRandomFileName() + Path.GetExtension(croppedImage.FileName);
                var filePath = Path.Combine(uploadPath, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await croppedImage.CopyToAsync(stream);
                }

                return Ok(new { status = "success", filePath = "/uploads/" + fileName });
            }

            return BadRequest(new { status = "fail", message = "Image upload failed." });
        }

        // Edit existing employee - GET
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

        // Edit existing employee - POST
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

        // Delete employee - GET
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

        // Delete employee - POST
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
