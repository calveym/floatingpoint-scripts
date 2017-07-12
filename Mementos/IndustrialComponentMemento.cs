using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using CloudCity;

namespace Autelia.Serialization.Mementos.Unity
{
    [JsonObject(MemberSerialization.Fields)]
    [System.Serializable]
    public sealed class IndustrialComponentMemento : BehaviourMemento<IndustrialComponent, IndustrialComponentMemento>
    {

        float _productionMulti;
        int _baseCost;
        int _buyCost;
        bool _usable;
        IndustrialTracker _linkedTracker;
        MaterialMemento _industrialMaterial;

        protected override bool Serialize(IndustrialComponent originator)
        {
            base.Serialize(originator);

            _productionMulti = originator.productionMulti;
            _baseCost = originator.baseCost;
            _buyCost = originator.buyCost;
            _usable = originator.usable;
            _industrialMaterial = originator.industrialMaterial;
            _linkedTracker = originator.linkedTracker;

            return true;
        }

        protected override void Deserialize(ref IndustrialComponent behaviour)
        {
            base.Deserialize(ref behaviour);

            behaviour.productionMulti = _productionMulti;
            behaviour.baseCost = _baseCost;
            behaviour.buyCost = _buyCost;
            behaviour.usable = _usable;
            behaviour.industrialMaterial = _industrialMaterial;
        }

        public static implicit operator IndustrialComponentMemento(IndustrialComponent indc)
        { return Create(indc); }

        public static implicit operator IndustrialComponent(IndustrialComponentMemento industrialComponentMemento)
        {
            if (industrialComponentMemento == null)
                return null;
            return industrialComponentMemento;
        }
    }
}
