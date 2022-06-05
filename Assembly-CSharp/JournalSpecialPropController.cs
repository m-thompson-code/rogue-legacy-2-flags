using System;
using FMODUnity;
using RL_Windows;
using SceneManagement_RL;
using Sigtrap.Relays;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x020004DA RID: 1242
public class JournalSpecialPropController : BaseSpecialPropController, IDisplaySpeechBubble
{
	// Token: 0x17001177 RID: 4471
	// (get) Token: 0x06002E53 RID: 11859 RVA: 0x0009C9AC File Offset: 0x0009ABAC
	// (set) Token: 0x06002E54 RID: 11860 RVA: 0x0009C9B4 File Offset: 0x0009ABB4
	public string TitleOverrideLocID { get; private set; }

	// Token: 0x17001178 RID: 4472
	// (get) Token: 0x06002E55 RID: 11861 RVA: 0x0009C9BD File Offset: 0x0009ABBD
	// (set) Token: 0x06002E56 RID: 11862 RVA: 0x0009C9C5 File Offset: 0x0009ABC5
	public string EntryOverrideLocID { get; private set; }

	// Token: 0x17001179 RID: 4473
	// (get) Token: 0x06002E57 RID: 11863 RVA: 0x0009C9CE File Offset: 0x0009ABCE
	// (set) Token: 0x06002E58 RID: 11864 RVA: 0x0009C9D6 File Offset: 0x0009ABD6
	public JournalCategoryType JournalCategoryTypeOverride { get; private set; }

	// Token: 0x1700117A RID: 4474
	// (get) Token: 0x06002E59 RID: 11865 RVA: 0x0009C9DF File Offset: 0x0009ABDF
	// (set) Token: 0x06002E5A RID: 11866 RVA: 0x0009C9E7 File Offset: 0x0009ABE7
	public JournalType JournalType { get; private set; }

	// Token: 0x1700117B RID: 4475
	// (get) Token: 0x06002E5B RID: 11867 RVA: 0x0009C9F0 File Offset: 0x0009ABF0
	// (set) Token: 0x06002E5C RID: 11868 RVA: 0x0009C9F8 File Offset: 0x0009ABF8
	public DialogueWindowStyle DialogueWindowStyle { get; private set; }

	// Token: 0x1700117C RID: 4476
	// (get) Token: 0x06002E5D RID: 11869 RVA: 0x0009CA01 File Offset: 0x0009AC01
	// (set) Token: 0x06002E5E RID: 11870 RVA: 0x0009CA09 File Offset: 0x0009AC09
	public DialoguePortraitType PortraitType { get; private set; }

	// Token: 0x1700117D RID: 4477
	// (get) Token: 0x06002E5F RID: 11871 RVA: 0x0009CA12 File Offset: 0x0009AC12
	public IRelayLink FinishedReadingRelay
	{
		get
		{
			return this.m_finishedReadingRelay.link;
		}
	}

	// Token: 0x1700117E RID: 4478
	// (get) Token: 0x06002E60 RID: 11872 RVA: 0x0009CA20 File Offset: 0x0009AC20
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

	// Token: 0x1700117F RID: 4479
	// (get) Token: 0x06002E61 RID: 11873 RVA: 0x0009CAC5 File Offset: 0x0009ACC5
	public SpeechBubbleType BubbleType
	{
		get
		{
			return SpeechBubbleType.PointOfInterest;
		}
	}

	// Token: 0x06002E62 RID: 11874 RVA: 0x0009CAC8 File Offset: 0x0009ACC8
	protected override void Awake()
	{
		base.Awake();
		this.m_onJournalWindowClosed = new UnityAction(this.OnJournalWindowClosed);
		this.m_onTransitionStart = new Action(this.OnTransitionStart);
	}

	// Token: 0x06002E63 RID: 11875 RVA: 0x0009CAF4 File Offset: 0x0009ACF4
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

	// Token: 0x06002E64 RID: 11876 RVA: 0x0009CC10 File Offset: 0x0009AE10
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

	// Token: 0x06002E65 RID: 11877 RVA: 0x0009CF0C File Offset: 0x0009B10C
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

	// Token: 0x06002E66 RID: 11878 RVA: 0x0009CF78 File Offset: 0x0009B178
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

	// Token: 0x06002E67 RID: 11879 RVA: 0x0009D086 File Offset: 0x0009B286
	protected override void DisableProp(bool firstTimeDisabled)
	{
	}

	// Token: 0x06002E68 RID: 11880 RVA: 0x0009D088 File Offset: 0x0009B288
	private void OnEnable()
	{
		SceneLoader_RL.TransitionStartRelay.AddListener(this.m_onTransitionStart, false);
	}

	// Token: 0x06002E69 RID: 11881 RVA: 0x0009D09C File Offset: 0x0009B29C
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

	// Token: 0x06002E6A RID: 11882 RVA: 0x0009D124 File Offset: 0x0009B324
	private void OnTransitionStart()
	{
		StudioEventEmitter component = base.GetComponent<StudioEventEmitter>();
		if (component)
		{
			component.Stop();
		}
	}

	// Token: 0x040024F2 RID: 9458
	[SerializeField]
	private int m_journalIndexOverride = -1;

	// Token: 0x040024F3 RID: 9459
	[SerializeField]
	private UnityEvent m_journalWindowOpenedUnityEvent;

	// Token: 0x040024F4 RID: 9460
	[SerializeField]
	private UnityEvent m_journalWindowClosedUnityEvent;

	// Token: 0x040024F5 RID: 9461
	private WindowID m_openWindow;

	// Token: 0x040024FC RID: 9468
	private Relay m_finishedReadingRelay = new Relay();

	// Token: 0x040024FD RID: 9469
	private Action m_onTransitionStart;

	// Token: 0x040024FE RID: 9470
	private UnityAction m_onJournalWindowClosed;
}
