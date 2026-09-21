using _01_Work.KJY.Code.NpcCodes;
using _01_Work.LCM._01.Scripts.Entities;
using Assets._01_Work.YHB.Scripts.Core;
using Assets._01_Work.YHB.Scripts.NPCs.NPCRealizations.DataSets;
using DG.Tweening;
using UnityEngine;

namespace Assets._01_Work.YHB.Scripts.NPCs.NPCRealizations.Components
{
	public class NPCCollisionAnimationController : MonoBehaviour, IEntityComponent, IAfterInitialize
	{
		[SerializeField] private NPCCollisionAnimationPlayer collisionAnimationPlayer;
		[SerializeField] private NPCBehaviourSetSO collisionBehaviourSetSO;

		[SerializeField] private bool collisionWithRotate;
		[SerializeField] private float rotateDuration = .5f;

		[SerializeField] private float maxCollisionSpeed;

		[SerializeField] private string collisionLeftParam;
		private int _collisionLeftParamHash;
		[SerializeField] private string collisionStrongParam;
		private int _collisionStrongParamHash;

		private NPC _npc;
		private EntityAnimator _npcAnimator;
		private NPCDatasetPresetComponent _dataComp;
		private NPCDynamicData_Move _dynamicData;

		public void Initialize(Entity entity)
		{
			_npc = entity as NPC;

			_collisionStrongParamHash = Animator.StringToHash(collisionStrongParam);
			_collisionLeftParamHash = Animator.StringToHash(collisionLeftParam);

			_npcAnimator = _npc.GetComp<NPCAnimator>();
			_dataComp = _npc.GetComp<NPCDatasetPresetComponent>();
		}

		public void AfterInitialize()
		{
			_dynamicData = _dataComp.GetDataSet<NPCDynamicData_Move>();
		}

		//private void OnTriggerEnter(Collider collider)
		//{
		//	Debug.Log("skdh");
		//	CheckAndChangeCollision(collider.transform);
		//}

		private void OnCollisionEnter(Collision collision)
		{
			CheckAndChangeCollision(collision.transform);
		}

		private void CheckAndChangeCollision(Transform trm)
		{
			if (!collisionBehaviourSetSO.Contains(_npc.CurrentState))
				return;

			if (trm.TryGetComponent<ICollisionable>(out var collisionable))
			{
				bool isRun = collisionable.IsRun || _dynamicData.isRun;
				_npcAnimator.SetParam(_collisionStrongParamHash, isRun);

				// 좌우 판별
				Vector3 targetPos = trm.position;
				Vector3 myPos = _npc.transform.position;
				Vector3 v = targetPos - myPos;

				// f v
				float dotCrossValue = Vector3.Dot(Vector3.Cross(_npc.transform.forward, v), Vector3.down);
				// 대상이 npc기준 좌측에 있어도 false가 되기에 판별식의 부등호 방향을 바꿈.
				// 취소
				bool isLeft = dotCrossValue > 0;

				// 앞뒤 판별
				float dotValue = Vector3.Dot(_npc.transform.forward, v);
				bool isFront = dotValue > 0;

				isLeft = isFront ? isLeft : !isLeft;

				if (collisionWithRotate)
				{
					Vector3 dir = trm.position - _npc.transform.position;
					dir.y = 0;
					Vector3 rotation = Quaternion.LookRotation(dir).eulerAngles;

					_npc.transform.DORotate(rotation, rotateDuration);
				}

				// 앞뒤 충돌에 따라 좌우 변경
				_npcAnimator.SetParam(_collisionLeftParamHash, isLeft);

				collisionAnimationPlayer.Play();
			}
		}
	}
}
