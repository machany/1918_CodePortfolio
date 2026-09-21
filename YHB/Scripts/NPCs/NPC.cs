using _01_Work.LCM._01.Scripts.Entities;
using _01_Work.LCM._01.Scripts.FSM;
using AgamaLibrary.Unity.DataStructures;
using Alchemy.Inspector;
using Assets._01_Work.YHB.Scripts.Core.LODUpdate;
using Assets._01_Work.YHB.Scripts.Situations;
using KHG.Utilities.RandomSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets._01_Work.YHB.Scripts.NPCs
{
	public abstract class NPC : Entity, ISituationable, ILODUpdateable
	{
		private const string RAND_KEY = "NPC_";
		private static int _uniqueRandNumber;
		private string _randKey;

		[FoldoutGroup("situation and behaviour Setting")]
		[SerializeField] protected SerializedDictionary<NPCBehaviourEnum, SituationToBehaviourSO> behaviourBySituation;

		[FoldoutGroup("animation")]
		[SerializeField] private SerializedDictionary<NPCAnimationEnum, string> animationHashName;
		protected Dictionary<NPCAnimationEnum, int> _animationHash;

		[FoldoutGroup("fsm")]
		[SerializeField] private List<NPCStateDataSO> stateDataList;
		[FoldoutGroup("fsm")]
		[SerializeField] private NPCBehaviourEnum startBehaviour;

		private EntityStateMachine<NPCBehaviourEnum> _stateMachine;
		protected NPCBehaviourEnum _currentBehaviour;
		public NPCBehaviourEnum CurrentState { get; protected set; }

		public Vector3 InvokedSituationPosition { get; private set; }

		[FoldoutGroup("frame update")]
		[SerializeField][Range(1, 255)] private byte _updateFrame = 60;
		public byte UpdateFrame
		{
			get => _updateFrame;
			set
			{
				byte v = value;
				
				if (v < 1)
					v = 1;

				if (v == _updateFrame)
					return;

				_updateFrame = v;
				OnUpdateFrameChanged?.Invoke(_updateFrame);
			}
		}
		public Action<int> OnUpdateFrameChanged;
		public bool Enabled => gameObject.activeSelf;

		// NPC에 종속된 component들이 이걸 구독하여 사용
		public Action OnUpdate;
		public Action OnDestroying;

		// 변수명이 너무 긴데
		// 줄일 수 없나
		// 시점 (last) 의미 (change behaviour) 값 (time) 특수 조건 (by situation)
		// 에반데
		//[SerializeField] private float _behaviorChangeTime_BySituation;
		//[SerializeField] private float _behaviorChangeTime_BySituation_RandomRange;
		//private float _lastBehaviorChangeTime_BySituation;
		//private float _nextWaitTime_ChangeBehaviour_BySituation;

		// 변수는 값 보관소.
		// 따라서 어떤 시점에 저정되거나 어느 조건을 가질 필요가 없을 듯.
		[SerializeField] private float _behaviorDuration;
		[SerializeField] private float _behaviorDuration_Range;
		private float _behaviorDurationTimer;
		private float _nextBehaviorDuration;

		private Queue<NPCBehaviourEnum> _behaviourToChangeist;
		private bool _lockChangeBehaviour;

		public GameObject GameObject => gameObject;

		public float DeltaTime { get; protected set; }

		protected override void Awake()
		{
			_lockChangeBehaviour = false;

			_randKey = $"{RAND_KEY}{_uniqueRandNumber++}";

			_behaviourToChangeist = new Queue<NPCBehaviourEnum>();
			_behaviorDurationTimer = .0f;

			_animationHash = new Dictionary<NPCAnimationEnum, int>();
			foreach (var item in animationHashName.GetDict())
				_animationHash.Add(item.Key, Animator.StringToHash(item.Value));

			base.Awake();

			// Awake에서 초기화를 처리 한 뒤 state 생성.
			_stateMachine = new EntityStateMachine<NPCBehaviourEnum>(this, stateDataList.ToArray());
		}

		protected virtual void Start()
		{
			SetNextBheaviourDuration();
			LODUpdateManager.Instance.Register(this, true);

			ChangeBehaviour(startBehaviour);
		}

		private void SetNextBheaviourDuration()
		{
			_nextBehaviorDuration = _behaviorDuration
				+ DeterministicRandomManager.Instance.Get(_randKey)
				.Get(-_behaviorDuration_Range, _behaviorDuration_Range);
		}

		public void SetEnable(bool value)
		{
			gameObject.SetActive(value);
		}

		protected virtual void OnDestroy()
		{
			OnDestroying?.Invoke();
		}

		public virtual void FrameUpdate(float deltaTime)
		{
			// dt저장
			DeltaTime = deltaTime;

			// _behaviourToChangeist이 empty가 아닐 때 _behaviorDurationTimer 증가
			if (_behaviourToChangeist.Count > 0)
			{
				if ((_behaviorDurationTimer += DeltaTime) >= _nextBehaviorDuration)
				{
					_behaviorDurationTimer = .0f;
					SetNextBheaviourDuration();

					NPCBehaviourEnum behaviour = _behaviourToChangeist.Dequeue();
					ChangeBehaviour(behaviour);
				}
			}

			// 자신을 먼저 update
			UpdateCore();
			// 구독한 컴포넌트를 업데이트 시키고
			OnUpdate?.Invoke();

			// state를 업데이트시킴
			_stateMachine.UpdateStateMachine();
		}

		

		protected virtual void UpdateCore() { }

		public void InvokeSituation(Situation situation, Vector3 invokedPosition)
		{
			Debug.Log($"Invoked situation : {situation}");

			InvokedSituationPosition = invokedPosition;

			if (behaviourBySituation.GetDict().TryGetValue(_currentBehaviour, out SituationToBehaviourSO sTB)) // 현재 행동에서 상황에 따른 행동을 가져오고
			{
				if (sTB.situationToBehaviour.GetDict().TryGetValue(situation, out NPCBehaviourEnum behaviour)) // 상황에 따른 행동에 상황에 대입되는 행동을 가져와봄.
					_behaviourToChangeist.Enqueue(behaviour);
				else
					Debug.Log($"{gameObject.name} is can't change {situation} to behaviour");
			}
			else
				Debug.Log($"{gameObject.name} haven't stbSO of {_currentBehaviour}");

			InvokeSituation(situation);
		}

		protected virtual void InvokeSituation(Situation situation) { }

		public NPCBehaviourEnum GetCurrentBehaviour()
			=> _currentBehaviour;

		// 기본적으로 ChangeBehaviourCore 구현 그 외의 특수한 경우라면 override
		public virtual void ChangeBehaviour(NPCBehaviourEnum newBehaviour, bool saveNewBehaviour = true)
		{
			if (_lockChangeBehaviour)
			{
				Debug.Log("<color=red>Lock change behaviour</color>");
				return;
			}

			CurrentState = newBehaviour;

			if (saveNewBehaviour)
				_currentBehaviour = newBehaviour;
			_stateMachine.ChangeState(newBehaviour);
		}

		public int GetAnimationHash(NPCAnimationEnum animationEnum)
		{
			return _animationHash[animationEnum];
		}

		public void SetLockChangeBehaviour(bool lockValue)
			=> _lockChangeBehaviour = lockValue;

#if UNITY_EDITOR
		[FoldoutGroup("situation and behaviour Setting")]
		[Button]
		private void Behaviour_By_Situation_Distinct()
		{
			behaviourBySituation.Distinct();
		}

		[FoldoutGroup("animation")]
		[Button]
		private void Animation_Hash_Name_Distinct()
		{
			animationHashName.Distinct();
		}

		[FoldoutGroup("fsm")]
		[Button]
		private void State_Data_Distinct()
		{
			stateDataList = stateDataList.Distinct(NPCStateDataSO.EqualityNPCStateDataComparer.Default).ToList();
		}

		[FoldoutGroup("fsm")]
		[Button]
		private void Change_Behaviour(NPCBehaviourEnum behaviour, bool saveBehaviour)
		{
			ChangeBehaviour(behaviour, saveBehaviour);
		}
#endif
	}
}
