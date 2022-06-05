using System;
using System.Collections;
using System.Collections.Generic;
using SceneManagement_RL;
using UnityEngine;

// Token: 0x020003F9 RID: 1017
public class ChallengeTunnelController : MonoBehaviour, IRoomConsumer
{
	// Token: 0x17000F34 RID: 3892
	// (get) Token: 0x060025C2 RID: 9666 RVA: 0x0007C94C File Offset: 0x0007AB4C
	// (set) Token: 0x060025C3 RID: 9667 RVA: 0x0007C954 File Offset: 0x0007AB54
	public BaseRoom Room { get; private set; }

	// Token: 0x060025C4 RID: 9668 RVA: 0x0007C95D File Offset: 0x0007AB5D
	private void Awake()
	{
		this.m_onEnterChallenge = new Action<MonoBehaviour, EventArgs>(this.OnEnterChallenge);
	}

	// Token: 0x060025C5 RID: 9669 RVA: 0x0007C971 File Offset: 0x0007AB71
	private void OnEnable()
	{
		Messenger<UIMessenger, UIEvent>.AddListener(UIEvent.ChallengeNPC_EnterChallenge, this.m_onEnterChallenge);
	}

	// Token: 0x060025C6 RID: 9670 RVA: 0x0007C980 File Offset: 0x0007AB80
	private void OnDisable()
	{
		Messenger<UIMessenger, UIEvent>.RemoveListener(UIEvent.ChallengeNPC_EnterChallenge, this.m_onEnterChallenge);
	}

	// Token: 0x060025C7 RID: 9671 RVA: 0x0007C98F File Offset: 0x0007AB8F
	public void SetRoom(BaseRoom room)
	{
		this.Room = room;
	}

	// Token: 0x060025C8 RID: 9672 RVA: 0x0007C998 File Offset: 0x0007AB98
	private void OnEnterChallenge(MonoBehaviour sender, EventArgs args)
	{
		ChallengeType challengeType = (args as ChallengeOmniUIDescriptionEventArgs).ChallengeType;
		this.EnterChallenge(challengeType, true);
	}

	// Token: 0x060025C9 RID: 9673 RVA: 0x0007C9BC File Offset: 0x0007ABBC
	private void EnterTunnel(TunnelSpawnController tunnelSpawnController, ChallengeType challengeType)
	{
		SaveManager.DisableSaving = true;
		List<Room> list;
		if (!GameUtility.IsInLevelEditor)
		{
			list = WorldBuilder.BiomeControllers[this.Room.BiomeType].RoomsConnectedByTunnel;
		}
		else
		{
			list = new List<Room>();
			foreach (BaseRoom baseRoom in OnPlayManager.RoomList)
			{
				list.Add(baseRoom as Room);
			}
		}
		foreach (Room room in list)
		{
			if (!(room == this.Room))
			{
				room.GridPointManager.SetRoomSeed(UnityEngine.Random.Range(0, int.MaxValue));
				RoomSaveData roomSaveData = SaveManager.StageSaveData.GetRoomSaveData(room.BiomeType, room.BiomeControllerIndex);
				if (!roomSaveData.IsNativeNull())
				{
					roomSaveData.Clear();
				}
			}
		}
		this.m_prevTrophyRank = ChallengeManager.GetChallengeTrophyRank(challengeType, true);
		ChallengeManager.ChallengeTunnelController = this;
		ChallengeManager.SetActiveChallenge(challengeType);
		ChallengeManager.SetupCharacter();
		PlayerManager.GetPlayerController().SetFacing(true);
		GC.Collect();
		tunnelSpawnController.Tunnel.ForceEnterTunnel(false, null);
	}

