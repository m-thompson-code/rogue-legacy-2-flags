using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x0200039C RID: 924
public class FairyRuleHUDController : MonoBehaviour, ILocalizable
{
	// Token: 0x17000DFA RID: 3578
	// (get) Token: 0x06001EB7 RID: 7863 RVA: 0x00010117 File Offset: 0x0000E317
	public CanvasGroup Panel
	{
		get
		{
			return this.m_panel;
		}
	}

	// Token: 0x17000DFB RID: 3579
	// (get) Token: 0x06001EB8 RID: 7864 RVA: 0x0001011F File Offset: 0x0000E31F
	public CanvasGroup BGCanvasGroup
	{
		get
		{
			return this.m_bgCanvasGroup;
		}
	}

	// Token: 0x06001EB9 RID: 7865 RVA: 0x000A0C00 File Offset: 0x0009EE00
	private void Awake()
	{
		if (FairyRuleHUDController.m_hudEntryArray_STATIC == null)
		{
			FairyRuleHUDController.m_hudEntryArray_STATIC = new FairyRuleHUDEntry[5];
			for (int i = 0; i < 5; i++)
			{
				FairyRuleHUDEntry fairyRuleHUDEntry = UnityEngine.Object.Instantiate<FairyRuleHUDEntry>(this.m_fairyRuleHUDEntryPrefab, this.m_entryGroup.transform);
				fairyRuleHUDEntry.gameObject.SetActive(false);
				FairyRuleHUDController.m_hudEntryArray_STATIC[i] = fairyRuleHUDEntry;
			}
		}
		this.m_hudEntryTable = new Dictionary<FairyRule, FairyRuleHUDEntry>();
		this.m_onPlayerEnterFairyRoom = new Action<MonoBehaviour, EventArgs>(this.OnPlayerEnterFairyRoom);
		this.m_onFairyRoomStateChange = new Action<MonoBehaviour, EventArgs>(this.OnFairyRoomStateChange);
		this.m_onPlayerExitFairyRoom = new Action<MonoBehaviour, EventArgs>(this.OnPlayerExitFairyRoom);
		this.m_onPlayerDeath = new Action<MonoBehaviour, EventArgs>(this.OnPlayerDeath);
		this.m_onFairyRuleStateChange = new Action<MonoBehaviour, EventArgs>(this.OnFairyRuleStateChange);
		this.m_refreshText = new Action<MonoBehaviour, EventArgs>(this.RefreshText);
	}

	// Token: 0x06001EBA RID: 7866 RVA: 0x000A0CD0 File Offset: 0x0009EED0
	private void Start()
	{
		this.SetHUDIsVisible(false);
		Messenger<GameMessenger, GameEvent>.AddListener(GameEvent.PlayerEnterFairyRoom, this.m_onPlayerEnterFairyRoom);
		Messenger<GameMessenger, GameEvent>.AddListener(GameEvent.FairyRoomStateChange, this.m_onFairyRoomStateChange);
		Messenger<GameMessenger, GameEvent>.AddListener(GameEvent.PlayerExitFairyRoom, this.m_onPlayerExitFairyRoom);
		Messenger<GameMessenger, GameEvent>.AddListener(GameEvent.PlayerDeath, this.m_onPlayerDeath);
		Messenger<GameMessenger, GameEvent>.AddListener(GameEvent.FairyRoomRuleStateChange, this.m_onFairyRuleStateChange);
		this.m_fairyHUDCollider.SetCanvasGroup(this.BGCanvasGroup);
		this.BGCanvasGroup.alpha = 0.7f;
	}

	// Token: 0x06001EBB RID: 7867 RVA: 0x000A0D48 File Offset: 0x0009EF48
	private void OnDestroy()
	{
		Messenger<GameMessenger, GameEvent>.RemoveListener(GameEvent.PlayerEnterFairyRoom, this.m_onPlayerEnterFairyRoom);
		Messenger<GameMessenger, GameEvent>.RemoveListener(GameEvent.FairyRoomStateChange, this.m_onFairyRoomStateChange);
		Messenger<GameMessenger, GameEvent>.RemoveListener(GameEvent.PlayerExitFairyRoom, this.m_onPlayerExitFairyRoom);
		Messenger<GameMessenger, GameEvent>.RemoveListener(GameEvent.PlayerDeath, this.m_onPlayerDeath);
		Messenger<GameMessenger, GameEvent>.RemoveListener(GameEvent.FairyRoomRuleStateChange, this.m_onFairyRuleStateChange);
		UnityEngine.Object.Destroy(this.m_fairyHUDCollider);
		if (FairyRuleHUDController.m_hudEntryArray_STATIC != null)
		{
			Array.Clear(FairyRuleHUDController.m_hudEntryArray_STATIC, 0, FairyRuleHUDController.m_hudEntryArray_STATIC.Length);
			FairyRuleHUDController.m_hudEntryArray_STATIC = null;
		}
	}

