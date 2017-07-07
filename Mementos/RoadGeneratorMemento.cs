using System.Collections.Generic;
using CloudCity;
using Newtonsoft.Json;
using UnityEngine;

namespace Autelia.Serialization.Mementos.Unity
{
	[JsonObject(MemberSerialization.Fields)]
	[System.Serializable]
	public sealed class RoadGeneratorMemento : BehaviourMemento<RoadGenerator, RoadGeneratorMemento>
	{
		private DictionaryMemento<Dictionary<Vector3, GameObject>, Vector3, GameObject> _roads;
		private DictionaryMemento<Dictionary<Vector3, string>, Vector3, string> _surroundingRoads;
		private int _numRoads;
		private GameObjectMemento _smallStraight;
		private GameObjectMemento _smallTurn;
		private GameObjectMemento _smallTJunction;
		private GameObjectMemento _smallXJunction;

		protected override bool Serialize(RoadGenerator originator)
		{
			base.Serialize(originator);
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