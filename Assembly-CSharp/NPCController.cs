using System;
using System.Collections;
using FMODUnity;
using RLAudio;
using RL_Windows;
using UnityEngine;

// Token: 0x02000431 RID: 1073
public class NPCController : MonoBehaviour, IDisplaySpeechBubble, IAudioEventEmitter
{
	// Token: 0x17000F16 RID: 3862
	// (get) Token: 0x06002278 RID: 8824 RVA: 0x00009A7B File Offset: 0x00007C7B
	public string Description
	{
		get
		{
			return this.ToString();
		}
	}

	// Token: 0x17000F17 RID: 3863
	// (get) Token: 0x06002279 RID: 8825 RVA: 0x000AACD0 File Offset: 0x000A8ED0
	public bool ShouldDisplaySpeechBubble
	{
		get
		{
			return NPCDialogueManager.CanSpeak(this.m_npcType);
		}
	}

	// Token: 0x17000F18 RID: 3864
	// (get) Token: 0x0600227A RID: 8826 RVA: 0x000AACE8 File Offset: 0x000A8EE8
	public SpeechBubbleType BubbleType
	{
		get
		{
			return SpeechBubbleType.Dialogue;
		}
	}

	// Token: 0x17000F19 RID: 3865
	// (get) Token: 0x0600227B RID: 8827 RVA: 0x000126F1 File Offset: 0x000108F1
	public NPCType NPCType
	{
		get
		{
			return this.m_npcType;
		}
	}

	// Token: 0x17000F1A RID: 3866
	// (get) Token: 0x0600227C RID: 8828 RVA: 0x000126F9 File Offset: 0x000108F9
	// (set) Token: 0x0600227D RID: 8829 RVA: 0x00012701 File Offset: 0x00010901
	public Animator Animator { get; private set; }

	// Token: 0x17000F1B RID: 3867
	// (get) Token: 0x0600227E RID: 8830 RVA: 0x0001270A File Offset: 0x0001090A
	// (set) Token: 0x0600227F RID: 8831 RVA: 0x00012712 File Offset: 0x00010912
	public NPCState CurrentState { get; private set; } = NPCState.Idle;

	// Token: 0x06002280 RID: 8832 RVA: 0x000AACF8 File Offset: 0x000A8EF8
	private void Awake()
	{
		this.Animator = base.GetComponent<Animator>();
		this.m_onSkillTreeOpened = new Action<MonoBehaviour, EventArgs>(this.OnSkillTreeOpened);
		this.m_onSkillTreeClosed = new Action<MonoBehaviour, EventArgs>(this.OnSkillTreeClosed);
		Messenger<UIMessenger, UIEvent>.AddListener(UIEvent.SkillTree_Opened, this.m_onSkillTreeOpened);
		Messenger<UIMessenger, UIEvent>.AddListener(UIEvent.SkillTree_Closed, this.m_onSkillTreeClosed);
	}

	// Token: 0x06002281 RID: 8833 RVA: 0x0001271B File Offset: 0x0001091B
	private void OnEnable()
	{
		if (this.Animator)
		{
			this.AssignAnimatorNPCType();
			this.EatPizza();
		}
		this.UpdateHeartState();
	}

	// Token: 0x06002282 RID: 8834 RVA: 0x000AAD50 File Offset: 0x000A8F50
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

	// Token: 0x06002283 RID: 8835 RVA: 0x0001273C File Offset: 0x0001093C
	private void OnDisable()
	{
		this.m_animatorNPCTypeAssigned = false;
	}

	// Token: 0x06002284 RID: 8836 RVA: 0x00012745 File Offset: 0x00010945
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

	// Token: 0x06002285 RID: 8837 RVA: 0x00012774 File Offset: 0x00010974
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

	// Token: 0x06002286 RID: 8838 RVA: 0x000127A2 File Offset: 0x000109A2
	private void OnDestroy()
	{
		Messenger<UIMessenger, UIEvent>.RemoveListener(UIEvent.SkillTree_Opened, this.m_onSkillTreeOpened);
		Messenger<UIMessenger, UIEvent>.RemoveListener(UIEvent.SkillTree_Closed, this.m_onSkillTreeClosed);
	}

	// Token: 0x06002287 RID: 8839 RVA: 0x000AADA4 File Offset: 0x000A8FA4
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

	// Token: 0x06002288 RID: 8840 RVA: 0x000AADF8 File Offset: 0x000A8FF8
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

