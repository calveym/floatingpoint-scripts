using System.Collections.Generic;
using CloudCity;
using Newtonsoft.Json;
using UnityEngine;

namespace Autelia.Serialization.Mementos.Unity
{
    [JsonObject(MemberSerialization.Fields)]
    [System.Serializable]
    public sealed class NightTimeMemento : BehaviourMemento<NightTime, NightTimeMemento>
    {

        //MaterialMemento _mat;

        protected override bool Serialize(NightTime originator)
        {
            base.Serialize(originator);

            //_mat = originator.mat;

            return true;
        }

        protected override void Deserialize(ref NightTime r)
        {
            base.Deserialize(ref r);

            //r.mat = _mat;
        }

        public static implicit operator NightTimeMemento(NightTime nightTime)
        { return Create(nightTime); }

        public static implicit operator NightTime(NightTimeMemento nightTimeMemento)
        {
            if (nightTimeMemento == null)
                return null;
            return nightTimeMemento.Originator;
        }
    }
}