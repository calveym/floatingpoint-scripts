using Newtonsoft.Json;
using UnityEngine;
using CloudCity;

namespace Autelia.Serialization.Mementos.Unity
{
    [JsonObject(MemberSerialization.Fields)]
    [System.Serializable]
    public sealed class FireTrackerMemento : BehaviourMemento<FireTracker, FireTrackerMemento>
    {
        int _amount;
        string _type;
        int _cost;
        MaterialMemento _sphereMaterial;

        protected override bool Serialize(FireTracker originator)
        {
            base.Serialize(originator);

            _amount = originator.amount;
            _type = originator.type;
            _cost = originator.cost;
            _sphereMaterial = originator.sphereMaterial;

            return true;
        }

        protected override void Deserialize(ref FireTracker behaviour)
        {
            base.Deserialize(ref behaviour);
            behaviour.buyCost = 0;
            behaviour.amount = _amount;
            behaviour.type = _type;
            behaviour.cost = _cost;
            behaviour.sphereMaterial = _sphereMaterial;
        }

        public static implicit operator FireTrackerMemento(FireTracker fireTracker)
        { return Create(fireTracker); }

        public static implicit operator FireTracker(FireTrackerMemento fireTrackerMemento)
        {
            if (fireTrackerMemento == null)
                return null;
            return fireTrackerMemento.Originator;
        }
    }
}