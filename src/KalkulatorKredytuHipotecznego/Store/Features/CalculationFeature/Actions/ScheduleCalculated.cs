using Calculator.Schedule;
using System.Collections.Generic;

namespace KalkulatorKredytuHipotecznego.Store.Features.CalculationFeature.Actions
{
    public record ScheduleCalculated(IReadOnlyList<InstallmentDetails> ScheduleInstallmentsDetails);
}