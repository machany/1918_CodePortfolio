namespace AgamaLibrary.Unity.MonoBehaviourSingletons
{
	public interface IMonoBehaviourSingleton
	{
		// helper
		internal virtual void Internally_InitializeMonoBehaviourSingleton_FromInstance() { }
		/// <summary>
		/// awake이전 호출
		/// </summary>
		internal virtual void Internally_LoadSceneInitialize() { }
	}
}
