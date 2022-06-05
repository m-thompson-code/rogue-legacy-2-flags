using System;
using FMODUnity;
using RL_Windows;
using SceneManagement_RL;
using Sigtrap.Relays;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x02000819 RID: 2073
public class JournalSpecialPropController : BaseSpecialPropController, IDisplaySpeechBubble
{
	// Token: 0x1700172A RID: 5930
	// (get) Token: 0x06003FE3 RID: 16355 RVA: 0x00023493 File Offset: 0x00021693
	// (set) Token: 0x06003FE4 RID: 16356 RVA: 0x0002349B File Offset: 0x0002169B
	public string TitleOverrideLocID { get; private set; }

	// Token: 0x1700172B RID: 5931
	// (get) Token: 0x06003FE5 RID: 16357 RVA: 0x000234A4 File Offset: 0x000216A4
	// (set) Token: 0x06003FE6 RID: 16358 RVA: 0x000234AC File Offset: 0x000216AC
	public string EntryOverrideLocID { get; private set; }

	// Token: 0x1700172C RID: 5932
	// (get) Token: 0x06003FE7 RID: 16359 RVA: 0x000234B5 File Offset: 0x000216B5
	// (set) Token: 0x06003FE8 RID: 16360 RVA: 0x000234BD File Offset: 0x000216BD
	public JournalCategoryType JournalCategoryTypeOverride { get; private set; }

	// Token: 0x1700172D RID: 5933
	// (get) Token: 0x06003FE9 RID: 16361 RVA: 0x000234C6 File Offset: 0x000216C6
	// (set) Token: 0x06003FEA RID: 16362 RVA: 0x000234CE File Offset: 0x000216CE
	public JournalType JournalType { get; private set; }

	// Token: 0x1700172E RID: 5934
	// (get) Token: 0x06003FEB RID: 16363 RVA: 0x000234D7 File Offset: 0x000216D7
	// (set) Token: 0x06003FEC RID: 16364 RVA: 0x000234DF File Offset: 0x000216DF
	public DialogueWindowStyle DialogueWindowStyle { get; private set; }

	// Token: 0x1700172F RID: 5935
	// (get) Token: 0x06003FED RID: 16365 RVA: 0x000234E8 File Offset: 0x000216E8
	// (set) Token: 0x06003FEE RID: 16366 RVA: 0x000234F0 File Offset: 0x000216F0
	public DialoguePortraitType PortraitType { get; private set; }

	// Token: 0x17001730 RID: 5936
	// (get) Token: 0x06003FEF RID: 16367 RVA: 0x000234F9 File Offset: 0x000216F9
	public IRelayLink FinishedReadingRelay
	{
		get
		{
			return this.m_finishedReadingRelay.link;
		}
	}

	// Token: 0x17001731 RID: 5937
	// (get) Token: 0x06003FF0 RID: 16368 RVA: 0x000FFBF8 File Offset: 0x000FDDF8
	public bool ShouldDisplaySpeechBubble
	{
		get
		{
			if (base.IsPropComplete || !string.IsNullOrEmpty(this.EntryOverrideLocID) || !string.IsNullOrEmpty(this.TitleOverrideLocID))
			{
				return false;
			}
			BiomeType biomeType = PlayerManager.GetCurrentPlayerRoom().BiomeType;
			JournalCategoryType journalCategoryType = JournalType_RL.ConvertBiomeToJournalCategoryType(biomeType);
			if (journalCategoryType == JournalCategoryType.None)
			{
				return false;
			}
			if (this.JournalType == JournalType.MemoryFragment && SaveManager.PlayerSaveData.GetHeirloomLevel(HeirloomType.UnlockMemory) < 1)
			{
				return false;
			}
			int numJournals = Journal_EV.GetNumJournals(biomeType, this.JournalType);
			return SaveManager.PlayerSaveData.GetJournalsRead(journalCategoryType, this.JournalType) < numJournals && (this.JournalCategoryTypeOverride != JournalCategoryType.Study || SaveManager.PlayerSaveData.GetJournalsRead(JournalCategoryType.Study, JournalType.Journal) > 0);
		}
	}

	// Token: 0x17001732 RID: 5938
	// (get) Token: 0x06003FF1 RID: 16369 RVA: 0x00004A8D File Offset: 0x00002C8D
	public SpeechBubbleType BubbleType
	{
		get
		{
			return SpeechBubbleType.PointOfInterest;
		}
	}

