using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200067D RID: 1661
public class RelicHUDController : MonoBehaviour
{
	// Token: 0x060032B0 RID: 12976 RVA: 0x000D9804 File Offset: 0x000D7A04
	private void Awake()
	{
		this.m_relicIconGroup.gameObject.SetActive(false);
		this.m_iconSortMethod = new Comparison<RelicHUDIconController>(this.IconSort);
		this.m_relicIconPool = new List<RelicHUDIconController>(5);
		this.m_activeRelicIconDict = new Dictionary<RelicType, RelicHUDIconController>(5);
		for (int i = 0; i < 5; i++)
		{
			RelicHUDIconController relicHUDIconController = UnityEngine.Object.Instantiate<RelicHUDIconController>(this.m_relicIconTemplate, this.m_relicIconGroup.transform);
			relicHUDIconController.gameObject.SetActive(false);
			this.m_relicIconPool.Add(relicHUDIconController);
		}
		this.m_updateAllRelics = new Action<MonoBehaviour, EventArgs>(this.UpdateAllRelics);
		this.m_showCanvasGroup = new Action<MonoBehaviour, EventArgs>(this.ShowCanvasGroup);
		this.m_hideCanvasGroup = new Action<MonoBehaviour, EventArgs>(this.HideCanvasGroup);
		this.m_onRelicChanged = new Action<MonoBehaviour, EventArgs>(this.OnRelicChanged);
		this.m_onRelicPurified = new Action<MonoBehaviour, EventArgs>(this.OnRelicPurified);
		this.m_relicIconTemplate.gameObject.SetActive(false);
		if (GameUtility.IsInLevelEditor)
		{
			Messenger<GameMessenger, GameEvent>.AddListener(GameEvent.LevelEditorWorldCreationComplete, this.m_updateAllRelics);
			Messenger<GameMessenger, GameEvent>.AddListener(GameEvent.LevelEditorWorldCreationComplete, this.m_showCanvasGroup);
		}
		else
		{
			Messenger<GameMessenger, GameEvent>.AddListener(GameEvent.WorldCreationComplete, this.m_updateAllRelics);
			Messenger<GameMessenger, GameEvent>.AddListener(GameEvent.WorldCreationComplete, this.m_showCanvasGroup);
		}
		Messenger<GameMessenger, GameEvent>.AddListener(GameEvent.RelicsReset, this.m_updateAllRelics);
		Messenger<GameMessenger, GameEvent>.AddListener(GameEvent.PlayerDeath, this.m_hideCanvasGroup);
	}

	// Token: 0x060032B1 RID: 12977 RVA: 0x000D9948 File Offset: 0x000D7B48
	private void OnDestroy()
	{
		if (GameUtility.IsInLevelEditor)
		{
			Messenger<GameMessenger, GameEvent>.RemoveListener(GameEvent.LevelEditorWorldCreationComplete, this.m_updateAllRelics);
			Messenger<GameMessenger, GameEvent>.RemoveListener(GameEvent.LevelEditorWorldCreationComplete, this.m_showCanvasGroup);
		}
		else
		{
			Messenger<GameMessenger, GameEvent>.RemoveListener(GameEvent.WorldCreationComplete, this.m_updateAllRelics);
			Messenger<GameMessenger, GameEvent>.RemoveListener(GameEvent.WorldCreationComplete, this.m_showCanvasGroup);
		}
		Messenger<GameMessenger, GameEvent>.RemoveListener(GameEvent.RelicsReset, this.m_updateAllRelics);
		Messenger<GameMessenger, GameEvent>.RemoveListener(GameEvent.PlayerDeath, this.m_hideCanvasGroup);
	}

	// Token: 0x060032B2 RID: 12978 RVA: 0x000D99AC File Offset: 0x000D7BAC
	private void OnEnable()
	{
		Messenger<GameMessenger, GameEvent>.AddListener(GameEvent.RelicLevelChanged, this.m_onRelicChanged);
		Messenger<GameMessenger, GameEvent>.AddListener(GameEvent.RelicStatsChanged, this.m_onRelicChanged);
		Messenger<GameMessenger, GameEvent>.AddListener(GameEvent.RelicPurified, this.m_onRelicPurified);
		Messenger<UIMessenger, UIEvent>.AddListener(UIEvent.SkillTree_Opened, this.m_hideCanvasGroup);
		Messenger<UIMessenger, UIEvent>.AddListener(UIEvent.SkillTree_Closed, this.m_showCanvasGroup);
	}

