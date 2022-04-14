namespace Domain;

public partial record WarsawInterbankOfferedRatePeriod : ValueObject<int>
{
    public static WarsawInterbankOfferedRatePeriod Wibor3M = new WarsawInterbankOfferedRatePeriod(3);
    public static WarsawInterbankOfferedRatePeriod Wibor6M = new WarsawInterbankOfferedRatePeriod(6);

    public static WarsawInterbankOfferedRatePeriod Create(int period)
    {
        return period switch
        {
            3 => WarsawInterbankOfferedRatePeriod.Wibor3M,
            6 => WarsawInterbankOfferedRatePeriod.Wibor6M,
            _ => throw new ArgumentOutOfRangeException(nameof(period))
        };
    }
}