	// Token: 0x06003FF2 RID: 16370 RVA: 0x00023506 File Offset: 0x00021706
	protected override void Awake()
	{
		base.Awake();
		this.m_onJournalWindowClosed = new UnityAction(this.OnJournalWindowClosed);
		this.m_onTransitionStart = new Action(this.OnTransitionStart);
	}

	// Token: 0x06003FF3 RID: 16371 RVA: 0x000FFCA0 File Offset: 0x000FDEA0
	protected override void InitializePooledPropOnEnter()
	{
		Prop component = base.GetComponent<Prop>();
		if (component && component.PropSpawnController)
		{
			JournalEntryOverride component2 = component.PropSpawnController.gameObject.GetComponent<JournalEntryOverride>();
			if (component2)
			{
				this.TitleOverrideLocID = component2.TitleLocIDOverride;
				this.EntryOverrideLocID = component2.EntryLocIDOverride;
				this.JournalType = component2.JournalType;
				this.JournalCategoryTypeOverride = component2.JournalCategoryTypeOverride;
				this.DialogueWindowStyle = component2.DialogueWindowStyle;
				this.PortraitType = component2.PortraitType;
				if (this.JournalType == JournalType.ScarMemory)
				{
					ChallengeType challengeType;
					if (Challenge_EV.ScarUnlockTable.TryGetValue(this.EntryOverrideLocID, out challengeType))
					{
						bool flag = ChallengeManager.GetFoundState(challengeType) != FoundState.NotFound;
						bool flag2 = Challenge_EV.ScarBossRequirementTable.ContainsKey(challengeType) && SaveManager.PlayerSaveData.GetFlag(Challenge_EV.ScarBossRequirementTable[challengeType]);
						if (flag || !flag2)
						{
							base.gameObject.SetActive(false);
							return;
						}
					}
					else
					{
						Debug.Log("<color=red>Could not spawn ScarMemory because the requested Loc ID is not in the Challenge_EV.ScarUnlockTable: " + this.EntryOverrideLocID + "</color>");
						base.gameObject.SetActive(false);
					}
				}
			}
		}
	}

