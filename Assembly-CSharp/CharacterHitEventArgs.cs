using System;

// Token: 0x020007E5 RID: 2021
public class CharacterHitEventArgs : EventArgs
{
	// Token: 0x0600436A RID: 17258 RVA: 0x000EC799 File Offset: 0x000EA999
	public CharacterHitEventArgs(IDamageObj attacker, BaseCharacterController victim, float damageTaken)
	{
		this.Initialize(attacker, victim, damageTaken);
	}

	// Token: 0x0600436B RID: 17259 RVA: 0x000EC7AA File Offset: 0x000EA9AA
	public void Initialize(IDamageObj attacker, BaseCharacterController victim, float damageTaken)
	{
		this.Attacker = attacker;
		this.Victim = victim;
		this.DamageTaken = damageTaken;
	}

	// Token: 0x170016D2 RID: 5842
	// (get) Token: 0x0600436C RID: 17260 RVA: 0x000EC7C1 File Offset: 0x000EA9C1
	// (set) Token: 0x0600436D RID: 17261 RVA: 0x000EC7C9 File Offset: 0x000EA9C9
	public IDamageObj Attacker { get; private set; }

	// Token: 0x170016D3 RID: 5843
	// (get) Token: 0x0600436E RID: 17262 RVA: 0x000EC7D2 File Offset: 0x000EA9D2
	// (set) Token: 0x0600436F RID: 17263 RVA: 0x000EC7DA File Offset: 0x000EA9DA
	public BaseCharacterController Victim { get; private set; }

	// Token: 0x170016D4 RID: 5844
	// (get) Token: 0x06004370 RID: 17264 RVA: 0x000EC7E3 File Offset: 0x000EA9E3
	// (set) Token: 0x06004371 RID: 17265 RVA: 0x000EC7EB File Offset: 0x000EA9EB
	public float DamageTaken { get; private set; }
}
