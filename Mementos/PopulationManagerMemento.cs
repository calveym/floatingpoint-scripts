using System.Collections.Generic;
using CloudCity;
using Newtonsoft.Json;
using UnityEngine;

namespace Autelia.Serialization.Mementos.Unity
{
    [JsonObject(MemberSerialization.Fields)]
    [System.Serializable]
    public sealed class PopulationManagerMemento : BehaviourMemento<PopulationManager, PopulationManagerMemento>
    {

        int _population, _unallocatedPopulation, _unemployedPopulation;


        protected override bool Serialize(PopulationManager originator)
        {
            base.Serialize(originator);

            _population = originator.population;
            _unallocatedPopulation = originator.unallocatedPopulation;
            _unemployedPopulation = originator.unemployedPopulation;

            return true;
        }

        protected override void Deserialize(ref PopulationManager r)
        {
            base.Deserialize(ref r);
        }

        public static implicit operator PopulationManagerMemento(PopulationManager populationManager)
        { return Create(populationManager); }

        public static implicit operator PopulationManager(PopulationManagerMemento populationManagerMemento)
        {
            if (populationManagerMemento == null)
                return null;
            return populationManagerMemento.Originator;
        }
    }
}