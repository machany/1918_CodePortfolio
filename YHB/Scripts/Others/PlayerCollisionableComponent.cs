using _01_Work.LCM._01.Scripts.Players;
using Assets._01_Work.YHB.Scripts.Core;
using UnityEngine;

namespace Assets._01_Work.YHB.Scripts.Others
{
	public class PlayerCollisionableComponent : MonoBehaviour, ICollisionable
	{
		[SerializeField] private Player player;
		public bool IsRun => player.IsRunning;
	}
}
