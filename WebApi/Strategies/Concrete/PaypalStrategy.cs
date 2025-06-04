using WebApi.Attributes;
using WebApi.Dtos;
using WebApi.Strategies.Abstract.PaymentStrategies;

namespace WebApi.Strategies.Concrete;

[PaymentStrategy("paypal")]
public class PayPalPaymentStrategy : IPaymentStrategy
{
    private readonly string _email;

    public PayPalPaymentStrategy(PaymentRequestDto request)
    {
        if (string.IsNullOrWhiteSpace(request.PayPalEmail))
        {
            throw new ArgumentException("PayPal email bilgisi (PayPalEmail) PaymentRequestDto içinde eksik veya geçersiz.");
        }
        _email = request.PayPalEmail;
    }

    public string Pay(decimal amount)
    {
        return $"{amount:C} TL, '{_email}' PayPal hesabı ile başarıyla ödendi.";
    }
}