	// Token: 0x06003FF4 RID: 16372 RVA: 0x000FFDBC File Offset: 0x000FDFBC
	public void TriggerJournal()
	{
		if (base.IsPropComplete && string.IsNullOrEmpty(this.EntryOverrideLocID) && string.IsNullOrEmpty(this.TitleOverrideLocID))
		{
			WindowManager.SetWindowIsOpen(WindowID.Journal, true);
			this.m_openWindow = WindowID.Journal;
		}
		else if (this.JournalType == JournalType.MemoryFragment && SaveManager.PlayerSaveData.GetHeirloomLevel(HeirloomType.UnlockMemory) <= 0)
		{
			DialogueManager.StartNewDialogue(null, NPCState.Idle);
			DialogueManager.AddDialogue("LOC_ID_HEIRLOOM_TITLE_MEMORY_UNREADABLE_1", "LOC_ID_HEIRLOOM_TEXT_MEMORY_UNREADABLE_1", SaveManager.PlayerSaveData.CurrentCharacter.IsFemale, this.DialogueWindowStyle, DialoguePortraitType.None, NPCState.None, 0.015f);
			WindowManager.SetWindowIsOpen(WindowID.Dialogue, true);
			this.m_openWindow = WindowID.Dialogue;
		}
		else if (string.IsNullOrEmpty(this.EntryOverrideLocID) || string.IsNullOrEmpty(this.TitleOverrideLocID))
		{
			BiomeType biomeType = PlayerManager.GetCurrentPlayerRoom().BiomeType;
			biomeType = BiomeType_RL.GetGroupedBiomeType(biomeType);
			JournalCategoryType journalCategoryType = JournalType_RL.ConvertBiomeToJournalCategoryType(biomeType);
			if (this.JournalCategoryTypeOverride != JournalCategoryType.None)
			{
				journalCategoryType = this.JournalCategoryTypeOverride;
			}
			if (journalCategoryType == JournalCategoryType.None)
			{
				WindowManager.SetWindowIsOpen(WindowID.Journal, true);
				this.m_openWindow = WindowID.Journal;
			}
			else
			{
				int num;
				if (this.m_journalIndexOverride > -1)
				{
					num = this.m_journalIndexOverride;
				}
				else
				{
					num = SaveManager.PlayerSaveData.GetJournalsRead(journalCategoryType, this.JournalType);
					num++;
					SaveManager.PlayerSaveData.SetJournalsRead(journalCategoryType, this.JournalType, 1, true, false);
					if (!SaveManager.PlayerSaveData.GetFlag(PlayerSaveFlag.JournalReadOnce))
					{
						SaveManager.PlayerSaveData.SetFlag(PlayerSaveFlag.JournalReadOnce, true);
					}
				}
				if (num > 0 && num <= Journal_EV.GetNumJournals(journalCategoryType, this.JournalType))
				{
					JournalEntry journalEntry = Journal_EV.GetJournalEntry(journalCategoryType, this.JournalType, num - 1);
					if (!journalEntry.IsEmpty)
					{
						DialogueManager.StartNewDialogue(null, NPCState.Idle);
						DialogueManager.AddDialogue(journalEntry.TitleLocID, journalEntry.TextLocID, SaveManager.PlayerSaveData.CurrentCharacter.IsFemale, this.DialogueWindowStyle, this.PortraitType, NPCState.None, 0.015f);
						WindowManager.SetWindowIsOpen(WindowID.Dialogue, true);
						this.m_openWindow = WindowID.Dialogue;
					}
					else
					{
						Debug.Log(string.Concat(new string[]
						{
							"<color=red>Could not display journal entry: ",
							(num - 1).ToString(),
							" in biome: ",
							biomeType.ToString(),
							". Entry does not exist.</color>"
						}));
					}
				}
				else if (num > Journal_EV.GetNumJournals(journalCategoryType, this.JournalType))
				{
					WindowManager.SetWindowIsOpen(WindowID.Journal, true);
					this.m_openWindow = WindowID.Journal;
				}
			}
			this.PropComplete();
		}
		else
		{
			if (this.EntryOverrideLocID == "LOC_ID_JOURNAL_DESCRIPTION_STUDY_BOSS_NEW_DIMENSIONS_4")
			{
				RuntimeManager.StudioSystem.setParameterByName("mus_fmf_memoryActive", 1f, false);
			}
			DialogueManager.StartNewDialogue(null, NPCState.Idle);
			DialogueManager.AddDialogue(this.TitleOverrideLocID, this.EntryOverrideLocID, SaveManager.PlayerSaveData.CurrentCharacter.IsFemale, this.DialogueWindowStyle, this.PortraitType, NPCState.None, 0.015f);
			WindowManager.SetWindowIsOpen(WindowID.Dialogue, true);
			this.m_openWindow = WindowID.Dialogue;
		}
		if (this.m_openWindow != WindowID.None)
		{
			WindowController windowController = WindowManager.GetWindowController(this.m_openWindow);
			if (windowController != null)
			{
				if (this.m_journalWindowOpenedUnityEvent != null)
				{
					this.m_journalWindowOpenedUnityEvent.Invoke();
				}
				windowController.WindowClosedEvent.AddListener(this.m_onJournalWindowClosed);
			}
		}
	}

	// Token: 0x06003FF5 RID: 16373 RVA: 0x001000B8 File Offset: 0x000FE2B8
	private void OnJournalWindowClosed()
	{
		WindowManager.GetWindowController(this.m_openWindow).WindowClosedEvent.RemoveListener(this.m_onJournalWindowClosed);
		if (this.m_journalWindowClosedUnityEvent != null)
		{
			this.m_journalWindowClosedUnityEvent.Invoke();
		}
		if (this.JournalType == JournalType.ScarMemory)
		{
			base.gameObject.SetActive(false);
		}
		if (this.m_openWindow == WindowID.Dialogue)
		{
			this.HandleAchievements();
		}
		this.m_finishedReadingRelay.Dispatch();
	}

