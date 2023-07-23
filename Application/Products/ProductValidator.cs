using FluentValidation;
using Persistence;

namespace Application.Products;
public class ProductValidator : AbstractValidator<CreateProductDto>
{
    private readonly DataContext _context;

    public ProductValidator(DataContext context)
    {
        RuleFor(x => x).Must(CheckProduct)
            .WithMessage("Product exist");
        RuleFor(x => x.Name).NotEmpty()
            .MaximumLength(128);

        RuleFor(x => x.ProduceDate).NotEmpty().NotEqual(DateTime.MinValue);

        RuleFor(x => x.Description).MaximumLength(512);

        RuleFor(x => x.ManufactureEmail).NotEmpty()
            .EmailAddress();

        RuleFor(x => x.ManufacturePhone)
            .Matches("09[0-9]{9}$").WithMessage("Phone number is incorrect");
        _context = context;
    }
    protected bool CheckProduct(CreateProductDto product)
    {
        return !_context.Products.Any(x 
            => x.ProduceDate == product.ProduceDate 
            && x.ManufactureEmail == product.ManufactureEmail
            && x.Id != product.Id);
    }
}