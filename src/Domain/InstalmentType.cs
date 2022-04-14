namespace Domain;

public partial record InstallmentType : ValueObject<InstallmentType.InstallmentTypeValues>
{
    public enum InstallmentTypeValues
    {
        Flat = 1,
        Decreasing = 2
    }

    public IInstallmentCalculationStrategy Strategy => Value == InstallmentTypeValues.Decreasing
        ? new DecreasingInstallmentsCalculationStrategy()
        : new FlatInstallmentsCalculationStrategy();
}