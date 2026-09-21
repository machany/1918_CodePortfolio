using _01_Work.LCM._01.Scripts.Entities;
using _01_Work.LCM._01.Scripts.Interact;
using Assets._01_Work.YHB.Scripts.NPCs.NPCRealizations.DataSets;
using DG.Tweening;
using UnityEngine;

namespace Assets._01_Work.YHB.Scripts.NPCs.NPCRealizations.Components
{
	public class NPCInteractSubTitle : MonoBehaviour, IInteractable, IEntityComponent, IAfterInitialize
	{
		[SerializeField] private float rotateDuration = .5f;
		[SerializeField] private bool autoTalk = false;

		private DefNPC _defNPC;
		private NPCDatasetPresetComponent _dataComp;
		private NPCAnimator _npcAnimator;

		public Vector3 playerPosition { get; set; }

		public void Initialize(Entity entity)
		{
			_defNPC = entity as DefNPC;
			_dataComp = _defNPC.GetComp<NPCDatasetPresetComponent>();
			_npcAnimator = _defNPC.GetComp<NPCAnimator>();
		}

		public void AfterInitialize()
		{
			_dataComp.GetDataSet<NPCStaticData_AnimationInfo>().canChangeTalkAnim = autoTalk;
		}

		public void InteractAction()
		{
			Vector3 dir = playerPosition - _defNPC.transform.position;
			dir.y = 0;
			Vector3 rotation = Quaternion.LookRotation(dir).eulerAngles;

			_npcAnimator.SetParam(_defNPC.GetAnimationHash(NPCAnimationEnum.TALK));

			Sequence seq = DOTween.Sequence();
			seq.Append(_defNPC.transform.DORotate(rotation, rotateDuration));
			seq.AppendCallback(() => _defNPC.OnTalk?.Invoke());
		}
	}
}
