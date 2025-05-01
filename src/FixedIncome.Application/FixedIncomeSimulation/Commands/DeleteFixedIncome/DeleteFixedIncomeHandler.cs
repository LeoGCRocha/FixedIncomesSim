using FixedIncome.Domain.Common.Abstractions;
using FixedIncome.Infrastructure.Persistence.Abstractions;
using MediatR;

namespace FixedIncome.Application.FixedIncomeSimulation.Commands.DeleteFixedIncome;

public class DeleteFixedIncomeHandler : IRequestHandler<DeleteFixedIncomeCommand>
{
    private readonly IUnitOfWork _unitOfWork;

    public DeleteFixedIncomeHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(DeleteFixedIncomeCommand request, CancellationToken cancellationToken)
    {
       await _unitOfWork.FixedIncomeRepository.DeleteAsync(request.Id);
    }
}