using System.Collections.Generic;
using CloudCity;
using Newtonsoft.Json;
using UnityEngine;

namespace Autelia.Serialization.Mementos.Unity
{
    [JsonObject(MemberSerialization.Fields)]
    [System.Serializable]
    public sealed class EconomyManagerMemento : BehaviourMemento<EconomyManager, EconomyManagerMemento>
    {

        float _balance;
        float _goods;

        protected override bool Serialize(EconomyManager originator)
        {
            base.Serialize(originator);

            _balance = originator.balance;
            _goods = originator.goods;
            return true;
        }

        protected override void Deserialize(ref EconomyManager r)
        {
            base.Deserialize(ref r);
            r.balance = _balance;
            r.goods = _goods;

            Debug.Log("Restoring previous balance: " + r.balance);
        }

        public static implicit operator EconomyManagerMemento(EconomyManager economyManager)
        { return Create(economyManager); }

        public static implicit operator EconomyManager(EconomyManagerMemento economyManagerMemento)
        {
            if (economyManagerMemento == null)
                return null;
            return economyManagerMemento.Originator;
        }
    }
}