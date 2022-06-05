using System;
using System.Collections.Generic;

// Token: 0x020007CF RID: 1999
public class RoomSpawnControllerManager
{
	// Token: 0x17001697 RID: 5783
	// (get) Token: 0x06003D97 RID: 15767 RVA: 0x0002219D File Offset: 0x0002039D
	public BaseRoom Room { get; }

	// Token: 0x17001698 RID: 5784
	// (get) Token: 0x06003D98 RID: 15768 RVA: 0x000221A5 File Offset: 0x000203A5
	// (set) Token: 0x06003D99 RID: 15769 RVA: 0x000221AD File Offset: 0x000203AD
	public ISpawnController[] SpawnControllers { get; private set; }

	// Token: 0x17001699 RID: 5785
	// (get) Token: 0x06003D9A RID: 15770 RVA: 0x000221B6 File Offset: 0x000203B6
	// (set) Token: 0x06003D9B RID: 15771 RVA: 0x000221BE File Offset: 0x000203BE
	public SpawnLogicController[] SpawnLogicControllers { get; private set; }

	// Token: 0x1700169A RID: 5786
	// (get) Token: 0x06003D9C RID: 15772 RVA: 0x000221C7 File Offset: 0x000203C7
	// (set) Token: 0x06003D9D RID: 15773 RVA: 0x000221CF File Offset: 0x000203CF
	public EnemySpawnController[] EnemySpawnControllers { get; private set; }

	// Token: 0x1700169B RID: 5787
	// (get) Token: 0x06003D9E RID: 15774 RVA: 0x000221D8 File Offset: 0x000203D8
	// (set) Token: 0x06003D9F RID: 15775 RVA: 0x000221E0 File Offset: 0x000203E0
	public ISimpleSpawnController[] SimpleSpawnControllers_NoProps { get; private set; }

	// Token: 0x1700169C RID: 5788
	// (get) Token: 0x06003DA0 RID: 15776 RVA: 0x000221E9 File Offset: 0x000203E9
	// (set) Token: 0x06003DA1 RID: 15777 RVA: 0x000221F1 File Offset: 0x000203F1
	public TunnelSpawnController[] TunnelSpawnControllers { get; private set; }

	// Token: 0x1700169D RID: 5789
	// (get) Token: 0x06003DA2 RID: 15778 RVA: 0x000221FA File Offset: 0x000203FA
	// (set) Token: 0x06003DA3 RID: 15779 RVA: 0x00022202 File Offset: 0x00020402
	public PropSpawnController[] PropSpawnControllers { get; private set; }

	// Token: 0x1700169E RID: 5790
	// (get) Token: 0x06003DA4 RID: 15780 RVA: 0x0002220B File Offset: 0x0002040B
	// (set) Token: 0x06003DA5 RID: 15781 RVA: 0x00022213 File Offset: 0x00020413
	public ChestSpawnController[] ChestSpawnControllers { get; private set; }

	// Token: 0x1700169F RID: 5791
	// (get) Token: 0x06003DA6 RID: 15782 RVA: 0x0002221C File Offset: 0x0002041C
	// (set) Token: 0x06003DA7 RID: 15783 RVA: 0x00022224 File Offset: 0x00020424
	public IHazardSpawnController[] HazardSpawnControllers { get; private set; }

	// Token: 0x06003DA8 RID: 15784 RVA: 0x0002222D File Offset: 0x0002042D
	public RoomSpawnControllerManager(BaseRoom room)
	{
		this.Room = room;
		this.InitializeProperties();
	}

	// Token: 0x06003DA9 RID: 15785 RVA: 0x000F972C File Offset: 0x000F792C
	private void InitializeProperties()
	{
		this.GenerateAllSpawnControllers();
		for (int i = 0; i < this.EnemySpawnControllers.Length; i++)
		{
			this.EnemySpawnControllers[i].SetEnemyIndex(i);
		}
		for (int j = 0; j < this.ChestSpawnControllers.Length; j++)
		{
			this.ChestSpawnControllers[j].SetChestIndex(j);
		}
	}

