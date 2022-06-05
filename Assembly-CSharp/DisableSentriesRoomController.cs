using System;
using UnityEngine;

// Token: 0x02000604 RID: 1540
public class DisableSentriesRoomController : MonoBehaviour, IRoomConsumer
{
	// Token: 0x170013D0 RID: 5072
	// (get) Token: 0x060037EB RID: 14315 RVA: 0x000BF4AA File Offset: 0x000BD6AA
	// (set) Token: 0x060037EC RID: 14316 RVA: 0x000BF4B2 File Offset: 0x000BD6B2
	public BaseRoom Room { get; private set; }

	// Token: 0x060037ED RID: 14317 RVA: 0x000BF4BB File Offset: 0x000BD6BB
	public void SetRoom(BaseRoom room)
	{
		this.Room = room;
		this.Room.PlayerEnterRelay.AddListener(new Action<object, RoomViaDoorEventArgs>(this.OnPlayerEnter), false);
	}

	// Token: 0x060037EE RID: 14318 RVA: 0x000BF4E2 File Offset: 0x000BD6E2
	private void OnDestroy()
	{
		if (this.Room)
		{
			this.Room.PlayerEnterRelay.RemoveListener(new Action<object, RoomViaDoorEventArgs>(this.OnPlayerEnter));
		}
	}

	// Token: 0x060037EF RID: 14319 RVA: 0x000BF510 File Offset: 0x000BD710
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