	// Token: 0x060025CA RID: 9674 RVA: 0x0007CB08 File Offset: 0x0007AD08
	public void EnterChallenge(ChallengeType challengeType, bool animateTransition)
	{
		if (challengeType == ChallengeType.Tutorial && ChallengeManager.GetTotalTrophiesEarned(true) >= ChallengeManager.GetTotalTrophyCount() - 1)
		{
			challengeType = ChallengeType.TutorialPurified;
		}
		TunnelSpawnController tunnelSpawnController;
		if (!this.m_tunnelSpawnControllers.TryGetValue(challengeType, out tunnelSpawnController))
		{
			throw new Exception("There is no entry for ChallengeType: " + challengeType.ToString() + " in ChallengeTunnelController.");
		}
		if (tunnelSpawnController.IsNativeNull() || tunnelSpawnController.Tunnel.IsNativeNull())
		{
			throw new Exception("There is no corresponding TunnelSpawnController for ChallengeType: " + challengeType.ToString() + " in ChallengeTunnelController.");
		}
		if (animateTransition)
		{
			SceneLoader_RL.RunTransitionWithLogic(delegate()
			{
				this.EnterTunnel(tunnelSpawnController, challengeType);
			}, TransitionID.ScreenDistortion, false);
			return;
		}
		this.EnterTunnel(tunnelSpawnController, challengeType);
	}

	// Token: 0x060025CB RID: 9675 RVA: 0x0007CBF8 File Offset: 0x0007ADF8
	public void ReturnToDriftHouse(bool broadcastExitEvent = true)
	{
		ChallengeType challengeType = ChallengeManager.ActiveChallenge.ChallengeType;
		if (!this.Room.IsNativeNull())
		{
			PlayerManager.GetCurrentPlayerRoom().PlayerExit(null);
			LocalTeleporterController.StopTeleportPlayer();
			this.Room.PlacePlayerInRoom(this.m_exitSpawnPoint.transform.localPosition);
		}
		else
		{
			LocalTeleporterController.StopTeleportPlayer();
			Debug.Log("<color=red>Cannot exit challenge. No entrance room found.</color>");
		}
		ChallengeManager.SetActiveChallenge(ChallengeType.None);
		ChallengeManager.RestoreCharacter(true);
		Messenger<UIMessenger, UIEvent>.Broadcast(UIEvent.StopGlobalTimer, null, null);
		Messenger<UIMessenger, UIEvent>.Broadcast(UIEvent.HideGlobalTimer, null, null);
		if (broadcastExitEvent)
		{
			Messenger<UIMessenger, UIEvent>.Broadcast(UIEvent.ChallengeNPC_ExitChallenge, null, null);
		}
		SaveManager.DisableSaving = false;
		if (ChallengeManager.NeedsSave)
		{
			SaveManager.SaveCurrentProfileGameData(SaveDataType.GameMode, SavingType.FileOnly, true, null);
			ChallengeManager.NeedsSave = false;
			ChallengeTrophyRank challengeTrophyRank = ChallengeManager.GetChallengeTrophyRank(challengeType, true);
			if (this.m_prevTrophyRank < challengeTrophyRank)
			{
				int num = Souls_EV.GetChallengeSoulsRewarded(challengeType, challengeTrophyRank) - Souls_EV.GetChallengeSoulsRewarded(challengeType, this.m_prevTrophyRank);
				if (num > 0)
				{
					base.StartCoroutine(this.SpawnSouls(num));
				}
			}
		}
	}

	// Token: 0x060025CC RID: 9676 RVA: 0x0007CCD7 File Offset: 0x0007AED7
	private IEnumerator SpawnSouls(int soulsGained)
	{
		SoulDrop.FakeSoulCounter_STATIC = soulsGained;
		Messenger<GameMessenger, GameEvent>.Broadcast(GameEvent.SoulChanged, null, null);
		float delay = Time.time + 0.25f;
		while (Time.time < delay)
		{
			yield return null;
		}
		Vector3 position = this.m_challengeNPCSpawner.PropInstance.Pivot.transform.position;
		position.y += 1f;
		ItemDropManager.DropItem(ItemDropType.Soul, soulsGained, position, true, true, false);
		yield break;
	}

	// Token: 0x04001FA0 RID: 8096
	[SerializeField]
	private GameObject m_exitSpawnPoint;

	// Token: 0x04001FA1 RID: 8097
	[SerializeField]
	private ChallengeTypeTunnelSpawnControllerDictionary m_tunnelSpawnControllers;

	// Token: 0x04001FA2 RID: 8098
	[SerializeField]
	private PropSpawnController m_challengeNPCSpawner;

	// Token: 0x04001FA3 RID: 8099
	private ChallengeTrophyRank m_prevTrophyRank;

	// Token: 0x04001FA4 RID: 8100
	private Action<MonoBehaviour, EventArgs> m_onEnterChallenge;
}
