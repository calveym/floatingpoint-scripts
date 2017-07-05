using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;
using CloudCity;

namespace Autelia.Serialization.Mementos.Unity
{
    [JsonObject(MemberSerialization.Fields)]
    [System.Serializable]
    public sealed class RoadGeneratorMemento : BehaviourMemento<RoadGenerator, RoadGeneratorMemento>
    {
        Dictionary<Vector3, GameObject> _roads;
        Dictionary<Vector3, string> _surroundingRoads;
        int _numRoads;
        GameObject _smallStraight;
        GameObject _smallTurn;
        GameObject _smallTJunction;
        GameObject _smallXJunction;

        protected override bool Serialize(RoadGenerator originator)
        {
            base.Serialize(originator);
            Debug.Log("Serializing road gen");
            _roads = originator.roads;
            _surroundingRoads = originator.surroundingRoads;
            _numRoads = originator.numRoads;
            _smallStraight = originator.smallRoadStraight;
            _smallTurn = originator.smallRoadTurn;
            _smallTJunction = originator.smallRoadTJunction;
            _smallXJunction = originator.smallRoadXJunction;

            return true;
        }

        protected override void Deserialize(ref RoadGenerator r)
        {
            base.Deserialize(ref r);
            r.roads = _roads;
            r.surroundingRoads = _surroundingRoads;
            r.numRoads = _numRoads;
            r.smallRoadStraight = _smallStraight;
            r.smallRoadTurn = _smallTurn;
            r.smallRoadTJunction = _smallTJunction;
            r.smallRoadXJunction = _smallXJunction;
        }

        public static implicit operator RoadGeneratorMemento(RoadGenerator roadGenerator)
        { return Create(roadGenerator); }

        public static implicit operator RoadGenerator(RoadGeneratorMemento roadGeneratorMemento)
        {
            if (roadGeneratorMemento == null)
                return null;
            return roadGeneratorMemento.Originator;
        }
    }
}