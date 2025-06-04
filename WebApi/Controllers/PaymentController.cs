using Microsoft.AspNetCore.Mvc;
using WebApi.Contexts;
using WebApi.Dtos;
using WebApi.Strategies.Abstract.PaymentStrategies;

namespace WebApi.Controllers;

public class PaymentController : ControllerBase
{
    private readonly Dictionary<string, Func<PaymentRequestDto, IPaymentStrategy>> _paymentStrategyFactories;

    public PaymentController(Dictionary<string, Func<PaymentRequestDto, IPaymentStrategy>> paymentStrategyFactories)
    {
        _paymentStrategyFactories = paymentStrategyFactories;
    }

    [HttpPost("checkout")]
    public IActionResult ProcessPayment([FromBody] PaymentRequestDto request)
    {
        if (request == null)
        {
            return BadRequest("İstek boş olamaz.");
        }
        if (string.IsNullOrWhiteSpace(request.PaymentMethod))
        {
            return BadRequest("Ödeme yöntemi (PaymentMethod) belirtilmelidir.");
        }

        if (!_paymentStrategyFactories.TryGetValue(request.PaymentMethod, out var strategyFactoryFunc))
        {
            return BadRequest($"Geçersiz ödeme yöntemi: {request.PaymentMethod}. Desteklenenler: {string.Join(", ", _paymentStrategyFactories.Keys)}.");
        }

        ShoppingCartContext cart = new ShoppingCartContext(request.Amount);
        IPaymentStrategy selectedStrategy;

        try
        {
            selectedStrategy = strategyFactoryFunc(request);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message); // Strateji constructor'ından gelen hata
        }
        catch (Exception ex)
        {
            // Genel hata yönetimi (loglama vb.)
            // Logger.LogError(ex, "Strateji oluşturulurken beklenmedik bir hata oluştu.");
            return StatusCode(500, "Ödeme stratejisi oluşturulurken bir sunucu hatası oluştu.");
        }

        cart.SetPaymentStrategy(selectedStrategy);
        string paymentResult = cart.Checkout();

        if (paymentResult.StartsWith("Hata:"))
        {
            return BadRequest(paymentResult);
        }
        return Ok(new { Message = paymentResult });
    }
}