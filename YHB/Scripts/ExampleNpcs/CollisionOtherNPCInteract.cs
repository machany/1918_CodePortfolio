using Assets._01_Work.YHB.Scripts.NPCs;
using System;
using _01_Work.LCM._01.Scripts.Entities;
using UnityEngine;

namespace Assets._01_Work.YHB.Scripts.ExampleNpcs
{
	public class CollisionOtherNPCInteract : MonoBehaviour
	{
		[SerializeField] private NPC npc;
		[SerializeField] private NPCAnimator animator;
		[SerializeField] private EntityAnimatorTrigger trigger;

		[VisibleEnum(typeof(NPCAnimationEnum))]
		public void ChangeAnimation(int animationEnum)
		{
			animator.ChangeAnimation(npc.GetAnimationHash((NPCAnimationEnum)animationEnum));
			trigger.OnAnimationEndTrigger += HandleAnimationEndTrigger;
		}

		private void HandleAnimationEndTrigger()
		{
			animator.ChangeAnimToBefore();
			trigger.OnAnimationEndTrigger -= HandleAnimationEndTrigger;
		}
	}
}
