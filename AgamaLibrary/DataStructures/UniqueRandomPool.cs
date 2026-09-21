using System;
using System.Collections.Generic;
using System.Linq;

namespace AgamaLibrary.DataStructures
{
	/// <summary>
	/// 범위를 지정하고 그 범위 안에서 값을 중복없이 가져옵니다.
	/// (항목 변경 시 인덱스를 재셔플하여 비중복을 유지합니다.)
	/// </summary>
	public class UniqueRandomPool<T>
	{
		public event Action OnReset;

		private List<T> _range;
		private List<int> _shuffledIndices;
		private int _currentIndex;

		private Random _random;
		private readonly bool _allowDuplicate;

		public int RangeCount => _range.Count;
		public int ValueCount => _range.Count - _currentIndex;
		public IEnumerable<T> Range => _range;

		public UniqueRandomPool(IEnumerable<T> range = null, bool allowDuplicate = false, Random random = null)
		{
			_range = new List<T>();
			_shuffledIndices = new List<int>();
			_currentIndex = 0;

			_random = random ?? new Random();
			_allowDuplicate = allowDuplicate;

			if (range != null)
				_range.AddRange(range);

			ResetIndices();
		}

		private void ResetIndices()
		{
			_currentIndex = 0;
			_shuffledIndices.Clear();
			_shuffledIndices.AddRange(Enumerable.Range(0, _range.Count));

			// Fisher-Yates Shuffle
			int n = _shuffledIndices.Count;
			while (n > 1)
			{
				n--;
				int k = _random.Next(n + 1);
				// 튜플 스왑을 사용하여 간결하게 구현
				(_shuffledIndices[n], _shuffledIndices[k]) = (_shuffledIndices[k], _shuffledIndices[n]);
			}
		}

		/// <summary>
		/// 범위를 초기화하고 값을 설정합니다. (항목 변경 시 무조건 재셔플됩니다.)
		/// </summary>
		public void SetRange(T range)
		{
			ClearRange();
			_range.Add(range);
			ResetIndices();
		}

		/// <summary>
		/// 범위를 초기화하고 값을 설정합니다. (항목 변경 시 무조건 재셔플됩니다.)
		/// </summary>
		public void SetRange(IEnumerable<T> range)
		{
			if (range == null)
				return;

			ClearRange();
			_range.AddRange(range);

			if (!_allowDuplicate)
				_range = _range.Distinct().ToList();

			ResetIndices();
		}

		/// <summary>
		/// 값(<typeparamref name="T"/>)을 범위에 추가합니다. (항목 변경 시 무조건 재셔플됩니다.)
		/// </summary>
		public bool AddRange(T range)
		{
			if (range == null)
				return false;

			if (_allowDuplicate || !_range.Contains(range))
			{
				_range.Add(range);
				ResetIndices();
				return true;
			}
			return false;
		}

		/// <summary>
		/// 지정된 값의 컬렉션을 범위에 추가합니다. (항목 변경 시 무조건 재셔플됩니다.)
		/// </summary>
		public void AddRange(IEnumerable<T> range)
		{
			if (range == null)
				return;

			// 추가될 항목이 있는지 먼저 확인
			int originalCount = _range.Count;
			_range.AddRange(range);

			if (!_allowDuplicate)
				_range = _range.Distinct().ToList();

			// 실제로 범위가 변경된 경우에만 셔플
			if (_range.Count != originalCount)
				ResetIndices();
		}

		/// <summary>
		/// 범위의 값(<typeparamref name="T"/>)을 삭제합니다. (항목 변경 시 무조건 재셔플됩니다.)
		/// </summary>
		public void RemoveRange(T value)
		{
			if (_range.Remove(value))
				ResetIndices();
		}

		/// <summary>
		/// 범위의 값(<typeparamref name="T"/>)을 삭제합니다. (항목 변경 시 무조건 재셔플됩니다.)
		/// </summary>
		public void RemoveRange(IEnumerable<T> value)
		{
			if (value == null)
				return;

			bool removed = false;
			foreach (T item in value)
				if (_range.Remove(item))
					removed = true;

			if (removed)
				ResetIndices();
		}

		/// <summary>
		/// 범위를 초기화합니다.
		/// </summary>
		public void ClearRange()
		{
			_range.Clear();
			ResetIndices();
		}

		/// <summary>
		/// 값의 범위를 초기화합니다. (인덱스 리스트를 재섞습니다)
		/// </summary>
		public void Reset()
		{
			// 인덱스를 다시 섞음
			ResetIndices();
			OnReset?.Invoke();
		}

		/// <summary>
		/// 렌덤으로 범위 안의 T값을 가져옵니다.
		/// 한 범위가 다 지나기 전까지 같은 값을 가져오지 않습니다. (O(1) 성능)
		/// </summary>
		/// <returns>범위가 비어 있을 시 기본값(<typeparamref name="T"/>)을 반환합니다.</returns>
		public T GetValue()
		{
			if (_range.Count <= 0)
				return default(T);

			// 남은 항목이 없으면 재셔플
			if (_currentIndex >= _range.Count)
				Reset();

			// 섞인 인덱스 리스트에서 현재 인덱스에 해당하는 인덱스를 가져옴
			int actualIndex = _shuffledIndices[_currentIndex];
			T value = _range[actualIndex];

			// 포인터 이동
			_currentIndex++;

			return value;
		}

		/// <summary>
		/// 범위 안의 값(<typeparamref name="T"/>)을 렌덤으로 가져옵니다.
		/// </summary>
		/// <returns>값(<typeparamref name="T"/>)을 반환합니다. 만약 범위에 아무것도 없다면 default(<typeparamref name="T"/>)를 반환합니다.</returns>
		public T GetRangeInValue()
		{
			if (_range.Count <= 0)
				return default(T);

			return _range[_random.Next(0, _range.Count)];
		}
	}
}