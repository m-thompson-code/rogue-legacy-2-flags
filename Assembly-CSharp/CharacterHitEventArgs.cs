using System;

// Token: 0x02000CAB RID: 3243
public class CharacterHitEventArgs : EventArgs
{
	// Token: 0x06005CF3 RID: 23795 RVA: 0x000331AF File Offset: 0x000313AF
	public CharacterHitEventArgs(IDamageObj attacker, BaseCharacterController victim, float damageTaken)
	{
		this.Initialize(attacker, victim, damageTaken);
	}

	// Token: 0x06005CF4 RID: 23796 RVA: 0x000331C0 File Offset: 0x000313C0
	public void Initialize(IDamageObj attacker, BaseCharacterController victim, float damageTaken)
	{
		this.Attacker = attacker;
		this.Victim = victim;
		this.DamageTaken = damageTaken;
	}

	// Token: 0x17001ED0 RID: 7888
	// (get) Token: 0x06005CF5 RID: 23797 RVA: 0x000331D7 File Offset: 0x000313D7
	// (set) Token: 0x06005CF6 RID: 23798 RVA: 0x000331DF File Offset: 0x000313DF
	public IDamageObj Attacker { get; private set; }

	// Token: 0x17001ED1 RID: 7889
	// (get) Token: 0x06005CF7 RID: 23799 RVA: 0x000331E8 File Offset: 0x000313E8
	// (set) Token: 0x06005CF8 RID: 23800 RVA: 0x000331F0 File Offset: 0x000313F0
	public BaseCharacterController Victim { get; private set; }

	// Token: 0x17001ED2 RID: 7890
	// (get) Token: 0x06005CF9 RID: 23801 RVA: 0x000331F9 File Offset: 0x000313F9
	// (set) Token: 0x06005CFA RID: 23802 RVA: 0x00033201 File Offset: 0x00031401
	public float DamageTaken { get; private set; }
}
