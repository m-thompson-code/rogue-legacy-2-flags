using System;

// Token: 0x02000C9C RID: 3228
public class PlayerCardOpenedEventArgs : EventArgs
{
	// Token: 0x06005C9D RID: 23709 RVA: 0x00032E01 File Offset: 0x00031001
	public PlayerCardOpenedEventArgs(PlayerSaveData playerData)
	{
		this.Initialize(playerData);
	}

	// Token: 0x06005C9E RID: 23710 RVA: 0x00032E10 File Offset: 0x00031010
	public void Initialize(PlayerSaveData playerData)
	{
		this.PlayerData = playerData;
	}

	// Token: 0x17001EB4 RID: 7860
	// (get) Token: 0x06005C9F RID: 23711 RVA: 0x00032E19 File Offset: 0x00031019
	// (set) Token: 0x06005CA0 RID: 23712 RVA: 0x00032E21 File Offset: 0x00031021
	public PlayerSaveData PlayerData { get; private set; }
}
