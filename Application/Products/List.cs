using API.Helper.DTOS;
using Application.Core;
using Application.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Products;
public class List
{
    public class Query : IRequest<Result<PagedList<GetProductDto>>>
    {
        public ProductParam Params { get; set; }
    }

    public class Handler : IRequestHandler<Query, Result<PagedList<GetProductDto>>>
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public Handler(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Result<PagedList<GetProductDto>>> Handle(Query request, CancellationToken cancellationToken)
        {
            var query = _context.Products
                .Where(x => x.IsAvailable == request.Params.IsAvailable)
                .ProjectTo<GetProductDto>(_mapper.ConfigurationProvider)
                .OrderBy(x => x.CreatedAt)
                .AsNoTracking();

            if (!string.IsNullOrEmpty(request.Params.Q))
                query = query.Where(x => x.Name.Contains(request.Params.Q));

            if (!string.IsNullOrEmpty(request.Params.ManufactureEmail))
                query = query.Where(x => x.ManufactureEmail == request.Params.ManufactureEmail);

            if (!string.IsNullOrEmpty(request.Params.ManufacturePhone))
                query = query.Where(x => x.ManufacturePhone == request.Params.ManufacturePhone);

            if (request.Params.CreatorId != Guid.Empty)
                query = query.Where(x => x.CreatorId == request.Params.CreatorId);

            if (request.Params.ProduceFrom != DateTime.MinValue)
                query = query.Where(x => x.ProduceDate >= request.Params.ProduceFrom);

            if (request.Params.ProduceTo != DateTime.MinValue)
                query = query.Where(x => x.ProduceDate <= request.Params.ProduceTo);

            return Result<PagedList<GetProductDto>>.Success(
            await PagedList<GetProductDto>.CreateAsync(query, request.Params.Pn, request.Params.Ps));
        }
    }
}
