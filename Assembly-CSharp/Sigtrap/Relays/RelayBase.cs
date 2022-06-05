using System;
using Sigtrap.Relays.Binding;

namespace Sigtrap.Relays
{
	// Token: 0x02000950 RID: 2384
	public abstract class RelayBase<TDelegate> : IRelayLinkBase<TDelegate> where TDelegate : class
	{
		// Token: 0x17001AD4 RID: 6868
		// (get) Token: 0x060050CF RID: 20687 RVA: 0x0011E12D File Offset: 0x0011C32D
		public uint listenerCount
		{
			get
			{
				return this._count;
			}
		}

		// Token: 0x17001AD5 RID: 6869
		// (get) Token: 0x060050D0 RID: 20688 RVA: 0x0011E135 File Offset: 0x0011C335
		public uint oneTimeListenersCount
		{
			get
			{
				return this._onceCount;
			}
		}

		// Token: 0x060050D1 RID: 20689 RVA: 0x0011E13D File Offset: 0x0011C33D
		public bool Contains(TDelegate listener)
		{
			return this.Contains(this._listeners, this._count, listener);
		}

		// Token: 0x060050D2 RID: 20690 RVA: 0x0011E154 File Offset: 0x0011C354
		public bool AddListener(TDelegate listener, bool allowDuplicates = false)
		{
			if (!allowDuplicates && this.Contains(listener))
			{
				return false;
			}
			if (this._count == this._cap)
			{
				this._cap *= 2U;
				this._listeners = this.Expand(this._listeners, this._cap, this._count);
			}
			this._listeners[(int)this._count] = listener;
			this._count += 1U;
			return true;
		}

		// Token: 0x060050D3 RID: 20691 RVA: 0x0011E1CA File Offset: 0x0011C3CA
		public IRelayBinding BindListener(TDelegate listener, bool allowDuplicates = false)
		{
			if (this.AddListener(listener, allowDuplicates))
			{
				return new RelayBinding<TDelegate>(this, listener, allowDuplicates, true);
			}
			return null;
		}

		// Token: 0x060050D4 RID: 20692 RVA: 0x0011E1E4 File Offset: 0x0011C3E4
		public bool AddOnce(TDelegate listener, bool allowDuplicates = false)
		{
			if (!allowDuplicates && this.Contains(this._listenersOnce, this._onceCount, listener))
			{
				return false;
			}
			if (this._onceCount == this._onceCap)
			{
				if (this._onceCap == 0U)
				{
					this._onceCap = 1U;
				}
				else
				{
					this._onceCap *= 2U;
				}
				this._listenersOnce = this.Expand(this._listenersOnce, this._onceCap, this._onceCount);
			}
			this._listenersOnce[(int)this._onceCount] = listener;
			this._onceCount += 1U;
			return true;
		}

		// Token: 0x060050D5 RID: 20693 RVA: 0x0011E278 File Offset: 0x0011C478
		public bool RemoveListener(TDelegate listener)
		{
			bool result = false;
			for (uint num = 0U; num < this._count; num += 1U)
			{
				if (this._listeners[(int)num].Equals(listener))
				{
					this.RemoveAt(num);
					result = true;
					break;
				}
			}
			return result;
		}

		// Token: 0x060050D6 RID: 20694 RVA: 0x0011E2C4 File Offset: 0x0011C4C4
		public bool RemoveOnce(TDelegate listener)
		{
			bool result = false;
			for (uint num = 0U; num < this._onceCount; num += 1U)
			{
				if (this._listenersOnce[(int)num].Equals(listener))
				{
					this.RemoveOnceAt(num);
					result = true;
					break;
				}
			}
			return result;
		}

		// Token: 0x060050D7 RID: 20695 RVA: 0x0011E310 File Offset: 0x0011C510
		public void RemoveAll(bool removePersistentListeners = true, bool removeOneTimeListeners = true)
		{
			if (removePersistentListeners)
			{
				Array.Clear(this._listeners, 0, (int)this._cap);
				this._count = 0U;
			}
			if (removeOneTimeListeners && this._onceCount > 0U)
			{
				Array.Clear(this._listenersOnce, 0, (int)this._onceCap);
				this._onceCount = 0U;
			}
		}

		// Token: 0x060050D8 RID: 20696 RVA: 0x0011E35E File Offset: 0x0011C55E
		protected void RemoveAt(uint i)
		{
			this._count = this.RemoveAt(this._listeners, this._count, i);
		}

		// Token: 0x060050D9 RID: 20697 RVA: 0x0011E379 File Offset: 0x0011C579
		protected void RemoveOnceAt(uint i)
		{
			this._onceCount = this.RemoveAt(this._listenersOnce, this._onceCount, i);
		}

		// Token: 0x060050DA RID: 20698 RVA: 0x0011E394 File Offset: 0x0011C594
		protected uint RemoveAt(TDelegate[] arr, uint count, uint i)
		{
			count -= 1U;
			for (uint num = i; num < count; num += 1U)
			{
				arr[(int)num] = arr[(int)(num + 1U)];
			}
			arr[(int)count] = default(TDelegate);
			return count;
		}

		// Token: 0x060050DB RID: 20699 RVA: 0x0011E3D4 File Offset: 0x0011C5D4
		private bool Contains(TDelegate[] arr, uint c, TDelegate d)
		{
			for (uint num = 0U; num < c; num += 1U)
			{
				if (arr[(int)num].Equals(d))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x060050DC RID: 20700 RVA: 0x0011E40C File Offset: 0x0011C60C
		private TDelegate[] Expand(TDelegate[] arr, uint cap, uint count)
		{
			TDelegate[] array = new TDelegate[cap];
			int num = 0;
			while ((long)num < (long)((ulong)count))
			{
				array[num] = arr[num];
				num++;
			}
			return array;
		}

		// Token: 0x0400432E RID: 17198
		protected bool _hasLink;

		// Token: 0x0400432F RID: 17199
		protected TDelegate[] _listeners = new TDelegate[1];

		// Token: 0x04004330 RID: 17200
		protected uint _count;

		// Token: 0x04004331 RID: 17201
		protected uint _cap = 1U;

		// Token: 0x04004332 RID: 17202
		protected TDelegate[] _listenersOnce;

		// Token: 0x04004333 RID: 17203
		protected uint _onceCount;

		// Token: 0x04004334 RID: 17204
		protected uint _onceCap;

		// Token: 0x04004335 RID: 17205
		protected static IndexOutOfRangeException _eIOOR = new IndexOutOfRangeException("Fewer listeners than expected. See guidelines in Relay.cs on using RemoveListener and RemoveAll within Relay listeners.");
	}
}
