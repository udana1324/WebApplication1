using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApplication1.Models;
using System.Data.Entity;

namespace WebApplication1.Tests.Model
{
    class WorkTasksTests : WorkTask
    {
        private List<WorkTask> workTasks = new List<WorkTask>();

        public Exception ex { get; set; }

        public IEnumerable<WorkTask> GetTasks()
        {
            return workTasks.ToList();
        }

        public WorkTask GetTask(Guid id)
        {
            return workTasks.FirstOrDefault(wt => wt.Id == id);
        }

        public void insert(WorkTask task)
        {
            workTasks.Add(task);
        }

        public void update(WorkTask task)
        {
            foreach (WorkTask workTask in workTasks)
            {
                if (workTask.Id == task.Id)
                {
                    workTasks.Remove(workTask);
                    workTasks.Add(task);
                    break;
                }
            }
        }

        public void delete(Guid id)
        {
            workTasks.Remove(GetTask(id));
        }

        public int save()
        {
           return 1;
        }
    }
}
