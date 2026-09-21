using _01_Work.LCM._01.Scripts.Entities;
using Assets._01_Work.YHB.Scripts.NPCs.NPCRealizations.Components;

namespace Assets._01_Work.YHB.Scripts.NPCs.NPCRealizations.States
{
	// StaticState		=>	Composite패턴의 Leaf에 가깝다.			실질적인 행동들이다. 걷기, 뛰기 등 실제 행동을 뜻한다.
	// DynamicState		=>	Composite패턴의 Composite에 가깝다.	이 State들은 StaticState를 이용, 전환한다. (BT로 Enum별 FSM과 유사하다.) 현재의 상태을 뜻한다.
	public abstract class DefNPCState : NPCState
	{
		protected DefNPC _defNPC;
		protected NPCDatasetPresetComponent _dataComp;

		protected DefNPCState(Entity entity, int animationHash) : base(entity, animationHash)
		{
			_defNPC = _npc as DefNPC;
			_dataComp = _npc.GetComp<NPCDatasetPresetComponent>();
		}

		protected string GetRandKey()
			=> $"{_defNPC.randomKey}_{_animationHash}";
	}
}
