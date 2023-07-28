using Application.Products;
using Application.Test.Fakes;
using FluentValidation.TestHelper;
using Persistence;

namespace Application.Test;

public class ProductValidatorTests
{
    private readonly DataContext _context;

    public ProductValidatorTests()
    {
        var databaseManager = new DatabaseManager();
        _context = databaseManager.GetDbContext();
    }

    [Fact]
    public void Duplicate_ProduceDate_And_ManufactureEmail_Should_Fail_Validation()
    {
        var dupProduct = _context.Products
            .Select(x => new CreateProductDto
            {
                Name = x.Name,
                ProduceDate = x.ProduceDate,
                ManufactureEmail = x.ManufactureEmail,
                ManufacturePhone = x.ManufacturePhone
            }).FirstOrDefault();

        if (dupProduct != null)
        {
            var validator = new ProductValidator(_context);
            var result = validator.TestValidate(dupProduct);

            result.ShouldHaveValidationErrorFor(x => x);
        }

    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("    ")]
    public void ShouldHaveErrorWhenProductNameIsNullOrEmpty(string productName)
    {
        var validator = new ProductValidator(_context);
        var product = new CreateProductDto { Name = productName };

        var result = validator.TestValidate(product);

        result.ShouldHaveValidationErrorFor(x => x.Name);
    }

    [Theory]
    [InlineData("invalid-email")]
    [InlineData("user.com")]
    [InlineData("user@.com")]
    [InlineData("user@domain")]
    public void ShouldHaveErrorWhenManufactureEmailIsInvalid(string manufactureEmail)
    {
        var validator = new ProductValidator(_context);
        var product = new CreateProductDto { ManufactureEmail = manufactureEmail };

        var result = validator.TestValidate(product);

        result.ShouldHaveValidationErrorFor(x => x.ManufactureEmail);
    }

    [Theory]
    [InlineData("09123456789")]
    [InlineData("09333333333")]
    [InlineData("09000000000")]
    public void ShouldNotHaveErrorWhenManufacturePhoneIsValid(string manufacturePhone)
    {
        var validator = new ProductValidator(_context);
        var product = new CreateProductDto
        {
            Name = "Test product",
            ProduceDate = DateTime.Now,
            ManufacturePhone = manufacturePhone,
            ManufactureEmail = "test@email.com"
        };

        var result = validator.TestValidate(product);

        result.ShouldNotHaveAnyValidationErrors();
    }

    [Theory]
    [InlineData("0912345678")]
    [InlineData("08333333333")]
    [InlineData("91234567890")]
    public void ShouldNotHaveErrorWhenManufacturePhoneIsInValid(string manufacturePhone)
    {
        var validator = new ProductValidator(_context);
        var product = new CreateProductDto
        {
            Name = "Test product",
            ProduceDate = DateTime.Now,
            ManufacturePhone = manufacturePhone,
            ManufactureEmail = "test@email.com"
        };

        var result = validator.TestValidate(product);

        result.ShouldHaveValidationErrorFor(x => x.ManufacturePhone);
    }

    [Fact]
    public void ShouldHaveErrorWhenProduceDateIsNotSet()
    {
        var validator = new ProductValidator(_context);
        var product = new CreateProductDto { ProduceDate = default(DateTime) };

        var result = validator.TestValidate(product);

        result.ShouldHaveValidationErrorFor(x => x.ProduceDate);
    }

    [Fact]
    public void ShouldNotHaveErrorWhenAllPropertiesAreValid()
    {
        var validator = new ProductValidator(_context);
        var product = new CreateProductDto
        {
            Name = "Valid Product",
            ProduceDate = DateTime.Now.AddDays(1),
            ManufacturePhone = "09123456789",
            ManufactureEmail = "valid-email@example.com",
        };

        var result = validator.TestValidate(product);

        result.ShouldNotHaveAnyValidationErrors();
    }
}
