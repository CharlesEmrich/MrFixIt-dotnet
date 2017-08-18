﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MrFixIt.Models;
using Microsoft.EntityFrameworkCore;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

// Handles Routes related to Jobs, definitely the largest chunk of data being moved around by the app.
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

        //Boy, I sure hope converting all of these to AJAX wasn't part of the project. This one is still using pure MVC/Entity Framework stuff.
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

        public IActionResult Pending(int id)
        {
            var thisItem = db.Jobs.FirstOrDefault(items => items.JobId == id);
            return View(thisItem);
        }

        [HttpPost]
        public IActionResult Pending(int JobId, string Title, string Description)
        {
            Job job = db.Jobs.FirstOrDefault(i => i.JobId == JobId);
            job.Pending = true;
            db.Entry(job).State = EntityState.Modified;

            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public IActionResult Complete(int id)
        {
            var thisItem = db.Jobs.FirstOrDefault(items => items.JobId == id);
            return View(thisItem);
        }

        [HttpPost]
        public IActionResult Complete(int JobId, string Title, string Description)
        {
            Job job = db.Jobs.FirstOrDefault(i => i.JobId == JobId);
            //Completed tasks should not be pending.
            job.Pending = false;
            //They should, however, be copmlete.
            job.Completed = true;
            db.Entry(job).State = EntityState.Modified;

            //We also need to make the worker available again.
            Worker worker = db.Workers.FirstOrDefault(i => i.UserName == User.Identity.Name);
            worker.Available = true;
            db.Entry(worker).State = EntityState.Modified;

            db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
