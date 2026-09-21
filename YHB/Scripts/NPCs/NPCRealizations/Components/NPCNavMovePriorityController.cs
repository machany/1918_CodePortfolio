using _01_Work.KJY.Code.NpcCodes;
using _01_Work.LCM._01.Scripts.Entities;
using KHG.Utilities.RandomSystem;
using UnityEngine;

namespace Assets._01_Work.YHB.Scripts.NPCs.NPCRealizations.Components
{
	public class NPCNavMovePriorityController : MonoBehaviour, IEntityComponent
	{
		private const string RAND_KEY = "NPC_NavMove_Priority_Controller_";

		[SerializeField] private float resetPriorityTime = 5f;

		private static int _uniqueRandomNumber;
		private string _randKey;

		private NPC _npc;
		private NPCNavComponentContoller _navCompContoller;

		private float _lastTime;

		[SerializeField] private int _originPriority = 50;
		[SerializeField] private int _priorityRandomRange = 10;

		public void Initialize(Entity entity)
		{
			_npc = entity as NPC;

			_navCompContoller = _npc.GetComp<NPCNavComponentContoller>();

			_npc.OnUpdate += UpdateCore;
			_lastTime = 0;

			_randKey = $"{RAND_KEY}{_uniqueRandomNumber++}";
		}

		private void OnDestroy()
		{
			_npc.OnUpdate -= UpdateCore;
		}

		private void UpdateCore()
		{
			if ((_lastTime += _npc.DeltaTime) >= resetPriorityTime)
			{
				_lastTime = 0;
				_navCompContoller.NavAgent.avoidancePriority = _originPriority + DeterministicRandomManager.Instance.Get(_randKey).Get(-_priorityRandomRange, _priorityRandomRange);
			}
		}

		public void SetPriorityRandomRange(int priorityRandomRange)
			=> _priorityRandomRange = priorityRandomRange;
		public void SetPriority(int originPriority)
			=> _originPriority = originPriority;
	}
}
