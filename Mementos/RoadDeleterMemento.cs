using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;
using CloudCity;

namespace Autelia.Serialization.Mementos.Unity
{
    [JsonObject(MemberSerialization.Fields)]
    [System.Serializable]
    public sealed class RoadDeleterMemento : BehaviourMemento<RoadDeleter, RoadDeleterMemento>
    {
        protected override bool Serialize(RoadDeleter originator)
        {
            base.Serialize(originator);
            return true;
        }

        protected override void Deserialize(ref RoadDeleter r)
        {
            base.Deserialize(ref r);
        }

        public static implicit operator RoadDeleterMemento(RoadDeleter RoadDeleter)
        { return Create(RoadDeleter); }

        public static implicit operator RoadDeleter(RoadDeleterMemento RoadDeleterMemento)
        {
            if (RoadDeleterMemento == null)
                return null;
            return RoadDeleterMemento.Originator;
        }
    }
}