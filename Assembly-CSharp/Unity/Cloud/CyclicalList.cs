using System;
using System.Collections;
using System.Collections.Generic;

namespace Unity.Cloud
{
	// Token: 0x02000D14 RID: 3348
	public class CyclicalList<T> : IList<T>, ICollection<T>, IEnumerable<T>, IEnumerable
	{
		// Token: 0x06005F72 RID: 24434 RVA: 0x0003498A File Offset: 0x00032B8A
		public CyclicalList(int capacity)
		{
			this.items = new T[capacity];
		}

		// Token: 0x17001F45 RID: 8005
		// (get) Token: 0x06005F73 RID: 24435 RVA: 0x0003499E File Offset: 0x00032B9E
		public int Capacity
		{
			get
			{
				return this.items.Length;
			}
		}

		// Token: 0x17001F46 RID: 8006
		// (get) Token: 0x06005F74 RID: 24436 RVA: 0x000349A8 File Offset: 0x00032BA8
		public int Count
		{
			get
			{
				return this.count;
			}
		}

		// Token: 0x17001F47 RID: 8007
		// (get) Token: 0x06005F75 RID: 24437 RVA: 0x00003CD2 File Offset: 0x00001ED2
		public bool IsReadOnly
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17001F48 RID: 8008
		public T this[int index]
		{
			get
			{
				if (index < 0 || index >= this.count)
				{
					throw new IndexOutOfRangeException();
				}
				return this.items[this.GetPointer(index)];
			}
			set
			{
				if (index < 0 || index >= this.count)
				{
					throw new IndexOutOfRangeException();
				}
				this.items[this.GetPointer(index)] = value;
			}
		}

		// Token: 0x06005F78 RID: 24440 RVA: 0x001652E8 File Offset: 0x001634E8
		public void Add(T item)
		{
			this.items[this.nextPointer] = item;
			this.count++;
			if (this.count > this.items.Length)
			{
				this.count = this.items.Length;
			}
			this.nextPointer++;
			if (this.nextPointer >= this.items.Length)
			{
				this.nextPointer = 0;
			}
		}

		// Token: 0x06005F79 RID: 24441 RVA: 0x000349FF File Offset: 0x00032BFF
		public void Clear()
		{
			this.count = 0;
			this.nextPointer = 0;
		}

		// Token: 0x06005F7A RID: 24442 RVA: 0x00165358 File Offset: 0x00163558
		public bool Contains(T item)
		{
			foreach (T t in this)
			{
				if (t.Equals(item))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06005F7B RID: 24443 RVA: 0x001653B8 File Offset: 0x001635B8
		public void CopyTo(T[] array, int arrayIndex)
		{
			int num = 0;
			foreach (T t in this)
			{
				int num2 = arrayIndex + num;
				if (num2 >= array.Length)
				{
					break;
				}
				array[num2] = t;
				num++;
			}
		}

		// Token: 0x06005F7C RID: 24444 RVA: 0x00034A0F File Offset: 0x00032C0F
		public IEnumerator<T> GetEnumerator()
		{
			return new CyclicalList<T>.Enumerator(this);
		}

		// Token: 0x06005F7D RID: 24445 RVA: 0x00034A1C File Offset: 0x00032C1C
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		// Token: 0x06005F7E RID: 24446 RVA: 0x00034A24 File Offset: 0x00032C24
		public T GetNextEviction()
		{
			return this.items[this.nextPointer];
		}

		// Token: 0x06005F7F RID: 24447 RVA: 0x00034A37 File Offset: 0x00032C37
		private int GetPointer(int index)
		{
			if (index < 0 || index >= this.count)
			{
				throw new IndexOutOfRangeException();
			}
			if (this.count < this.items.Length)
			{
				return index;
			}
			return (this.nextPointer + index) % this.count;
		}

		// Token: 0x06005F80 RID: 24448 RVA: 0x00165414 File Offset: 0x00163614
		public int IndexOf(T item)
		{
			int num = 0;
			foreach (T t in this)
			{
				if (t.Equals(item))
				{
					return num;
				}
				num++;
			}
			return -1;
		}

		// Token: 0x06005F81 RID: 24449 RVA: 0x00034A6D File Offset: 0x00032C6D
		public void Insert(int index, T item)
		{
			if (index < 0 || index >= this.count)
			{
				throw new IndexOutOfRangeException();
			}
		}

		// Token: 0x06005F82 RID: 24450 RVA: 0x00003CD2 File Offset: 0x00001ED2
		public bool Remove(T item)
		{
			return false;
		}

		// Token: 0x06005F83 RID: 24451 RVA: 0x00034A6D File Offset: 0x00032C6D
		public void RemoveAt(int index)
		{
			if (index < 0 || index >= this.count)
			{
				throw new IndexOutOfRangeException();
			}
		}

		// Token: 0x04004E56 RID: 20054
		private int count;

		// Token: 0x04004E57 RID: 20055
		private T[] items;

		// Token: 0x04004E58 RID: 20056
		private int nextPointer;

		// Token: 0x02000D15 RID: 3349
		private struct Enumerator : IEnumerator<T>, IEnumerator, IDisposable
		{
			// Token: 0x06005F84 RID: 24452 RVA: 0x00034A82 File Offset: 0x00032C82
			public Enumerator(CyclicalList<T> list)
			{
				this.list = list;
				this.currentIndex = -1;
			}

			// Token: 0x17001F49 RID: 8009
			// (get) Token: 0x06005F85 RID: 24453 RVA: 0x00165478 File Offset: 0x00163678
			public T Current
			{
				get
				{
					if (this.currentIndex < 0 || this.currentIndex >= this.list.Count)
					{
						return default(T);
					}
					return this.list[this.currentIndex];
				}
			}

			// Token: 0x17001F4A RID: 8010
			// (get) Token: 0x06005F86 RID: 24454 RVA: 0x00034A92 File Offset: 0x00032C92
			object IEnumerator.Current
			{
				get
				{
					return this.Current;
				}
			}

			// Token: 0x06005F87 RID: 24455 RVA: 0x00002FCA File Offset: 0x000011CA
			public void Dispose()
			{
			}

			// Token: 0x06005F88 RID: 24456 RVA: 0x00034A9F File Offset: 0x00032C9F
			public bool MoveNext()
			{
				this.currentIndex++;
				return this.currentIndex < this.list.count;
			}

			// Token: 0x06005F89 RID: 24457 RVA: 0x00034AC2 File Offset: 0x00032CC2
			public void Reset()
			{
				this.currentIndex = 0;
			}

			// Token: 0x04004E59 RID: 20057
			private int currentIndex;

			// Token: 0x04004E5A RID: 20058
			private CyclicalList<T> list;
		}
	}
}
