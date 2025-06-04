namespace WebApi.Strategies.Abstract.PaymentStrategies;

public interface IPaymentStrategy
{
    string Pay(decimal amount);
}