	// Token: 0x06003FF6 RID: 16374 RVA: 0x00100124 File Offset: 0x000FE324
	private void HandleAchievements()
	{
		if (this.JournalType == JournalType.MemoryFragment && SaveManager.PlayerSaveData.GetHeirloomLevel(HeirloomType.UnlockMemory) <= 0)
		{
			return;
		}
		BiomeType biomeType = PlayerManager.GetCurrentPlayerRoom().BiomeType;
		biomeType = BiomeType_RL.GetGroupedBiomeType(biomeType);
		if (biomeType == BiomeType.Tutorial && this.EntryOverrideLocID == "LOC_ID_JOURNAL_DESCRIPTION_TUTORIAL_HIDDEN_MESSAGE_1")
		{
			StoreAPIManager.GiveAchievement(AchievementType.StoryTutorial, StoreType.All);
			return;
		}
		JournalCategoryType journalCategoryType = JournalType_RL.ConvertBiomeToJournalCategoryType(biomeType);
		if (this.JournalCategoryTypeOverride != JournalCategoryType.None)
		{
			journalCategoryType = this.JournalCategoryTypeOverride;
		}
		if (journalCategoryType == JournalCategoryType.None)
		{
			return;
		}
		int journalsRead = SaveManager.PlayerSaveData.GetJournalsRead(journalCategoryType, JournalType.Journal);
		int journalsRead2 = SaveManager.PlayerSaveData.GetJournalsRead(journalCategoryType, JournalType.MemoryFragment);
		if (journalsRead == Journal_EV.GetNumJournals(journalCategoryType, JournalType.Journal) && journalsRead2 == Journal_EV.GetNumJournals(journalCategoryType, JournalType.MemoryFragment))
		{
			if (journalCategoryType <= JournalCategoryType.Study)
			{
				if (journalCategoryType == JournalCategoryType.Castle)
				{
					StoreAPIManager.GiveAchievement(AchievementType.StoryJournalsCastle, StoreType.All);
					return;
				}
				if (journalCategoryType == JournalCategoryType.Forest)
				{
					StoreAPIManager.GiveAchievement(AchievementType.StoryJournalsForest, StoreType.All);
					return;
				}
				if (journalCategoryType != JournalCategoryType.Study)
				{
					return;
				}
				StoreAPIManager.GiveAchievement(AchievementType.StoryJournalsStudy, StoreType.All);
				return;
			}
			else
			{
				if (journalCategoryType == JournalCategoryType.Bridge)
				{
					StoreAPIManager.GiveAchievement(AchievementType.StoryJournalsBridge, StoreType.All);
					return;
				}
				if (journalCategoryType == JournalCategoryType.Tower)
				{
					StoreAPIManager.GiveAchievement(AchievementType.StoryJournalsTower, StoreType.All);
					return;
				}
				if (journalCategoryType != JournalCategoryType.Cave)
				{
					return;
				}
				StoreAPIManager.GiveAchievement(AchievementType.StoryJournalsCave, StoreType.All);
			}
		}
	}

	// Token: 0x06003FF7 RID: 16375 RVA: 0x00002FCA File Offset: 0x000011CA
	protected override void DisableProp(bool firstTimeDisabled)
	{
	}

	// Token: 0x06003FF8 RID: 16376 RVA: 0x00023532 File Offset: 0x00021732
	private void OnEnable()
	{
		SceneLoader_RL.TransitionStartRelay.AddListener(this.m_onTransitionStart, false);
	}

	// Token: 0x06003FF9 RID: 16377 RVA: 0x00100234 File Offset: 0x000FE434
	protected override void OnDisable()
	{
		base.OnDisable();
		this.FinishedReadingRelay.RemoveAll(true, true);
		if (this.EntryOverrideLocID == "LOC_ID_JOURNAL_DESCRIPTION_STUDY_BOSS_NEW_DIMENSIONS_4")
		{
			RuntimeManager.StudioSystem.setParameterByName("mus_fmf_memoryActive", 0f, false);
		}
		if (base.Room && base.Room.BiomeType != BiomeType.HubTown)
		{
			this.EntryOverrideLocID = null;
			this.TitleOverrideLocID = null;
		}
		SceneLoader_RL.TransitionStartRelay.RemoveListener(this.m_onTransitionStart);
	}

	// Token: 0x06003FFA RID: 16378 RVA: 0x001002BC File Offset: 0x000FE4BC
	private void OnTransitionStart()
	{
		StudioEventEmitter component = base.GetComponent<StudioEventEmitter>();
		if (component)
		{
			component.Stop();
		}
	}

	// Token: 0x040031F2 RID: 12786
	[SerializeField]
	private int m_journalIndexOverride = -1;

	// Token: 0x040031F3 RID: 12787
	[SerializeField]
	private UnityEvent m_journalWindowOpenedUnityEvent;

	// Token: 0x040031F4 RID: 12788
	[SerializeField]
	private UnityEvent m_journalWindowClosedUnityEvent;

	// Token: 0x040031F5 RID: 12789
	private WindowID m_openWindow;

	// Token: 0x040031FC RID: 12796
	private Relay m_finishedReadingRelay = new Relay();

	// Token: 0x040031FD RID: 12797
	private Action m_onTransitionStart;

	// Token: 0x040031FE RID: 12798
	private UnityAction m_onJournalWindowClosed;
}
