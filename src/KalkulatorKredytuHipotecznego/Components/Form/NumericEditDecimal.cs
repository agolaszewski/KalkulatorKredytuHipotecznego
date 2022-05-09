using Microsoft.AspNetCore.Components;
using System.Threading.Tasks;

namespace KalkulatorKredytuHipotecznego.Components.Form
{
    public partial class NumericEditDecimal
    {
        [Parameter]
        public decimal Min { get; set; }

        [Parameter]
        public decimal Max { get; set; }

        [Parameter]
        public decimal Step { get; set; }

        [Parameter]
        public decimal Value { get; set; }

        [Parameter]
        public EventCallback<decimal> ValueChanged { get; set; }

        [Parameter]
        public RenderFragment StartContent { get; set; }

        [Parameter]
        public RenderFragment EndContent { get; set; }

        public Task OnDecresedClicked()
        {
            Value -= Step;
            if (Value < Min)
            {
                Value = Min;
            }
            return ValueChanged.InvokeAsync(Value);
        }

        public Task OnIncreaseClicked()
        {
            Value += Step;
            if (Value > Max)
            {
                Value = Max;
            }
            return ValueChanged.InvokeAsync(Value);
        }

        public Task OnValueChanged(string value)
        {
            Value = decimal.Parse(value);

            if (Value < Min)
            {
                Value = Min;
            }

            if (Value > Max)
            {
                Value = Max;
            }

            return ValueChanged.InvokeAsync(Value);
        }
    }
}