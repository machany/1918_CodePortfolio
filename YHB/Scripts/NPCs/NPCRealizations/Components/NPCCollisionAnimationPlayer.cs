using _01_Work.KJY.Code.NpcCodes;
using _01_Work.LCM._01.Scripts.Entities;
using UnityEngine;

namespace Assets._01_Work.YHB.Scripts.NPCs.NPCRealizations.Components
{
	public class NPCCollisionAnimationPlayer : MonoBehaviour, IEntityComponent
	{
		[SerializeField] private string collisionAnimation;
		private int _collisionAnimationHash;
		private bool _playing;

		private DefNPC _defNPC;
		private NPCAnimator _npcAnimator;
		private EntityAnimatorTrigger _animationTrigger;
		private NavMovement _navMovement;

		public void Initialize(Entity entity)
		{
			_defNPC = entity as DefNPC;
			_npcAnimator = _defNPC.GetComp<NPCAnimator>();
			_animationTrigger = _defNPC.GetComp<EntityAnimatorTrigger>();
			_navMovement = _defNPC.GetComp<NavMovement>();

			_collisionAnimationHash = Animator.StringToHash(collisionAnimation);

			_playing = false;
		}

		public void Play()
		{
			if (_playing)
			{
				// play중 애니메이션이 바뀌면 진입하지 못하니 구독 해제를 안하면 안되니
				_animationTrigger.OnAnimationEndTrigger -= HandleAnimationEndTrigger;
			}

			_playing = true;
			_npcAnimator.SetParam(_collisionAnimationHash, true);

			_animationTrigger.OnAnimationEndTrigger += HandleAnimationEndTrigger;
			_navMovement.SetStop(true);
		}

		private void HandleAnimationEndTrigger()
		{
			_playing = false;
			_animationTrigger.OnAnimationEndTrigger -= HandleAnimationEndTrigger;
			_navMovement.SetStop(false);
			_npcAnimator.SetParam(_collisionAnimationHash, false);
		}

		private void OnDestroy()
		{
			_animationTrigger.OnAnimationEndTrigger -= HandleAnimationEndTrigger;
		}
	}
}
