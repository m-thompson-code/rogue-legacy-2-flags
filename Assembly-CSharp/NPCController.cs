using System;
using System.Collections;
using FMODUnity;
using RLAudio;
using RL_Windows;
using UnityEngine;

// Token: 0x0200026A RID: 618
public class NPCController : MonoBehaviour, IDisplaySpeechBubble, IAudioEventEmitter
{
	// Token: 0x17000BD7 RID: 3031
	// (get) Token: 0x0600188F RID: 6287 RVA: 0x0004CF4B File Offset: 0x0004B14B
	public string Description
	{
		get
		{
			return this.ToString();
		}
	}

	// Token: 0x17000BD8 RID: 3032
	// (get) Token: 0x06001890 RID: 6288 RVA: 0x0004CF54 File Offset: 0x0004B154
	public bool ShouldDisplaySpeechBubble
	{
		get
		{
			return NPCDialogueManager.CanSpeak(this.m_npcType);
		}
	}

	// Token: 0x17000BD9 RID: 3033
	// (get) Token: 0x06001891 RID: 6289 RVA: 0x0004CF6C File Offset: 0x0004B16C
	public SpeechBubbleType BubbleType
	{
		get
		{
			return SpeechBubbleType.Dialogue;
		}
	}

	// Token: 0x17000BDA RID: 3034
	// (get) Token: 0x06001892 RID: 6290 RVA: 0x0004CF7A File Offset: 0x0004B17A
	public NPCType NPCType
	{
		get
		{
			return this.m_npcType;
		}
	}

	// Token: 0x17000BDB RID: 3035
	// (get) Token: 0x06001893 RID: 6291 RVA: 0x0004CF82 File Offset: 0x0004B182
	// (set) Token: 0x06001894 RID: 6292 RVA: 0x0004CF8A File Offset: 0x0004B18A
	public Animator Animator { get; private set; }

	// Token: 0x17000BDC RID: 3036
	// (get) Token: 0x06001895 RID: 6293 RVA: 0x0004CF93 File Offset: 0x0004B193
	// (set) Token: 0x06001896 RID: 6294 RVA: 0x0004CF9B File Offset: 0x0004B19B
	public NPCState CurrentState { get; private set; } = NPCState.Idle;

	// Token: 0x06001897 RID: 6295 RVA: 0x0004CFA4 File Offset: 0x0004B1A4
	private void Awake()
	{
		this.Animator = base.GetComponent<Animator>();
		this.m_onSkillTreeOpened = new Action<MonoBehaviour, EventArgs>(this.OnSkillTreeOpened);
		this.m_onSkillTreeClosed = new Action<MonoBehaviour, EventArgs>(this.OnSkillTreeClosed);
		Messenger<UIMessenger, UIEvent>.AddListener(UIEvent.SkillTree_Opened, this.m_onSkillTreeOpened);
		Messenger<UIMessenger, UIEvent>.AddListener(UIEvent.SkillTree_Closed, this.m_onSkillTreeClosed);
	}

	// Token: 0x06001898 RID: 6296 RVA: 0x0004CFFB File Offset: 0x0004B1FB
	private void OnEnable()
	{
		if (this.Animator)
		{
			this.AssignAnimatorNPCType();
			this.EatPizza();
		}
		this.UpdateHeartState();
	}

	// Token: 0x06001899 RID: 6297 RVA: 0x0004D01C File Offset: 0x0004B21C
	private void AssignAnimatorNPCType()
	{
		if (this.Animator && !this.m_animatorNPCTypeAssigned)
		{
			if (global::AnimatorUtility.HasParameter(this.Animator, NPCController.NPC_ANIMATOR_PARAM))
			{
				this.Animator.SetInteger(NPCController.NPC_ANIMATOR_PARAM, (int)this.m_npcType);
			}
			this.m_animatorNPCTypeAssigned = true;
		}
	}

