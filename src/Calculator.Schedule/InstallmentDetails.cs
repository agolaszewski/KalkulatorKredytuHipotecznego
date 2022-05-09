using Domain;

namespace Calculator.Schedule;

public record InstallmentDetails
{
    public InstallmentDetails(int index, InstallmentDate installmentDate, Installment installment, Interest interest, CreditAmount creditAmount, Days installmentPeriod)
    {
        Index = index;
        InstallmentDate = installmentDate.Value;
        Installment = installment.Value;
        RateOfInterest = interest.Value * 100M;
        Interest = creditAmount.Value * interest.Value * (installmentPeriod.Value / 365M);
        Principal = Installment - Interest;
        CreditAmount = creditAmount.Value - Principal;
    }

    public static InstallmentDetails LastInstallment(int index, InstallmentDate installmentDate, Interest interest, CreditAmount creditAmount, Days installmentPeriod)
    {
        var interestTemp = creditAmount.Value * interest.Value * (installmentPeriod.Value / 365M);
        var principal = creditAmount.Value;

        return new InstallmentDetails
        {
            Index = index,
            Interest = interestTemp,
            RateOfInterest = interest.Value * 100M,
            Principal = principal,
            CreditAmount = 0,
            Installment = interestTemp + principal,
            InstallmentDate = installmentDate.Value
        };
    }

    private InstallmentDetails()
    {
    }

    public decimal Principal { get; set; }

    public decimal Interest { get; set; }

    public int Index { get; set; }

    public decimal Installment { get; set; }

    public DateTime InstallmentDate { get; set; }

    public decimal CreditAmount { get; set; }

    public decimal RateOfInterest { get; set; }
}