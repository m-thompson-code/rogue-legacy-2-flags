using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020001F9 RID: 505
public class FairyRuleHUDController : MonoBehaviour, ILocalizable
{
	// Token: 0x17000AE8 RID: 2792
	// (get) Token: 0x06001566 RID: 5478 RVA: 0x000424DE File Offset: 0x000406DE
	public CanvasGroup Panel
	{
		get
		{
			return this.m_panel;
		}
	}

	// Token: 0x17000AE9 RID: 2793
	// (get) Token: 0x06001567 RID: 5479 RVA: 0x000424E6 File Offset: 0x000406E6
	public CanvasGroup BGCanvasGroup
	{
		get
		{
			return this.m_bgCanvasGroup;
		}
	}

	// Token: 0x06001568 RID: 5480 RVA: 0x000424F0 File Offset: 0x000406F0
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

	// Token: 0x06001569 RID: 5481 RVA: 0x000425C0 File Offset: 0x000407C0
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

	// Token: 0x0600156A RID: 5482 RVA: 0x00042638 File Offset: 0x00040838
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

	// Token: 0x0600156B RID: 5483 RVA: 0x000426B0 File Offset: 0x000408B0
	private void OnPlayerDeath(MonoBehaviour sender, EventArgs eventArgs)
	{
		this.SetHUDIsVisible(false);
	}

	// Token: 0x0600156C RID: 5484 RVA: 0x000426BC File Offset: 0x000408BC
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

	// Token: 0x0600156D RID: 5485 RVA: 0x00042950 File Offset: 0x00040B50
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

	// Token: 0x0600156E RID: 5486 RVA: 0x000429CD File Offset: 0x00040BCD
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

	// Token: 0x0600156F RID: 5487 RVA: 0x000429EC File Offset: 0x00040BEC
	private void OnPlayerExitFairyRoom(MonoBehaviour sender, EventArgs eventArgs)
	{
		FairyRoomEnteredEventArgs fairyRoomEnteredEventArgs = eventArgs as FairyRoomEnteredEventArgs;
		if (fairyRoomEnteredEventArgs != null && fairyRoomEnteredEventArgs.FairyRoomController == this.m_fairyRoomController)
		{
			this.SetHUDIsVisible(false);
		}
	}

	// Token: 0x06001570 RID: 5488 RVA: 0x00042A20 File Offset: 0x00040C20
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

	// Token: 0x06001571 RID: 5489 RVA: 0x00042B6C File Offset: 0x00040D6C
	private void SetHUDIsVisible(bool isVisible)
	{
		this.Panel.gameObject.SetActive(isVisible);
		this.m_particleEffect.gameObject.SetActive(isVisible);
	}

	// Token: 0x06001572 RID: 5490 RVA: 0x00042B90 File Offset: 0x00040D90
	private void OnEnable()
	{
		Messenger<UIMessenger, UIEvent>.AddListener(UIEvent.LanguageChanged, this.m_refreshText);
	}

	// Token: 0x06001573 RID: 5491 RVA: 0x00042B9F File Offset: 0x00040D9F
	private void OnDisable()
	{
		Messenger<UIMessenger, UIEvent>.RemoveListener(UIEvent.LanguageChanged, this.m_refreshText);
	}

	// Token: 0x06001574 RID: 5492 RVA: 0x00042BB0 File Offset: 0x00040DB0
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

	// Token: 0x040014A9 RID: 5289
	[SerializeField]
	private CanvasGroup m_panel;

	// Token: 0x040014AA RID: 5290
	[SerializeField]
	private GameObject m_entryGroup;

	// Token: 0x040014AB RID: 5291
	[SerializeField]
	private CanvasGroup m_bgCanvasGroup;

	// Token: 0x040014AC RID: 5292
	[SerializeField]
	private FairyRuleHUDEntry m_fairyRuleHUDEntryPrefab;

	// Token: 0x040014AD RID: 5293
	[SerializeField]
	private FadeOutHUDCollider m_fairyHUDCollider;

	// Token: 0x040014AE RID: 5294
	[SerializeField]
	private ParticleSystem m_particleEffect;

	// Token: 0x040014AF RID: 5295
	[SerializeField]
	private Image m_orbImage;

	// Token: 0x040014B0 RID: 5296
	private FairyRoomController m_fairyRoomController;

	// Token: 0x040014B1 RID: 5297
	private Dictionary<FairyRule, FairyRuleHUDEntry> m_hudEntryTable;

	// Token: 0x040014B2 RID: 5298
	private static FairyRuleHUDEntry[] m_hudEntryArray_STATIC;

	// Token: 0x040014B3 RID: 5299
	private Tween m_fadeInTween;

	// Token: 0x040014B4 RID: 5300
	private Action<MonoBehaviour, EventArgs> m_onPlayerEnterFairyRoom;

	// Token: 0x040014B5 RID: 5301
	private Action<MonoBehaviour, EventArgs> m_onFairyRoomStateChange;

	// Token: 0x040014B6 RID: 5302
	private Action<MonoBehaviour, EventArgs> m_onPlayerExitFairyRoom;

	// Token: 0x040014B7 RID: 5303
	private Action<MonoBehaviour, EventArgs> m_onPlayerDeath;

	// Token: 0x040014B8 RID: 5304
	private Action<MonoBehaviour, EventArgs> m_onFairyRuleStateChange;

	// Token: 0x040014B9 RID: 5305
	private Action<MonoBehaviour, EventArgs> m_refreshText;

	// Token: 0x040014BA RID: 5306
	private float m_timerValue;
}
