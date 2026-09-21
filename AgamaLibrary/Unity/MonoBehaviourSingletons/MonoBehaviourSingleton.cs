using System;
using UnityEngine;

namespace AgamaLibrary.Unity.MonoBehaviourSingletons
{
	// 설마 실수로 singleton을 제거하겠어..?
	// 제거하면 의도적으로 필요해서 제거한거겠지
	// =============================================주의 사항
	// 다른 곳에서 OnDestroy에서 접근하면 게임 종료시 nullrefer발생 가능. 따라서 OnDestroy에서 접근시 null체크 필요함.
	// runtime중 Destroy하는 이상한 행동은 하지 말 것.
	[DefaultExecutionOrder(int.MinValue)]
	public abstract class MonoBehaviourSingleton<T> : MonoBehaviour, IMonoBehaviourSingleton where T : MonoBehaviourSingleton<T>
	{
		/// <summary>
		/// You must perform a null check when accessing it in OnDestroy.
		/// </summary>
		/// <remarks>
		/// 만약 OnDestroy 등에서 접근 시 반드시 null체크를 해야합니다.
		/// </remarks>
		public static T Instance { get; private set; } = null;

		public Action OnDestroying;

		/// <summary>
		/// If this option is true, Destroyed when the scene is loaded.
		/// </summary>
		/// <remarks>
		/// 이 옵션이 참이라면, 씬이 로드될 때 파괴됩니다.
		/// </remarks>
		protected abstract bool IsDestroyOnLoad { get; }

		private bool _isInitialized = false;

		// MonoBehaviourSingletonBuilder에서 실행함.
		internal static void Internally_InitializeMonoBehaviourSingleton()
		{
			var founds = UnityEngine.Object.FindObjectsByType<T>(
				FindObjectsInactive.Include,
				FindObjectsSortMode.None
			);

			if (founds.Length > 0)
			{
				Debug.Log($"[{typeof(T).Name}] find object");

				Instance = founds[0];
				if (founds.Length > 1)
					for (int i = 1; i < founds.Length; i++)
						Destroy(founds[i]);
			}
			else
			{
				Debug.Log($"[{typeof(T).Name}] create object");

				GameObject gameObject = new GameObject($"Singleton_{typeof(T).Name}");
				T comp = gameObject.AddComponent<T>();
				Instance = comp;
			}

			if (!Instance._isInitialized)
			{
				if (!Instance.IsDestroyOnLoad)
					DontDestroyOnLoad(Instance.gameObject);

				Instance.Internally_Initialize();
				Instance._isInitialized = true;
			}

			SceneLoader.Instance.Internally_AddMonoBehaviourSingleton(Instance);
		}

		internal void Internally_InitializeMonoBehaviourSingleton_FromInstance()
		{
			Internally_InitializeMonoBehaviourSingleton();
		}

		internal virtual void Internally_LoadSceneInitialize() { }

		/// <summary>
		/// 초기화를 진행합니다.
		/// Awake 이후에 실행되며 생성자 수준의 초기화가 필요할 때 사용합니다.
		/// </summary>
		protected virtual void Internally_Initialize() { }

		protected virtual void OnDestroy()
		{
			if (Instance == this)
			{
				OnDestroying?.Invoke();

				SceneLoader.Instance?.Internally_RemoveMonoBehaviourSingleton(Instance);
				Instance = null;
				Debug.Log($"[{typeof(T).Name}] destroyed object");
			}
		}
	}
}
