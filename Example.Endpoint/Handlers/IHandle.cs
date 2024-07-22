namespace Example.Endpoint.Handlers;

public interface IHandle<in T>
{
    Task HandleAsync(T message);
}