namespace KalkulatorKredytuHipotecznego.Domain;

public partial record WarsawInterbankOfferedRatePeriod : ValueObject<int>
{
    public static WarsawInterbankOfferedRatePeriod Wibor3M = new WarsawInterbankOfferedRatePeriod(3);
    public static WarsawInterbankOfferedRatePeriod Wibor6M = new WarsawInterbankOfferedRatePeriod(6);
}