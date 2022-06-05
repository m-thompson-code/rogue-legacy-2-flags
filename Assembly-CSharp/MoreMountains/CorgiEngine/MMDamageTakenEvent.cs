using System;
using UnityEngine;

namespace MoreMountains.CorgiEngine
{
	// Token: 0x02000F19 RID: 3865
	public struct MMDamageTakenEvent
	{
		// Token: 0x06006F96 RID: 28566 RVA: 0x0003D87C File Offset: 0x0003BA7C
		public MMDamageTakenEvent(Character affectedCharacter, GameObject instigator, float currentHealth, float damageCaused, float previousHealth)
		{
			this.AffectedCharacter = affectedCharacter;
			this.Instigator = instigator;
			this.CurrentHealth = currentHealth;
			this.DamageCaused = damageCaused;
			this.PreviousHealth = previousHealth;
		}

		// Token: 0x040059B9 RID: 22969
		public Character AffectedCharacter;

		// Token: 0x040059BA RID: 22970
		public GameObject Instigator;

		// Token: 0x040059BB RID: 22971
		public float CurrentHealth;

		// Token: 0x040059BC RID: 22972
		public float DamageCaused;

		// Token: 0x040059BD RID: 22973
		public float PreviousHealth;
	}
}