	// Token: 0x06002289 RID: 8841 RVA: 0x000AAED0 File Offset: 0x000A90D0
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

	// Token: 0x0600228A RID: 8842 RVA: 0x000127BE File Offset: 0x000109BE
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

	// Token: 0x0600228B RID: 8843 RVA: 0x000AAF28 File Offset: 0x000A9128
	public void RunNextNPCDialogue(Action onDialogueComplete = null)
	{
		int index = SaveManager.PlayerSaveData.GetNPCDialoguesRead(this.NPCType) + 1;
		this.RunNPCDialogueAtIndex(index, onDialogueComplete);
		SaveManager.PlayerSaveData.TriggerGlobalNPCDialogueCD = true;
	}

	// Token: 0x0600228C RID: 8844 RVA: 0x000AAF5C File Offset: 0x000A915C
	public void RepeatLastNPCDialogue(Action onDialogueComplete = null)
	{
		int num = SaveManager.PlayerSaveData.GetNPCDialoguesRead(this.NPCType);
		num = Mathf.Clamp(num, 0, NPCDialogue_EV.NPCDialogueTable[this.NPCType].Length - 1);
		this.RunNPCDialogueAtIndex(num, onDialogueComplete);
	}

	// Token: 0x0600228D RID: 8845 RVA: 0x000AAFA0 File Offset: 0x000A91A0
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

	// Token: 0x0600228E RID: 8846 RVA: 0x000AB048 File Offset: 0x000A9248
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

	// Token: 0x17000F1C RID: 3868
	// (get) Token: 0x0600228F RID: 8847 RVA: 0x000127CD File Offset: 0x000109CD
	public bool IsBestFriend
	{
		get
		{
			return NPCController.GetBestFriendState(this.m_npcType);
		}
	}

	// Token: 0x06002290 RID: 8848 RVA: 0x000127DA File Offset: 0x000109DA
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

	// Token: 0x06002291 RID: 8849 RVA: 0x0001280E File Offset: 0x00010A0E
	public void ShowHeart()
	{
		if (this.m_heartGO)
		{
			this.m_heartGO.SetActive(true);
		}
	}

	// Token: 0x06002292 RID: 8850 RVA: 0x00012829 File Offset: 0x00010A29
	public void HideHeart()
	{
		if (this.m_heartGO)
		{
			this.m_heartGO.SetActive(false);
		}
	}

	// Token: 0x06002293 RID: 8851 RVA: 0x000AB0A8 File Offset: 0x000A92A8
	private bool CanEatPizza()
	{
		NPCType npctype = this.NPCType;
		return npctype <= NPCType.Totem;
	}

	// Token: 0x04001F24 RID: 7972
	[SerializeField]
	private NPCType m_npcType;

	// Token: 0x04001F25 RID: 7973
	[SerializeField]
	[EventRef]
	private string m_eatPizzaAudioEvent;

	// Token: 0x04001F26 RID: 7974
	[SerializeField]
	private GameObject m_heartGO;

	// Token: 0x04001F27 RID: 7975
	private static int NPC_ANIMATOR_PARAM = Animator.StringToHash("NPC");

	// Token: 0x04001F28 RID: 7976
	private static int m_idleHash = Animator.StringToHash("Idle");

	// Token: 0x04001F29 RID: 7977
	private static int m_atAttentionHash = Animator.StringToHash("StandingAtAttention");

	// Token: 0x04001F2A RID: 7978
	private static int m_speakingHash = Animator.StringToHash("Speaking");

	// Token: 0x04001F2B RID: 7979
	private static int m_speakingPauseHash = Animator.StringToHash("SpeakingPause");

	// Token: 0x04001F2C RID: 7980
	private static int m_emote1Hash = Animator.StringToHash("Emote1");

	// Token: 0x04001F2D RID: 7981
	private static int m_emote2Hash = Animator.StringToHash("Emote2");

	// Token: 0x04001F2E RID: 7982
	private static int EAT_PIZZA_ANIMATOR_STATE_HASH = Animator.StringToHash("EatPizza");

	// Token: 0x04001F2F RID: 7983
	private Coroutine m_eatPizzaCoroutine;

	// Token: 0x04001F30 RID: 7984
	private bool m_animatorNPCTypeAssigned;

	// Token: 0x04001F31 RID: 7985
	private Action<MonoBehaviour, EventArgs> m_onSkillTreeOpened;

	// Token: 0x04001F32 RID: 7986
	private Action<MonoBehaviour, EventArgs> m_onSkillTreeClosed;
}
