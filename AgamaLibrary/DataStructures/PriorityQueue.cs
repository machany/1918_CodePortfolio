using System;
using System.Collections.Generic;

public class PriorityQueue<T, TPriority> where TPriority : IComparable<TPriority>
{
	private List<(T Element, TPriority Priority)> _heap = new List<(T, TPriority)>();

	public int Count => _heap.Count;

	public void Enqueue(T element, TPriority priority)
	{
		_heap.Add((element, priority));
		SiftUp(_heap.Count - 1); // 요소를 위로 이동
	}

	/// <summary>
	/// 가장 높은 우선순위를 가진 요소를 반환 후 제거
	/// </summary>
	/// <exception cref="InvalidOperationException">큐가 비어있는 경우</exception>
	public T Dequeue()
	{
		if (Count == 0)
			throw new InvalidOperationException("PriorityQueue is empty.");

		T element = _heap[0].Element;

		int lastIndex = _heap.Count - 1;
		_heap[0] = _heap[lastIndex];
		_heap.RemoveAt(lastIndex);

		if (Count > 0)
			SiftDown(0);

		return element;
	}

	/// <summary>
	/// 가장 높은 우선순위를 가진 요소를 반환.
	/// </summary>
	/// <exception cref="InvalidOperationException">큐가 비어있는 경우</exception>
	public T Peek()
	{
		if (Count == 0)
			throw new InvalidOperationException("PriorityQueue is empty.");
		return _heap[0].Element;
	}

	public TPriority PeekPriority()
	{
		if (Count == 0)
			throw new InvalidOperationException("PriorityQueue is empty.");
		return _heap[0].Priority;
	}

	public void Clear()
	{
		_heap.Clear();
	}

	// 요소를 위로 올리는 힙 정렬 메소드
	private void SiftUp(int index)
	{
		while (index > 0)
		{
			int parentIndex = (index - 1) / 2;

			// 현재 요소의 우선순위가 부모보다 높거나 같으면 정지
			if (_heap[index].Priority.CompareTo(_heap[parentIndex].Priority) >= 0)
				break;

			Swap(index, parentIndex);
			index = parentIndex;
		}
	}

	// 요소를 아래로 내리는 힙 정렬 메소드
	private void SiftDown(int index)
	{
		while (true)
		{
			int leftChildIndex = 2 * index + 1;
			int rightChildIndex = 2 * index + 2;
			int smallestIndex = index; // 현재 노드, 왼쪽 자식, 오른쪽 자식 중 우선순위가 가장 높은 인덱스

			// 왼쪽 자식 확인
			if (leftChildIndex < Count &&
				_heap[leftChildIndex].Priority.CompareTo(_heap[smallestIndex].Priority) < 0)
			{
				smallestIndex = leftChildIndex;
			}

			// 오른쪽 자식 확인
			if (rightChildIndex < Count &&
				_heap[rightChildIndex].Priority.CompareTo(_heap[smallestIndex].Priority) < 0)
			{
				smallestIndex = rightChildIndex;
			}

			// 가장 작은 요소가 현재 요소가 아니라면 교환하고 계속 진행
			if (smallestIndex != index)
			{
				Swap(index, smallestIndex);
				index = smallestIndex;
			}
			else
				break;
		}
	}

	private void Swap(int i, int j)
	{
		var temp = _heap[i];
		_heap[i] = _heap[j];
		_heap[j] = temp;
	}
}