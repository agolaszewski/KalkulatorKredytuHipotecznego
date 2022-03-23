namespace KalkulatorKredytuHipotecznego.Domain;

public record ValueObject<T>
{
    public T Value { get; init; }
}