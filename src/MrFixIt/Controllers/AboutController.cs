using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace MrFixIt.Controllers
{
    public class AboutController : Controller
    {
        // GET: /<controller>/
        //Handles the return of the About page view.
        public IActionResult Index()
        {
            return View();
        }
    }
}
