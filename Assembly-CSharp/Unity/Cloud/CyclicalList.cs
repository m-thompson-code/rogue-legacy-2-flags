using System;
using System.Collections;
using System.Collections.Generic;

namespace Unity.Cloud
{
	// Token: 0x02000830 RID: 2096
	public class CyclicalList<T> : IList<T>, ICollection<T>, IEnumerable<T>, IEnumerable
	{
		// Token: 0x06004542 RID: 17730 RVA: 0x000F766B File Offset: 0x000F586B
		public CyclicalList(int capacity)
		{
			this.items = new T[capacity];
		}

		// Token: 0x17001719 RID: 5913
		// (get) Token: 0x06004543 RID: 17731 RVA: 0x000F767F File Offset: 0x000F587F
		public int Capacity
		{
			get
			{
				return this.items.Length;
			}
		}

		// Token: 0x1700171A RID: 5914
		// (get) Token: 0x06004544 RID: 17732 RVA: 0x000F7689 File Offset: 0x000F5889
		public int Count
		{
			get
			{
				return this.count;
			}
		}

		// Token: 0x1700171B RID: 5915
		// (get) Token: 0x06004545 RID: 17733 RVA: 0x000F7691 File Offset: 0x000F5891
		public bool IsReadOnly
		{
			get
			{
				return false;
			}
		}

		// Token: 0x1700171C RID: 5916
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

		// Token: 0x06004548 RID: 17736 RVA: 0x000F76E4 File Offset: 0x000F58E4
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

		// Token: 0x06004549 RID: 17737 RVA: 0x000F7754 File Offset: 0x000F5954
		public void Clear()
		{
			this.count = 0;
			this.nextPointer = 0;
		}

		// Token: 0x0600454A RID: 17738 RVA: 0x000F7764 File Offset: 0x000F5964
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

		// Token: 0x0600454B RID: 17739 RVA: 0x000F77C4 File Offset: 0x000F59C4
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

		// Token: 0x0600454C RID: 17740 RVA: 0x000F7820 File Offset: 0x000F5A20
		public IEnumerator<T> GetEnumerator()
		{
			return new CyclicalList<T>.Enumerator(this);
		}

		// Token: 0x0600454D RID: 17741 RVA: 0x000F782D File Offset: 0x000F5A2D
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		// Token: 0x0600454E RID: 17742 RVA: 0x000F7835 File Offset: 0x000F5A35
		public T GetNextEviction()
		{
			return this.items[this.nextPointer];
		}

		// Token: 0x0600454F RID: 17743 RVA: 0x000F7848 File Offset: 0x000F5A48
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

		// Token: 0x06004550 RID: 17744 RVA: 0x000F7880 File Offset: 0x000F5A80
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

		// Token: 0x06004551 RID: 17745 RVA: 0x000F78E4 File Offset: 0x000F5AE4
		public void Insert(int index, T item)
		{
			if (index < 0 || index >= this.count)
			{
				throw new IndexOutOfRangeException();
			}
		}

		// Token: 0x06004552 RID: 17746 RVA: 0x000F78F9 File Offset: 0x000F5AF9
		public bool Remove(T item)
		{
			return false;
		}

		// Token: 0x06004553 RID: 17747 RVA: 0x000F78FC File Offset: 0x000F5AFC
		public void RemoveAt(int index)
		{
			if (index < 0 || index >= this.count)
			{
				throw new IndexOutOfRangeException();
			}
		}

		// Token: 0x04003B17 RID: 15127
		private int count;

		// Token: 0x04003B18 RID: 15128
		private T[] items;

		// Token: 0x04003B19 RID: 15129
		private int nextPointer;

		// Token: 0x02000E55 RID: 3669
		private struct Enumerator : IEnumerator<T>, IEnumerator, IDisposable
		{
			// Token: 0x06006C71 RID: 27761 RVA: 0x00193F69 File Offset: 0x00192169
			public Enumerator(CyclicalList<T> list)
			{
				this.list = list;
				this.currentIndex = -1;
			}

			// Token: 0x17002357 RID: 9047
			// (get) Token: 0x06006C72 RID: 27762 RVA: 0x00193F7C File Offset: 0x0019217C
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

			// Token: 0x17002358 RID: 9048
			// (get) Token: 0x06006C73 RID: 27763 RVA: 0x00193FC0 File Offset: 0x001921C0
			object IEnumerator.Current
			{
				get
				{
					return this.Current;
				}
			}

			// Token: 0x06006C74 RID: 27764 RVA: 0x00193FCD File Offset: 0x001921CD
			public void Dispose()
			{
			}

			// Token: 0x06006C75 RID: 27765 RVA: 0x00193FCF File Offset: 0x001921CF
			public bool MoveNext()
			{
				this.currentIndex++;
				return this.currentIndex < this.list.count;
			}

			// Token: 0x06006C76 RID: 27766 RVA: 0x00193FF2 File Offset: 0x001921F2
			public void Reset()
			{
				this.currentIndex = 0;
			}

			// Token: 0x040057B3 RID: 22451
			private int currentIndex;

			// Token: 0x040057B4 RID: 22452
			private CyclicalList<T> list;
		}
	}
}
