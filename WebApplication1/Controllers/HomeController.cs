using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using EmployeeManagement.Models;
using EmployeeManagement.ViewModels;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Authorization;

namespace EmployeeManagement.Controllers
{
    //[Route("Home")]
    //[Route("[controller]/[action]")] // will replace it with controller name which is Home
   // [Authorize] // make sure user logged in before view this controller actions
    public class HomeController : Controller
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IHostingEnvironment hostingEnvironment;
        private readonly ILogger<HomeController> logger;

        public HomeController(IEmployeeRepository employeeRepository,
            IHostingEnvironment hostingEnvironment,
            ILogger<HomeController> logger)
        {
            _employeeRepository = employeeRepository;
            this.hostingEnvironment = hostingEnvironment;
            this.logger = logger;
        }

        // Attributes Routing Which Work if we not use Conventional Routing if we not detect in startup {controller}/{action}
        //[Route("~/Home")]
        ////[Route("Index")]
        ////[Route("[action]")]  //will replace it with action name which is Index
        //[Route("~/")]
        //[AllowAnonymous] //specify this action to be global
        public ViewResult Index()
        {
            var model = _employeeRepository.GetEmployees();
            return View(model); // View(path optional,model)
        }

        //[Route("{id?}")]
        // if we want pass another param like name we put Details (int? id, string name) 
        // so we pass name to route like /home/details/2?name=mostafa
        public ViewResult Details(int? id) 
        {
            //throw new Exception("Erro in Details View");
            logger.LogTrace("Log Trace");
            logger.LogCritical("Log Critical");
            logger.LogDebug("Log Debug");
            logger.LogError("Log Error");
            logger.LogInformation("Log Information");
            logger.LogWarning("Log Warning");

            //ObjectResult will convert response to content negotiation ex : content:application/xml , content : application/json
            //return new ObjectResult(_employeeRepository.GetEmployee(2));
            Employee employee = _employeeRepository.GetEmployee(id.Value);
            //ViewData is Dictionary of Weakly Typed objects
            //use string keys to store data
            // Resolved at RunTime
            if(employee == null)
            {
                Response.StatusCode = 404;
                return View("EmployeeNotFound", id.Value);
            }
            ViewData["employee"] = employee;
            ViewBag.employee = employee;
            //return View("MyViews/Details.cshtml");
            return View(employee);
        }
        // if we not put attribute route here will take by default [Route("")] which is /home
        //[Route("[action]")]
        public ViewResult Test()
        {
            HomeDetailsViewModel homeDetailsViewModel = new HomeDetailsViewModel()
            {
                Employee = _employeeRepository.GetEmployee(1),
                PageTitle = "Test Page"
            };

            return View(homeDetailsViewModel);
        }

        [HttpGet]
        public ViewResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(EmployeeCreateViewModel model)
        {
            if (ModelState.IsValid)
            {
                string uniquefilename = ProcessUploadedFile(model);
                Employee newEmployee = new Employee
                {
                    Name = model.Name,
                    Email = model.Email,
                    Department = model.Department,
                    PhotoPath = uniquefilename
                };
                _employeeRepository.Add(newEmployee);
                return RedirectToAction("Details", new { id = newEmployee.ID });
            }

            return View();

        }

        [HttpGet]
        public ViewResult Edit(int id)
        {
            Employee employee = _employeeRepository.GetEmployee(id);
            EmployeeEditViewModel employeeEditView = new EmployeeEditViewModel
            {
                ID = employee.ID,
                Name = employee.Name,
                Department = employee.Department,
                Email = employee.Email,
                ExistingPhotoPath = employee.PhotoPath
            };
            return View(employeeEditView);
        }

        [HttpPost]
        public IActionResult Edit(EmployeeEditViewModel model)
        {
            if (ModelState.IsValid)
            {
                Employee employee = _employeeRepository.GetEmployee(model.ID);
                employee.Name = model.Name;
                employee.Email = model.Email;
                employee.Department = model.Department;
                if(model.Photo != null) // if choose photo
                {
                    if(model.ExistingPhotoPath != null)
                    {
                        string filename = Path.Combine(hostingEnvironment.WebRootPath, "images", model.ExistingPhotoPath);
                        System.IO.File.Delete(filename);
                    }
                    employee.PhotoPath = ProcessUploadedFile(model);
                }
                
                _employeeRepository.Update(employee);
                return RedirectToAction("Details", new { id = employee.ID });
            }

            return View();

        }

        private string ProcessUploadedFile(EmployeeCreateViewModel model)
        {
            string uniquefilename = null;
            if (model.Photo != null)
            {
                string uploadsFolder = Path.Combine(hostingEnvironment.WebRootPath, "images");
                uniquefilename = Guid.NewGuid().ToString() + "_" + model.Photo.FileName; // Guid return new unique id
                string filePath = Path.Combine(uploadsFolder, uniquefilename);
                using (var filestream = new FileStream(filePath, FileMode.Create))
                {
                    /* use using statement to dispose FileStream after create image 
                     else will fire erro when deleting image that image accessed by another process
                     */
                    model.Photo.CopyTo(filestream);

                }
            }

            return uniquefilename;
        }
    }
}