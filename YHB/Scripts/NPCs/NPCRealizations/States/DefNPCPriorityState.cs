using _01_Work.LCM._01.Scripts.Entities;
using Assets._01_Work.YHB.Scripts.NPCs.NPCRealizations.Components;
using Assets._01_Work.YHB.Scripts.NPCs.NPCRealizations.DataSets;

namespace Assets._01_Work.YHB.Scripts.NPCs.NPCRealizations.States
{
	public abstract class DefNPCPriorityState : DefNPCState
	{
		protected NPCStaticData_Move _moveStaticData;
		protected NPCNavMovePriorityController _priorityController;

		protected abstract NPCBehaviourEnum Behaviour { get; }

		public DefNPCPriorityState(Entity entity, int animationHash) : base(entity, animationHash)
		{
			_priorityController = _npc.GetComp<NPCNavMovePriorityController>();

			_moveStaticData = _dataComp.GetDataSet<NPCStaticData_Move>();
		}

		public override void Enter()
		{
			base.Enter();

			SetNavAgentPriority(Behaviour);
		}

		protected void SetNavAgentPriority(NPCBehaviourEnum type)
		{
			if (_moveStaticData.priorityValue.GetDict().TryGetValue(type, out BehaviourPrioritySet value))
			{
				_priorityController.SetPriority(value.priority);
				_priorityController.SetPriorityRandomRange(value.priorityRange);
			}
		}
	}
}
