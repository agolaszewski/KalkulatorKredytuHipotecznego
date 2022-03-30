using KalkulatorKredytuHipotecznego.Store.States;

public partial record MortgageLoan
{
    public MortgageLoan(CalculationState state)
    {
        InstallmentType = state.InstallmentType;
    }

    InstallmentType InstallmentType { get; set; }
}