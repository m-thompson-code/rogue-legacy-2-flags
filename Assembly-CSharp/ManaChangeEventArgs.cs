using System;

// Token: 0x020007B5 RID: 1973
public class ManaChangeEventArgs : EventArgs
{
	// Token: 0x0600425F RID: 16991 RVA: 0x000EBC18 File Offset: 0x000E9E18
	public ManaChangeEventArgs(PlayerController player, float newMana, float prevMana)
	{
		this.Initialise(player, newMana, prevMana);
	}

	// Token: 0x06004260 RID: 16992 RVA: 0x000EBC29 File Offset: 0x000E9E29
	public void Initialise(PlayerController player, float newMana, float prevMana)
	{
		this.NewManaValue = newMana;
		this.PrevManaValue = prevMana;
		this.Player = player;
	}

	// Token: 0x17001679 RID: 5753
	// (get) Token: 0x06004261 RID: 16993 RVA: 0x000EBC40 File Offset: 0x000E9E40
	// (set) Token: 0x06004262 RID: 16994 RVA: 0x000EBC48 File Offset: 0x000E9E48
	public PlayerController Player { get; private set; }

	// Token: 0x1700167A RID: 5754
	// (get) Token: 0x06004263 RID: 16995 RVA: 0x000EBC51 File Offset: 0x000E9E51
	// (set) Token: 0x06004264 RID: 16996 RVA: 0x000EBC59 File Offset: 0x000E9E59
	public float NewManaValue { get; private set; }

	// Token: 0x1700167B RID: 5755
	// (get) Token: 0x06004265 RID: 16997 RVA: 0x000EBC62 File Offset: 0x000E9E62
	// (set) Token: 0x06004266 RID: 16998 RVA: 0x000EBC6A File Offset: 0x000E9E6A
	public float PrevManaValue { get; private set; }
}