	// Token: 0x06001EBC RID: 7868 RVA: 0x00010127 File Offset: 0x0000E327
	private void OnPlayerDeath(MonoBehaviour sender, EventArgs eventArgs)
	{
		this.SetHUDIsVisible(false);
	}

	// Token: 0x06001EBD RID: 7869 RVA: 0x000A0DC0 File Offset: 0x0009EFC0
	private void OnPlayerEnterFairyRoom(MonoBehaviour sender, EventArgs eventArgs)
	{
		base.StopAllCoroutines();
		FairyRoomEnteredEventArgs fairyRoomEnteredEventArgs = eventArgs as FairyRoomEnteredEventArgs;
		if (fairyRoomEnteredEventArgs == null || !(fairyRoomEnteredEventArgs.FairyRoomController != null))
		{
			Debug.LogFormat("<color=red>[{0}] Failed to cast eventArgs as given Type", new object[]
			{
				this
			});
			return;
		}
		this.m_fairyRoomController = fairyRoomEnteredEventArgs.FairyRoomController;
		if (this.m_fairyRoomController.FairyRoomRuleEntries.Any((FairyRoomRuleEntry entry) => entry.FairyRuleID == FairyRuleID.HiddenChest))
		{
			return;
		}
		foreach (FairyRuleHUDEntry fairyRuleHUDEntry in FairyRuleHUDController.m_hudEntryArray_STATIC)
		{
			if (fairyRuleHUDEntry.gameObject.activeSelf)
			{
				fairyRuleHUDEntry.gameObject.SetActive(false);
			}
		}
		this.m_hudEntryTable.Clear();
		if (fairyRoomEnteredEventArgs.FairyRoomController.FairyRoomRuleEntries.Count > FairyRuleHUDController.m_hudEntryArray_STATIC.Length)
		{
			throw new Exception("Fairy Rule HUD Controller currently only supports " + FairyRuleHUDController.m_hudEntryArray_STATIC.Length.ToString() + " entries.");
		}
		for (int j = 0; j < fairyRoomEnteredEventArgs.FairyRoomController.FairyRoomRuleEntries.Count; j++)
		{
			FairyRoomRuleEntry fairyRoomRuleEntry = fairyRoomEnteredEventArgs.FairyRoomController.FairyRoomRuleEntries[j];
			FairyRuleHUDEntry fairyRuleHUDEntry2 = FairyRuleHUDController.m_hudEntryArray_STATIC[j];
			fairyRuleHUDEntry2.gameObject.SetActive(true);
			fairyRuleHUDEntry2.Initialise("• " + LocalizationManager.GetString(fairyRoomRuleEntry.FairyRule.Description, SaveManager.PlayerSaveData.CurrentCharacter.IsFemale, false));
			if (this.m_fairyRoomController.State == FairyRoomState.Passed)
			{
				fairyRuleHUDEntry2.SetState(FairyRoomState.Passed);
			}
			else if (this.m_fairyRoomController.State == FairyRoomState.Failed)
			{
				fairyRuleHUDEntry2.SetState(FairyRoomState.Failed);
			}
			else
			{
				fairyRuleHUDEntry2.SetState(fairyRoomRuleEntry.FairyRule.State);
				if (fairyRoomRuleEntry.FairyRuleID == FairyRuleID.TimeLimit)
				{
					TimeLimit_FairyRule timeLimit_FairyRule = fairyRoomRuleEntry.FairyRule as TimeLimit_FairyRule;
					if (timeLimit_FairyRule)
					{
						base.StartCoroutine(this.FairyTimerCoroutine(timeLimit_FairyRule, fairyRuleHUDEntry2));
					}
				}
			}
			this.m_hudEntryTable.Add(fairyRoomRuleEntry.FairyRule, fairyRuleHUDEntry2);
		}
		this.Panel.alpha = 0f;
		if (this.m_fadeInTween != null)
		{
			this.m_fadeInTween.StopTweenWithConditionChecks(false, this.Panel, "FairyRuleFade");
		}
		this.m_fadeInTween = TweenManager.TweenTo(this.Panel, 0.5f, new EaseDelegate(Ease.None), new object[]
		{
			"alpha",
			1
		});
		this.m_fadeInTween.ID = "FairyRuleFade";
		this.SetHUDIsVisible(true);
	}

