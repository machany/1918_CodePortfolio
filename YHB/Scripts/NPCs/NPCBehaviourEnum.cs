using System;

namespace Assets._01_Work.YHB.Scripts.NPCs
{
	// npc 행동
	[Serializable]
	public enum NPCBehaviourEnum
	{
		// Static	=> **state**내에서	전환을		이루는 행동 단위, 즉, 일반적인 FSM의 state
		// Dynamic	=> **situation으로**	전환되는		상황에 따라서 전환되는 상태 보통 Static state로 전환 전에 필요한 정보를 설정하는 역할

		// 높은 직관성을 위한 간단한 요약 및 정의 변경
		// Static	=>	StartBehaviour로 설정시 치명적 버그 발생
		// Static	=>	StartBehaviour로 설정시 치명적 버그가 발생하지 않음

		None = 0,
		S_Idle,					// Static
		S_Walk,					// Static
		S_Run,					// Static
		Aiming,
		Attack,
		D_Patrol,				// Dynamic		// idel, walk, run의 조합
		D_ChargeReady,          // Dynamic		// Charge직전
		D_Charge,				// Dynamic		// 아주 먼 곳으로 run의 조합
		D_Panic,                // Dynamic		// 정지 및 애니메이션 실행
		D_Rest,                 // Dynamic
		D_Howl,					// Dynamic
		S_Sprint,				// Static
		S_Howling,				// Static
		S_Collision,            // Static
		D_Dead,					// Static
		D_Stand,                // Dynamic
	}
}
