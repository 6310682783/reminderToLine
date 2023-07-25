
using Autofac;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Quartz;
using Quartz.Impl;
using Quartz.Logging;
using ReminderSchedule.Models;
using ReminderSchedule.Services;
using System.Diagnostics.CodeAnalysis;
using System.Web.Mvc;

//class Program
//{

//    private static async Task Main(string[] args)
//    {


//        LogProvider.SetCurrentLogProvider(new ConsoleLogProvider());

//        // Grab the Scheduler instance from the Factory
//        StdSchedulerFactory factory = new StdSchedulerFactory();
//        IScheduler scheduler = await factory.GetScheduler();

//        // and start it off
//        await scheduler.Start();

//        // define the job and tie it to our HelloJob class
//        IJobDetail job = JobBuilder.Create<ReminderJob>()
//            .WithIdentity("jobMonitoring", "ReminderJob")
//            .Build();

//        // Trigger the job to run now, and then repeat every 10 seconds
//        ITrigger trigger = TriggerBuilder.Create()
//            .WithIdentity("trigger1", "Smart400")
//            .StartNow()
//            .WithSimpleSchedule(x => x
//                .WithIntervalInSeconds(1) // ต้องการให้แก้เป็น 1 นาที
//                .RepeatForever())
//            .Build();

//        // Tell quartz to schedule the job using our trigger
//        await scheduler.ScheduleJob(job, trigger);

//        // some sleep to show what's happening
//        //await Task.Delay(TimeSpan.FromSeconds(60));

//        // and last shut down the scheduler when you are ready to close your program
//        //await scheduler.Shutdown();

//        Console.WriteLine("Press any key to close the application");
//        Console.ReadKey();

//    }

//    // simple log provider to get something to the console
//    private class ConsoleLogProvider : ILogProvider
//    {
//        public Logger GetLogger(string name)
//        {
//            return (level, func, exception, parameters) =>
//            {
//                if (level >= LogLevel.Info && func != null)
//                {
//                    Console.WriteLine("[" + DateTime.Now.ToLongTimeString() + "] [" + level + "] " + func(), parameters);
//                }
//                return true;
//            };
//        }

//        public IDisposable OpenNestedContext(string message)
//        {
//            throw new NotImplementedException();
//        }

//        public IDisposable OpenMappedContext(string key, object value, bool destructure = false)
//        {
//            throw new NotImplementedException();
//        }
//    }
//}

public class Program
{
    public static void Main(string[] args)
    {
        CreateHostBuilder(args).Build().Run();
    }

    public static IHostBuilder CreateHostBuilder(string[] args) =>
  Host.CreateDefaultBuilder(args)
      .ConfigureServices((hostContext, services) =>
      {
          services.AddQuartz(q =>
          {
              q.UseMicrosoftDependencyInjectionScopedJobFactory();

              // Create a "key" for the job
              var jobKey = new JobKey("ReminderJob");

              // Register the job with the DI container
              q.AddJob<ReminderJob>(opts => opts.WithIdentity(jobKey));

              // Create a trigger for the job
              q.AddTrigger(opts => opts
                  .ForJob(jobKey) // link to the HelloWorldJob
                  .WithIdentity("ReminderJob-trigger") // give the trigger a unique name
                  .WithCronSchedule("0/5 * * * * ?")); // run every 5 seconds

          });
          services.AddQuartzHostedService(q => q.WaitForJobsToComplete = true);
          // ...
      });
}



