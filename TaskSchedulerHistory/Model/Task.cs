using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskSchedulerHistory.Model
{
    public class TaskModel
    {
        public string Name { get; set; }
        public string LevelDisplayName { get; set; }
        public DateTime? TimeCreated { get; set; }
        public int Id { get; set; }
        public string TaskDisplayName { get; set; }
        public string OpcodeDisplayName { get; set; }
        public string ActivityId { get; set; }
        public string FormatDescription { get; set; }

        public TaskModel GetTask(EventLogRecord logRecord, String Name) {
           return  new TaskModel
            {
                Name = Name,
                LevelDisplayName = logRecord.LevelDisplayName.ToString(),
                TimeCreated = logRecord.TimeCreated,
                Id = logRecord.Id,
                TaskDisplayName = logRecord.TaskDisplayName.ToString(),
                OpcodeDisplayName = logRecord.OpcodeDisplayName.ToString(),
                ActivityId = logRecord.ActivityId.ToString(),
                FormatDescription = logRecord.FormatDescription().ToString()
            };
        }

        public string GetInformationTaskLine(TaskModel Task, string MachineName)
        {
            return $@"
                    <tr>
                        <td>{Task.Name}</td>
                        <td>{MachineName}</td>
                        <td>{Task.LevelDisplayName}</td>
                        <td>{Task.TimeCreated}</td>
                        <td>{Task.Id}</td>
                        <td>{Task.TaskDisplayName}</td>
                        <td>{Task.OpcodeDisplayName}</td>
                        <td>{Task.ActivityId}</td>
                        <td>{Task.FormatDescription}</td>
                    </tr>
                     ";
        }

        public string[] GetTasksName()
        {
            Console.WriteLine("Enter the name of tasks separated by commas");
            var T = Console.ReadLine();
            return T.Split(',');
        }

        public int GetQuantityResult()
        {
            try
            {
                Console.WriteLine("Enter the amount of results per task");
                var T = Convert.ToInt16(Console.ReadLine());
                return T;
            }
            catch
            {
                Console.WriteLine("Please enter numbers only");
                return GetQuantityResult();
            }
        }

        public void GenerateTableFile(string TableRows, string MachineName)
        {
            try
            {
                var way = $@"c:\Tasks{MachineName}.html";
                Console.WriteLine($"Generating file");
                StreamWriter salvar = new StreamWriter(way);
                salvar.WriteLine(HtmlStructure(TableRows));
                salvar.Close();
                Console.WriteLine($@"File Tasks-{MachineName}.html successfully generated in c:\");
                Console.WriteLine($@"want to open the file? enter y for yes or n for no");

                var result = Console.ReadKey();
                if(result.Key.ToString().ToUpper() == "Y")
                {
                    System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();
                    startInfo.FileName = way;
                    System.Diagnostics.Process.Start(startInfo);
                    Console.ReadKey();
                }
            }
            catch 
            {
                Console.WriteLine($"Unable to generate file, please run as administrator and try again");
                Console.ReadKey();
                Environment.Exit(0);
            }
        }

        public string HtmlStructure(string TableRows)
        {
            return @"
                <!DOCTYPE html>
                                <html>
                                <head>
	                                <meta http-equiv='Content-type' content='text/html; charset=utf-8'>
	                                <meta name='viewport' content='width=device-width,initial-scale=1'>
	                                <title>Task Scheduler History</title>
	                                <link href='https://maxcdn.bootstrapcdn.com/bootstrap/4.0.0/css/bootstrap.min.css' rel='stylesheet' id='bootstrap-css'>
	                                <link href='https://cdn.datatables.net/1.10.20/css/jquery.dataTables.min.css' rel='stylesheet' id='bootstrap-css'>,
	                                <script src='https://cdnjs.cloudflare.com/ajax/libs/jquery/3.2.1/jquery.min.js'></script>
	                                <script src='https://maxcdn.bootstrapcdn.com/bootstrap/4.0.0/js/bootstrap.min.js'></script>
	                                <script src='https://cdn.datatables.net/1.10.20/js/jquery.dataTables.min.js'></script>
	                                <script type='text/javascript'>
		                                $(document).ready(function() {
			                                $('#Table').DataTable();
		                                } );
	                                </script>
                                </head>
                                <body>
    
                                <div class='container-fluid'>    
									<table id='Table' class='table table-striped table-bordered'  width='100%'>
										<thead>
											<tr>
												<th>Task</th>
												<th>Machine Name</th>
												<th>Level</th>
												<th>Time Created</th>
												<th>Id</th>
												<th>Task Display Name</th>
												<th>Opcode Display Name</th>
												<th>Activity Id</th>
												<th>Description</th>    
											  </tr>
										</thead>			
										<tbody>" 
                                           +TableRows +
										@"</tbody>
									</table>
                                </div>	
                                </body>
                                </html>
                             "; 
        }


    }
}
