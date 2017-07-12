using System.Collections.Generic;
using CloudCity;
using Newtonsoft.Json;
using UnityEngine;

namespace Autelia.Serialization.Mementos.Unity
{
    [JsonObject(MemberSerialization.Fields)]
    [System.Serializable]
    public sealed class IndustrialTrackerMemento : BehaviourMemento<IndustrialTracker, IndustrialTrackerMemento>
    {

        string _buildingName;
        float _buyCost;
        float _baseCost;
        string _type;
        int _capacity;
        bool _grabbableObject;
        float _longtermHappiness;
        int _cumulativeUnhappiness;
        bool _usable;
        bool _validPosition;

        // Ind vars
        float _sellPrice;

        protected override bool Serialize(IndustrialTracker originator)
        {
            base.Serialize(originator);

            _buildingName = originator.buildingName;
            _buyCost = originator.buyCost;
            _baseCost = originator.baseCost;
            _type = originator.type;
            _capacity = originator.capacity;
            _grabbableObject = originator.grabbableObject;
            _longtermHappiness = originator.longtermHappiness;
            _cumulativeUnhappiness = originator.cumulativeUnhappiness;
            _usable = originator.usable;
            _validPosition = originator.validPosition;

            _sellPrice = originator.sellPrice;

            return true;
        }

        protected override void Deserialize(ref IndustrialTracker r)
        {
            base.Deserialize(ref r);
            r.buildingName = _buildingName;
            r.buyCost = _buyCost;
            r.baseCost = _baseCost;
            r.type = _type;
            r.capacity = _capacity;
            r.users = 0;
            r.grabbableObject = _grabbableObject;
            r.longtermHappiness = _longtermHappiness;
            r.cumulativeUnhappiness = _cumulativeUnhappiness;
            r.usable = _usable;
            r.validPosition = _validPosition;

            r.checkEnable = true;
            r.sellPrice = _sellPrice;
        }

        public static implicit operator IndustrialTrackerMemento(IndustrialTracker industrialTracker)
        { return Create(industrialTracker); }

        public static implicit operator IndustrialTracker(IndustrialTrackerMemento industrialTrackerMemento)
        {
            if (industrialTrackerMemento == null)
                return null;
            return industrialTrackerMemento.Originator;
        }
    }
}