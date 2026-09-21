using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace AgamaLibrary.Unity.DataStructures
{
	[Serializable]
	public class SerializedDictionary<TK, TV>
	{
		[SerializeField] private List<SerializedDictionaryPair<TK, TV>> dictValues;

		private Lazy<Dictionary<TK, TV>> _dict;

		public SerializedDictionary()
		{
			_dict = new Lazy<Dictionary<TK, TV>>(() =>
			{
				var dict = new Dictionary<TK, TV>();
				if (dictValues != null)
				{
					foreach (var item in dictValues)
						dict.TryAdd(item.key, item.value);
				}
				return dict;
			});
		}

		public Dictionary<TK, TV> GetDict()
		{
			return _dict.Value;
		}

		public void Distinct()
		{
			dictValues = dictValues.Distinct(new EqualitySerializedDictionaryPairComparer<TK, TV>()).ToList();
		}
	}
}
