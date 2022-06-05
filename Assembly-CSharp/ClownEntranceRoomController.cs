using System;
using System.Collections;
using RL_Windows;
using UnityEngine;

// Token: 0x02000854 RID: 2132
public class ClownEntranceRoomController : BaseSpecialRoomController
{
	// Token: 0x060041BC RID: 16828 RVA: 0x0002467E File Offset: 0x0002287E
	public ChestObj GetChest(ClownGoalType goalType)
	{
		switch (goalType)
		{
		case ClownGoalType.Bronze:
			return this.m_bronzeChest;
		case ClownGoalType.Silver:
			return this.m_silverChest;
		case ClownGoalType.Gold:
			return this.m_goldChest;
		default:
			return null;
		}
	}

	// Token: 0x060041BD RID: 16829 RVA: 0x000246AC File Offset: 0x000228AC
	private IEnumerator Start()
	{
		while (base.Room == null)
		{
			yield return null;
		}
		ObjectReferenceFinder finder = base.Room.gameObject.GetComponent<ObjectReferenceFinder>();
		GameObject @object = finder.GetObject("ClownRoomTunnel");
		TunnelSpawnController tunnelSpawner = @object.GetComponent<TunnelSpawnController>();
		while (tunnelSpawner.Tunnel == null)
		{
			yield return null;
		}
		this.m_tunnel = tunnelSpawner.Tunnel;
		GameObject object2 = finder.GetObject("BronzeChest");
		ChestSpawnController bronzeChestSpawner = object2.GetComponent<ChestSpawnController>();
		GameObject object3 = finder.GetObject("SilverChest");
		ChestSpawnController silverChestSpawner = object3.GetComponent<ChestSpawnController>();
		GameObject object4 = finder.GetObject("GoldChest");
		ChestSpawnController goldChestSpawner = object4.GetComponent<ChestSpawnController>();
		while (bronzeChestSpawner.ChestInstance == null || silverChestSpawner.ChestInstance == null || goldChestSpawner.ChestInstance == null)
		{
			yield return null;
		}
		this.m_bronzeChest = bronzeChestSpawner.ChestInstance;
		this.m_silverChest = silverChestSpawner.ChestInstance;
		this.m_goldChest = goldChestSpawner.ChestInstance;
		while (!this.m_bronzeChest.IsInitialised || !this.m_silverChest.IsInitialised || !this.m_goldChest.IsInitialised)
		{
			yield return null;
		}
		this.m_bronzeChest.gameObject.SetActive(false);
		this.m_silverChest.gameObject.SetActive(false);
		this.m_goldChest.gameObject.SetActive(false);
		this.m_tunnel.SetIsLocked(true);
		yield break;
	}

	// Token: 0x060041BE RID: 16830 RVA: 0x001087E0 File Offset: 0x001069E0
	public void UnlockChestReward(ClownGoalType goalType)
	{
		switch (goalType)
		{
		case ClownGoalType.Bronze:
			this.GetChest(ClownGoalType.Bronze).gameObject.SetActive(true);
			break;
		case ClownGoalType.Silver:
			this.GetChest(ClownGoalType.Bronze).gameObject.SetActive(true);
			this.GetChest(ClownGoalType.Silver).gameObject.SetActive(true);
			break;
		case ClownGoalType.Gold:
			this.GetChest(ClownGoalType.Bronze).gameObject.SetActive(true);
			this.GetChest(ClownGoalType.Silver).gameObject.SetActive(true);
			this.GetChest(ClownGoalType.Gold).gameObject.SetActive(true);
			break;
		}
		this.m_tunnel.SetIsLocked(true);
	}

	// Token: 0x060041BF RID: 16831 RVA: 0x00108880 File Offset: 0x00106A80
	public void TriggerDialogue(GameObject obj)
	{
		this.m_tunnel.SetIsLocked(false);
		DialogueManager.StartNewDialogue(null, NPCState.Idle);
		DialogueManager.AddNonLocDialogue("Clown", "Clown challenge time!", false, DialogueWindowStyle.HorizontalUpper, DialoguePortraitType.None, NPCState.None, 0.015f);
		DialogueManager.AddNonLocDialogue("Clown", "The goal is to destroy ALL targets.  You are only allowed to attack 20 times.", false, DialogueWindowStyle.HorizontalUpper, DialoguePortraitType.None, NPCState.None, 0.015f);
		DialogueManager.AddNonLocDialogue("Clown", "Don't suck.", false, DialogueWindowStyle.HorizontalUpper, DialoguePortraitType.None, NPCState.None, 0.015f);
		WindowManager.SetWindowIsOpen(WindowID.Dialogue, true);
	}

	// Token: 0x04003370 RID: 13168
	private ChestObj m_bronzeChest;

	// Token: 0x04003371 RID: 13169
	private ChestObj m_silverChest;

	// Token: 0x04003372 RID: 13170
	private ChestObj m_goldChest;

	// Token: 0x04003373 RID: 13171
	private Tunnel m_tunnel;
}
