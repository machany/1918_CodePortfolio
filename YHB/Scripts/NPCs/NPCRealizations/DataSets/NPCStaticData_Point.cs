using KHG.Utilities.RandomSystem;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets._01_Work.YHB.Scripts.NPCs.NPCRealizations.DataSets
{
	[Serializable]
	public sealed class NPCStaticData_Point : INPCDataSets
	{
		private const int CHARGE_DIRECTION_MULTIFLIER = 500;
		private const string RAND_KEY = "NPCStaticData_Point_";

		private string _randKey;
		private static int _uniqueNumber;

		[SerializeField] private Transform chargePoint;
		[SerializeField] private List<Transform> dugoutPoints;
		[SerializeField] private float _pointRandRange = 1;
		[SerializeField] private Vector3 _chargeDirection;
		public Vector3 ChargeDirection
		{
			get => _chargeDirection;
			private set => _chargeDirection = value.normalized;
		}

		private Dictionary<string, int> _offsetDict;

		// unique rand pool은 Random기반이라 비적절함.
		private List<int> _indexList;
		private int _index;

		public void Initialize(string randKey)
		{
			_randKey = $"{RAND_KEY}{_uniqueNumber++}";

			_indexList = new List<int>();
			for (int i = 0; i < dugoutPoints.Count; ++i)
				_indexList.Add(i);

			_offsetDict ??= new Dictionary<string, int>();
			_offsetDict.Clear();

			Vector3 temp = ChargeDirection;
			temp.y = 0;
			ChargeDirection = temp;

			_index = int.MaxValue;
		}

		public Vector3 GetDugoutPoints(string randKey)
		{
			Debug.Assert(dugoutPoints.Count > 0, "dugoutPoints count not be zero.");
			return dugoutPoints[GetIndex(randKey)].position;
		}

		private int GetIndex(string randKey)
		{
			if (_index >= _indexList.Count)
			{
				Shuffle(randKey);
				_index = 0;
			}

			return _indexList[_index++];
		}

		private void Shuffle(string randKey)
		{
			var random = DeterministicRandomManager.Instance.Get(randKey);

			for (int i = _indexList.Count - 1; i > 0; --i)
			{
				int select = random.Get(0, i + 1);
				(_indexList[i], _indexList[select]) = (_indexList[select], _indexList[i]);
			}
		}

		public Vector3 GetChargeDestination(Vector3 position)
		{
			Debug.Assert(chargePoint != null, "chargePoint is null");
				float y = chargePoint.position.y;

			Vector3 dir = ChargeDirection;
			Vector3 destination = (position + dir * CHARGE_DIRECTION_MULTIFLIER);

			destination.x += DeterministicRandomManager.Instance.Get(_randKey).Get(-_pointRandRange, _pointRandRange);
			destination.z += DeterministicRandomManager.Instance.Get(_randKey).Get(-_pointRandRange, _pointRandRange);
			destination.y = y;

			return destination;
		}

		public void DrawGizmos()
		{
			Gizmos.color = Color.red;
			Vector3 startPos = Vector3.up * 10;
			Vector3 endPos = ChargeDirection;
			endPos.y = 0;
			endPos = ChargeDirection.normalized * 100;
			endPos.y = 15;
			Gizmos.DrawLine(startPos, endPos);
		}
	}
}
