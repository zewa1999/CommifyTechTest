using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Tax.Calculator.Commify.Business;
using Tax.Calculator.Commify.Contracts.Request;
using Tax.Calculator.Commify.Contracts.Response;
using Tax.Calculator.Commify.Controllers;

namespace Tax.Calculator.Commify.Tests;

public class TaxCalculatorControllerTests
{
    private readonly TaxCalculatorController _controller;
    private readonly Mock<ITaxCalculatorService> _taxCalculatorServiceMock;
    private readonly Mock<ILogger<TaxCalculatorController>> _loggerMock;

    public TaxCalculatorControllerTests()
    {
        _taxCalculatorServiceMock = new Mock<ITaxCalculatorService>();
        _loggerMock = new Mock<ILogger<TaxCalculatorController>>();
        _controller = new TaxCalculatorController(_taxCalculatorServiceMock.Object, _loggerMock.Object);
    }

    [Fact]
    public async Task CalculateTaxAsync_Should_Return_Ok_If_Service_Returns_Taxes()
    {
        var salaryRequest = new CalculateTaxRequestDto
        {
            GrossAnnualSalary = 111111
        };

        _taxCalculatorServiceMock.Setup(x => x.CalculateTaxAsync(salaryRequest, CancellationToken.None))
            .ReturnsAsync(new CalculateTaxResponseDto
            {
                GrossAnnualSalary = 40000,
                GrossMonthlySalary = 3333.33M,
                NetAnnualSalary = 29000,
                NetMonthlySalary = 2416.67M,
                AnnualTaxPaid = 11000.00M,
                MonthlyTaxPaid = 916.67M,
            });

        var response = await _controller.CalculateTaxAsync(salaryRequest, CancellationToken.None);

        OkObjectResult okResult = Assert.IsType<OkObjectResult>(response);
    }

    [Fact]
    public async Task CalculateTaxAsync_Should_Return_InternalServerError_If_Service_ThrowsException()
    {
        _controller.ControllerContext.HttpContext = new DefaultHttpContext();
        var salaryRequest = new CalculateTaxRequestDto
        {
            GrossAnnualSalary = 15000
        };

        _taxCalculatorServiceMock.Setup(x => x.CalculateTaxAsync(salaryRequest, CancellationToken.None))
            .ThrowsAsync(new Exception("Something went wrong"));

        var response = await _controller.CalculateTaxAsync(salaryRequest, CancellationToken.None);

        ObjectResult internalServerErrorResult = (ObjectResult)response;

        Assert.Equal(500, internalServerErrorResult.StatusCode);
    }
}