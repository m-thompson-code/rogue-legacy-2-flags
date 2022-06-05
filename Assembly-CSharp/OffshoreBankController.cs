using System;
using FMODUnity;
using RLAudio;
using RL_Windows;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x02000436 RID: 1078
public class OffshoreBankController : BaseSpecialPropController, IDisplaySpeechBubble, IAudioEventEmitter
{
	// Token: 0x17000F1F RID: 3871
	// (get) Token: 0x060022A1 RID: 8865 RVA: 0x000047A7 File Offset: 0x000029A7
	public SpeechBubbleType BubbleType
	{
		get
		{
			return SpeechBubbleType.Dialogue;
		}
	}

	// Token: 0x17000F20 RID: 3872
	// (get) Token: 0x060022A2 RID: 8866 RVA: 0x0001286A File Offset: 0x00010A6A
	public bool ShouldDisplaySpeechBubble
	{
		get
		{
			return this.HasEventDialogue();
		}
	}

	// Token: 0x17000F21 RID: 3873
	// (get) Token: 0x060022A3 RID: 8867 RVA: 0x00009A7B File Offset: 0x00007C7B
	public string Description
	{
		get
		{
			return this.ToString();
		}
	}

	// Token: 0x060022A4 RID: 8868 RVA: 0x00012872 File Offset: 0x00010A72
	private bool HasEventDialogue()
	{
		if (SaveManager.PlayerSaveData.EndingSpawnRoom == EndingSpawnRoomType.AboveGround && !SaveManager.PlayerSaveData.GetFlag(PlayerSaveFlag.SeenParade))
		{
			return !this.m_endingSpeechBubblePlayed;
		}
		return !SaveManager.PlayerSaveData.GetFlag(PlayerSaveFlag.OffshoreBankDialogue_Intro);
	}

	// Token: 0x060022A5 RID: 8869 RVA: 0x000AB32C File Offset: 0x000A952C
	protected override void Awake()
	{
		base.Awake();
		this.m_storedScale = this.m_goldSavedText.transform.localScale;
		this.m_goldSavedText.text = "";
		this.m_npcController = base.GetComponent<NPCController>();
		this.m_onGoldSavedChanged = new Action<MonoBehaviour, EventArgs>(this.OnGoldSavedChanged);
		this.m_closeBank = new Action(this.CloseBank);
	}

	// Token: 0x060022A6 RID: 8870 RVA: 0x000AB398 File Offset: 0x000A9598
	protected override void InitializePooledPropOnEnter()
	{
		this.m_endingSpeechBubblePlayed = false;
		this.m_goldSavedText.text = "[Coin_Icon] " + SaveManager.PlayerSaveData.GoldSaved.ToString();
		EndingSpawnRoomTypeController component = base.Room.GetComponent<EndingSpawnRoomTypeController>();
		if (!component || component.EndingSpawnRoomType != EndingSpawnRoomType.AboveGround)
		{
			this.m_goldSavedText.gameObject.SetLayerRecursively(25, false);
			this.m_interactable.SpeechBubble.gameObject.SetLayerRecursively(25, false);
			if (!this.m_goldSavedText.gameObject.activeSelf)
			{
				this.m_goldSavedText.gameObject.SetActive(true);
			}
		}
		else if (!SaveManager.PlayerSaveData.GetFlag(PlayerSaveFlag.SeenParade))
		{
			if (this.m_goldSavedText.gameObject.activeSelf)
			{
				this.m_goldSavedText.gameObject.SetActive(false);
			}
		}
		else if (!this.m_goldSavedText.gameObject.activeSelf)
		{
			this.m_goldSavedText.gameObject.SetActive(true);
		}
		Vector3 localScale = this.m_goldSavedText.transform.localScale;
		if (this.m_goldSavedText.transform.lossyScale.x < 0f)
		{
			localScale.x *= -1f;
			this.m_goldSavedText.transform.localScale = localScale;
		}
	}