	// Token: 0x0600189A RID: 6298 RVA: 0x0004D06D File Offset: 0x0004B26D
	private void OnDisable()
	{
		this.m_animatorNPCTypeAssigned = false;
	}

	// Token: 0x0600189B RID: 6299 RVA: 0x0004D076 File Offset: 0x0004B276
	private void OnSkillTreeOpened(object sender, EventArgs args)
	{
		if (this.Animator)
		{
			this.Animator.enabled = false;
		}
		if (this.m_eatPizzaCoroutine != null)
		{
			base.StopCoroutine(this.m_eatPizzaCoroutine);
		}
	}

	// Token: 0x0600189C RID: 6300 RVA: 0x0004D0A5 File Offset: 0x0004B2A5
	private void OnSkillTreeClosed(object sender, EventArgs args)
	{
		if (this.Animator)
		{
			this.Animator.enabled = true;
		}
		if (base.gameObject.activeInHierarchy)
		{
			this.EatPizza();
		}
	}

	// Token: 0x0600189D RID: 6301 RVA: 0x0004D0D3 File Offset: 0x0004B2D3
	private void OnDestroy()
	{
		Messenger<UIMessenger, UIEvent>.RemoveListener(UIEvent.SkillTree_Opened, this.m_onSkillTreeOpened);
		Messenger<UIMessenger, UIEvent>.RemoveListener(UIEvent.SkillTree_Closed, this.m_onSkillTreeClosed);
	}

	// Token: 0x0600189E RID: 6302 RVA: 0x0004D0F0 File Offset: 0x0004B2F0
	private int GetStateHash(NPCState state)
	{
		switch (state)
		{
		case NPCState.Idle:
			return NPCController.m_idleHash;
		case NPCState.AtAttention:
			return NPCController.m_atAttentionHash;
		case NPCState.Speaking:
			return NPCController.m_speakingHash;
		case NPCState.SpeakingPause:
			return NPCController.m_speakingPauseHash;
		case NPCState.Emote1:
			return NPCController.m_emote1Hash;
		case NPCState.Emote2:
			return NPCController.m_emote2Hash;
		default:
			return 0;
		}
	}

	// Token: 0x0600189F RID: 6303 RVA: 0x0004D144 File Offset: 0x0004B344
	public void SetNPCState(NPCState state, bool skipTransition = false)
	{
		if (!this.Animator)
		{
			return;
		}
		this.Animator.SetBool(this.GetStateHash(NPCState.Idle), false);
		this.Animator.SetBool(this.GetStateHash(NPCState.AtAttention), false);
		this.Animator.SetBool(this.GetStateHash(NPCState.Speaking), false);
		this.Animator.SetBool(this.GetStateHash(NPCState.SpeakingPause), false);
		this.Animator.SetBool(this.GetStateHash(NPCState.Emote1), false);
		this.Animator.SetBool(this.GetStateHash(NPCState.Emote2), false);
		this.Animator.SetBool(this.GetStateHash(state), true);
		this.AssignAnimatorNPCType();
		if (skipTransition)
		{
			this.Animator.Update(1f);
			this.Animator.Update(1f);
		}
		this.CurrentState = state;
		this.EatPizza();
	}

	// Token: 0x060018A0 RID: 6304 RVA: 0x0004D21C File Offset: 0x0004B41C
	private void EatPizza()
	{
		if (this.m_eatPizzaCoroutine != null)
		{
			base.StopCoroutine(this.m_eatPizzaCoroutine);
		}
		if (!this.CanEatPizza())
		{
			return;
		}
		if (!SaveManager.PlayerSaveData.GetFlag(PlayerSaveFlag.PizzaGirlUnlocked))
		{
			return;
		}
		if (this.CurrentState != NPCState.Idle)
		{
			return;
		}
		this.m_eatPizzaCoroutine = base.StartCoroutine(this.EatPizzaCoroutine());
	}

