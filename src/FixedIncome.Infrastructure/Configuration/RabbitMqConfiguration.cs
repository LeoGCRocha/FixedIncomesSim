namespace FixedIncome.Infrastructure.Configuration;

public class RabbitMqConfiguration : IBaseConfiguration
{
    private readonly string Hostname;
    private readonly string Port;
    private readonly string Username;
    private readonly string Password;
    
    public string GetConnectionString()
    {
        return $"amqp://{Username}:{Password}@{Hostname}:{Port}";
    }
}