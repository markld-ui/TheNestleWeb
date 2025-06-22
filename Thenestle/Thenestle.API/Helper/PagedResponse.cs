namespace Thenestle.API.Helper
{
    public class PagedResponse<T>
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalCount { get; set; }
        public List<T> Data { get; set; }

        public PagedResponse(List<T> data, int pageNumber, int pageSize, int totalCount)
        {
            PageNumber = pageNumber;
            PageSize = pageSize;
            TotalCount = totalCount;
            Data = data;
        }
    }
}