	// Token: 0x06001EBE RID: 7870 RVA: 0x000A1054 File Offset: 0x0009F254
	private void OnFairyRoomStateChange(object sender, EventArgs args)
	{
		FairyRoomState state = (args as FairyRoomEnteredEventArgs).FairyRoomController.State;
		Color color;
		if (state <= FairyRoomState.Running)
		{
			if (state != FairyRoomState.NotRunning)
			{
				if (state == FairyRoomState.Running)
				{
					ColorUtility.TryParseHtmlString("#E7BF51", out color);
					goto IL_64;
				}
			}
		}
		else
		{
			if (state == FairyRoomState.Passed)
			{
				ColorUtility.TryParseHtmlString("#3ACF00", out color);
				goto IL_64;
			}
			if (state == FairyRoomState.Failed)
			{
				ColorUtility.TryParseHtmlString("#CA000B", out color);
				goto IL_64;
			}
		}
		ColorUtility.TryParseHtmlString("#000000", out color);
		IL_64:
		this.m_orbImage.color = color;
	}

	// Token: 0x06001EBF RID: 7871 RVA: 0x00010130 File Offset: 0x0000E330
	private IEnumerator FairyTimerCoroutine(TimeLimit_FairyRule fairyRule, FairyRuleHUDEntry hudEntry)
	{
		this.m_timerValue = 0f;
		while (fairyRule && fairyRule.State != FairyRoomState.Passed && fairyRule.State != FairyRoomState.Failed)
		{
			int num = (int)fairyRule.TimeRemaining;
			if (this.m_timerValue != (float)num)
			{
				this.m_timerValue = (float)num;
				hudEntry.SetText("• " + fairyRule.TimeRemainingDescription);
			}
			yield return null;
		}
		hudEntry.SetText("• " + fairyRule.TimeRemainingDescription);
		yield break;
	}

	// Token: 0x06001EC0 RID: 7872 RVA: 0x000A10D4 File Offset: 0x0009F2D4
	private void OnPlayerExitFairyRoom(MonoBehaviour sender, EventArgs eventArgs)
	{
		FairyRoomEnteredEventArgs fairyRoomEnteredEventArgs = eventArgs as FairyRoomEnteredEventArgs;
		if (fairyRoomEnteredEventArgs != null && fairyRoomEnteredEventArgs.FairyRoomController == this.m_fairyRoomController)
		{
			this.SetHUDIsVisible(false);
		}
	}