	// Token: 0x060032B3 RID: 12979 RVA: 0x000D99FC File Offset: 0x000D7BFC
	private void OnDisable()
	{
		Messenger<GameMessenger, GameEvent>.RemoveListener(GameEvent.RelicLevelChanged, this.m_onRelicChanged);
		Messenger<GameMessenger, GameEvent>.RemoveListener(GameEvent.RelicStatsChanged, this.m_onRelicChanged);
		Messenger<GameMessenger, GameEvent>.RemoveListener(GameEvent.RelicPurified, this.m_onRelicPurified);
		Messenger<UIMessenger, UIEvent>.RemoveListener(UIEvent.SkillTree_Opened, this.m_hideCanvasGroup);
		Messenger<UIMessenger, UIEvent>.RemoveListener(UIEvent.SkillTree_Closed, this.m_showCanvasGroup);
	}

	// Token: 0x060032B4 RID: 12980 RVA: 0x0001BBDD File Offset: 0x00019DDD
	private void ShowCanvasGroup(object sender, EventArgs args)
	{
		if (this.m_activeRelicIconDict.Count > 0)
		{
			this.m_relicIconGroup.gameObject.SetActive(true);
		}
	}

	// Token: 0x060032B5 RID: 12981 RVA: 0x0001BBFE File Offset: 0x00019DFE
	private void HideCanvasGroup(object sender, EventArgs args)
	{
		this.m_relicIconGroup.gameObject.SetActive(false);
	}

	// Token: 0x060032B6 RID: 12982 RVA: 0x000D9A4C File Offset: 0x000D7C4C
	private void UpdateAllRelics(MonoBehaviour sender, EventArgs args)
	{
		bool flag = false;
		if (PlayerManager.IsInstantiated)
		{
			foreach (RelicType relicType in RelicType_RL.TypeArray)
			{
				if (relicType != RelicType.None)
				{
					RelicObj relic = SaveManager.PlayerSaveData.GetRelic(relicType);
					if (relic.Level > 0)
					{
						relic.ApplyRelic(0);
					}
					if (this.UpdateRelicState(relicType))
					{
						flag = true;
					}
				}
			}
		}
		if (flag)
		{
			this.UpdateRelicOrder();
		}
		if (this.m_activeRelicIconDict.Count > 0)
		{
			if (!this.m_relicIconGroup.gameObject.activeSelf)
			{
				this.m_relicIconGroup.gameObject.SetActive(true);
				return;
			}
		}
		else if (this.m_relicIconGroup.gameObject.activeSelf)
		{
			this.m_relicIconGroup.gameObject.SetActive(false);
		}
	}

	// Token: 0x060032B7 RID: 12983 RVA: 0x000D9B08 File Offset: 0x000D7D08
	private void UpdateRelicOrder()
	{
		this.m_relicIconPool.Sort(this.m_iconSortMethod);
		int num = 0;
		foreach (RelicHUDIconController relicHUDIconController in this.m_relicIconPool)
		{
			if (relicHUDIconController.RelicType != RelicType.None)
			{
				relicHUDIconController.transform.SetSiblingIndex(num);
				num++;
			}
		}
	}

	// Token: 0x060032B8 RID: 12984 RVA: 0x000D9B80 File Offset: 0x000D7D80
	private int IconSort(RelicHUDIconController a, RelicHUDIconController b)
	{
		int relicType = (int)a.RelicType;
		int relicType2 = (int)b.RelicType;
		if (relicType > relicType2)
		{
			return 1;
		}
		if (relicType < relicType2)
		{
			return -1;
		}
		return 0;
	}

