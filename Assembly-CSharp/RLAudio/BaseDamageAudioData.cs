using System;
using System.Collections.Generic;
using UnityEngine;

namespace RLAudio
{
	// Token: 0x020008DF RID: 2271
	public abstract class BaseDamageAudioData : MonoBehaviour
	{
		// Token: 0x17001842 RID: 6210
		// (get) Token: 0x06004AA7 RID: 19111
		public abstract string BreakableDamageAudioPath { get; }

		// Token: 0x17001843 RID: 6211
		// (get) Token: 0x06004AA8 RID: 19112
		public abstract string CharacterDamageAudioPath { get; }

		// Token: 0x17001844 RID: 6212
		// (get) Token: 0x06004AA9 RID: 19113 RVA: 0x0010C8B0 File Offset: 0x0010AAB0
		// (set) Token: 0x06004AAA RID: 19114 RVA: 0x0010C8B8 File Offset: 0x0010AAB8
		public Dictionary<string, float> HitAudioParameters { get; private set; }

		// Token: 0x06004AAB RID: 19115 RVA: 0x0010C8C1 File Offset: 0x0010AAC1
		public void AddHitAudioParameter(string parameterName, float parameterValue)
		{
			if (this.HitAudioParameters == null)
			{
				this.HitAudioParameters = new Dictionary<string, float>();
			}
			if (!this.HitAudioParameters.ContainsKey(parameterName))
			{
				this.HitAudioParameters.Add(parameterName, parameterValue);
				return;
			}
			this.HitAudioParameters[parameterName] = parameterValue;
		}

		// Token: 0x06004AAC RID: 19116 RVA: 0x0010C8FF File Offset: 0x0010AAFF
		public void RemoveAudioParameter(string parameterName)
		{
			if (this.HitAudioParameters != null && this.HitAudioParameters.ContainsKey(parameterName))
			{
				this.HitAudioParameters.Remove(parameterName);
			}
		}

		// Token: 0x06004AAD RID: 19117 RVA: 0x0010C924 File Offset: 0x0010AB24
		protected virtual void OnDisable()
		{
			if (this.HitAudioParameters != null)
			{
				this.HitAudioParameters.Clear();
			}
		}
	}
}
