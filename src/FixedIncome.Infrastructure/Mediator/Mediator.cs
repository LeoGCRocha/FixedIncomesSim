using FixedIncome.Application.Mediator;

namespace FixedIncome.Infrastructure.Mediator;

public class Mediator(IServiceProvider provider) : IMediator
{
    public Task<TResponse> Send<TResponse>(IRequest<TResponse> request, CancellationToken cancellationToken = default)
    {
        var type = request.GetType();
        var handlerType = typeof(IRequestHandler<,>).MakeGenericType(type, typeof(TResponse));

        object handlerObject = provider.GetService(handlerType) 
            ?? throw new NotImplementedException($"Handler to {handlerType} was not implemented.");

        var method = handlerType.GetMethod("Handle")
                     ?? throw new NotImplementedException($"Method Handle was not implemented to type {handlerType}");

        var task = method.Invoke(handlerObject, [request, cancellationToken]);

        return (task as Task<TResponse>)!;
    }

    public Task Send(IRequest request, CancellationToken cancellationToken = default)
    {
        var type = request.GetType();
        var handlerType = typeof(IRequestHandler<>).MakeGenericType(type);

        object handlerObject = provider.GetService(handlerType)
                               ?? throw new NotImplementedException($"Handler to {handlerType} was not implemented.");

        var method = handlerType.GetMethod("Handle")
                     ?? throw new NotImplementedException($"Method Handle was not implemented to type {handlerType}");

        method.Invoke(handlerObject, [request, cancellationToken]);

        return Task.CompletedTask;
    }
}