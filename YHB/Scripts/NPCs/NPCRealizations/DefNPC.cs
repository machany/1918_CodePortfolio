using Assets._01_Work.YHB.Scripts.Situations;
using UnityEngine;
using UnityEngine.Events;

namespace Assets._01_Work.YHB.Scripts.NPCs.NPCRealizations
{
	public class DefNPC : NPC
	{
		public const string RANDOM_KEY = "D_NPC";

		protected static int _staticUniqueNumber;
		[HideInInspector] public string randomKey;

		public UnityEvent<Situation> OnSituation;
		public UnityEvent OnTalk;

		protected override void Awake()
		{
			base.Awake();

			randomKey = $"{RANDOM_KEY}{_staticUniqueNumber++}";
		}

		public override void InvokeDead()
		{
			base.InvokeDead();

			ChangeBehaviour(NPCBehaviourEnum.D_Dead);
		}

		public override void FrameUpdate(float deltaTime)
		{
			if (!IsDead)
				base.FrameUpdate(deltaTime);
		}

		protected override void InvokeSituation(Situation situation)
		{
			OnSituation?.Invoke(situation);
		}
	}
}
