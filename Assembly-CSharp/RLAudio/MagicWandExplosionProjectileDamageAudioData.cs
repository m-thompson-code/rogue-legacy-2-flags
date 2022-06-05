using System;

namespace RLAudio
{
	// Token: 0x020008FA RID: 2298
	public class MagicWandExplosionProjectileDamageAudioData : ProjectileDamageAudioData
	{
		// Token: 0x17001861 RID: 6241
		// (get) Token: 0x06004B6C RID: 19308 RVA: 0x0010F2C4 File Offset: 0x0010D4C4
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