	// Token: 0x060032B9 RID: 12985 RVA: 0x000D9BA8 File Offset: 0x000D7DA8
	private bool UpdateRelicState(RelicType relicType)
	{
		bool result = false;
		RelicHUDIconController relicHUDIconController;
		if (SaveManager.PlayerSaveData.GetRelic(relicType).Level > 0)
		{
			RelicHUDIconController freeRelicIcon;
			if (!this.m_activeRelicIconDict.TryGetValue(relicType, out freeRelicIcon))
			{
				freeRelicIcon = this.GetFreeRelicIcon();
				freeRelicIcon.SetRelicType(relicType);
				freeRelicIcon.UpdateRelic();
				if (!freeRelicIcon.gameObject.activeSelf)
				{
					freeRelicIcon.gameObject.SetActive(true);
				}
				this.m_activeRelicIconDict.Add(relicType, freeRelicIcon);
				result = true;
			}
			else
			{
				freeRelicIcon.UpdateRelic();
			}
		}
		else if (this.m_activeRelicIconDict.TryGetValue(relicType, out relicHUDIconController))
		{
			relicHUDIconController.SetRelicType(RelicType.None);
			relicHUDIconController.gameObject.SetActive(false);
			this.m_activeRelicIconDict.Remove(relicType);
		}
		return result;
	}

	// Token: 0x060032BA RID: 12986 RVA: 0x000D9C54 File Offset: 0x000D7E54
	private RelicHUDIconController GetFreeRelicIcon()
	{
		foreach (RelicHUDIconController relicHUDIconController in this.m_relicIconPool)
		{
			if (relicHUDIconController.RelicType == RelicType.None)
			{
				return relicHUDIconController;
			}
		}
		int count = this.m_relicIconPool.Count;
		for (int i = 0; i < 5; i++)
		{
			RelicHUDIconController relicHUDIconController2 = UnityEngine.Object.Instantiate<RelicHUDIconController>(this.m_relicIconTemplate, this.m_relicIconGroup.transform);
			relicHUDIconController2.gameObject.SetActive(false);
			this.m_relicIconPool.Add(relicHUDIconController2);
		}
		return this.m_relicIconPool[count];
	}

	// Token: 0x060032BB RID: 12987 RVA: 0x000D9D08 File Offset: 0x000D7F08
	private void OnRelicChanged(MonoBehaviour sender, EventArgs args)
	{
		RelicChangedEventArgs relicChangedEventArgs = args as RelicChangedEventArgs;
		if (this.UpdateRelicState(relicChangedEventArgs.RelicType))
		{
			this.UpdateRelicOrder();
		}
		if (this.m_activeRelicIconDict.Count > 0)
		{
			if (!this.m_relicIconGroup.gameObject.activeSelf)
			{
				this.m_relicIconGroup.gameObject.SetActive(true);
				return;
			}
		}
		else if (this.m_relicIconGroup.gameObject.activeSelf)
		{
			this.m_relicIconGroup.gameObject.SetActive(false);
		}
	}

	// Token: 0x060032BC RID: 12988 RVA: 0x000D9D88 File Offset: 0x000D7F88
	private void OnRelicPurified(MonoBehaviour sender, EventArgs args)
	{
		RelicChangedEventArgs relicChangedEventArgs = args as RelicChangedEventArgs;
		if (this.m_activeRelicIconDict.ContainsKey(relicChangedEventArgs.RelicType))
		{
			this.m_activeRelicIconDict[relicChangedEventArgs.RelicType].PlayPurifyEffect();
		}
	}

	// Token: 0x0400297C RID: 10620
	private const int STARTING_DICTIONARY_SIZE = 5;

	// Token: 0x0400297D RID: 10621
	[SerializeField]
	private CanvasGroup m_relicIconGroup;

	// Token: 0x0400297E RID: 10622
	[SerializeField]
	private RelicHUDIconController m_relicIconTemplate;

	// Token: 0x0400297F RID: 10623
	private List<RelicHUDIconController> m_relicIconPool;

	// Token: 0x04002980 RID: 10624
	private Dictionary<RelicType, RelicHUDIconController> m_activeRelicIconDict;

	// Token: 0x04002981 RID: 10625
	private Comparison<RelicHUDIconController> m_iconSortMethod;

	// Token: 0x04002982 RID: 10626
	private Action<MonoBehaviour, EventArgs> m_updateAllRelics;

	// Token: 0x04002983 RID: 10627
	private Action<MonoBehaviour, EventArgs> m_showCanvasGroup;

	// Token: 0x04002984 RID: 10628
	private Action<MonoBehaviour, EventArgs> m_hideCanvasGroup;

	// Token: 0x04002985 RID: 10629
	private Action<MonoBehaviour, EventArgs> m_onRelicChanged;

	// Token: 0x04002986 RID: 10630
	private Action<MonoBehaviour, EventArgs> m_onRelicPurified;
}
