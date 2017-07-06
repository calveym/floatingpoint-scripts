using Newtonsoft.Json;
using UnityEngine;
using VRTK.GrabAttachMechanics;

namespace Autelia.Serialization.Mementos.Unity
{
    [JsonObject(MemberSerialization.Fields)]
    [System.Serializable]
    public sealed class VRTK_ChildOfControllerGrabAttachMemento : BehaviourMemento<VRTK_ChildOfControllerGrabAttach, VRTK_ChildOfControllerGrabAttachMemento>
    {
        bool _precisionGrab;

        protected override bool Serialize(VRTK_ChildOfControllerGrabAttach originator)
        {
            base.Serialize(originator);

            _precisionGrab = originator.precisionGrab;

            return true;
        }

        protected override void Deserialize(ref VRTK_ChildOfControllerGrabAttach grabAttach)
        {
            base.Deserialize(ref grabAttach);

            grabAttach.precisionGrab = _precisionGrab;
        }

        public static implicit operator VRTK_ChildOfControllerGrabAttachMemento(VRTK_ChildOfControllerGrabAttach grabAttach)
        { return Create(grabAttach); }

        public static implicit operator VRTK_ChildOfControllerGrabAttach(VRTK_ChildOfControllerGrabAttachMemento grabAttachMemento)
        {
            if (grabAttachMemento == null)
                return null;
            return grabAttachMemento.Originator;
        }
    }
}