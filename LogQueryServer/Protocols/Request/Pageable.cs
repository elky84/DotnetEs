namespace LogQueryServer.Protocols.Request
{
    public class Pageable
    {
        public int From { get; set; }

        public int Size { get; set; } = 100;
    }
}
