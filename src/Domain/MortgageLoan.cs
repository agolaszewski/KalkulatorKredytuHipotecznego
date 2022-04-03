using KalkulatorKredytuHipotecznego.Store.States;

namespace KalkulatorKredytuHipotecznego.Domain;

public partial record MortgageLoan
{
    public MortgageLoan(CalculationState state)
    {
        InstallmentType = state.InstallmentType;
    }

    Store.States.InstallmentType InstallmentType { get; set; }
}