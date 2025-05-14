using FixedIncome.Domain.Common.Abstractions;

namespace FixedIncome.Application.Mediator;

public interface IRequest<out TResponse>;

public interface IRequest;

public interface IRequestHandler<in TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    public Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken = default);
}

public interface IRequestHandler<in TRequest>
    where TRequest : IRequest
{
    public Task Handle(TRequest request, CancellationToken cancellationToken = default);
}

public interface INotificationHandler<in TNotification>
    where TNotification : INotification
{
    public Task Handle(TNotification notification, CancellationToken cancellationToken = default);
}