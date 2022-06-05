using System;

// Token: 0x020007D6 RID: 2006
public class PlayerCardOpenedEventArgs : EventArgs
{
	// Token: 0x06004314 RID: 17172 RVA: 0x000EC3EB File Offset: 0x000EA5EB
	public PlayerCardOpenedEventArgs(PlayerSaveData playerData)
	{
		this.Initialize(playerData);
	}

	// Token: 0x06004315 RID: 17173 RVA: 0x000EC3FA File Offset: 0x000EA5FA
	public void Initialize(PlayerSaveData playerData)
	{
		this.PlayerData = playerData;
	}

	// Token: 0x170016B6 RID: 5814
	// (get) Token: 0x06004316 RID: 17174 RVA: 0x000EC403 File Offset: 0x000EA603
	// (set) Token: 0x06004317 RID: 17175 RVA: 0x000EC40B File Offset: 0x000EA60B
	public PlayerSaveData PlayerData { get; private set; }
}
