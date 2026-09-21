using System;

namespace Assets._01_Work.YHB.Scripts.Situations
{
	// 일어난 상황
	[Serializable]
	public enum Situation
	{
		Default = 0,
		ArtilleryNearby,		// 주변에서 포격 발생
		ArtilleryDistant,		// 멀리서 포격 발생
		WoundedNearby,			// 주변에서 부상 발생
		WoundedDistant,			// 멀리서 부상 발생
		DeathNearby,			// 주변에서 사망 발생
		DeathDistanty,			// 멀리서 사망 발생
		ChargeReadyOrder,		// 돌격 대기 명령
		ChargeOrder,			// 돌격 명령
		RetreatOrder,			// 후퇴 명령
		EnemyApproaching,		// 적 접근
		Dead,					// 즉사
	}
}
