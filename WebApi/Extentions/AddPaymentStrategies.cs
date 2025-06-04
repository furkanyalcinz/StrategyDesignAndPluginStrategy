using System.Reflection;
using WebApi.Attributes;
using WebApi.Dtos;
using WebApi.Strategies.Abstract.PaymentStrategies;

namespace WebApi.Extentions;

public static class AddPaymentStrategies
{
    public static Dictionary<string, Func<PaymentRequestDto, IPaymentStrategy>> DiscoverPaymentStrategies()
    {
        var strategyFactories = new Dictionary<string, Func<PaymentRequestDto, IPaymentStrategy>>(StringComparer.OrdinalIgnoreCase);

        // Sadece mevcut assembly'yi veya belirli plugin assembly'lerini tara.
        // Gerçek bir plugin mimarisinde, bir "plugins" klasöründeki DLL'ler taranabilir.
        var assembliesToScan = new[] { Assembly.GetExecutingAssembly() }; // Bu örnekte sadece çalışan assembly

        foreach (var assembly in assembliesToScan)
        {
            var types = assembly.GetTypes()
                .Where(t => typeof(IPaymentStrategy).IsAssignableFrom(t) &&
                            !t.IsInterface &&
                            !t.IsAbstract &&
                            t.GetCustomAttribute<PaymentStrategyAttribute>() != null);

            foreach (var type in types)
            {
                var attribute = type.GetCustomAttribute<PaymentStrategyAttribute>();
                if (attribute != null)
                {
                    var constructor = type.GetConstructor(new[] { typeof(PaymentRequestDto) });
                    if (constructor == null)
                    {
                        // Uygun constructor bulunamazsa logla veya hata fırlat
                        Console.WriteLine($"UYARI: {type.FullName} stratejisi PaymentRequestDto alan bir constructor'a sahip değil.");
                        continue;
                    }

                    // Stratejiyi oluşturan bir lambda fonksiyonu oluştur ve dictionary'ye ekle
                    strategyFactories.Add(attribute.Key, (requestDto) =>
                    {
                        try
                        {
                            // Constructor'ı çağırarak strateji nesnesini oluştur
                            // Hata yönetimi (örn: ArgumentException) strateji constructor'ı içinde yapılmalı
                            return (IPaymentStrategy)constructor.Invoke(new object[] { requestDto });
                        }
                        catch (TargetInvocationException ex) when (ex.InnerException is ArgumentException argumentEx)
                        {
                            // Constructor içindeki ArgumentException'ı yakala ve yeniden fırlat
                            throw argumentEx;
                        }
                        // Diğer TargetInvocationException türleri genel bir hataya işaret edebilir
                    });
                    Console.WriteLine($"Plugin keşfedildi ve kaydedildi: {attribute.Key} -> {type.FullName}");
                }
            }
        }
        return strategyFactories;
    }

}