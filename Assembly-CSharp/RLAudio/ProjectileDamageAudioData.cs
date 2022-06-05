using System;

namespace RLAudio
{
	// Token: 0x0200090B RID: 2315
	public class ProjectileDamageAudioData : BaseDamageAudioData
	{
		// Token: 0x17001874 RID: 6260
		// (get) Token: 0x06004BEC RID: 19436 RVA: 0x00110D6A File Offset: 0x0010EF6A
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

		// Token: 0x17001875 RID: 6261
		// (get) Token: 0x06004BED RID: 19437 RVA: 0x00110D80 File Offset: 0x0010EF80
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

		// Token: 0x06004BEE RID: 19438 RVA: 0x00110D98 File Offset: 0x0010EF98
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

		// Token: 0x06004BEF RID: 19439 RVA: 0x00110DFC File Offset: 0x0010EFFC
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

		// Token: 0x04003FF2 RID: 16370
		private string m_characterDamageAudioPath;

		// Token: 0x04003FF3 RID: 16371
		private string m_breakableDamageAudioEventPath;

		// Token: 0x04003FF4 RID: 16372
		private bool m_arePathsSet;
	}
}
