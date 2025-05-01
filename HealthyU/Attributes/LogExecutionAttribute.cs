namespace HealthyU.WebApi.Attributes;

[AttributeUsage(AttributeTargets.Method)]
public class LogExecutionAttribute : Attribute
{
    public string Message { get; }

    public LogExecutionAttribute(string? message)
    {
        Message = string.IsNullOrEmpty(message) ? "unknown api" : message;
    }
}
