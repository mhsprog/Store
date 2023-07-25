using System.Text.Json;

namespace API.Extensions
{
    public static class HttpExtensions
    {
        public static void AddPaginationHeader(this HttpResponse response, int current,
            int pageSize, int items, int pages)
        {
            var paginationHeader = new
            {
                current,
                pageSize,
                items,
                pages
            };
            response.Headers.Add("Pagination", JsonSerializer.Serialize(paginationHeader));
            response.Headers.Add("Access-Control-Expose-Headers", "Pagination");
        }
    }
}
