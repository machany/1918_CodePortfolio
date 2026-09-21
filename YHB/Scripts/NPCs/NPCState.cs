using _01_Work.KJY.Code.NpcCodes;
using _01_Work.LCM._01.Scripts.Entities;
using _01_Work.LCM._01.Scripts.FSM;
using UnityEngine;

namespace Assets._01_Work.YHB.Scripts.NPCs
{
	public class NPCState : EntityState
	{
		protected NPC _npc;
		protected NPCAnimator _npcAnimator;
		protected NavMovement _navMovement;

		public NPCState(Entity entity, int animationHash) : base(entity, animationHash)
		{
			_npc = entity as NPC;
			_npcAnimator = _npc.GetCompo<NPCAnimator>();
			_navMovement = _npc.GetCompo<NavMovement>();
			Debug.Assert(_npcAnimator != null, "animator is null");
		}

		public override void Enter()
		{
			_npcAnimator.ChangeAnimation(_animationHash);
			_isTriggerCall = false;
			_animatorTrigger.OnAnimationEndTrigger += AnimationEndTrigger;
		}

		public override void Update() { }

		public override void Exit()
		{
			_npcAnimator.ChangeAnimToBefore();
			_animatorTrigger.OnAnimationEndTrigger -= AnimationEndTrigger;
		}
	}
}
