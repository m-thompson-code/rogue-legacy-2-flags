using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020003DD RID: 989
public class RelicHUDController : MonoBehaviour
{
	// Token: 0x06002474 RID: 9332 RVA: 0x0007946C File Offset: 0x0007766C
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

	// Token: 0x06002475 RID: 9333 RVA: 0x000795B0 File Offset: 0x000777B0
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

	// Token: 0x06002476 RID: 9334 RVA: 0x00079614 File Offset: 0x00077814
	private void OnEnable()
	{
		Messenger<GameMessenger, GameEvent>.AddListener(GameEvent.RelicLevelChanged, this.m_onRelicChanged);
		Messenger<GameMessenger, GameEvent>.AddListener(GameEvent.RelicStatsChanged, this.m_onRelicChanged);
		Messenger<GameMessenger, GameEvent>.AddListener(GameEvent.RelicPurified, this.m_onRelicPurified);
		Messenger<UIMessenger, UIEvent>.AddListener(UIEvent.SkillTree_Opened, this.m_hideCanvasGroup);
		Messenger<UIMessenger, UIEvent>.AddListener(UIEvent.SkillTree_Closed, this.m_showCanvasGroup);
	}

	// Token: 0x06002477 RID: 9335 RVA: 0x00079664 File Offset: 0x00077864
	private void OnDisable()
	{
		Messenger<GameMessenger, GameEvent>.RemoveListener(GameEvent.RelicLevelChanged, this.m_onRelicChanged);
		Messenger<GameMessenger, GameEvent>.RemoveListener(GameEvent.RelicStatsChanged, this.m_onRelicChanged);
		Messenger<GameMessenger, GameEvent>.RemoveListener(GameEvent.RelicPurified, this.m_onRelicPurified);
		Messenger<UIMessenger, UIEvent>.RemoveListener(UIEvent.SkillTree_Opened, this.m_hideCanvasGroup);
		Messenger<UIMessenger, UIEvent>.RemoveListener(UIEvent.SkillTree_Closed, this.m_showCanvasGroup);
	}

	// Token: 0x06002478 RID: 9336 RVA: 0x000796B2 File Offset: 0x000778B2
	private void ShowCanvasGroup(object sender, EventArgs args)
	{
		if (this.m_activeRelicIconDict.Count > 0)
		{
			this.m_relicIconGroup.gameObject.SetActive(true);
		}
	}

	// Token: 0x06002479 RID: 9337 RVA: 0x000796D3 File Offset: 0x000778D3
	private void HideCanvasGroup(object sender, EventArgs args)
	{
		this.m_relicIconGroup.gameObject.SetActive(false);
	}

	// Token: 0x0600247A RID: 9338 RVA: 0x000796E8 File Offset: 0x000778E8
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

	// Token: 0x0600247B RID: 9339 RVA: 0x000797A4 File Offset: 0x000779A4
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

	// Token: 0x0600247C RID: 9340 RVA: 0x0007981C File Offset: 0x00077A1C
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

	// Token: 0x0600247D RID: 9341 RVA: 0x00079844 File Offset: 0x00077A44
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

	// Token: 0x0600247E RID: 9342 RVA: 0x000798F0 File Offset: 0x00077AF0
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

	// Token: 0x0600247F RID: 9343 RVA: 0x000799A4 File Offset: 0x00077BA4
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

	// Token: 0x06002480 RID: 9344 RVA: 0x00079A24 File Offset: 0x00077C24
	private void OnRelicPurified(MonoBehaviour sender, EventArgs args)
	{
		RelicChangedEventArgs relicChangedEventArgs = args as RelicChangedEventArgs;
		if (this.m_activeRelicIconDict.ContainsKey(relicChangedEventArgs.RelicType))
		{
			this.m_activeRelicIconDict[relicChangedEventArgs.RelicType].PlayPurifyEffect();
		}
	}

	// Token: 0x04001F04 RID: 7940
	private const int STARTING_DICTIONARY_SIZE = 5;

	// Token: 0x04001F05 RID: 7941
	[SerializeField]
	private CanvasGroup m_relicIconGroup;

	// Token: 0x04001F06 RID: 7942
	[SerializeField]
	private RelicHUDIconController m_relicIconTemplate;

	// Token: 0x04001F07 RID: 7943
	private List<RelicHUDIconController> m_relicIconPool;

	// Token: 0x04001F08 RID: 7944
	private Dictionary<RelicType, RelicHUDIconController> m_activeRelicIconDict;

	// Token: 0x04001F09 RID: 7945
	private Comparison<RelicHUDIconController> m_iconSortMethod;

	// Token: 0x04001F0A RID: 7946
	private Action<MonoBehaviour, EventArgs> m_updateAllRelics;

	// Token: 0x04001F0B RID: 7947
	private Action<MonoBehaviour, EventArgs> m_showCanvasGroup;

	// Token: 0x04001F0C RID: 7948
	private Action<MonoBehaviour, EventArgs> m_hideCanvasGroup;

	// Token: 0x04001F0D RID: 7949
	private Action<MonoBehaviour, EventArgs> m_onRelicChanged;

	// Token: 0x04001F0E RID: 7950
	private Action<MonoBehaviour, EventArgs> m_onRelicPurified;
}
