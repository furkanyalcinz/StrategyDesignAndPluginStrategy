using WebApi.Attributes;
using WebApi.Dtos;
using WebApi.Strategies.Abstract.PaymentStrategies;

namespace WebApi.Strategies.Concrete;

[PaymentStrategy("creditcard")] 
public class CreditCardPaymentStrategy: IPaymentStrategy
{
    private readonly string _cardNumber;
    private readonly string _cardHolderName;

    // Constructor'ı PaymentRequestDto alacak şekilde güncelle
    public CreditCardPaymentStrategy(PaymentRequestDto request)
    {
        if (string.IsNullOrWhiteSpace(request.CardNumber) || string.IsNullOrWhiteSpace(request.CardHolderName))
        {
            throw new ArgumentException("Kredi kartı bilgileri (CardNumber, CardHolderName) PaymentRequestDto içinde eksik veya geçersiz.");
        }
        _cardNumber = request.CardNumber;
        _cardHolderName = request.CardHolderName;
    }

    public string Pay(decimal amount)
    {
        return $"{amount:C} TL, '{_cardHolderName}' adına kayıtlı {_cardNumber} numaralı kredi kartı ile başarıyla ödendi.";
    }
}

