using Fluxor;
using Fluxor.Blazor.Web.Components;
using KalkulatorKredytuHipotecznego.Store.States;
using Microsoft.AspNetCore.Components;
using System.Collections.Generic;

namespace KalkulatorKredytuHipotecznego.Pages.Main
{
    public partial class Index : FluxorComponent
    {
        [Inject]
        public IState<CalculationState> State { get; set; }

        [Inject]
        private IDispatcher Dispatcher { get; set; }

        protected override void OnParametersSet()
        {
            BaseState<CalculationState>.Dispatcher = Dispatcher;
        }

        private string selectedTab = "basicDetails";

        private void OnSelectedTabChanged(string name)
        {
            selectedTab = name;
        }

        private List<int> Numbers = new List<int>();

        private void OnCalculateButtonClicked()
        {
            Numbers.Add(Numbers.Count + 1);
        }
    }
}