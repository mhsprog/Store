using Application.Core;
using AutoMapper;
using FluentValidation;
using MediatR;
using Persistence;

namespace Application.Products;
public class Edit
{
    public class Command : IRequest<Result<Unit>>
    {
        public CreateProductDto Product { get; set; }
    }
    public class CheckValidation : AbstractValidator<Command>
    {
        public CheckValidation(DataContext context)
        {
            RuleFor(x => x.Product).SetValidator(new ProductValidator(context));
        }
    }

    public class Handler : IRequestHandler<Command, Result<Unit>>
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public Handler(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<Result<Unit>> Handle(Command request, CancellationToken cancellationToken)
        {
            var product = await _context.Products.FindAsync(request.Product.Id);

            if (product == null) return null;

            _mapper.Map(request.Product, product);
            product.UpdatedAt = DateTime.Now;

            var result = await _context.SaveChangesAsync() > 0;

            if (!result)
            {
                return Result<Unit>.Failure("Failed to update product");
            }
            return Result<Unit>.Success(Unit.Value);

        }
    }
}
