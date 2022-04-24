using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Fluxor;
using KalkulatorKredytuHipotecznego.Store.States;
using Provider.Indexes;

namespace KalkulatorKredytuHipotecznego.Store.Features.CalculationFeature
{
    public class CalculationFeatureRegistration : Feature<CalculationState>
    {
        private readonly IndexProviderFactory _indexProviderFactory;

        public CalculationFeatureRegistration(IndexProviderFactory indexProviderFactory)
        {
            _indexProviderFactory = indexProviderFactory;
        }

        public override string GetName() => nameof(CalculationState);


        protected override CalculationState GetInitialState() => new CalculationState(_indexProviderFactory);

    }
}
