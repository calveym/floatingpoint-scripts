using System.Collections.Generic;
using CloudCity;
using Newtonsoft.Json;
using UnityEngine;

namespace Autelia.Serialization.Mementos.Unity
{
    [JsonObject(MemberSerialization.Fields)]
    [System.Serializable]
    public sealed class ResidentialTrackerMemento : BehaviourMemento<ResidentialTracker, ResidentialTrackerMemento>
    {

        bool firstPass = true;

        string _buildingName;
        float _buyCost;
        float _baseCost;
        string _type;
        int _capacity;
        int _users;
        int _unemployed;
        bool _grabbableObject;
        float _longtermHappiness;
        int _cumulativeUnhappiness;
        bool _usable;
        bool _validPosition;

        protected override bool Serialize(ResidentialTracker originator)
        {
            base.Serialize(originator);

            _buildingName = originator.buildingName;
            _buyCost = originator.buyCost;
            _baseCost = originator.baseCost;
            _type = originator.type;
            _capacity = originator.capacity;
            _users = originator.users;
            _unemployed = originator.unemployedPopulation;
            _grabbableObject = originator.grabbableObject;
            _longtermHappiness = originator.longtermHappiness;
            _cumulativeUnhappiness = originator.cumulativeUnhappiness;
            _usable = originator.usable;
            _validPosition = originator.validPosition;

            return true;
        }

        protected override void Deserialize(ref ResidentialTracker r)
        {
            base.Deserialize(ref r);
            r.buildingName = _buildingName;
            r.buyCost = _buyCost;
            r.baseCost = _baseCost;
            r.type = _type;
            r.capacity = _capacity;
            r.users = _users;
            r.unemployedPopulation = _unemployed;
            r.grabbableObject = _grabbableObject;
            r.longtermHappiness = _longtermHappiness;
            r.cumulativeUnhappiness = _cumulativeUnhappiness;
            r.usable = _usable;
            r.validPosition = _validPosition;
            Debug.Log("Deserialized residential instance ID: " + r.GetInstanceID());
            Debug.Log("Deserialized residential parent: " + r.gameObject.transform.parent);
        }

        public static implicit operator ResidentialTrackerMemento(ResidentialTracker residentialTracker)
        { return Create(residentialTracker); }

        public static implicit operator ResidentialTracker(ResidentialTrackerMemento residentialTrackerMemento)
        {
            if (residentialTrackerMemento == null)
                return null;
            return residentialTrackerMemento.Originator;
        }
    }
}