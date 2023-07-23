using Application.Core;
using Application.Interfaces;
using AutoMapper;
using Domain;
using FluentValidation;
using MediatR;
using Persistence;

namespace Application.Products;
public class Create
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
        private readonly IUserAccessor _userAccessor;

        public Handler(DataContext context, IMapper mapper, IUserAccessor userAccessor)
        {
            _context = context;
            _mapper = mapper;
            _userAccessor = userAccessor;
        }

        public async Task<Result<Unit>> Handle(Command request, CancellationToken cancellationToken)
        {
            var userId = _userAccessor.GetUserId();
            if (userId == Guid.Empty) return Result<Unit>.Failure("The user is not logged in");

            var product = _mapper.Map<Product>(request.Product);
            product.CreatorId = userId;

            _context.Products.Add(product);

            var result = await _context.SaveChangesAsync() > 0;

            if (!result)
            {
                return Result<Unit>.Failure("Failed to create product");
            }
            return Result<Unit>.Success(Unit.Value);
        }
    }
}
