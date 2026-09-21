using _01_Work.LCM._01.Scripts.Entities;
using Assets._01_Work.YHB.Scripts.NPCs;

namespace Assets._01_Work.YHB.Scripts.ExampleNpcs.ExampleStates
{
	// NPCState를 구현 받아서 구현하면 됨.
	// bt처럼 구현하면 됨.
	// animation change event는 _npcAnimator.ChangeAnimation(바꿀 애니메이션); 등으로 처리하면됨.
	public class ExampleNpcIdleState : NPCState
	{
		public ExampleNpcIdleState(Entity entity, int animationHash) : base(entity, animationHash)
		{
		}
	}
}
