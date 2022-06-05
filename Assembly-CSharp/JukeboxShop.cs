using System;
using RL_Windows;
using UnityEngine.Events;

// Token: 0x02000911 RID: 2321
public class JukeboxShop : BaseShop, IRoomConsumer, IDisplaySpeechBubble
{
	// Token: 0x170018EA RID: 6378
	// (get) Token: 0x0600467C RID: 18044 RVA: 0x00026B83 File Offset: 0x00024D83
	// (set) Token: 0x0600467D RID: 18045 RVA: 0x00026B8B File Offset: 0x00024D8B
	public BaseRoom Room { get; private set; }

	// Token: 0x170018EB RID: 6379
	// (get) Token: 0x0600467E RID: 18046 RVA: 0x0011431C File Offset: 0x0011251C
	public override bool ShouldDisplaySpeechBubble
	{
		get
		{
			foreach (SongID songID in SongType_RL.TypeArray)
			{
				if (songID != SongID.None && Jukebox_EV.JukeboxDataDict.ContainsKey(songID) && SaveManager.PlayerSaveData.GetSongFoundState(songID) == FoundState.FoundButNotViewed)
				{
					return true;
				}
			}
			return false;
		}
	}

	// Token: 0x170018EC RID: 6380
	// (get) Token: 0x0600467F RID: 18047 RVA: 0x000047A4 File Offset: 0x000029A4
	public override SpeechBubbleType BubbleType
	{
		get
		{
			return SpeechBubbleType.GearAvailable;
		}
	}

	// Token: 0x06004680 RID: 18048 RVA: 0x00026B94 File Offset: 0x00024D94
	protected override void Awake()
	{
		base.Awake();
		this.m_closeShop = new UnityAction(this.CloseShop);
	}

	// Token: 0x06004681 RID: 18049 RVA: 0x00026BAF File Offset: 0x00024DAF
	public void SetRoom(BaseRoom room)
	{
		this.Room = room;
		this.Room.PlayerEnterRelay.AddListener(new Action<object, RoomViaDoorEventArgs>(this.OnPlayerEnterRoom), false);
	}

	// Token: 0x170018ED RID: 6381
	// (get) Token: 0x06004682 RID: 18050 RVA: 0x00009A7B File Offset: 0x00007C7B
	public string Description
	{
		get
		{
			return this.ToString();
		}
	}

	// Token: 0x06004683 RID: 18051 RVA: 0x00003CD2 File Offset: 0x00001ED2
	protected override bool HasEventDialogue()
	{
		return false;
	}

	// Token: 0x06004684 RID: 18052 RVA: 0x00026BD6 File Offset: 0x00024DD6
	private void OnDestroy()
	{
		if (this.Room)
		{
			this.Room.PlayerEnterRelay.RemoveListener(new Action<object, RoomViaDoorEventArgs>(this.OnPlayerEnterRoom));
		}
	}

	// Token: 0x06004685 RID: 18053 RVA: 0x00114364 File Offset: 0x00112564
	private void OnPlayerEnterRoom(object sender, EventArgs args)
	{
		SoulShopObj soulShopObj = SaveManager.ModeSaveData.GetSoulShopObj(SoulShopType.UnlockJukebox);
		if (!soulShopObj.IsNativeNull())
		{
			if (soulShopObj.CurrentOwnedLevel <= 0)
			{
				base.gameObject.SetActive(false);
				return;
			}
			PropSpawnController propSpawnController = this.Room.gameObject.FindObjectReference("PropBehindJukebox", false, false);
			if (propSpawnController && propSpawnController.PropInstance)
			{
				propSpawnController.PropInstance.gameObject.SetActive(false);
			}
		}
	}

	// Token: 0x06004686 RID: 18054 RVA: 0x001143E0 File Offset: 0x001125E0
	protected override void OpenShop_Internal()
	{
		if (!WindowManager.GetIsWindowLoaded(WindowID.Jukebox))
		{
			WindowManager.LoadWindow(WindowID.Jukebox);
		}
		WindowManager.GetWindowController(WindowID.Jukebox).WindowClosedEvent.AddListener(this.m_closeShop);
		if (JukeboxShop.CanSubmitStoreAchievements)
		{
			Achievement_EV.RunAchievementSafetyChecks();
			StoreAPIManager.GiveAllUnlockedAchievements();
			JukeboxShop.CanSubmitStoreAchievements = false;
		}
		WindowManager.SetWindowIsOpen(WindowID.Jukebox, true);
	}

	// Token: 0x06004687 RID: 18055 RVA: 0x00026C02 File Offset: 0x00024E02
	public override void CloseShop()
	{
		base.CloseShop();
		WindowManager.GetWindowController(WindowID.Jukebox).WindowClosedEvent.RemoveListener(this.m_closeShop);
	}

	// Token: 0x04003661 RID: 13921
	private UnityAction m_closeShop;

	// Token: 0x04003662 RID: 13922
	public static bool CanSubmitStoreAchievements = true;
}
