
using Application.Interfaces;
using Application.Products;
using Application.Test.Fakes;
using AutoMapper;
using Domain;
using Moq;
using Persistence;
using System.Text.Json;
using Xunit.Abstractions;

namespace Application.Test;
public class CreateProductTests
{
    private readonly DataContext _context;
    private readonly ITestOutputHelper _outputHelper;

    public CreateProductTests(ITestOutputHelper outputHelper)
    {
        var databaseManager = new DatabaseManager();
        _context = databaseManager.GetDbContext();
        _outputHelper = outputHelper;
    }

    [Fact]
    public async Task Create_Product_Without_UserId_Should_Fail()
    {
        var mapperMock = new Mock<IMapper>();
        mapperMock.Setup(mapper => mapper.Map<Product>(It.IsAny<CreateProductDto>()))
                      .Returns(new Product
                      {
                          Name = "Test Product",
                          ProduceDate = DateTime.Now,
                          ManufacturePhone = "09123456789",
                          ManufactureEmail = "test@example.com",
                          IsAvailable = true,
                          Description = "This is a test product.",
                          CreatorId = Guid.Parse("0a8e7060-c28a-4d89-8c87-08db8d2068f0"),
                      });

        var userAccessorMock = new Mock<IUserAccessor>();
        userAccessorMock.Setup(x => x.GetUserId()).Returns(Guid.Empty);

        var handler = new Create.Handler(_context, mapperMock.Object, userAccessorMock.Object);
        var productDto = new CreateProductDto
        {
            Name = "Test Product",
            ProduceDate = DateTime.Now,
            ManufacturePhone = "09123456789",
            ManufactureEmail = "test@example.com",
            IsAvailable = true,
            Description = "This is a test product."
        };

        var command = new Create.Command { Product = productDto };

        var result = await handler.Handle(command, CancellationToken.None);
        _outputHelper.WriteLine(JsonSerializer.Serialize(result));
        Assert.False(result.IsSuccess);
        Assert.Equal("The user is not logged in", result.Message);
    }

    [Fact]
    public async Task Product_Create_Update_Delete_Flow_Success()
    {
        var productId = Guid.NewGuid();
        var fakeUserAccessor = new FakeUserAccessor();
        var mapperMock = _getMapperMock(productId, fakeUserAccessor.GetUserId());


        var createHandler = new Create.Handler(_context, mapperMock.Object, fakeUserAccessor);
        var productDto = new CreateProductDto { Id = productId};
        var createCommand = new Create.Command { Product = productDto };
        var result = await createHandler.Handle(createCommand, CancellationToken.None);
        Assert.True(result.IsSuccess);

        var editHandler = new Edit.Handler(_context, mapperMock.Object);
        var editCommand = new Edit.Command { Product = productDto };
        result = await editHandler.Handle(editCommand, CancellationToken.None);
        Assert.True(result.IsSuccess);

        var deleteHandler = new Delete.Handler(_context);
        var deleteCommand = new Delete.Command { Id =  productId };
        result = await deleteHandler.Handle(deleteCommand, CancellationToken.None);
        Assert.True(result.IsSuccess);

    }

    private Mock<IMapper> _getMapperMock(Guid productId, Guid creatorId)
    {
        var mapperMock = new Mock<IMapper>();
        mapperMock.Setup(mapper => mapper.Map<Product>(It.IsAny<CreateProductDto>()))
                      .Returns(new Product
                      {
                          Id = productId,
                          Name = "Test Product",
                          ProduceDate = DateTime.Now,
                          ManufacturePhone = "09123456789",
                          ManufactureEmail = "test@example.com",
                          IsAvailable = true,
                          Description = "This is a test product.",
                          CreatorId = creatorId,
                      });
        return mapperMock;
    }
}
