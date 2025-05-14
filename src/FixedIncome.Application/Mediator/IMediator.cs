using FixedIncome.Domain.Common.Abstractions;

namespace FixedIncome.Application.Mediator;

public interface IMediator
{
    public Task<TResponse> Send<TResponse>(IRequest<TResponse> request, CancellationToken cancellationToken = default);
    public Task Send(IRequest request, CancellationToken cancellationToken = default);

    public Task Publish<TNotification>(TNotification notification, CancellationToken cancellationToken = default)
        where TNotification : INotification;
}