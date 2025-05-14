using FixedIncome.Application.Abstractions;
using FixedIncome.Application.Helper;
using FixedIncome.Domain.FixedIncomeSimulation;
using FixedIncome.Domain.FixedIncomeSimulation.FixedIncomeBalances;
using FixedIncome.Domain.FixedIncomeSimulation.FixedIncomeOrders;
using FixedIncome.Infrastructure.Persistence.Abstractions;
using FixedIncome.Domain.FixedIncomeSimulation.Repositories;
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
            await BulkCopyFixedIncomeOrders(transaction, fixedIncomeSim.GetOrders, fixedIncomeSim.Id);
            await BulkCopyFixedIncomeOrderEvents(transaction, fixedIncomeSim.GetOrders);
            await BulkCopyFixedIncomeBalance(transaction, fixedIncomeSim.GetBalances, fixedIncomeSim.Id);
            
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
        var returnedObject = fixedIncomeSim.FixedIncomeSimulationToLinq();
        
        await transaction.DataConnection.BulkCopyAsync(new BulkCopyOptions()
        {
            SchemaName = "fixed_incomes",
            TableName = "fixed_income_simulation"
        }, [ returnedObject ]);
    }

    private static async Task BulkCopyFixedIncomeOrders(DataConnectionTransaction transaction,
        IEnumerable<FixedIncomeOrder> orders,
        Guid fixedIncomeSimulationId)
    {
        var ordersElements = orders.Select(order => order.FixedIncomeOrderToLinq(fixedIncomeSimulationId));
        
        await transaction.DataConnection.BulkCopyAsync(new BulkCopyOptions()
        {
            SchemaName = "fixed_incomes",
            TableName = "fixed_income_order"
        }, ordersElements);
    }

    private static async Task BulkCopyFixedIncomeOrderEvents(DataConnectionTransaction transaction,
        IEnumerable<FixedIncomeOrder> orders)
    {
        List<FixedIncomeEventLinq2Db> orderEvents = [];

        foreach (var order in orders)
            orderEvents.AddRange(order.GetEvents.Select(ordEvent => ordEvent.OrderEventToLinq(order.Id)));
        
        await transaction.DataConnection.BulkCopyAsync(new BulkCopyOptions()
        {
            SchemaName = "fixed_incomes",
            TableName = "fixed_income_order_event"
        }, orderEvents);
    }

    private static async Task BulkCopyFixedIncomeBalance(DataConnectionTransaction transaction,
        IEnumerable<FixedIncomeBalance> fixedIncomeBalances,
        Guid id)
    {
        var balanceObjects = fixedIncomeBalances.Select(balance => balance.BalanceToLinq(id));
        
        await transaction.DataConnection.BulkCopyAsync(new BulkCopyOptions()
        {
            SchemaName = "fixed_incomes",
            TableName = "fixed_income_balance"
        }, balanceObjects);
    }
}