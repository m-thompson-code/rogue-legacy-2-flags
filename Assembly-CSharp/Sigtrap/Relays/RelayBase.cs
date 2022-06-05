using System;
using Sigtrap.Relays.Binding;

namespace Sigtrap.Relays
{
	// Token: 0x02000EF8 RID: 3832
	public abstract class RelayBase<TDelegate> : IRelayLinkBase<TDelegate> where TDelegate : class
	{
		// Token: 0x17002417 RID: 9239
		// (get) Token: 0x06006EC3 RID: 28355 RVA: 0x0003D089 File Offset: 0x0003B289
		public uint listenerCount
		{
			get
			{
				return this._count;
			}
		}

		// Token: 0x17002418 RID: 9240
		// (get) Token: 0x06006EC4 RID: 28356 RVA: 0x0003D091 File Offset: 0x0003B291
		public uint oneTimeListenersCount
		{
			get
			{
				return this._onceCount;
			}
		}

		// Token: 0x06006EC5 RID: 28357 RVA: 0x0003D099 File Offset: 0x0003B299
		public bool Contains(TDelegate listener)
		{
			return this.Contains(this._listeners, this._count, listener);
		}

		// Token: 0x06006EC6 RID: 28358 RVA: 0x0018C688 File Offset: 0x0018A888
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

		// Token: 0x06006EC7 RID: 28359 RVA: 0x0003D0AE File Offset: 0x0003B2AE
		public IRelayBinding BindListener(TDelegate listener, bool allowDuplicates = false)
		{
			if (this.AddListener(listener, allowDuplicates))
			{
				return new RelayBinding<TDelegate>(this, listener, allowDuplicates, true);
			}
			return null;
		}

		// Token: 0x06006EC8 RID: 28360 RVA: 0x0018C700 File Offset: 0x0018A900
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

		// Token: 0x06006EC9 RID: 28361 RVA: 0x0018C794 File Offset: 0x0018A994
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

		// Token: 0x06006ECA RID: 28362 RVA: 0x0018C7E0 File Offset: 0x0018A9E0
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

		// Token: 0x06006ECB RID: 28363 RVA: 0x0018C82C File Offset: 0x0018AA2C
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

		// Token: 0x06006ECC RID: 28364 RVA: 0x0003D0C5 File Offset: 0x0003B2C5
		protected void RemoveAt(uint i)
		{
			this._count = this.RemoveAt(this._listeners, this._count, i);
		}

		// Token: 0x06006ECD RID: 28365 RVA: 0x0003D0E0 File Offset: 0x0003B2E0
		protected void RemoveOnceAt(uint i)
		{
			this._onceCount = this.RemoveAt(this._listenersOnce, this._onceCount, i);
		}

		// Token: 0x06006ECE RID: 28366 RVA: 0x0018C87C File Offset: 0x0018AA7C
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

		// Token: 0x06006ECF RID: 28367 RVA: 0x0018C8BC File Offset: 0x0018AABC
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

		// Token: 0x06006ED0 RID: 28368 RVA: 0x0018C8F4 File Offset: 0x0018AAF4
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

		// Token: 0x04005935 RID: 22837
		protected bool _hasLink;

		// Token: 0x04005936 RID: 22838
		protected TDelegate[] _listeners = new TDelegate[1];

		// Token: 0x04005937 RID: 22839
		protected uint _count;

		// Token: 0x04005938 RID: 22840
		protected uint _cap = 1U;

		// Token: 0x04005939 RID: 22841
		protected TDelegate[] _listenersOnce;

		// Token: 0x0400593A RID: 22842
		protected uint _onceCount;

		// Token: 0x0400593B RID: 22843
		protected uint _onceCap;

		// Token: 0x0400593C RID: 22844
		protected static IndexOutOfRangeException _eIOOR = new IndexOutOfRangeException("Fewer listeners than expected. See guidelines in Relay.cs on using RemoveListener and RemoveAll within Relay listeners.");
	}
}
