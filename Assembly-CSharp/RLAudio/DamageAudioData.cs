using System;
using FMODUnity;
using UnityEngine;
using UnityEngine.Serialization;

namespace RLAudio
{
	// Token: 0x02000E5A RID: 3674
	public class DamageAudioData : BaseDamageAudioData
	{
		// Token: 0x17002132 RID: 8498
		// (get) Token: 0x060067A2 RID: 26530 RVA: 0x00039362 File Offset: 0x00037562
		public override string BreakableDamageAudioPath
		{
			get
			{
				return this.m_breakableAudioEventPath;
			}
		}

		// Token: 0x17002133 RID: 8499
		// (get) Token: 0x060067A3 RID: 26531 RVA: 0x0003936A File Offset: 0x0003756A
		public override string CharacterDamageAudioPath
		{
			get
			{
				return this.m_characterAudioEventPath;
			}
		}

		// Token: 0x040053F2 RID: 21490
		[SerializeField]
		[EventRef]
		[FormerlySerializedAs("m_characterFmodEvent")]
		private string m_breakableAudioEventPath;

		// Token: 0x040053F3 RID: 21491
		[SerializeField]
		[EventRef]
		[FormerlySerializedAs("m_characterFmodEvent")]
		private string m_characterAudioEventPath;
	}
}