	// Token: 0x060018A1 RID: 6305 RVA: 0x0004D274 File Offset: 0x0004B474
	private IEnumerator EatPizzaCoroutine()
	{
		float delay = UnityEngine.Random.Range(5f, 45f);
		delay += Time.time;
		while (Time.time < delay)
		{
			yield return null;
		}
		if (string.IsNullOrEmpty(this.m_eatPizzaAudioEvent))
		{
			Debug.Log("<color=red>NPC (" + this.m_npcType.ToString() + ") attempting to eat pizza without an assigned audio event!</color>");
		}
		AudioManager.PlayOneShotAttached(this, this.m_eatPizzaAudioEvent, base.gameObject);
		this.Animator.Play(NPCController.EAT_PIZZA_ANIMATOR_STATE_HASH);
		delay = Time.time + UnityEngine.Random.Range(20f, 60f);
		while (Time.time < delay)
		{
			yield return null;
		}
		this.EatPizza();
		yield break;
	}

	// Token: 0x060018A2 RID: 6306 RVA: 0x0004D284 File Offset: 0x0004B484
	public void RunNextNPCDialogue(Action onDialogueComplete = null)
	{
		int index = SaveManager.PlayerSaveData.GetNPCDialoguesRead(this.NPCType) + 1;
		this.RunNPCDialogueAtIndex(index, onDialogueComplete);
		SaveManager.PlayerSaveData.TriggerGlobalNPCDialogueCD = true;
	}

	// Token: 0x060018A3 RID: 6307 RVA: 0x0004D2B8 File Offset: 0x0004B4B8
	public void RepeatLastNPCDialogue(Action onDialogueComplete = null)
	{
		int num = SaveManager.PlayerSaveData.GetNPCDialoguesRead(this.NPCType);
		num = Mathf.Clamp(num, 0, NPCDialogue_EV.NPCDialogueTable[this.NPCType].Length - 1);
		this.RunNPCDialogueAtIndex(num, onDialogueComplete);
	}

	// Token: 0x060018A4 RID: 6308 RVA: 0x0004D2FC File Offset: 0x0004B4FC
	private void RunNPCDialogueAtIndex(int index, Action onDialogueComplete = null)
	{
		NPCDialogueEntry[] array;
		if (NPCDialogue_EV.NPCDialogueTable.TryGetValue(this.NPCType, out array))
		{
			if (array != null)
			{
				if (index >= array.Length)
				{
					this.RepeatLastNPCDialogue(onDialogueComplete);
					return;
				}
				NPCDialogueEntry npcdialogueEntry = NPCDialogue_EV.NPCDialogueTable[this.NPCType][index];
				string text = npcdialogueEntry.TitleLocID;
				if (string.IsNullOrEmpty(text))
				{
					text = NPCDialogue_EV.GetNPCTitleLocID(this.NPCType);
				}
				DialogueManager.StartNewNPCDialogue(this, NPCState.Idle);
				DialogueManager.AddDialogue(text, npcdialogueEntry.LocID, SaveManager.PlayerSaveData.CurrentCharacter.IsFemale, DialogueWindowStyle.HorizontalUpper, DialoguePortraitType.None, NPCState.None, 0.015f);
				WindowManager.SetWindowIsOpen(WindowID.Dialogue, true);
				if (onDialogueComplete != null)
				{
					DialogueManager.AddDialogueCompleteEndHandler(onDialogueComplete);
					return;
				}
			}
			else if (onDialogueComplete != null)
			{
				DialogueManager.AddDialogueCompleteEndHandler(onDialogueComplete);
			}
		}
	}

	// Token: 0x060018A5 RID: 6309 RVA: 0x0004D3A4 File Offset: 0x0004B5A4
	public void UpdateHeartState()
	{
		if (SaveManager.PlayerSaveData.EndingSpawnRoom == EndingSpawnRoomType.AboveGround)
		{
			if (this.m_heartGO)
			{
				this.m_heartGO.SetActive(this.IsBestFriend);
				return;
			}
		}
		else if (this.m_heartGO && this.m_heartGO.activeSelf)
		{
			this.m_heartGO.SetActive(false);
		}
	}

