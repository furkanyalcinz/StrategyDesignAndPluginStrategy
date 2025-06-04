using System.Diagnostics.CodeAnalysis;
using WebApi.Strategies.Abstract.PaymentStrategies;

namespace WebApi.Contexts;

// Bağlam Sınıfı
public class ShoppingCartContext(decimal totalAmount)
{
    private IPaymentStrategy? _paymentStrategy;

    // Stratejiyi çalışma zamanında ayarlamak için metot
    public void SetPaymentStrategy(IPaymentStrategy paymentStrategy)
    {
        _paymentStrategy = paymentStrategy;
    }

    [SuppressMessage("ReSharper", "ConditionIsAlwaysTrueOrFalseAccordingToNullableAPIContract")]
    public string Checkout()
    {
        if (_paymentStrategy == null)
        {
            Console.WriteLine("Lütfen bir ödeme yöntemi seçin.");
            return "Ödeme yapılamadı.";
        }
        // Ödeme işlemi seçilen stratejiye delege edilir
        return _paymentStrategy.Pay(totalAmount);
    }
}