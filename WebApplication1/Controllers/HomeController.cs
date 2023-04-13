using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Text;

namespace WebApplication1.Controllers
{
    public class HomeController : Controller
    {
        WorkTasksCS _context = new WorkTasksCS();
        public ActionResult Index()
        {
            var taskList = _context.WorkTasks.ToList();
            return View(taskList);
        }

        [HttpGet]
        public ActionResult Create()
        {
            List<SelectListItem> priorityItems = new List<SelectListItem>();
            priorityItems.Add(new SelectListItem { Text = "Low", Value = "Low" });
            priorityItems.Add(new SelectListItem { Text = "Medium", Value = "Medium" });
            priorityItems.Add(new SelectListItem { Text = "High", Value = "High" });

            ViewBag.PriorityList = priorityItems;
            return View();
        }

        [HttpPost]
        public ActionResult Create(WorkTask task)
        {
            task.Id = Guid.NewGuid();
            task.TaskCreatedBy = User.Identity.Name;
            task.TaskCreatedDate = DateTime.Now.Date;
            task.TaskStatus = "Posted";
            _context.WorkTasks.Add(task);
            _context.SaveChanges();
            ViewBag.Message = "Work Task Successfully Added";
            return RedirectToAction("index");
        }

        public ViewResult Edit()
        {
            throw new NotImplementedException();
        }

        public ActionResult Edit(Guid? id)
        {
            if (id == null)
                return RedirectToAction("index");
            var dataTask = _context.WorkTasks.Where(x => x.Id == id).FirstOrDefault();
            List<SelectListItem> priorityItems = new List<SelectListItem>();
            priorityItems.Add(new SelectListItem { Text = "Low", Value = "Low" });
            priorityItems.Add(new SelectListItem { Text = "Medium", Value = "Medium" });
            priorityItems.Add(new SelectListItem { Text = "High", Value = "High" });

            ViewBag.PriorityList = priorityItems;
            return View(dataTask);
        }

        [HttpPost]
        public ActionResult Edit(WorkTask task)
        {
            var dataTask = _context.WorkTasks.Where(x => x.Id == task.Id).FirstOrDefault();
            if (dataTask != null)
            {
                dataTask.TaskModifiedBy = User.Identity.Name;
                dataTask.TaskModifiedDate = DateTime.Now.Date;
                dataTask.TaskName = task.TaskName;
                dataTask.TaskDescription = task.TaskDescription;
                dataTask.TaskDueDate = task.TaskDueDate;
                dataTask.TaskPriority = task.TaskPriority;

                _context.SaveChanges();
                ViewBag.Message = "Work Task Successfully Updated";
            }
            
            return RedirectToAction("index");
        }

        public ActionResult Detail(Guid? id)
        {
            if (id == null)
                return RedirectToAction("index");
            var dataTask = _context.WorkTasks.Where(x => x.Id == id).FirstOrDefault();
            
            return View(dataTask);
        }

        [HttpPost]
        public ActionResult Process(WorkTask task)
        {
            var dataTask = _context.WorkTasks.Where(x => x.Id == task.Id).FirstOrDefault();
            if (dataTask != null)
            {
                if (dataTask.TaskStatus == "Posted")
                {
                    dataTask.TaskStatus = "Processed";
                }
                else if (dataTask.TaskStatus == "Processed")
                {
                    dataTask.TaskStatus = "Final Inspection";
                }
                else if (dataTask.TaskStatus == "Final Inspection")
                {
                    dataTask.TaskStatus = "Finished";
                }
                dataTask.TaskModifiedBy = User.Identity.Name;
                dataTask.TaskModifiedDate = DateTime.Now.Date;

                _context.SaveChanges();
                ViewBag.Message = "Work Task Successfully Updated";
            }

            return RedirectToAction("index");
        }

        public ActionResult Delete(Guid id)
        {
            var data = _context.WorkTasks.Where(x => x.Id == id).FirstOrDefault();
            _context.WorkTasks.Remove(data);
            _context.SaveChanges();
            ViewBag.Messsage = "Record Deleted Successfully";
            return RedirectToAction("index");
        }
    }
}