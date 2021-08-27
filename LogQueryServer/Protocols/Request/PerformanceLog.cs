namespace LogQueryServer.Protocols.Request
{
    public class PerformanceLog
    {
        public string LogLevel { get; set; }

        public long AccountDbId { get; set; }

        public int UserDbId { get; set; }
    }
}
