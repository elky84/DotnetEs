using EzAspDotNet.Exception;
using EzAspDotNet.Service;
using System;

namespace Server.Services
{
    public class LogRepeatedServie : RepeatedTimerService
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
            catch (Exception e)
            {
                e.ExceptionLog();
            }
        }
    }
}
