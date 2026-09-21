// 만일 오류가 생긴다면 https://github.com/annulusgames/Alchemy를 설치하고 연결 시키길 바람.
// 그래도 오류가 난다면 주석으로 "// here!" 라고 표시된 부분의 줄을 삭제하면 됨.

using Alchemy.Inspector; // here!
using System;
using System.Collections.Generic;

namespace AgamaLibrary.Unity.DataStructures
{
	public class EqualitySerializedDictionaryPairComparer<K, V> : IEqualityComparer<SerializedDictionaryPair<K, V>>
	{
		public bool Equals(SerializedDictionaryPair<K, V> x, SerializedDictionaryPair<K, V> y)
		{
			return EqualityComparer<K>.Default.Equals(x.key, y.key);
		}

		public int GetHashCode(SerializedDictionaryPair<K, V> obj)
		{
			return obj.key?.GetHashCode() ?? 0;
		}
	}

	[Serializable]
	[HorizontalGroup] // here!
	public struct SerializedDictionaryPair<K, V>
	{
		[LabelWidth(30f)] // here!
		public K key;
		[LabelWidth(40f)] // here!
		public V value;
	}
}
