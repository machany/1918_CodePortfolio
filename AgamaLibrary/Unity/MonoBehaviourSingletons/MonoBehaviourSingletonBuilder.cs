using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace AgamaLibrary.Unity.MonoBehaviourSingletons
{
	// 언젠가 필히 커스텀 라이프사이클을 만들어야겠다.

	// SceneLoader와 상호 참조
	public sealed class MonoBehaviourSingletonBuilder
	{
		private const string INITIALIZE_METHOD_NAME = "Internally_InitializeMonoBehaviourSingleton";

		private static HashSet<MethodInfo> _initializeSingltonMethodSet;

		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
		private static void Internally_InitializeAllSingletons()
		{
			_initializeSingltonMethodSet = new HashSet<MethodInfo>();

			MonoBehaviourSingleton<SceneLoader>.Internally_InitializeMonoBehaviourSingleton();

			var assemblies = AppDomain.CurrentDomain.GetAssemblies();

			var singletonBaseTypes = assemblies
				.SelectMany(a => SafeGetTypes(a))
				.Where(t => t.IsClass && !t.IsAbstract)
				.Where(t =>
					t.BaseType != null &&
					t.BaseType.IsGenericType &&
					t.BaseType.GetGenericTypeDefinition() == typeof(MonoBehaviourSingleton<>))
				.Select(t => t.BaseType)
				.Distinct()
				.ToList();

			Debug.Log($"[MBSP] Singleton base types detected : {singletonBaseTypes.Count}");

			int success = 0;
			int fail = 0;

			foreach (var baseType in singletonBaseTypes)
			{
				try
				{
					var initMethod = baseType.GetMethod(
						INITIALIZE_METHOD_NAME,
						BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy
					);

					if (initMethod == null)
					{
						fail++;
						Debug.LogError($"[MBSP] Static method {INITIALIZE_METHOD_NAME} not found: {baseType.FullName}");
						continue;
					}
					_initializeSingltonMethodSet.Add(initMethod);

					success++;
				}
				// 리플렉션 실패
				catch (TargetInvocationException ex)
				{
					fail++;
					Debug.LogError($"[MBSP] Exception during initialize: {baseType.FullName} - {ex.InnerException?.GetType().Name}: {ex.InnerException?.Message}");
				}
				catch (Exception ex)
				{
					fail++;
					Debug.LogError($"[MBSP] Initialize failed: {baseType.FullName} - {ex.GetType().Name}: {ex.Message}");
				}
			}

			Debug.Log($"[MBSP] Initialize complete. success: {success}, fail: {fail}");
			Internally_InvokeSingletonMethods();

			SceneLoader.Instance.Internally_InvokeInitialize_OnLoadScene();
		}

		private static IEnumerable<Type> SafeGetTypes(Assembly assembly)
		{
			try
			{
				return assembly.GetTypes();
			}
			catch (ReflectionTypeLoadException ex)
			{
				if (ex.LoaderExceptions != null && ex.LoaderExceptions.Length > 0)
					Debug.LogWarning($"[MBSP] Failed to load some types: {assembly.FullName} - count of {ex.LoaderExceptions.Length} exception");
				return ex.Types.Where(t => t != null);
			}
			catch (Exception ex)
			{
				Debug.LogError($"[MBSP] Assembly type load failed: {assembly.FullName} - {ex.GetType().Name}: {ex.Message}");
				return Array.Empty<Type>();
			}
		}

		internal static void Internally_InvokeSingletonMethods()
		{
			int success = 0;
			int fail = 0;

			foreach (var method in _initializeSingltonMethodSet)
			{
				try
				{
					method.Invoke(null, null);
					success++;
				}
				catch (Exception ex)
				{
					fail++;
					Debug.LogError($"[MBSP] Invoke method failed: {ex.GetType().Name}: {ex.Message}");
				}
			}

			Debug.Log($"[MBSP] Invoke initialize methods complete. success: {success}, fail: {fail}");
		}
	}
}