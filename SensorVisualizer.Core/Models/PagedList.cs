using System.Collections.Generic;

namespace SensorVisualizer.Core.Models
{
    public class PagedList<T>
    {
        public List<T> Data { get; set; }
        public int TotalCount { get; set; }

        public PagedList(List<T> data, int totalCount)
        {
            Data = data;
            TotalCount = totalCount;
        }
    }
}
