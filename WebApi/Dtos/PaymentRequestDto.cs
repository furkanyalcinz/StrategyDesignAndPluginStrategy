namespace WebApi.Dtos;

// API'ye gönderilecek istek modeli
public record PaymentRequestDto
{
    public decimal Amount { get; set; }
    public required string PaymentMethod { get; set; } // "CreditCard", "PayPal", "BankTransfer"

    // Kredi Kartı bilgileri (PaymentMethod "CreditCard" ise kullanılır)
    public string? CardNumber { get; set; }
    public string? CardHolderName { get; set; }

    // PayPal bilgileri (PaymentMethod "PayPal" ise kullanılır)
    public string? PayPalEmail { get; set; }

    // Banka Havalesi bilgileri (PaymentMethod "BankTransfer" ise kullanılır)
    public string? Iban { get; set; }
}