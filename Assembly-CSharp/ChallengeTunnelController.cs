using System;
using System.Collections;
using System.Collections.Generic;
using SceneManagement_RL;
using UnityEngine;

// Token: 0x020006A2 RID: 1698
public class ChallengeTunnelController : MonoBehaviour, IRoomConsumer
{
	// Token: 0x170013E3 RID: 5091
	// (get) Token: 0x06003430 RID: 13360 RVA: 0x0001CA94 File Offset: 0x0001AC94
	// (set) Token: 0x06003431 RID: 13361 RVA: 0x0001CA9C File Offset: 0x0001AC9C
	public BaseRoom Room { get; private set; }

	// Token: 0x06003432 RID: 13362 RVA: 0x0001CAA5 File Offset: 0x0001ACA5
	private void Awake()
	{
		this.m_onEnterChallenge = new Action<MonoBehaviour, EventArgs>(this.OnEnterChallenge);
	}

	// Token: 0x06003433 RID: 13363 RVA: 0x0001CAB9 File Offset: 0x0001ACB9
	private void OnEnable()
	{
		Messenger<UIMessenger, UIEvent>.AddListener(UIEvent.ChallengeNPC_EnterChallenge, this.m_onEnterChallenge);
	}

	// Token: 0x06003434 RID: 13364 RVA: 0x0001CAC8 File Offset: 0x0001ACC8
	private void OnDisable()
	{
		Messenger<UIMessenger, UIEvent>.RemoveListener(UIEvent.ChallengeNPC_EnterChallenge, this.m_onEnterChallenge);
	}

	// Token: 0x06003435 RID: 13365 RVA: 0x0001CAD7 File Offset: 0x0001ACD7
	public void SetRoom(BaseRoom room)
	{
		this.Room = room;
	}

	// Token: 0x06003436 RID: 13366 RVA: 0x000DC470 File Offset: 0x000DA670
	private void OnEnterChallenge(MonoBehaviour sender, EventArgs args)
	{
		ChallengeType challengeType = (args as ChallengeOmniUIDescriptionEventArgs).ChallengeType;
		this.EnterChallenge(challengeType, true);
	}

	// Token: 0x06003437 RID: 13367 RVA: 0x000DC494 File Offset: 0x000DA694
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

	// Token: 0x06003438 RID: 13368 RVA: 0x000DC5E0 File Offset: 0x000DA7E0
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

	// Token: 0x06003439 RID: 13369 RVA: 0x000DC6D0 File Offset: 0x000DA8D0
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

	// Token: 0x0600343A RID: 13370 RVA: 0x0001CAE0 File Offset: 0x0001ACE0
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

	// Token: 0x04002A41 RID: 10817
	[SerializeField]
	private GameObject m_exitSpawnPoint;

	// Token: 0x04002A42 RID: 10818
	[SerializeField]
	private ChallengeTypeTunnelSpawnControllerDictionary m_tunnelSpawnControllers;

	// Token: 0x04002A43 RID: 10819
	[SerializeField]
	private PropSpawnController m_challengeNPCSpawner;

	// Token: 0x04002A44 RID: 10820
	private ChallengeTrophyRank m_prevTrophyRank;

	// Token: 0x04002A45 RID: 10821
	private Action<MonoBehaviour, EventArgs> m_onEnterChallenge;
}
