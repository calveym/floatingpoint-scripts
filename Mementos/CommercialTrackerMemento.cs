using System.Collections.Generic;
using CloudCity;
using Newtonsoft.Json;
using UnityEngine;

namespace Autelia.Serialization.Mementos.Unity
{
    [JsonObject(MemberSerialization.Fields)]
    [System.Serializable]
    public sealed class CommercialTrackerMemento : BehaviourMemento<CommercialTracker, CommercialTrackerMemento>
    {

        string _buildingName;
        float _buyCost;
        float _baseCost;
        string _type;
        int _capacity;
        int _users;
        bool _grabbableObject;
        float _longtermHappiness;
        int _cumulativeUnhappiness;
        bool _usable;
        bool _validPosition;
        int _range;

        protected override bool Serialize(CommercialTracker originator)
        {
            base.Serialize(originator);

            _buildingName = originator.buildingName;
            _buyCost = originator.buyCost;
            _baseCost = originator.baseCost;
            _type = originator.type;
            _capacity = originator.capacity;
            _users = originator.users;
            _grabbableObject = originator.grabbableObject;
            _longtermHappiness = originator.longtermHappiness;
            _cumulativeUnhappiness = originator.cumulativeUnhappiness;
            _usable = originator.usable;
            _validPosition = originator.validPosition;
            _range = originator.range;

            return true;
        }

        protected override void Deserialize(ref CommercialTracker r)
        {
            base.Deserialize(ref r);
            r.buildingName = _buildingName;
            r.buyCost = _buyCost;
            r.baseCost = _baseCost;
            r.type = _type;
            r.capacity = _capacity;
            r.users = _users;
            r.grabbableObject = _grabbableObject;
            r.longtermHappiness = _longtermHappiness;
            r.cumulativeUnhappiness = _cumulativeUnhappiness;
            r.usable = _usable;
            r.validPosition = _validPosition;
        }

        public static implicit operator CommercialTrackerMemento(CommercialTracker commercialTracker)
        { return Create(commercialTracker); }

        public static implicit operator CommercialTracker(CommercialTrackerMemento commercialTrackerMemento)
        {
            if (commercialTrackerMemento == null)
                return null;
            return commercialTrackerMemento.Originator;
        }
    }
}