using ErrorOr;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Tax.Calculator.Commify.Business;
using Tax.Calculator.Commify.Contracts.Request;
using Tax.Calculator.Commify.Controllers;
using Tax.Calculator.Commify.Data.Entities;

namespace Tax.Calculator.Commify.Tests;

public class TaxBandControllerTests
{
    private readonly TaxBandController _controller;
    private readonly Mock<ITaxBandService> _taxBandServiceMock;
    private readonly Mock<ILogger<TaxBandController>> _loggerMock;

    public TaxBandControllerTests()
    {
        _taxBandServiceMock = new Mock<ITaxBandService>();
        _loggerMock = new Mock<ILogger<TaxBandController>>();
        _controller = new TaxBandController(_loggerMock.Object, _taxBandServiceMock.Object);
    }

    [Fact]
    public async Task CreateTaxBandAsync_Should_ReturnOk_WhenCreationIsSuccessful()
    {
        var requestDto = new CreateTaxBandRequestDto
        {
            BandName = "Tax Band D",
            LowerSalaryRange = 20000,
            UpperSalaryRange = 30000,
            TaxRate = 40
        };

        var mappedTaxBand = new TaxBand
        {
            Id = Guid.NewGuid(),
            BandName = requestDto.BandName,
            LowerSalaryRange = requestDto.LowerSalaryRange,
            UpperSalaryRange = requestDto.UpperSalaryRange,
            TaxRate = requestDto.TaxRate
        };

        ErrorOr<TaxBand> errorOrTaxBand = mappedTaxBand;

        _taxBandServiceMock
            .Setup(service => service.CreateTaxBandAsync(mappedTaxBand, It.IsAny<CancellationToken>()))
            .ReturnsAsync(errorOrTaxBand);

        var result = await _controller.CreateTaxBandAsync(requestDto, CancellationToken.None);

        var okResult = Assert.IsType<OkObjectResult>(result);
    }

    [Fact]
    public async Task CreateTaxBandAsync_Should_ReturnValidationProblem_WhenCreationFailsWithValidationError()
    {
        var requestDto = new CreateTaxBandRequestDto
        {
            BandName = "Tax Band D",
            LowerSalaryRange = 20000,
            UpperSalaryRange = 30000,
            TaxRate = 40
        };

        var validationError = Error.Validation(code: "InvalidTaxBand", description: "The tax band is invalid.");
        ErrorOr<TaxBand> errorOrTaxBand = ErrorOr<TaxBand>.From(new List<Error>
        {
            validationError
        });

        _taxBandServiceMock
            .Setup(service => service.CreateTaxBandAsync(It.IsAny<TaxBand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(errorOrTaxBand);

        var result = await _controller.CreateTaxBandAsync(requestDto, CancellationToken.None);

        var objectResult = Assert.IsType<ObjectResult>(result);
        Assert.IsType<ValidationProblemDetails>(objectResult.Value);
    }
}