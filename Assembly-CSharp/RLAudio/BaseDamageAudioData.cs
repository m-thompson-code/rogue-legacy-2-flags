using System;
using System.Collections.Generic;
using UnityEngine;

namespace RLAudio
{
	// Token: 0x02000E4F RID: 3663
	public abstract class BaseDamageAudioData : MonoBehaviour
	{
		// Token: 0x17002125 RID: 8485
		// (get) Token: 0x06006760 RID: 26464
		public abstract string BreakableDamageAudioPath { get; }

		// Token: 0x17002126 RID: 8486
		// (get) Token: 0x06006761 RID: 26465
		public abstract string CharacterDamageAudioPath { get; }

		// Token: 0x17002127 RID: 8487
		// (get) Token: 0x06006762 RID: 26466 RVA: 0x00038F45 File Offset: 0x00037145
		// (set) Token: 0x06006763 RID: 26467 RVA: 0x00038F4D File Offset: 0x0003714D
		public Dictionary<string, float> HitAudioParameters { get; private set; }

		// Token: 0x06006764 RID: 26468 RVA: 0x00038F56 File Offset: 0x00037156
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

		// Token: 0x06006765 RID: 26469 RVA: 0x00038F94 File Offset: 0x00037194
		public void RemoveAudioParameter(string parameterName)
		{
			if (this.HitAudioParameters != null && this.HitAudioParameters.ContainsKey(parameterName))
			{
				this.HitAudioParameters.Remove(parameterName);
			}
		}

		// Token: 0x06006766 RID: 26470 RVA: 0x00038FB9 File Offset: 0x000371B9
		protected virtual void OnDisable()
		{
			if (this.HitAudioParameters != null)
			{
				this.HitAudioParameters.Clear();
			}
		}
	}
}
