using System;
using System.Threading;

class Program
{
    static void Main(string[] args)
    {
        ScheduleJob();
    }

    static void ScheduleJob()
    {
        // Calculate the time until the next 5 PM
        DateTime now = DateTime.Now;
        DateTime nextRunTime = now.Date.AddDays(1).AddHours(17); // Tomorrow 5 PM

        TimeSpan timeUntilNextRun = nextRunTime - now;

        // Create a timer to run the job at the calculated time
        Timer timer = new Timer(RunJob, null, timeUntilNextRun, TimeSpan.FromDays(1)); // Repeat daily

        Console.WriteLine($"Job scheduled. Next run time: {nextRunTime}");

        // Keep the application running
        Console.ReadLine();
    }

    static void RunJob(object state)
    {
        // Your job logic goes here
        Console.WriteLine("Job is running at: " + DateTime.Now);
    }
}
