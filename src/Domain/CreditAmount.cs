namespace KalkulatorKredytuHipotecznego.Domain
{
    public partial record CreditAmount : ValueObject<decimal>
    {
        public static implicit operator CreditAmount(decimal value)
        {
            return new CreditAmount() { Value = value };
        }
    }
}