using System;
using UnityEngine;

// Token: 0x02000A25 RID: 2597
public class DisableSentriesRoomController : MonoBehaviour, IRoomConsumer
{
	// Token: 0x17001B27 RID: 6951
	// (get) Token: 0x06004E88 RID: 20104 RVA: 0x0002ABE9 File Offset: 0x00028DE9
	// (set) Token: 0x06004E89 RID: 20105 RVA: 0x0002ABF1 File Offset: 0x00028DF1
	public BaseRoom Room { get; private set; }

	// Token: 0x06004E8A RID: 20106 RVA: 0x0002ABFA File Offset: 0x00028DFA
	public void SetRoom(BaseRoom room)
	{
		this.Room = room;
		this.Room.PlayerEnterRelay.AddListener(new Action<object, RoomViaDoorEventArgs>(this.OnPlayerEnter), false);
	}

	// Token: 0x06004E8B RID: 20107 RVA: 0x0002AC21 File Offset: 0x00028E21
	private void OnDestroy()
	{
		if (this.Room)
		{
			this.Room.PlayerEnterRelay.RemoveListener(new Action<object, RoomViaDoorEventArgs>(this.OnPlayerEnter));
		}
	}

	// Token: 0x06004E8C RID: 20108 RVA: 0x0012DD64 File Offset: 0x0012BF64
	private void OnPlayerEnter(object sender, EventArgs args)
	{
		ISpawnController[] spawnControllers = this.Room.SpawnControllerManager.SpawnControllers;
		for (int i = 0; i < spawnControllers.Length; i++)
		{
			IHazardSpawnController hazardSpawnController = spawnControllers[i] as IHazardSpawnController;
			if (!hazardSpawnController.IsNativeNull() && hazardSpawnController.Type == HazardType.Sentry)
			{
				hazardSpawnController.Hazard.gameObject.SetActive(false);
			}
		}
	}
}
