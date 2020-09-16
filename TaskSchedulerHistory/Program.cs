using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskSchedulerHistory.Model;

namespace TaskSchedulerHistory
{
    class Program
    {
        static void Main(string[] args)
        {
          
            Console.WriteLine("Starting..."); 
            EventLogReader log2 = new EventLogReader("Microsoft-Windows-TaskScheduler/Operational");
            var TaskG = new TaskModel();
            var MachineName = Environment.MachineName;
            var TasksName = TaskG.GetTasksName();
            var QuantityResult = TaskG.GetQuantityResult();
            var Tasks = new List<TaskModel>();
            var Events = new List<TaskModel>();

            Console.WriteLine($"Getting information, please wait...");
            for (EventRecord eventInstance = log2.ReadEvent(); null != eventInstance; eventInstance = log2.ReadEvent())
                    foreach (var T in TasksName)                    
                        if (eventInstance.Properties.Select(p => p.Value).Contains($"\\{T.ToString()}"))
                            Tasks.Add(new TaskModel().GetTask((EventLogRecord)eventInstance, T));
                    
                
            foreach (var T in TasksName)
            {
                Console.WriteLine($"Getting task data {T}");
                var EventosLoc = Tasks.Where(t => t.Name == T).OrderByDescending(x => x.TimeCreated).Take(QuantityResult);
                EventosLoc.ToList().ForEach(a=> Events.Add(a));
            }

            var TableRows = String.Empty;
            Events.ForEach(g => TableRows += TaskG.GetInformationTaskLine(g, MachineName));
            TaskG.GenerateTableFile(TableRows, MachineName);  
        }
              
    }

}
