using System;
using System.Collections;
using UnityEngine;

namespace Unity.Cloud.UserReporting.Plugin
{
	// Token: 0x02000845 RID: 2117
	public class UnityUserReportingUpdater : IEnumerator
	{
		// Token: 0x0600460F RID: 17935 RVA: 0x000F9738 File Offset: 0x000F7938
		public UnityUserReportingUpdater()
		{
			this.waitForEndOfFrame = new WaitForEndOfFrame();
		}

		// Token: 0x17001763 RID: 5987
		// (get) Token: 0x06004610 RID: 17936 RVA: 0x000F974B File Offset: 0x000F794B
		// (set) Token: 0x06004611 RID: 17937 RVA: 0x000F9753 File Offset: 0x000F7953
		public object Current { get; private set; }

		// Token: 0x06004612 RID: 17938 RVA: 0x000F975C File Offset: 0x000F795C
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

		// Token: 0x06004613 RID: 17939 RVA: 0x000F97CD File Offset: 0x000F79CD
		public void Reset()
		{
			this.step = 0;
		}

		// Token: 0x04003B71 RID: 15217
		private int step;

		// Token: 0x04003B72 RID: 15218
		private WaitForEndOfFrame waitForEndOfFrame;
	}
}
