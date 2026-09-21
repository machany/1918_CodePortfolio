using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace AgamaLibrary.Unity.MonoBehaviourSingletons
{
	// MonoBehaviourSingletonBuilder와 상호 참조
	public class SceneLoader : MonoBehaviourSingleton<SceneLoader>
	{
		protected override bool IsDestroyOnLoad => false;
		private HashSet<IMonoBehaviourSingleton> _singletonSet = new HashSet<IMonoBehaviourSingleton>();

		protected override void Internally_Initialize()
		{
			SceneManager.sceneLoaded += HandleSceneLoaded;
		}

		protected override void OnDestroy()
		{
			SceneManager.sceneLoaded -= HandleSceneLoaded;

			base.OnDestroy();
		}

		private void HandleSceneLoaded(Scene arg0, LoadSceneMode arg1)
		{
			MonoBehaviourSingletonBuilder.Internally_InvokeSingletonMethods();

			var newList = RemoveDestroyObject(_singletonSet);

			_singletonSet.Clear();
			foreach (var item in newList)
				_singletonSet.Add(item);

			Internally_InvokeInitialize_OnLoadScene();
		}

		internal override void Internally_LoadSceneInitialize()
		{
			StopAllCoroutines();
		}

		public void LoadSceneAsync(string sceneName, LoadSceneMode sceneMode = LoadSceneMode.Single, Action<float> loadProgressPercent = null, Func<bool> sceneAllowCondition = null)
		{
			StartCoroutine(LoadSceneAsync(SceneManager.LoadSceneAsync(sceneName, sceneMode), loadProgressPercent, sceneAllowCondition));
		}

		public void LoadSceneAsync(int sceneNumber, LoadSceneMode sceneMode = LoadSceneMode.Single, Action<float> loadProgressPercent = null, Func<bool> sceneAllowCondition = null)
		{
			StartCoroutine(LoadSceneAsync(SceneManager.LoadSceneAsync(sceneNumber, sceneMode), loadProgressPercent, sceneAllowCondition));
		}

		private IEnumerator LoadSceneAsync(AsyncOperation asyncLoad, Action<float> loadProgressPercent = null, Func<bool> sceneAllowCondition = null)
		{
			if (sceneAllowCondition == null)
				sceneAllowCondition = () => true;

			// 씬 활성화 잠금
			asyncLoad.allowSceneActivation = false;

			while (asyncLoad.progress < 0.9f)
			{
				// progress는 0.9가 최대며, 나머지 0.1는 실제 씬을 활성화하는 과정이다.
				float progressValue = asyncLoad.progress / 0.9f;
				loadProgressPercent?.Invoke(progressValue);

				yield return null;
			}

			while (!sceneAllowCondition.Invoke())
				yield return null;

			// 씬 활성화
			asyncLoad.allowSceneActivation = true;

			//while (!asyncLoad.isDone)
			//	yield return null;
		}

		internal void Internally_AddMonoBehaviourSingleton(IMonoBehaviourSingleton singleton)
		{
#pragma warning disable CS0252 // 의도하지 않은 참조 비교가 있을 수 있습니다. 왼쪽을 캐스팅해야 합니다.
			if (singleton == null
				|| singleton == this)
				return;
#pragma warning restore CS0252 // 의도하지 않은 참조 비교가 있을 수 있습니다. 왼쪽을 캐스팅해야 합니다.

			_singletonSet.Add(singleton);
		}

		internal void Internally_AddMonoBehaviourSingleton(IEnumerable<IMonoBehaviourSingleton> singletons)
		{
			foreach (var singleton in singletons)
				Internally_AddMonoBehaviourSingleton(singleton);
		}

		internal void Internally_RemoveMonoBehaviourSingleton(IMonoBehaviourSingleton singleton)
		{
			_singletonSet.Remove(singleton);
		}

		internal void Internally_InvokeInitialize_OnLoadScene()
		{
			Debug.Log($"[MBSP] Initialize on load scene singleton : {_singletonSet.Count}");
			foreach (var singleton in _singletonSet)
				singleton.Internally_LoadSceneInitialize();
		}

		private IEnumerable<IMonoBehaviourSingleton> RemoveDestroyObject(IEnumerable<IMonoBehaviourSingleton> singletons)
		{
			List<IMonoBehaviourSingleton> queue = new List<IMonoBehaviourSingleton>();

			foreach (var singleton in singletons)
				if (singleton != null)
					queue.Add(singleton);

			return queue;
		}
	}
}