	// Token: 0x060022A7 RID: 8871 RVA: 0x000AB4F4 File Offset: 0x000A96F4
	private void OnEnable()
	{
		Messenger<GameMessenger, GameEvent>.AddListener(GameEvent.GoldSavedChanged, this.m_onGoldSavedChanged);
		if (SaveManager.PlayerSaveData.GetFlag(PlayerSaveFlag.PizzaGirlUnlocked))
		{
			if (UnityEngine.Random.Range(0f, 1f) < 0.5f)
			{
				this.m_pizzaSprite.SetActive(true);
			}
			else
			{
				this.m_pizzaSprite.SetActive(false);
			}
			this.m_pizzaSprite.layer = this.m_pizzaSprite.transform.parent.gameObject.layer;
		}
		if (HolidayLookController.IsHoliday(HolidayType.Christmas))
		{
			if (this.m_sprite.activeSelf)
			{
				this.m_sprite.SetActive(false);
			}
			if (!this.m_christmasSprite.activeSelf)
			{
				this.m_christmasSprite.SetActive(true);
				return;
			}
		}
		else
		{
			if (!this.m_sprite.activeSelf)
			{
				this.m_sprite.SetActive(true);
			}
			if (this.m_christmasSprite.activeSelf)
			{
				this.m_christmasSprite.SetActive(false);
			}
		}
	}

	// Token: 0x060022A8 RID: 8872 RVA: 0x000128AF File Offset: 0x00010AAF
	protected override void OnDisable()
	{
		base.OnDisable();
		Messenger<GameMessenger, GameEvent>.RemoveListener(GameEvent.GoldSavedChanged, this.m_onGoldSavedChanged);
	}

	// Token: 0x060022A9 RID: 8873 RVA: 0x000AB5E4 File Offset: 0x000A97E4
	private void OnGoldSavedChanged(object sender, EventArgs args)
	{
		CharonShop charonShop = sender as CharonShop;
		if (charonShop != null)
		{
			(EffectManager.PlayEffect(charonShop.gameObject, null, "GoldBurst_Effect", Vector3.zero, 0f, EffectStopType.Gracefully, EffectTriggerDirection.None) as BurstEffect).DestinationOverride = base.transform;
			if (args != null)
			{
				GoldChangedEventArgs goldChangedEventArgs = args as GoldChangedEventArgs;
				this.m_goldSavedDiff = goldChangedEventArgs.NewGoldAmount - goldChangedEventArgs.PreviousGoldAmount;
			}
			else
			{
				this.m_goldSavedDiff = 0;
			}
			base.Invoke("UpdateTextAnim", 0.5f);
			return;
		}
		this.m_goldSavedText.text = "[Coin_Icon] " + SaveManager.PlayerSaveData.GoldSaved.ToString();
	}

	// Token: 0x060022AA RID: 8874 RVA: 0x000AB68C File Offset: 0x000A988C
	public void UpdateTextAnim()
	{
		Vector3 localScale = this.m_storedScale + new Vector3(0.15f, 0.15f, 0.15f);
		this.m_goldSavedText.transform.localScale = localScale;
		TweenManager.TweenTo(this.m_goldSavedText.transform, 0.25f, new EaseDelegate(Ease.Back.EaseOut), new object[]
		{
			"localScale.x",
			this.m_storedScale.x,
			"localScale.y",
			this.m_storedScale.y,
			"localScale.z",
			this.m_storedScale.z
		});
		this.m_goldSavedText.text = "[Coin_Icon] " + SaveManager.PlayerSaveData.GoldSaved.ToString();
		TextPopupManager.DisplayTextAtAbsPos(TextPopupType.GoldCollected, this.m_goldSavedDiff.ToString(), new Vector2(0f, -1f), this.m_goldSavedText.gameObject, TextAlignmentOptions.Center);
		if (this.m_goldSavedUnityEvent != null)
		{
			this.m_goldSavedUnityEvent.Invoke();
		}
	}

