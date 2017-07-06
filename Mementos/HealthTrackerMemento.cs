using Newtonsoft.Json;
using UnityEngine;
using CloudCity;

namespace Autelia.Serialization.Mementos.Unity
{
    [JsonObject(MemberSerialization.Fields)]
    [System.Serializable]
    public sealed class HealthTrackerMemento : BehaviourMemento<HealthTracker, HealthTrackerMemento>
    {
        int _amount;
        string _type;
        int _cost;
        Material _sphereMaterial;

        protected override bool Serialize(HealthTracker originator)
        {
            base.Serialize(originator);

            _amount = originator.amount;
            _type = originator.type;
            _cost = originator.cost;
            _sphereMaterial = originator.sphereMaterial;

            return true;
        }

        protected override void Deserialize(ref HealthTracker behaviour)
        {
            base.Deserialize(ref behaviour);
            behaviour.buyCost = 0;
            behaviour.amount = _amount;
            behaviour.type = _type;
            behaviour.cost = _cost;
            behaviour.sphereMaterial = _sphereMaterial;
        }

        public static implicit operator HealthTrackerMemento(HealthTracker healthTracker)
        { return Create(healthTracker); }

        public static implicit operator HealthTracker(HealthTrackerMemento healthTrackerMemento)
        {
            if (healthTrackerMemento == null)
                return null;
            return healthTrackerMemento.Originator;
        }
    }
}