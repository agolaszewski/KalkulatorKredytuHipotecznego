namespace Domain;

public partial record MortgageLoan
{
    public MortgageLoan(InstallmentType installmentType)
    {
        InstallmentType = installmentType;
    }

    InstallmentType InstallmentType { get; set; }
}