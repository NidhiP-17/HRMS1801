namespace HRMS.Models
{
    public class PaginatedList<T> : List<T>
    {
        public int PageIndex { get; set; }
        public int TotalPages { get; set; }

        public PaginatedList(List<T> items, int count, int pageindex, int pagesize)
        {
            PageIndex = pageindex;
            TotalPages = (int)Math.Ceiling(count / (double)pagesize);
            this.AddRange(items);
        }
        public bool HasPrevpage => PageIndex > 1;

        public bool HasNextPage => PageIndex < TotalPages;

        public static PaginatedList<T> Create(List<T>? source, int pageIndex, int pagesize)
        {
            var count = source.Count;
            var items = source.Skip((pageIndex - 1) * pagesize).Take(pagesize).ToList();
            return new PaginatedList<T>(items, count, pageIndex, pagesize);
        }

    }
}