	// Token: 0x06003DAA RID: 15786 RVA: 0x000F9784 File Offset: 0x000F7984
	private void GenerateAllSpawnControllers()
	{
		RoomSpawnControllerManager.m_spawnControllerHelper_STATIC.Clear();
		RoomSpawnControllerManager.m_propSpawnerHelper_STATIC.Clear();
		RoomSpawnControllerManager.m_enemySpawnerHelper_STATIC.Clear();
		RoomSpawnControllerManager.m_noPropSimpleSpawnerHelper_STATIC.Clear();
		RoomSpawnControllerManager.m_chestSpawnerHelper_STATIC.Clear();
		RoomSpawnControllerManager.m_tunnelSpawnerHelper_STATIC.Clear();
		RoomSpawnControllerManager.m_hazardSpawnerHelper_STATIC.Clear();
		RoomSpawnControllerManager.m_spawnLogicHelper_STATIC.Clear();
		this.SpawnControllers = this.Room.gameObject.GetComponentsInChildren<ISpawnController>(true);
		foreach (ISpawnController spawnController in this.SpawnControllers)
		{
			if (spawnController.SpawnLogicController)
			{
				RoomSpawnControllerManager.m_spawnLogicHelper_STATIC.Add(spawnController.SpawnLogicController);
			}
			PropSpawnController propSpawnController = spawnController as PropSpawnController;
			if (propSpawnController)
			{
				RoomSpawnControllerManager.m_propSpawnerHelper_STATIC.Add(propSpawnController);
			}
			else
			{
				ISimpleSpawnController simpleSpawnController = spawnController as ISimpleSpawnController;
				if (simpleSpawnController != null)
				{
					RoomSpawnControllerManager.m_noPropSimpleSpawnerHelper_STATIC.Add(simpleSpawnController);
				}
				EnemySpawnController enemySpawnController = spawnController as EnemySpawnController;
				if (enemySpawnController)
				{
					RoomSpawnControllerManager.m_enemySpawnerHelper_STATIC.Add(enemySpawnController);
				}
				else
				{
					ChestSpawnController chestSpawnController = spawnController as ChestSpawnController;
					if (chestSpawnController)
					{
						RoomSpawnControllerManager.m_chestSpawnerHelper_STATIC.Add(chestSpawnController);
					}
					else
					{
						TunnelSpawnController tunnelSpawnController = spawnController as TunnelSpawnController;
						if (tunnelSpawnController)
						{
							RoomSpawnControllerManager.m_tunnelSpawnerHelper_STATIC.Add(tunnelSpawnController);
						}
						else
						{
							IHazardSpawnController hazardSpawnController = spawnController as IHazardSpawnController;
							if (hazardSpawnController != null)
							{
								RoomSpawnControllerManager.m_hazardSpawnerHelper_STATIC.Add(hazardSpawnController);
							}
						}
					}
				}
			}
		}
		this.PropSpawnControllers = RoomSpawnControllerManager.m_propSpawnerHelper_STATIC.ToArray();
		this.SimpleSpawnControllers_NoProps = RoomSpawnControllerManager.m_noPropSimpleSpawnerHelper_STATIC.ToArray();
		this.EnemySpawnControllers = RoomSpawnControllerManager.m_enemySpawnerHelper_STATIC.ToArray();
		this.TunnelSpawnControllers = RoomSpawnControllerManager.m_tunnelSpawnerHelper_STATIC.ToArray();
		this.ChestSpawnControllers = RoomSpawnControllerManager.m_chestSpawnerHelper_STATIC.ToArray();
		this.HazardSpawnControllers = RoomSpawnControllerManager.m_hazardSpawnerHelper_STATIC.ToArray();
		this.SpawnLogicControllers = RoomSpawnControllerManager.m_spawnLogicHelper_STATIC.ToArray();
		BaseRoom room = this.Room;
		PropSpawnController[] propSpawnControllers = this.PropSpawnControllers;
		for (int i = 0; i < propSpawnControllers.Length; i++)
		{
			propSpawnControllers[i].SetRoom(room);
		}
		EnemySpawnController[] enemySpawnControllers = this.EnemySpawnControllers;
		for (int i = 0; i < enemySpawnControllers.Length; i++)
		{
			enemySpawnControllers[i].SetRoom(room);
		}
		ChestSpawnController[] chestSpawnControllers = this.ChestSpawnControllers;
		for (int i = 0; i < chestSpawnControllers.Length; i++)
		{
			chestSpawnControllers[i].SetRoom(room);
		}
		TunnelSpawnController[] tunnelSpawnControllers = this.TunnelSpawnControllers;
		for (int i = 0; i < tunnelSpawnControllers.Length; i++)
		{
			tunnelSpawnControllers[i].SetRoom(room);
		}
		SpawnLogicController[] spawnLogicControllers = this.SpawnLogicControllers;
		for (int i = 0; i < spawnLogicControllers.Length; i++)
		{
			spawnLogicControllers[i].SetRoom(room);
		}
	}

	// Token: 0x04003089 RID: 12425
	private static List<ISpawnController> m_spawnControllerHelper_STATIC = new List<ISpawnController>();

	// Token: 0x0400308A RID: 12426
	private static List<PropSpawnController> m_propSpawnerHelper_STATIC = new List<PropSpawnController>();

	// Token: 0x0400308B RID: 12427
	private static List<EnemySpawnController> m_enemySpawnerHelper_STATIC = new List<EnemySpawnController>();

	// Token: 0x0400308C RID: 12428
	private static List<ISimpleSpawnController> m_noPropSimpleSpawnerHelper_STATIC = new List<ISimpleSpawnController>();

	// Token: 0x0400308D RID: 12429
	private static List<ChestSpawnController> m_chestSpawnerHelper_STATIC = new List<ChestSpawnController>();

	// Token: 0x0400308E RID: 12430
	private static List<TunnelSpawnController> m_tunnelSpawnerHelper_STATIC = new List<TunnelSpawnController>();

	// Token: 0x0400308F RID: 12431
	private static List<IHazardSpawnController> m_hazardSpawnerHelper_STATIC = new List<IHazardSpawnController>();

	// Token: 0x04003090 RID: 12432
	private static List<SpawnLogicController> m_spawnLogicHelper_STATIC = new List<SpawnLogicController>();
}
