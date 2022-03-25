using KalkulatorKredytuHipotecznego.Store.States;

namespace KalkulatorKredytuHipotecznego.Domain
{
    public record MortageLoan
    {
        public MortageLoan(CalculationState state)
        {
            InstalmentType = state.InstalmentType;
        }

        private CreditAmount CreditAmount; 

        InstalmentType InstalmentType { get; set; }
    }
}
