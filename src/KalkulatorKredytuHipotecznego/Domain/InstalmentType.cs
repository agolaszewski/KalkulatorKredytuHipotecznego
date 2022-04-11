namespace KalkulatorKredytuHipotecznego.Domain;

public partial record InstallmentType : ValueObject<Store.States.InstallmentType>
{
    public IInstallmentCalculationStrategy Strategy => Value == Store.States.InstallmentType.Decreasing
        ? new DecreasingInstallmentsCalculationStrategy()
        : new FlatInstallmentsCalculationStrategy();
}