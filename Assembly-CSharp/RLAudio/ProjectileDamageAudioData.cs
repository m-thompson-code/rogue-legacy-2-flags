using System;

namespace RLAudio
{
	// Token: 0x02000E88 RID: 3720
	public class ProjectileDamageAudioData : BaseDamageAudioData
	{
		// Token: 0x1700216D RID: 8557
		// (get) Token: 0x060068E7 RID: 26855 RVA: 0x0003A1B3 File Offset: 0x000383B3
		public override string BreakableDamageAudioPath
		{
			get
			{
				if (!this.m_arePathsSet)
				{
					this.SetAudioPaths();
				}
				return this.m_breakableDamageAudioEventPath;
			}
		}

		// Token: 0x1700216E RID: 8558
		// (get) Token: 0x060068E8 RID: 26856 RVA: 0x0003A1C9 File Offset: 0x000383C9
		public override string CharacterDamageAudioPath
		{
			get
			{
				if (!this.m_arePathsSet)
				{
					this.SetAudioPaths();
				}
				return this.m_characterDamageAudioPath;
			}
		}

		// Token: 0x060068E9 RID: 26857 RVA: 0x00180F28 File Offset: 0x0017F128
		private string GetProjectileName()
		{
			string text = base.name;
			if (text.EndsWith(" Clone"))
			{
				text = text.Substring(0, text.Length - " Clone".Length);
			}
			else if (text.EndsWith("(Clone)"))
			{
				text = text.Substring(0, text.Length - "(Clone)".Length);
			}
			return text;
		}

		// Token: 0x060068EA RID: 26858 RVA: 0x00180F8C File Offset: 0x0017F18C
		private void SetAudioPaths()
		{
			this.m_arePathsSet = true;
			ProjectileAudioLibraryEntry entry = ProjectileAudioLibrary.GetEntry(this.GetProjectileName());
			if (entry != null)
			{
				this.m_breakableDamageAudioEventPath = entry.HitItemEventPath;
				this.m_characterDamageAudioPath = entry.HitCharacterEventPath;
			}
		}

		// Token: 0x04005552 RID: 21842
		private string m_characterDamageAudioPath;

		// Token: 0x04005553 RID: 21843
		private string m_breakableDamageAudioEventPath;

		// Token: 0x04005554 RID: 21844
		private bool m_arePathsSet;
	}
}
