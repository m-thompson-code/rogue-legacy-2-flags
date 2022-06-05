using System;
using System.Collections.Generic;

// Token: 0x020004B7 RID: 1207
public class RoomSpawnControllerManager
{
	// Token: 0x17001130 RID: 4400
	// (get) Token: 0x06002CEB RID: 11499 RVA: 0x00098776 File Offset: 0x00096976
	public BaseRoom Room { get; }

	// Token: 0x17001131 RID: 4401
	// (get) Token: 0x06002CEC RID: 11500 RVA: 0x0009877E File Offset: 0x0009697E
	// (set) Token: 0x06002CED RID: 11501 RVA: 0x00098786 File Offset: 0x00096986
	public ISpawnController[] SpawnControllers { get; private set; }

	// Token: 0x17001132 RID: 4402
	// (get) Token: 0x06002CEE RID: 11502 RVA: 0x0009878F File Offset: 0x0009698F
	// (set) Token: 0x06002CEF RID: 11503 RVA: 0x00098797 File Offset: 0x00096997
	public SpawnLogicController[] SpawnLogicControllers { get; private set; }

	// Token: 0x17001133 RID: 4403
	// (get) Token: 0x06002CF0 RID: 11504 RVA: 0x000987A0 File Offset: 0x000969A0
	// (set) Token: 0x06002CF1 RID: 11505 RVA: 0x000987A8 File Offset: 0x000969A8
	public EnemySpawnController[] EnemySpawnControllers { get; private set; }

	// Token: 0x17001134 RID: 4404
	// (get) Token: 0x06002CF2 RID: 11506 RVA: 0x000987B1 File Offset: 0x000969B1
	// (set) Token: 0x06002CF3 RID: 11507 RVA: 0x000987B9 File Offset: 0x000969B9
	public ISimpleSpawnController[] SimpleSpawnControllers_NoProps { get; private set; }

	// Token: 0x17001135 RID: 4405
	// (get) Token: 0x06002CF4 RID: 11508 RVA: 0x000987C2 File Offset: 0x000969C2
	// (set) Token: 0x06002CF5 RID: 11509 RVA: 0x000987CA File Offset: 0x000969CA
	public TunnelSpawnController[] TunnelSpawnControllers { get; private set; }

	// Token: 0x17001136 RID: 4406
	// (get) Token: 0x06002CF6 RID: 11510 RVA: 0x000987D3 File Offset: 0x000969D3
	// (set) Token: 0x06002CF7 RID: 11511 RVA: 0x000987DB File Offset: 0x000969DB
	public PropSpawnController[] PropSpawnControllers { get; private set; }

	// Token: 0x17001137 RID: 4407
	// (get) Token: 0x06002CF8 RID: 11512 RVA: 0x000987E4 File Offset: 0x000969E4
	// (set) Token: 0x06002CF9 RID: 11513 RVA: 0x000987EC File Offset: 0x000969EC
	public ChestSpawnController[] ChestSpawnControllers { get; private set; }

	// Token: 0x17001138 RID: 4408
	// (get) Token: 0x06002CFA RID: 11514 RVA: 0x000987F5 File Offset: 0x000969F5
	// (set) Token: 0x06002CFB RID: 11515 RVA: 0x000987FD File Offset: 0x000969FD
	public IHazardSpawnController[] HazardSpawnControllers { get; private set; }

	// Token: 0x06002CFC RID: 11516 RVA: 0x00098806 File Offset: 0x00096A06
	public RoomSpawnControllerManager(BaseRoom room)
	{
		this.Room = room;
		this.InitializeProperties();
	}

	// Token: 0x06002CFD RID: 11517 RVA: 0x0009881C File Offset: 0x00096A1C
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

	// Token: 0x06002CFE RID: 11518 RVA: 0x00098874 File Offset: 0x00096A74
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

	// Token: 0x04002426 RID: 9254
	private static List<ISpawnController> m_spawnControllerHelper_STATIC = new List<ISpawnController>();

	// Token: 0x04002427 RID: 9255
	private static List<PropSpawnController> m_propSpawnerHelper_STATIC = new List<PropSpawnController>();

	// Token: 0x04002428 RID: 9256
	private static List<EnemySpawnController> m_enemySpawnerHelper_STATIC = new List<EnemySpawnController>();

	// Token: 0x04002429 RID: 9257
	private static List<ISimpleSpawnController> m_noPropSimpleSpawnerHelper_STATIC = new List<ISimpleSpawnController>();

	// Token: 0x0400242A RID: 9258
	private static List<ChestSpawnController> m_chestSpawnerHelper_STATIC = new List<ChestSpawnController>();

	// Token: 0x0400242B RID: 9259
	private static List<TunnelSpawnController> m_tunnelSpawnerHelper_STATIC = new List<TunnelSpawnController>();

	// Token: 0x0400242C RID: 9260
	private static List<IHazardSpawnController> m_hazardSpawnerHelper_STATIC = new List<IHazardSpawnController>();

	// Token: 0x0400242D RID: 9261
	private static List<SpawnLogicController> m_spawnLogicHelper_STATIC = new List<SpawnLogicController>();
}
