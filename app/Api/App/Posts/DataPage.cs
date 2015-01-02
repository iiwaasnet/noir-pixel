using System.Collections.Generic;

namespace Api.App.Posts
{
    public class DataPage<T>
    {
        public int TotalCount { get; set; }
        public int Skip { get; set; }
        public int Limit { get; set; }
        public IEnumerable<T> Data { get; set; }
    }
}