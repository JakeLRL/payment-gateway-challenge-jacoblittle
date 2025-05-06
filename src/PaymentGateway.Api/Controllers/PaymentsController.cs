using Microsoft.AspNetCore.Mvc;

using PaymentGateway.Api.Models.Requests;
using PaymentGateway.Api.Models.Responses;
using PaymentGateway.Api.Services;
using PaymentGateway.Api.Validation;

namespace PaymentGateway.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PaymentsController : Controller
{
    private readonly IPaymentsService _paymentsService;
    private readonly ILogger _logger;

    public PaymentsController(IPaymentsService paymentsService, ILogger<PaymentsController> logger)
    {
        _paymentsService = paymentsService;
        _logger = logger;
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<GetPaymentResponse?>> GetPaymentAsync(Guid id)
    {
        var payment = _paymentsService.GetPayment(id);

        _logger.LogInformation("GetPaymentAsync requested with id: {id}", id);

        if (payment == null)
        {
            _logger.LogInformation("No Payment found with id: {id}", id);
            return NotFound();
        }

        return new OkObjectResult(payment);
    }

    [HttpPost]
    public async Task<ActionResult<PostPaymentResponse?>> PostPaymentAsync(PostPaymentRequest request)
    {
        var validator = new PostPaymentRequestValidator();
        var validationResult = validator.Validate(request);

        if (!validationResult.IsValid)
        {
            return BadRequest(validationResult.Errors.FirstOrDefault());
        }

        var payment = await _paymentsService.AddPayment(request);

        if (payment == null)
        {
            return StatusCode(StatusCodes.Status503ServiceUnavailable, "BankSimulator is offline");
        }

        return new OkObjectResult(payment);
    }
}