namespace Hangfire_Explorer.Application.Implementations
{
    using System;
    using Hangfire;
    using Hangfire_Explorer.Application.Interfaces;

    public class Service : IService
    {
        /// <summary>
        /// Try do something
        /// and we catch the exception
        /// to create a retry in Hangfire
        /// </summary>
        public void GetException()
        {
            try
            {
                this.Create();
            }
            catch (Exception)
            {
                var jobId = BackgroundJob.Enqueue(
                    () => this.ProcessBackgroundRetry(30));

            }
        }

        /// <summary>
        /// Example of an action may raise an exception
        /// </summary>
        private void Create()
        {
            // Do something ...

            //Ups ..
            throw new Exception();
        }

        /// <summary>
        /// This function name is extremly important
        /// The Hangfire use the name to retry
        /// If you change the name, you will lose the already registraded retries
        /// </summary>
        /// <param name="i">Exemple param</param>
        [AutomaticRetry(Attempts = 5)]
        public void ProcessBackgroundRetry(int i)
        {
            try
            {
                this.Create();
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Create a Recurring job
        /// </summary>
        public void CreateRecurring()
        {
            RecurringJob.AddOrUpdate(
                () => Console.WriteLine("Recurring!"),
                Cron.Daily);
        }
    }
}
