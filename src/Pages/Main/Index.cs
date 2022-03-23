using Fluxor;
using Fluxor.Blazor.Web.Components;
using KalkulatorKredytuHipotecznego.Domain;
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
            var xd = new FlatInstalmentsCalculationStrategy();

            var lol = xd.Execute(160000.0M, new Years(10) , 2.37M, null);
            Numbers.Add(Numbers.Count + 1);
        }
    }
}