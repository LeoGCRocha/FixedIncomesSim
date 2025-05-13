using FixedIncome.Application.Abstractions;
using FixedIncome.Domain.FixedIncomeSimulation;
using FixedIncome.Domain.FixedIncomeSimulation.FixedIncomeBalances;
using FixedIncome.Domain.FixedIncomeSimulation.FixedIncomeOrders;
using FixedIncome.Infrastructure.Persistence.Abstractions;
using FixedIncome.Domain.FixedIncomeSimulation.Repositories;
using LinqToDB;
using LinqToDB.Data;
using LinqToDB.EntityFrameworkCore;

namespace FixedIncome.Infrastructure.Persistence;

public class UnitOfWork : IUnitOfWork
{
    private readonly ApplicationDbContext _dbContext;
    public IFixedIncomeRepository FixedIncomeRepository { get; }
    public IOutboxPatternRepository OutboxPatternRepository { get; }

    public UnitOfWork(ApplicationDbContext dbContext, IFixedIncomeRepository fixedIncomeRepository, IOutboxPatternRepository outboxPatternRepository)
    {
        _dbContext = dbContext;
        FixedIncomeRepository = fixedIncomeRepository;
        OutboxPatternRepository = outboxPatternRepository;
    }

    public void Dispose()
    {
        _dbContext.Dispose();
    }

    public async Task CommitAsync()
    {
        await _dbContext.SaveChangesAsync();
    }

    public async Task BulkCopyFixedIncomeSim(FixedIncomeSim fixedIncomeSim)
    {
        await using var l2db = _dbContext.CreateLinqToDBConnection();
        await using var transaction = await l2db.BeginTransactionAsync();

        try
        {
            await BulkCopyFixedIncome(transaction, fixedIncomeSim);
            await BulkCopyFixedIncomeOrders(transaction, fixedIncomeSim.GetOrders);
            await BulkCopyFixedIncomeOrderEvents(transaction, fixedIncomeSim.GetOrderEvents);
            await BulkCopyFixedIncomeBalance(transaction, fixedIncomeSim.GetBalances);
            
            await transaction.CommitAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Failed to save this on DB {ex.Message}");
            await transaction.RollbackAsync();
            throw;
        }
    }

    private static async Task BulkCopyFixedIncome(DataConnectionTransaction transaction, FixedIncomeSim fixedIncomeSim)
    {
        var mappedData = new
        {
            InvestmentTitle = fixedIncomeSim.Information.Title,
            InformationType = fixedIncomeSim.Information.Type,
            fixedIncomeSim.Id,
            fixedIncomeSim.StartDate,
            fixedIncomeSim.EndDate,
            fixedIncomeSim.StartAmount,
            fixedIncomeSim.MonthlyYield,
            fixedIncomeSim.MonthlyContribution,
            fixedIncomeSim.InvestedAmount,
            fixedIncomeSim.FinalGrossAmount,
            fixedIncomeSim.FinalNetAmount
        };
        
        await transaction.DataConnection.BulkCopyAsync(new BulkCopyOptions()
        {
            SchemaName = "fixed_incomes",
            TableName = "fixed_income_simulation"
        }, [mappedData]);
    }

    private static async Task BulkCopyFixedIncomeOrders(DataConnectionTransaction transaction,
        IEnumerable<FixedIncomeOrder> orders)
    {
        await transaction.DataConnection.BulkCopyAsync(new BulkCopyOptions()
        {
            SchemaName = "fixed_incomes",
            TableName = "fixed_income_order"
        }, orders);
    }

    private static async Task BulkCopyFixedIncomeOrderEvents(DataConnectionTransaction transaction,
        IEnumerable<FixedIncomeOrderEvent> orderEvents)
    {
        await transaction.DataConnection.BulkCopyAsync(new BulkCopyOptions()
        {
            SchemaName = "fixed_incomes",
            TableName = "fixed_income_order_event"
        }, orderEvents);
    }

    private static async Task BulkCopyFixedIncomeBalance(DataConnectionTransaction transaction,
        IEnumerable<FixedIncomeBalance> fixedIncomeBalances)
    {
        await transaction.DataConnection.BulkCopyAsync(new BulkCopyOptions()
        {
            SchemaName = "fixed_incomes",
            TableName = "fixed_income_balance"
        }, fixedIncomeBalances);
    }
}