	// Token: 0x06001EC1 RID: 7873 RVA: 0x000A1108 File Offset: 0x0009F308
	private void OnFairyRuleStateChange(MonoBehaviour sender, EventArgs eventArgs)
	{
		FairyRoomRuleStateChangeEventArgs fairyRoomRuleStateChangeEventArgs = eventArgs as FairyRoomRuleStateChangeEventArgs;
		if (fairyRoomRuleStateChangeEventArgs != null && fairyRoomRuleStateChangeEventArgs.Rule != null && fairyRoomRuleStateChangeEventArgs.Rule.ID != FairyRuleID.HiddenChest)
		{
			if (fairyRoomRuleStateChangeEventArgs.Rule.State == FairyRoomState.Failed)
			{
				using (Dictionary<FairyRule, FairyRuleHUDEntry>.Enumerator enumerator = this.m_hudEntryTable.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						KeyValuePair<FairyRule, FairyRuleHUDEntry> keyValuePair = enumerator.Current;
						keyValuePair.Value.SetState(FairyRoomState.Failed);
					}
					return;
				}
			}
			if (this.m_hudEntryTable == null)
			{
				Debug.LogFormat("<color=red>[{0}] HUD Entry Table has not been initialized</color>", new object[]
				{
					this
				});
				return;
			}
			if (!(fairyRoomRuleStateChangeEventArgs.Rule != null))
			{
				Debug.LogFormat("<color=red>[{0}] args.Rule is null</color>", new object[]
				{
					this
				});
				return;
			}
			if (this.m_hudEntryTable.ContainsKey(fairyRoomRuleStateChangeEventArgs.Rule))
			{
				this.m_hudEntryTable[fairyRoomRuleStateChangeEventArgs.Rule].SetState(fairyRoomRuleStateChangeEventArgs.Rule.State);
				return;
			}
			Debug.LogFormat("<color=red>[{0}] HUD Entry Table doesn't contain an entry with the given Key</color>", new object[]
			{
				this
			});
			return;
		}
		else if (fairyRoomRuleStateChangeEventArgs == null || fairyRoomRuleStateChangeEventArgs.Rule == null)
		{
			Debug.LogFormat("<color=red>[{0}] Failed to cast eventArgs as given Type</color>", new object[]
			{
				this
			});
		}
	}

	// Token: 0x06001EC2 RID: 7874 RVA: 0x0001014D File Offset: 0x0000E34D
	private void SetHUDIsVisible(bool isVisible)
	{
		this.Panel.gameObject.SetActive(isVisible);
		this.m_particleEffect.gameObject.SetActive(isVisible);
	}

	// Token: 0x06001EC3 RID: 7875 RVA: 0x00010171 File Offset: 0x0000E371
	private void OnEnable()
	{
		Messenger<UIMessenger, UIEvent>.AddListener(UIEvent.LanguageChanged, this.m_refreshText);
	}

	// Token: 0x06001EC4 RID: 7876 RVA: 0x00010180 File Offset: 0x0000E380
	private void OnDisable()
	{
		Messenger<UIMessenger, UIEvent>.RemoveListener(UIEvent.LanguageChanged, this.m_refreshText);
	}

	// Token: 0x06001EC5 RID: 7877 RVA: 0x000A1254 File Offset: 0x0009F454
	public void RefreshText(object sender, EventArgs args)
	{
		if (this.m_fairyRoomController)
		{
			for (int i = 0; i < this.m_fairyRoomController.FairyRoomRuleEntries.Count; i++)
			{
				FairyRuleHUDEntry fairyRuleHUDEntry = FairyRuleHUDController.m_hudEntryArray_STATIC[i];
				FairyRoomRuleEntry fairyRoomRuleEntry = this.m_fairyRoomController.FairyRoomRuleEntries[i];
				TimeLimit_FairyRule timeLimit_FairyRule = fairyRoomRuleEntry.FairyRule as TimeLimit_FairyRule;
				if (timeLimit_FairyRule)
				{
					fairyRuleHUDEntry.SetText("• " + timeLimit_FairyRule.TimeRemainingDescription);
				}
				else
				{
					fairyRuleHUDEntry.Initialise("• " + LocalizationManager.GetString(fairyRoomRuleEntry.FairyRule.Description, SaveManager.PlayerSaveData.CurrentCharacter.IsFemale, false));
				}
				fairyRuleHUDEntry.SetState(this.m_fairyRoomController.State);
			}
		}
	}

	// Token: 0x04001B71 RID: 7025
	[SerializeField]
	private CanvasGroup m_panel;

	// Token: 0x04001B72 RID: 7026
	[SerializeField]
	private GameObject m_entryGroup;

	// Token: 0x04001B73 RID: 7027
	[SerializeField]
	private CanvasGroup m_bgCanvasGroup;

	// Token: 0x04001B74 RID: 7028
	[SerializeField]
	private FairyRuleHUDEntry m_fairyRuleHUDEntryPrefab;

	// Token: 0x04001B75 RID: 7029
	[SerializeField]
	private FadeOutHUDCollider m_fairyHUDCollider;

	// Token: 0x04001B76 RID: 7030
	[SerializeField]
	private ParticleSystem m_particleEffect;

	// Token: 0x04001B77 RID: 7031
	[SerializeField]
	private Image m_orbImage;

	// Token: 0x04001B78 RID: 7032
	private FairyRoomController m_fairyRoomController;

	// Token: 0x04001B79 RID: 7033
	private Dictionary<FairyRule, FairyRuleHUDEntry> m_hudEntryTable;

	// Token: 0x04001B7A RID: 7034
	private static FairyRuleHUDEntry[] m_hudEntryArray_STATIC;

	// Token: 0x04001B7B RID: 7035
	private Tween m_fadeInTween;

	// Token: 0x04001B7C RID: 7036
	private Action<MonoBehaviour, EventArgs> m_onPlayerEnterFairyRoom;

	// Token: 0x04001B7D RID: 7037
	private Action<MonoBehaviour, EventArgs> m_onFairyRoomStateChange;

	// Token: 0x04001B7E RID: 7038
	private Action<MonoBehaviour, EventArgs> m_onPlayerExitFairyRoom;

	// Token: 0x04001B7F RID: 7039
	private Action<MonoBehaviour, EventArgs> m_onPlayerDeath;

	// Token: 0x04001B80 RID: 7040
	private Action<MonoBehaviour, EventArgs> m_onFairyRuleStateChange;

	// Token: 0x04001B81 RID: 7041
	private Action<MonoBehaviour, EventArgs> m_refreshText;

	// Token: 0x04001B82 RID: 7042
	private float m_timerValue;
}
