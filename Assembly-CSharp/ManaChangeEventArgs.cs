using System;

// Token: 0x02000C7B RID: 3195
public class ManaChangeEventArgs : EventArgs
{
	// Token: 0x06005BE8 RID: 23528 RVA: 0x0003262E File Offset: 0x0003082E
	public ManaChangeEventArgs(PlayerController player, float newMana, float prevMana)
	{
		this.Initialise(player, newMana, prevMana);
	}

	// Token: 0x06005BE9 RID: 23529 RVA: 0x0003263F File Offset: 0x0003083F
	public void Initialise(PlayerController player, float newMana, float prevMana)
	{
		this.NewManaValue = newMana;
		this.PrevManaValue = prevMana;
		this.Player = player;
	}

	// Token: 0x17001E77 RID: 7799
	// (get) Token: 0x06005BEA RID: 23530 RVA: 0x00032656 File Offset: 0x00030856
	// (set) Token: 0x06005BEB RID: 23531 RVA: 0x0003265E File Offset: 0x0003085E
	public PlayerController Player { get; private set; }

	// Token: 0x17001E78 RID: 7800
	// (get) Token: 0x06005BEC RID: 23532 RVA: 0x00032667 File Offset: 0x00030867
	// (set) Token: 0x06005BED RID: 23533 RVA: 0x0003266F File Offset: 0x0003086F
	public float NewManaValue { get; private set; }

	// Token: 0x17001E79 RID: 7801
	// (get) Token: 0x06005BEE RID: 23534 RVA: 0x00032678 File Offset: 0x00030878
	// (set) Token: 0x06005BEF RID: 23535 RVA: 0x00032680 File Offset: 0x00030880
	public float PrevManaValue { get; private set; }
}
