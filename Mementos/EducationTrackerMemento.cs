using Newtonsoft.Json;
using UnityEngine;
using CloudCity;

namespace Autelia.Serialization.Mementos.Unity
{
    [JsonObject(MemberSerialization.Fields)]
    [System.Serializable]
    public sealed class EducationTrackerMemento : BehaviourMemento<EducationTracker, EducationTrackerMemento>
    {
        int _amount;
        string _type;
        int _cost;
        Material _sphereMaterial;

        protected override bool Serialize(EducationTracker originator)
        {
            base.Serialize(originator);

            _amount = originator.amount;
            _type = originator.type;
            _cost = originator.cost;
            _sphereMaterial = originator.sphereMaterial;

            return true;
        }

        protected override void Deserialize(ref EducationTracker behaviour)
        {
            base.Deserialize(ref behaviour);
            behaviour.buyCost = 0;
            behaviour.amount = _amount;
            behaviour.type = _type;
            behaviour.cost = _cost;
            behaviour.sphereMaterial = _sphereMaterial;
        }

        public static implicit operator EducationTrackerMemento(EducationTracker educationTracker)
        { return Create(educationTracker); }

        public static implicit operator EducationTracker(EducationTrackerMemento educationTrackerMemento)
        {
            if (educationTrackerMemento == null)
                return null;
            return educationTrackerMemento.Originator;
        }
    }
}