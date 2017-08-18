using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MrFixIt.Models;
using Microsoft.EntityFrameworkCore;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace MrFixIt.Controllers
{
    public class JobsController : Controller
    {
        private MrFixItContext db = new MrFixItContext();

        // GET: /<controller>/
        public IActionResult Index()
        {
            return View(db.Jobs.Include(i => i.Worker).ToList());
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Job job)
        {
            db.Jobs.Add(job);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public IActionResult Claim(int id)
        {
            var thisItem = db.Jobs.FirstOrDefault(items => items.JobId == id);
            return View(thisItem);
        }

        [HttpPost]
        public IActionResult Claim(int JobId, string Title, string Description)
        {
            Job job = db.Jobs.FirstOrDefault(i => i.JobId == JobId);
            Worker worker = db.Workers.FirstOrDefault(i => i.UserName == User.Identity.Name);
            //The next two lines modify the .worker attribute of the job.
            job.Worker = worker;
            db.Entry(job).State = EntityState.Modified;
            //These lines modify the worker to make them unavailable.
            worker.Available = false;
            db.Entry(worker).State = EntityState.Modified;

            db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
