using _01_Work.LCM._01.Scripts.Entities;
using Assets._01_Work.YHB.Scripts.NPCs.NPCRealizations.DataSets;
using KHG.Utilities.RandomSystem;
using System;
using UnityEngine;

namespace Assets._01_Work.YHB.Scripts.NPCs.NPCRealizations.States.Dynamics
{
	public class DefNPCStandState : DefNPCPriorityState
	{
		protected override NPCBehaviourEnum Behaviour => NPCBehaviourEnum.D_Stand;

		private NPCStaticData_AnimationInfo _animationStaticData;

		private bool _canChangeTalkAnim;
		private float _nextTalkAnimationTime;

		public DefNPCStandState(Entity entity, int animationHash) : base(entity, animationHash)
		{
			_animationStaticData = _dataComp.GetDataSet<NPCStaticData_AnimationInfo>();
		}

		public override void Enter()
		{
			base.Enter();

			_canChangeTalkAnim = _animationStaticData.canChangeTalkAnim;
			SetNextTalkAnimTime();
			_animationStaticData.lookOtherObject.SetActive(true);
		}

		private void SetNextTalkAnimTime()
		{
			_nextTalkAnimationTime = Time.time + 
				_animationStaticData.talkAnimDuration +
				DeterministicRandomManager.Instance.Get(_defNPC.randomKey)
				.Get(-_animationStaticData.talkAnimDurationRange, _animationStaticData.talkAnimDurationRange);
		}

		public override void Update()
		{
			base.Update();

			if (_canChangeTalkAnim
				&& _animationStaticData.canTalkFrame <= _npc.UpdateFrame
				&& _nextTalkAnimationTime < Time.time)
			{
				SetNextTalkAnimTime();
				_npcAnimator.SetParam(_npc.GetAnimationHash(NPCAnimationEnum.TALK));
				_defNPC.OnTalk?.Invoke();
			}
		}

		public override void Exit()
		{
			_animationStaticData.lookOtherObject.SetActive(false);

			base.Exit();
		}
	}
}
