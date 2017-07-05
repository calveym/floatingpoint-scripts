using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;
using CloudCity;

namespace Autelia.Serialization.Mementos.Unity
{
    [JsonObject(MemberSerialization.Fields)]
    [System.Serializable]
    public sealed class FoliageTrackerMemento : BehaviourMemento<FoliageTracker, FoliageTrackerMemento>
    {

        bool _active;
        int _buyCost;
        int _radius;
        Material _sphereMaterial;
        Sphere _sphereScript;
        int _affectAmount;

        protected override bool Serialize(FoliageTracker originator)
        {
            base.Serialize(originator);
            return true;
        }

        protected override void Deserialize(ref FoliageTracker r)
        {
            base.Deserialize(ref r);
        }

        public static implicit operator FoliageTrackerMemento(FoliageTracker foliageTracker)
        { return Create(foliageTracker); }

        public static implicit operator FoliageTracker(FoliageTrackerMemento foliageTrackerMemento)
        {
            if (foliageTrackerMemento == null)
                return null;
            return foliageTrackerMemento.Originator;
        }
    }
}