	// Token: 0x060022AB RID: 8875 RVA: 0x000AB7AC File Offset: 0x000A99AC
	public void DisplayDialogue()
	{
		AudioManager.PlayOneShotAttached(this, this.m_greetingAudioEvent, base.gameObject);
		this.m_interactable.SetIsInteractableActive(false);
		if (SaveManager.PlayerSaveData.EndingSpawnRoom == EndingSpawnRoomType.AboveGround && !SaveManager.PlayerSaveData.GetFlag(PlayerSaveFlag.SeenParade))
		{
			this.RunEndingDialogue();
			return;
		}
		if (!SaveManager.PlayerSaveData.GetFlag(PlayerSaveFlag.OffshoreBankDialogue_Intro))
		{
			SaveManager.PlayerSaveData.SetFlag(PlayerSaveFlag.OffshoreBankDialogue_Intro, true);
			DialogueManager.StartNewDialogue(this.m_npcController, NPCState.Idle);
			DialogueManager.AddDialogue("LOC_ID_SAFE_TITLE_NAME_1", "LOC_ID_SAFE_DIALOGUE_INTRO_1", SaveManager.PlayerSaveData.CurrentCharacter.IsFemale, DialogueWindowStyle.HorizontalUpper, DialoguePortraitType.None, NPCState.None, 0.015f);
			WindowManager.SetWindowIsOpen(WindowID.Dialogue, true);
			DialogueManager.AddDialogueCompleteEndHandler(this.m_closeBank);
			return;
		}
		if (NPCDialogueManager.CanSpeak(this.m_npcController.NPCType))
		{
			this.m_npcController.RunNextNPCDialogue(this.m_closeBank);
			return;
		}
		this.DisplayGoldStats();
	}

	// Token: 0x060022AC RID: 8876 RVA: 0x000AB890 File Offset: 0x000A9A90
	private void DisplayGoldStats()
	{
		int goldSaved = SaveManager.PlayerSaveData.GoldSaved;
		int num = 1000 + (int)SkillTreeManager.GetSkillTreeObj(SkillTreeType.Gold_Saved_Cap_Up).CurrentStatGain;
		float num2 = 0.1f + SkillTreeManager.GetSkillTreeObj(SkillTreeType.Gold_Saved_Amount_Saved).CurrentStatGain;
		int goldGivenToCharon = SaveManager.PlayerSaveData.GoldGivenToCharon;
		string @string = LocalizationManager.GetString("LOC_ID_SAFE_TITLE_NAME_1", false, false);
		string text;
		if (SkillTreeManager.GetSkillObjLevel(SkillTreeType.Charon_Gold_Stat_Bonus) == 0)
		{
			text = LocalizationManager.GetString("LOC_ID_SAFE_DIALOGUE_STATS_1", SaveManager.PlayerSaveData.CurrentCharacter.IsFemale, true);
			text = string.Format(text, new object[]
			{
				goldSaved,
				num,
				Mathf.RoundToInt(num2 * 100f),
				goldGivenToCharon,
				""
			});
		}
		else
		{
			int charonGoldStatBonus = SkillTreeLogicHelper.GetCharonGoldStatBonus();
			int charonGoldRuneWeightBonus = SkillTreeLogicHelper.GetCharonGoldRuneWeightBonus();
			int charonGoldMilestoneCount = SkillTreeLogicHelper.GetCharonGoldMilestoneCount();
			int num3;
			if (charonGoldMilestoneCount < SkillTree_EV.GetMaxCharonBonusLevel())
			{
				num3 = SkillTree_EV.CHARON_GOLD_STAT_BONUS_MILESTONES[charonGoldMilestoneCount] - SaveManager.PlayerSaveData.GoldAcceptedByCharon;
			}
			else
			{
				num3 = int.MaxValue;
			}
			if (num3 != 2147483647)
			{
				text = LocalizationManager.GetString("LOC_ID_SAFE_DIALOGUE_STATS_2", false, true);
			}
			else
			{
				text = LocalizationManager.GetString("LOC_ID_SAFE_DIALOGUE_CHARON_DONATION_MAXED_1", false, true);
			}
			text = string.Format(text, new object[]
			{
				goldSaved,
				num,
				Mathf.RoundToInt(num2 * 100f),
				goldGivenToCharon,
				"",
				charonGoldStatBonus,
				num3.ToString(),
				charonGoldMilestoneCount,
				charonGoldRuneWeightBonus
			});
		}
		DialogueManager.StartNewDialogue(this.m_npcController, NPCState.Idle);
		DialogueManager.AddNonLocDialogue(@string, text, false, DialogueWindowStyle.HorizontalUpper, DialoguePortraitType.None, NPCState.None, 0.015f);
		WindowManager.SetWindowIsOpen(WindowID.Dialogue, true);
		DialogueManager.AddDialogueCompleteEndHandler(this.m_closeBank);
	}

