using Newtonsoft.Json;
using UnityEngine;

namespace Autelia.Serialization.Mementos.Unity
{
    [JsonObject(MemberSerialization.Fields)]
    [System.Serializable]
    public sealed class PowerTrackerMemento : BehaviourMemento<PowerTracker, PowerTrackerMemento>
    {
        int _amount;
        string _type;
        int _cost;
        Material _sphereMaterial;

        protected override bool Serialize(PowerTracker originator)
        {
            base.Serialize(originator);

            _amount = originator.amount;
            _type = originator.type;
            _cost = originator.cost;
            _sphereMaterial = originator.sphereMaterial;

            return true;
        }

        protected override void Deserialize(ref PowerTracker behaviour)
        {
            base.Deserialize(ref behaviour);
            behaviour.buyCost = 0;
            behaviour.amount = _amount;
            behaviour.type = _type;
            behaviour.cost = _cost;
            behaviour.sphereMaterial = _sphereMaterial;
        }

        public static implicit operator PowerTrackerMemento(PowerTracker powerTracker)
        { return Create(powerTracker); }

        public static implicit operator PowerTracker(PowerTrackerMemento powerTrackerMemento)
        {
            if (powerTrackerMemento == null)
                return null;
            return powerTrackerMemento.Originator;
        }
    }
}