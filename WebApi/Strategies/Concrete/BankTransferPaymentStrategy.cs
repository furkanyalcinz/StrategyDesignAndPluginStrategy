using WebApi.Attributes;
using WebApi.Dtos;
using WebApi.Strategies.Abstract.PaymentStrategies;

namespace WebApi.Strategies.Concrete;

[PaymentStrategy("banktransfer")]
public class BankTransferPaymentStrategy : IPaymentStrategy
{
    private readonly string _iban;

    public BankTransferPaymentStrategy(PaymentRequestDto request)
    {
        if (string.IsNullOrWhiteSpace(request.Iban))
        {
            throw new ArgumentException("IBAN bilgisi (Iban) PaymentRequestDto içinde eksik veya geçersiz.");
        }
        _iban = request.Iban;
    }

    public string Pay(decimal amount)
    {
        return $"{amount:C} TL, '{_iban}' IBAN numarasına havale talimatı başarıyla oluşturuldu.";
    }
}