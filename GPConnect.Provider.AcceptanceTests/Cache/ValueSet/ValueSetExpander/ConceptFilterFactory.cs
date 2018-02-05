namespace GPConnect.Provider.AcceptanceTests.Cache.ValueSet.ValueSetExpander
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Hl7.Fhir.Model;

    public class ConceptFilterFactory
    {
        private static ValueSet.FilterComponent _filter;
        private bool _isInclude;
        private static List<ValueSet.ContainsComponent> _flattenedContainsComponents;

        public ConceptFilterFactory(ValueSet.FilterComponent filter, bool isInclude)
        {
            _filter = filter;
            _isInclude = isInclude;
        }

        public List<ValueSet.ContainsComponent> ApplyFilter(List<ValueSet.ContainsComponent> conceptSets)
        {
            _flattenedContainsComponents = Flatten(conceptSets);

            switch (_filter.Op)
            {
                case FilterOperator.IsA:
                    return ApplyIsAOperator();
                default:
                    throw new NotImplementedException();
            }
        }

        private List<ValueSet.ContainsComponent> ApplyIsAOperator()
        {
            var filteredComponents = _flattenedContainsComponents
                .Where(cs => cs.Code.Equals(_filter.Value))
                .ToList();

            return Flatten(filteredComponents);
        }

        private static List<ValueSet.ContainsComponent> Flatten(List<ValueSet.ContainsComponent> containsComponents)
        {
            return containsComponents
                .SelectMany(containsComponent => Flatten(containsComponent.Contains))
                .Concat(new List<ValueSet.ContainsComponent>(containsComponents))
                .ToList();
        }
    }
}
