using System;

namespace RLAudio
{
	// Token: 0x02000E73 RID: 3699
	public class MagicWandExplosionProjectileDamageAudioData : ProjectileDamageAudioData
	{
		// Token: 0x17002152 RID: 8530
		// (get) Token: 0x0600684F RID: 26703 RVA: 0x0017F508 File Offset: 0x0017D708
		public override string CharacterDamageAudioPath
		{
			get
			{
				string result = base.CharacterDamageAudioPath;
				if (base.GetComponent<Projectile_RL>().ActualCritChance >= 100f)
				{
					result = "event:/SFX/Weapons/sfx_weapon_crit_wand_hit";
				}
				return result;
			}
		}
	}
}
