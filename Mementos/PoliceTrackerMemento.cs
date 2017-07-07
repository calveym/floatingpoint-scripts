using Newtonsoft.Json;
using UnityEngine;
using CloudCity;

namespace Autelia.Serialization.Mementos.Unity
{
    [JsonObject(MemberSerialization.Fields)]
    [System.Serializable]
    public sealed class PoliceTrackerMemento : BehaviourMemento<PoliceTracker, PoliceTrackerMemento>
    {
        int _amount;
        string _type;
        int _cost;
        MaterialMemento _sphereMaterial;

        protected override bool Serialize(PoliceTracker originator)
        {
            base.Serialize(originator);

            _amount = originator.amount;
            _type = originator.type;
            _cost = originator.cost;
            _sphereMaterial = originator.sphereMaterial;

            return true;
        }

        protected override void Deserialize(ref PoliceTracker behaviour)
        {
            base.Deserialize(ref behaviour);
            behaviour.buyCost = 0;
            behaviour.amount = _amount;
            behaviour.type = _type;
            behaviour.cost = _cost;
            behaviour.sphereMaterial = _sphereMaterial;
        }

        public static implicit operator PoliceTrackerMemento(PoliceTracker policeTracker)
        { return Create(policeTracker); }

        public static implicit operator PoliceTracker(PoliceTrackerMemento policeTrackerMemento)
        {
            if (policeTrackerMemento == null)
                return null;
            return policeTrackerMemento.Originator;
        }
    }
}