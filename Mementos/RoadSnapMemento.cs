using System.Collections.Generic;
using CloudCity;
using Newtonsoft.Json;
using UnityEngine;

namespace Autelia.Serialization.Mementos.Unity
{
    [JsonObject(MemberSerialization.Fields)]
    [System.Serializable]
    public sealed class RoadSnapMemento : BehaviourMemento<RoadSnap, RoadSnapMemento>
    {

        MaterialMemento _blockedMaterial;
        GameObject _snapCube;
        bool _manualUse, _manualSnap;
        GameObject _targetBoxPrefab;

        protected override bool Serialize(RoadSnap originator)
        {
            base.Serialize(originator);

            _blockedMaterial = originator.blockedMaterial;
            _snapCube = originator.snapCube;
            _manualSnap = originator.manualSnap;
            _manualUse = originator.manualUse;
            _targetBoxPrefab = originator.targetBoxPrefab;

            return true;
        }

        protected override void Deserialize(ref RoadSnap r)
        {
            base.Deserialize(ref r);

            r.blockedMaterial = _blockedMaterial;
            r.snapCube = _snapCube;
            r.manualUse = _manualUse;
            r.manualSnap = _manualSnap;
            r.targetBoxPrefab = _targetBoxPrefab;
        }

        public static implicit operator RoadSnapMemento(RoadSnap roadSnap)
        { return Create(roadSnap); }

        public static implicit operator RoadSnap(RoadSnapMemento roadSnapMemento)
        {
            if (roadSnapMemento == null)
                return null;
            return roadSnapMemento.Originator;
        }
    }
}