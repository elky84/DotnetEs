using LogQueryServer.Models;
using Nest;

namespace LogQueryServer.Protocols.Request
{
    public class FileLog
    {
        public string Level { get; set; }

        public long? AccountDbId { get; set; }

        public int? UserDbId { get; set; }

        public QueryContainer ToQueryContainer(QueryContainerDescriptor<FileLogData> queryContainerDescriptor)
        {
            var queryContainer = new QueryContainer();
            if (!string.IsNullOrEmpty(Level))
            {
                queryContainer &= queryContainerDescriptor.Match(mq => mq.Field("level").Query(Level));
            }

            if (AccountDbId.HasValue)
            {
                queryContainer &= queryContainerDescriptor.Match(mq => mq.Field("message").Query(AccountDbId.Value.ToString()));
            }

            if (UserDbId.HasValue)
            {
                queryContainer &= queryContainerDescriptor.Match(mq => mq.Field("message").Query(UserDbId.Value.ToString()));
            }

            return queryContainer;
        }
    }
}
