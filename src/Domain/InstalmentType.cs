namespace KalkulatorKredytuHipotecznego.Domain;

public record InstalmentType : ValueObject<Store.States.InstalmentType>
{

    public static implicit operator InstalmentType(Store.States.InstalmentType value)
    {
        return new InstalmentType() {Value = value};
    }

    private IInstalmentCalculationStrategy Strategy => Value == Store.States.InstalmentType.Decreasing ? new DecreasingInstalmentsCalculationStrategy() : null;
}