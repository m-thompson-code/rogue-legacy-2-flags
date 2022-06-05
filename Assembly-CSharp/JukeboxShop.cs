using System;
using RL_Windows;
using UnityEngine.Events;

// Token: 0x02000556 RID: 1366
public class JukeboxShop : BaseShop, IRoomConsumer, IDisplaySpeechBubble
{
	// Token: 0x1700124F RID: 4687
	// (get) Token: 0x06003225 RID: 12837 RVA: 0x000AA231 File Offset: 0x000A8431
	// (set) Token: 0x06003226 RID: 12838 RVA: 0x000AA239 File Offset: 0x000A8439
	public BaseRoom Room { get; private set; }

	// Token: 0x17001250 RID: 4688
	// (get) Token: 0x06003227 RID: 12839 RVA: 0x000AA244 File Offset: 0x000A8444
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

	// Token: 0x17001251 RID: 4689
	// (get) Token: 0x06003228 RID: 12840 RVA: 0x000AA28B File Offset: 0x000A848B
	public override SpeechBubbleType BubbleType
	{
		get
		{
			return SpeechBubbleType.GearAvailable;
		}
	}

	// Token: 0x06003229 RID: 12841 RVA: 0x000AA28E File Offset: 0x000A848E
	protected override void Awake()
	{
		base.Awake();
		this.m_closeShop = new UnityAction(this.CloseShop);
	}

	// Token: 0x0600322A RID: 12842 RVA: 0x000AA2A9 File Offset: 0x000A84A9
	public void SetRoom(BaseRoom room)
	{
		this.Room = room;
		this.Room.PlayerEnterRelay.AddListener(new Action<object, RoomViaDoorEventArgs>(this.OnPlayerEnterRoom), false);
	}

	// Token: 0x17001252 RID: 4690
	// (get) Token: 0x0600322B RID: 12843 RVA: 0x000AA2D0 File Offset: 0x000A84D0
	public string Description
	{
		get
		{
			return this.ToString();
		}
	}

	// Token: 0x0600322C RID: 12844 RVA: 0x000AA2D8 File Offset: 0x000A84D8
	protected override bool HasEventDialogue()
	{
		return false;
	}

	// Token: 0x0600322D RID: 12845 RVA: 0x000AA2DB File Offset: 0x000A84DB
	private void OnDestroy()
	{
		if (this.Room)
		{
			this.Room.PlayerEnterRelay.RemoveListener(new Action<object, RoomViaDoorEventArgs>(this.OnPlayerEnterRoom));
		}
	}

	// Token: 0x0600322E RID: 12846 RVA: 0x000AA308 File Offset: 0x000A8508
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

	// Token: 0x0600322F RID: 12847 RVA: 0x000AA384 File Offset: 0x000A8584
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

	// Token: 0x06003230 RID: 12848 RVA: 0x000AA3D7 File Offset: 0x000A85D7
	public override void CloseShop()
	{
		base.CloseShop();
		WindowManager.GetWindowController(WindowID.Jukebox).WindowClosedEvent.RemoveListener(this.m_closeShop);
	}

	// Token: 0x04002777 RID: 10103
	private UnityAction m_closeShop;

	// Token: 0x04002778 RID: 10104
	public static bool CanSubmitStoreAchievements = true;
}
