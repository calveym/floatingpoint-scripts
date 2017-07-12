using System.Collections.Generic;
using CloudCity;
using Newtonsoft.Json;
using UnityEngine;

namespace Autelia.Serialization.Mementos.Unity
{
    [JsonObject(MemberSerialization.Fields)]
    [System.Serializable]
    public sealed class SnapPointsMemento : BehaviourMemento<SnapPoints, SnapPointsMemento>
    {
        GameObjectMemento _snapCube;
        DictionaryMemento<Dictionary<string, Vector3>, string, Vector3> _originalPoints;
        BoundsMemento _bounds;

        protected override bool Serialize(SnapPoints originator)
        {
            base.Serialize(originator);

            _snapCube = originator.snapCube;
            _originalPoints = originator.originalPoints;
            _bounds = originator.bounds;

            return true;
        }

        protected override void Deserialize(ref SnapPoints r)
        {
            base.Deserialize(ref r);

            r.snapCube = _snapCube;
            r.originalPoints = _originalPoints;
            r.bounds = _bounds;
        }

        public static implicit operator SnapPointsMemento(SnapPoints snapPoints)
        { return Create(snapPoints); }

        public static implicit operator SnapPoints(SnapPointsMemento snapPointsMemento)
        {
            if (snapPointsMemento == null)
                return null;
            return snapPointsMemento.Originator;
        }
    }
}