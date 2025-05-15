namespace FixedIncome.Infrastructure.Exceptions;

public class RabbitMqMessagingException(string message) : Exception(message);