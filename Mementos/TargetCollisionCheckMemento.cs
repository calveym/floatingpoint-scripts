using System.Collections.Generic;
using CloudCity;
using Newtonsoft.Json;
using UnityEngine;

namespace Autelia.Serialization.Mementos.Unity
{
    [JsonObject(MemberSerialization.Fields)]
    [System.Serializable]
    public sealed class TargetCollisionCheckMemento : BehaviourMemento<TargetCollisionCheck, TargetCollisionCheckMemento>
    {

        GameObjectMemento _parentBuilding;

        protected override bool Serialize(TargetCollisionCheck originator)
        {
            base.Serialize(originator);

            _parentBuilding = originator.parentBuilding;

            return true;
        }

        protected override void Deserialize(ref TargetCollisionCheck r)
        {
            base.Deserialize(ref r);

            r.parentBuilding = _parentBuilding;
        }

        public static implicit operator TargetCollisionCheckMemento(TargetCollisionCheck targetCollisionCheck)
        { return Create(targetCollisionCheck); }

        public static implicit operator TargetCollisionCheck(TargetCollisionCheckMemento targetCollisionCheckMemento)
        {
            if (targetCollisionCheckMemento == null)
                return null;
            return targetCollisionCheckMemento.Originator;
        }
    }
}