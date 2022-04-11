using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Fluxor;
using KalkulatorKredytuHipotecznego.Store.States;

namespace KalkulatorKredytuHipotecznego.Store.Features.CalculationFeature
{
    public class CalculationFeatureRegistration : Feature<CalculationState>
    {
        public override string GetName() => nameof(CalculationState);


        protected override CalculationState GetInitialState() => new CalculationState();

    }
}