	// Token: 0x060022AD RID: 8877 RVA: 0x000ABA6C File Offset: 0x000A9C6C
	private void RunEndingDialogue()
	{
		string textLocID;
		if (!this.m_endingSpeechBubblePlayed)
		{
			this.m_endingSpeechBubblePlayed = true;
			if (!this.m_npcController.IsBestFriend)
			{
				textLocID = "LOC_ID_LIVING_SAFE_OUTRO_STRANGER_INTRO_1";
			}
			else
			{
				textLocID = "LOC_ID_LIVING_SAFE_OUTRO_FRIENDS_INTRO_1";
			}
		}
		else if (!this.m_npcController.IsBestFriend)
		{
			textLocID = "LOC_ID_LIVING_SAFE_OUTRO_STRANGER_REPEAT_1";
		}
		else
		{
			textLocID = "LOC_ID_LIVING_SAFE_OUTRO_FRIENDS_REPEAT_1";
		}
		DialogueManager.StartNewDialogue(this.m_npcController, NPCState.Idle);
		DialogueManager.AddDialogue(NPCDialogue_EV.GetNPCTitleLocID(this.m_npcController.NPCType), textLocID, SaveManager.PlayerSaveData.CurrentCharacter.IsFemale, DialogueWindowStyle.HorizontalUpper, DialoguePortraitType.None, NPCState.None, 0.015f);
		WindowManager.SetWindowIsOpen(WindowID.Dialogue, true);
		DialogueManager.AddDialogueCompleteEndHandler(this.m_closeBank);
	}

	// Token: 0x060022AE RID: 8878 RVA: 0x000128C4 File Offset: 0x00010AC4
	private void CloseBank()
	{
		AudioManager.PlayOneShotAttached(this, this.m_farewellAudioEvent, base.gameObject);
		this.m_interactable.SetIsInteractableActive(true);
	}

	// Token: 0x04001F3E RID: 7998
	[SerializeField]
	private TMP_Text m_goldSavedText;

	// Token: 0x04001F3F RID: 7999
	[SerializeField]
	private GameObject m_sprite;

	// Token: 0x04001F40 RID: 8000
	[SerializeField]
	private GameObject m_pizzaSprite;

	// Token: 0x04001F41 RID: 8001
	[SerializeField]
	private GameObject m_christmasSprite;

	// Token: 0x04001F42 RID: 8002
	[SerializeField]
	private UnityEvent m_goldSavedUnityEvent;

	// Token: 0x04001F43 RID: 8003
	[SerializeField]
	[EventRef]
	private string m_greetingAudioEvent;

	// Token: 0x04001F44 RID: 8004
	[SerializeField]
	[EventRef]
	private string m_farewellAudioEvent;

	// Token: 0x04001F45 RID: 8005
	private Vector3 m_storedScale;

	// Token: 0x04001F46 RID: 8006
	private int m_goldSavedDiff;

	// Token: 0x04001F47 RID: 8007
	private NPCController m_npcController;

	// Token: 0x04001F48 RID: 8008
	private Action<MonoBehaviour, EventArgs> m_onGoldSavedChanged;

	// Token: 0x04001F49 RID: 8009
	private Action m_closeBank;

	// Token: 0x04001F4A RID: 8010
	private bool m_endingSpeechBubblePlayed;
}
