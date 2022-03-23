namespace KalkulatorKredytuHipotecznego.Domain
{
    public record CreditAmount : ValueObject<decimal>
    {
        public static implicit operator CreditAmount(decimal value)
        {
            return new CreditAmount() { Value = value };
        }
    }
}