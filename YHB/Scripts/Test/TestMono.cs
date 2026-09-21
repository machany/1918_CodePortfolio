using AgamaLibrary.Unity.DataStructures;
using AgamaLibrary.Unity.MonoBehaviourSingletons;
using Alchemy.Inspector;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets._01_Work.YHB.Scripts.Test
{
	public class TestMono : MonoBehaviour
	{
		public SerializedDictionary<int, int> s;

		[SerializeField] private bool _flag;
		[SerializeField] private int sceneNumber;

		[Button]
		private void LoadSceneAsync()
		{
			SceneLoader.Instance.LoadSceneAsync(sceneNumber, LoadSceneMode.Single, x => Debug.Log(x + "%"), () => _flag);
		}

		[Button]
		private void NextScene()
		{
			_flag = true;
		}
	}
}
