using _01_Work.LCM._01.Scripts.Entities;
using System;
using UnityEngine;

namespace Assets._01_Work.YHB.Scripts.NPCs.NPCRealizations.States.Statics
{
	public class DefNPCCollisionState : DefNPCState
	{
		public DefNPCCollisionState(Entity entity, int animationHash) : base(entity, animationHash)
		{
		}

		public override void Enter()
		{
			base.Enter();

			_navMovement.SetStop(true);
			_navMovement.SetLockRotation(true);
			_npcAnimator.OnAnimatorMoveEvent.AddListener(HandleAnimationMove);
		}

		public override void Update()
		{
			base.Update();

			if (_isTriggerCall)
			{
				_npc.ChangeBehaviour(_npc.GetCurrentBehaviour(), false);
				return;
			}
		}

		public override void Exit()
		{
			_navMovement.SetStop(false);
			_navMovement.SetLockRotation(false);

			_npcAnimator.OnAnimatorMoveEvent.RemoveListener(HandleAnimationMove);
			_navMovement.WarpToPosition(_npc.transform.position);

			base.Exit();
		}

		private void HandleAnimationMove(Vector3 deltaPos, Quaternion deltaRotation)
		{
			_npc.transform.position += deltaPos;
			_npc.transform.rotation *= deltaRotation;
		}
	}
}
