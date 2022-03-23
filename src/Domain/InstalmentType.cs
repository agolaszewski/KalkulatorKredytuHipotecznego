namespace KalkulatorKredytuHipotecznego.Domain;

public record InstalmentType : ValueObject<Store.States.InstalmentType>
{
    private IInstalmentCalculationStrategy Strategy => Value == Store.States.InstalmentType.Decreasing ? new DecreasingInstalmentsCalculationStrategy() : null;
}