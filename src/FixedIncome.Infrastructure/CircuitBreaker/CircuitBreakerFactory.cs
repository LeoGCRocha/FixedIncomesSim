using Polly;
using FixedIncome.Infrastructure.Exceptions;

namespace FixedIncome.Infrastructure.CircuitBreaker;

public static class CircuitBreakerFactory
{
    public static ISyncPolicy CreateMessagePolicy()
    {
        var circuitBreakerPolicy = Policy
            .Handle<Exception>()
            .CircuitBreaker(
                exceptionsAllowedBeforeBreaking: 3,
                durationOfBreak: TimeSpan.FromSeconds(30)
            );

        return circuitBreakerPolicy;
    }
}