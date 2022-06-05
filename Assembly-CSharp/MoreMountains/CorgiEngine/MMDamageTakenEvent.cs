using System;
using UnityEngine;

namespace MoreMountains.CorgiEngine
{
	// Token: 0x0200096B RID: 2411
	public struct MMDamageTakenEvent
	{
		// Token: 0x06005195 RID: 20885 RVA: 0x00120154 File Offset: 0x0011E354
		public MMDamageTakenEvent(Character affectedCharacter, GameObject instigator, float currentHealth, float damageCaused, float previousHealth)
		{
			this.AffectedCharacter = affectedCharacter;
			this.Instigator = instigator;
			this.CurrentHealth = currentHealth;
			this.DamageCaused = damageCaused;
			this.PreviousHealth = previousHealth;
		}

		// Token: 0x0400439C RID: 17308
		public Character AffectedCharacter;

		// Token: 0x0400439D RID: 17309
		public GameObject Instigator;

		// Token: 0x0400439E RID: 17310
		public float CurrentHealth;

		// Token: 0x0400439F RID: 17311
		public float DamageCaused;

		// Token: 0x040043A0 RID: 17312
		public float PreviousHealth;
	}
}
