using System;
using System.Reflection;
using UnityEngine;

namespace AgamaLibrary.Unity.Methods
{
	public static class LidraryUnityMethod
	{
		/// <summary>
		/// Transform에서 <typeparamref name="T"/> 타입의 컴포넌트를 가져오거나 없으면 추가합니다.
		/// </summary>
		/// <typeparam name="T">가져오거나 추가할 컴포넌트 타입입니다.</typeparam>
		/// <param name="transform">컴포넌트를 가져오거나 추가할 Transform입니다.</param>
		/// <returns>찾은 또는 새로 추가된 <typeparamref name="T"/> 타입의 컴포넌트입니다.</returns>
		public static T ForceGetComponent<T>(this Transform transform) where T : Component
			=> transform.TryGetComponent(out T comp) ? comp : transform.gameObject.AddComponent<T>();

		/// <summary>
		/// Transform에서 <typeparamref name="T"/> 타입의 컴포넌트를 가져오거나 없으면 <typeparamref name="A"/> 타입의 컴포넌트를 추가합니다.
		/// </summary>
		/// <typeparam name="T">가져오거나 추가할 컴포넌트 타입입니다.</typeparam>
		/// <typeparam name="A"><typeparamref name="T"/>의 서브클래스 타입입니다.</typeparam>
		/// <param name="transform">컴포넌트를 가져오거나 추가할 Transform입니다.</param>
		/// <returns>찾은 <typeparamref name="T"/> 또는 새로 추가된 <typeparamref name="A"/>를 <typeparamref name="T"/> 타입으로 가져옵니다.</returns>
		public static T TryGetOrAddComponent<T, A>(this Transform transform)
			where A : Component, T
			=> transform.TryGetComponent(out T comp) ? comp : transform.gameObject.AddComponent<A>();

		public static T CopyComponent<T>(this GameObject destination, T originalComp, bool tryGet = false) where T : Component
		{
			T comp;
			if (tryGet)
				comp = destination.transform.ForceGetComponent<T>();
			else
				comp = destination.AddComponent(originalComp.GetType()) as T;
			return comp.CopyComponent(originalComp);
		}

		// gpt가 쓴거 제거하고 새로 썼는데 비슷하다;;
		public static T CopyComponent<T>(this T targetComponent, T originalComp)
		{
			Type originfCompType = originalComp.GetType();
			BindingFlags flags = BindingFlags.Public
								| BindingFlags.NonPublic
								| BindingFlags.Instance;

			FieldInfo[] fieldInfos = originfCompType.GetFields(flags);
			foreach (FieldInfo field in fieldInfos)
				field.SetValue(targetComponent, field.GetValue(originalComp));

			PropertyInfo[] propertyInfoes = originfCompType.GetProperties(flags);
			foreach (PropertyInfo property in propertyInfoes)
				if (property.CanWrite
					&& property.CanRead
					&& property.Name != "name")
					property.SetValue(targetComponent, property.GetValue(originalComp));

			return targetComponent;
		}
	}
}