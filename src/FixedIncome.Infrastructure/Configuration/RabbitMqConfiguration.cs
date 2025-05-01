namespace FixedIncome.Infrastructure.Configuration;

public class RabbitMqConfiguration : IBaseConfiguration
{
    public string Host { get; set; } = string.Empty;
    public string Port { get; set; } = string.Empty;
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;

    public string GetConnectionString()
    {
        return $"amqp://{Username}:{Password}@{Host}:{Port}";
    }
}