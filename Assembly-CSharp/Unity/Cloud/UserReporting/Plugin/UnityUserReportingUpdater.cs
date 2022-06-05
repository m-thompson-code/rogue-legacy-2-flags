using System;
using System.Collections;
using UnityEngine;

namespace Unity.Cloud.UserReporting.Plugin
{
	// Token: 0x02000D30 RID: 3376
	public class UnityUserReportingUpdater : IEnumerator
	{
		// Token: 0x06006066 RID: 24678 RVA: 0x000352BC File Offset: 0x000334BC
		public UnityUserReportingUpdater()
		{
			this.waitForEndOfFrame = new WaitForEndOfFrame();
		}

		// Token: 0x17001F9F RID: 8095
		// (get) Token: 0x06006067 RID: 24679 RVA: 0x000352CF File Offset: 0x000334CF
		// (set) Token: 0x06006068 RID: 24680 RVA: 0x000352D7 File Offset: 0x000334D7
		public object Current { get; private set; }

		// Token: 0x06006069 RID: 24681 RVA: 0x00166C14 File Offset: 0x00164E14
		public bool MoveNext()
		{
			if (this.step == 0)
			{
				UnityUserReporting.CurrentClient.Update();
				this.Current = null;
				this.step = 1;
				return true;
			}
			if (this.step == 1)
			{
				this.Current = this.waitForEndOfFrame;
				this.step = 2;
				return true;
			}
			if (this.step == 2)
			{
				UnityUserReporting.CurrentClient.UpdateOnEndOfFrame();
				this.Current = null;
				this.step = 3;
				return false;
			}
			return false;
		}

		// Token: 0x0600606A RID: 24682 RVA: 0x000352E0 File Offset: 0x000334E0
		public void Reset()
		{
			this.step = 0;
		}

		// Token: 0x04004ECB RID: 20171
		private int step;

		// Token: 0x04004ECC RID: 20172
		private WaitForEndOfFrame waitForEndOfFrame;
	}
}