	// Token: 0x17000BDD RID: 3037
	// (get) Token: 0x060018A6 RID: 6310 RVA: 0x0004D404 File Offset: 0x0004B604
	public bool IsBestFriend
	{
		get
		{
			return NPCController.GetBestFriendState(this.m_npcType);
		}
	}

	// Token: 0x060018A7 RID: 6311 RVA: 0x0004D411 File Offset: 0x0004B611
	public static bool GetBestFriendState(NPCType npcType)
	{
		if (npcType == NPCType.ChallengeHood)
		{
			return ChallengeManager.GetChallengeTrophyRank(ChallengeType.TutorialPurified, true) >= ChallengeTrophyRank.Bronze;
		}
		if (npcType - NPCType.NewGamePlusHood > 1)
		{
			return NPCDialogueManager.AllNPCDialoguesRead(npcType);
		}
		return SaveManager.PlayerSaveData.GetFlag(PlayerSaveFlag.FinalBoss_Prime_Defeated_FirstTime);
	}

	// Token: 0x060018A8 RID: 6312 RVA: 0x0004D445 File Offset: 0x0004B645
	public void ShowHeart()
	{
		if (this.m_heartGO)
		{
			this.m_heartGO.SetActive(true);
		}
	}

	// Token: 0x060018A9 RID: 6313 RVA: 0x0004D460 File Offset: 0x0004B660
	public void HideHeart()
	{
		if (this.m_heartGO)
		{
			this.m_heartGO.SetActive(false);
		}
	}

	// Token: 0x060018AA RID: 6314 RVA: 0x0004D47C File Offset: 0x0004B67C
	private bool CanEatPizza()
	{
		NPCType npctype = this.NPCType;
		return npctype <= NPCType.Totem;
	}

	// Token: 0x040017E5 RID: 6117
	[SerializeField]
	private NPCType m_npcType;

	// Token: 0x040017E6 RID: 6118
	[SerializeField]
	[EventRef]
	private string m_eatPizzaAudioEvent;

	// Token: 0x040017E7 RID: 6119
	[SerializeField]
	private GameObject m_heartGO;

	// Token: 0x040017E8 RID: 6120
	private static int NPC_ANIMATOR_PARAM = Animator.StringToHash("NPC");

	// Token: 0x040017E9 RID: 6121
	private static int m_idleHash = Animator.StringToHash("Idle");

	// Token: 0x040017EA RID: 6122
	private static int m_atAttentionHash = Animator.StringToHash("StandingAtAttention");

	// Token: 0x040017EB RID: 6123
	private static int m_speakingHash = Animator.StringToHash("Speaking");

	// Token: 0x040017EC RID: 6124
	private static int m_speakingPauseHash = Animator.StringToHash("SpeakingPause");

	// Token: 0x040017ED RID: 6125
	private static int m_emote1Hash = Animator.StringToHash("Emote1");

	// Token: 0x040017EE RID: 6126
	private static int m_emote2Hash = Animator.StringToHash("Emote2");

	// Token: 0x040017EF RID: 6127
	private static int EAT_PIZZA_ANIMATOR_STATE_HASH = Animator.StringToHash("EatPizza");

	// Token: 0x040017F0 RID: 6128
	private Coroutine m_eatPizzaCoroutine;

	// Token: 0x040017F1 RID: 6129
	private bool m_animatorNPCTypeAssigned;

	// Token: 0x040017F2 RID: 6130
	private Action<MonoBehaviour, EventArgs> m_onSkillTreeOpened;

	// Token: 0x040017F3 RID: 6131
	private Action<MonoBehaviour, EventArgs> m_onSkillTreeClosed;
}
