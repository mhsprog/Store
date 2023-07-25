using Microsoft.AspNetCore.Mvc;

namespace Application.Core;

public class PagingParams
{
    [FromQuery]
    public string Q { get; set; }

    private const int _maxPageSize = 50;

    /// <summary>
    /// Page number
    /// </summary>
    [FromQuery]
    public int Pn { get; set; } = 1;

    private int _pageSize = 10;

    /// <summary>
    /// Page size
    /// </summary>
    [FromQuery]
    public int Ps
    {
        get => _pageSize;
        set => _pageSize = (value > _maxPageSize) ? _maxPageSize : value;
    }
}
