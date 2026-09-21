using _01_Work.LCM._01.Scripts.Entities;
using UnityEngine;
using UnityEngine.AI;

namespace Assets._01_Work.YHB.Scripts.NPCs.NPCRealizations.Components
{
	// NavMovement로 NavAgent등에 접근이 가능하나
	// 그 component에서는 오직 이동과 관련된 로직만 처리하게하고(SetDestination, _navMovement.agent.autoTraverseOffMeshLink 등)
	// 이 component에서 enable등 contoll하게하여, 참조 확인을 쉽게하고 역할 구분을 명확하게 하기 위해 분리(agent참조)한다.
	public class NPCNavComponentContoller : MonoBehaviour, IEntityComponent
	{
		[field: SerializeField] public NavMeshAgent NavAgent {  get; private set; }
		[field: SerializeField] public NavMeshObstacle NavObstacle {  get; private set; }

		public void Initialize(Entity entity)
		{
			SetObstacleAndFreeze(false);
		}

		public void SetObstacleAndFreeze(bool freeze)
		{
			// 순사가 중요하여 if로 분기를 나눔.
			// nav는 비동기로 처리되기에
			if (freeze)
			{
				NavObstacle.enabled = true;

				NavAgent.isStopped = true;
				NavAgent.enabled = false;
			}
			else
			{
				NavAgent.enabled = true;
				NavAgent.isStopped = false;

				NavObstacle.enabled = false;
			}
		}
	}
}
