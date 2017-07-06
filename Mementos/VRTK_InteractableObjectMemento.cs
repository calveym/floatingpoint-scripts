using Newtonsoft.Json;
using UnityEngine;
using VRTK;

namespace Autelia.Serialization.Mementos.Unity
{
    [JsonObject(MemberSerialization.Fields)]
    [System.Serializable]
    public sealed class VRTK_InteractableObjectMemento : BehaviourMemento<VRTK_InteractableObject, VRTK_InteractableObjectMemento>
    {

        bool _isGrabbable,
            _isUsable,
            _holdButtonToGrab,
            _holdButtonToUse;

        protected override bool Serialize(VRTK_InteractableObject originator)
        {
            base.Serialize(originator);

            _isGrabbable = originator.isGrabbable;
            _isUsable = originator.isUsable;
            _holdButtonToGrab = originator.holdButtonToGrab;
            _holdButtonToUse = originator.holdButtonToUse;

            return true;
        }

        protected override void Deserialize(ref VRTK_InteractableObject interactableObject)
        {
            base.Deserialize(ref interactableObject);

            interactableObject.isGrabbable = _isGrabbable;
            interactableObject.isUsable = _isUsable;
            interactableObject.holdButtonToGrab = _holdButtonToGrab;
            interactableObject.holdButtonToUse = _holdButtonToUse;
        }

        public static implicit operator VRTK_InteractableObjectMemento(VRTK_InteractableObject interactableObject)
        { return Create(interactableObject); }

        public static implicit operator VRTK_InteractableObject(VRTK_InteractableObjectMemento interactableObjectMemento)
        {
            if (interactableObjectMemento == null)
                return null;
            return interactableObjectMemento.Originator;
        }
    }
}