namespace WebApi.Attributes;

[AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
public class PaymentStrategyAttribute(string key) : Attribute
{
    public string Key { get; } = key.ToLowerInvariant(); // Anahtarı küçük harfe çevirerek tutarlılık sağla
}