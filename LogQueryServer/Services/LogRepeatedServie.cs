using System;
using LogQueryServer.Exception;

namespace LogQueryServer.Services
{
    public class LogRepeatedServie : RepeatedService
    {
        public LogRepeatedServie()
            : base(new TimeSpan(0, 2, 0))
        {
        }

        protected override void DoWork(object state)
        {
            try
            {
               
            }
            catch (System.Exception e)
            {
                e.ExceptionLog();
            }
        }
    }
}
