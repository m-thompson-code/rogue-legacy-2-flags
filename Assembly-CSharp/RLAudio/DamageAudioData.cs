using System;
using FMODUnity;
using UnityEngine;
using UnityEngine.Serialization;

namespace RLAudio
{
	// Token: 0x020008EA RID: 2282
	public class DamageAudioData : BaseDamageAudioData
	{
		// Token: 0x1700184F RID: 6223
		// (get) Token: 0x06004AE9 RID: 19177 RVA: 0x0010D32C File Offset: 0x0010B52C
		public override string BreakableDamageAudioPath
		{
			get
			{
				return this.m_breakableAudioEventPath;
			}
		}

		// Token: 0x17001850 RID: 6224
		// (get) Token: 0x06004AEA RID: 19178 RVA: 0x0010D334 File Offset: 0x0010B534
		public override string CharacterDamageAudioPath
		{
			get
			{
				return this.m_characterAudioEventPath;
			}
		}

		// Token: 0x04003ED4 RID: 16084
		[SerializeField]
		[EventRef]
		[FormerlySerializedAs("m_characterFmodEvent")]
		private string m_breakableAudioEventPath;

		// Token: 0x04003ED5 RID: 16085
		[SerializeField]
		[EventRef]
		[FormerlySerializedAs("m_characterFmodEvent")]
		private string m_characterAudioEventPath;
	}
}
