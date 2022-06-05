using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000226 RID: 550
[CreateAssetMenu(menuName = "Custom/Libraries/Effect Library")]
public class EffectLibrary : ScriptableObject
{
	// Token: 0x17000B25 RID: 2853
	// (get) Token: 0x0600167F RID: 5759 RVA: 0x00046301 File Offset: 0x00044501
	public static EffectLibrary Instance
	{
		get
		{
			if (EffectLibrary.m_instance == null)
			{
				EffectLibrary.m_instance = CDGResources.Load<EffectLibrary>("Scriptable Objects/Libraries/EffectLibrary", "", true);
			}
			return EffectLibrary.m_instance;
		}
	}

	// Token: 0x06001680 RID: 5760 RVA: 0x0004632A File Offset: 0x0004452A
	public static IEnumerator LoadAsync()
	{
		CDGAsyncLoadRequest<EffectLibrary> library = CDGResources.LoadAsync<EffectLibrary>("Scriptable Objects/Libraries/EffectLibrary", "");
		while (!library.IsDone)
		{
			yield return null;
		}
		EffectLibrary.m_instance = library.Asset;
		yield break;
	}

	// Token: 0x06001681 RID: 5761 RVA: 0x00046334 File Offset: 0x00044534
	public static List<EffectEntry> GetEffectEntryList(EffectCategoryType category)
	{
		if (category <= EffectCategoryType.Enemies)
		{
			if (category <= EffectCategoryType.Player)
			{
				if (category == EffectCategoryType.Generic)
				{
					return EffectLibrary.Instance.m_genericEffectsList;
				}
				if (category == EffectCategoryType.Player)
				{
					return EffectLibrary.Instance.m_playerEffectsList;
				}
			}
			else
			{
				if (category == EffectCategoryType.PlayerInteraction)
				{
					return EffectLibrary.Instance.m_playerInteractionEffectsList;
				}
				if (category == EffectCategoryType.Enemies)
				{
					return EffectLibrary.Instance.m_enemyEffectsList;
				}
			}
		}
		else if (category <= EffectCategoryType.Projectiles)
		{
			if (category == EffectCategoryType.Props)
			{
				return EffectLibrary.Instance.m_propsEffectList;
			}
			if (category == EffectCategoryType.Projectiles)
			{
				return EffectLibrary.Instance.m_projectilesEffectList;
			}
		}
		else
		{
			if (category == EffectCategoryType.SelfAnimation)
			{
				return EffectLibrary.Instance.m_selfAnimationEffectList;
			}
			if (category == EffectCategoryType.UI)
			{
				return EffectLibrary.Instance.m_uiEffectList;
			}
		}
		return null;
	}

	// Token: 0x06001682 RID: 5762 RVA: 0x000463DA File Offset: 0x000445DA
	public static EffectEntry GetEffectEntry(EffectCategoryType category, int index)
	{
		return EffectLibrary.GetEffectEntryList(category)[index];
	}

	// Token: 0x06001683 RID: 5763 RVA: 0x000463E8 File Offset: 0x000445E8
	public static void AddEffectEntry(EffectCategoryType category, EffectEntry newEffectEntry)
	{
		EffectLibrary.GetEffectEntryList(category).Add(newEffectEntry);
	}

	// Token: 0x040015B8 RID: 5560
	public const string RESOURCES_PATH = "Scriptable Objects/Libraries/EffectLibrary";

	// Token: 0x040015B9 RID: 5561
	[SerializeField]
	private List<EffectEntry> m_genericEffectsList = new List<EffectEntry>();

	// Token: 0x040015BA RID: 5562
	[SerializeField]
	private List<EffectEntry> m_playerEffectsList = new List<EffectEntry>();

	// Token: 0x040015BB RID: 5563
	[SerializeField]
	private List<EffectEntry> m_playerInteractionEffectsList = new List<EffectEntry>();

	// Token: 0x040015BC RID: 5564
	[SerializeField]
	private List<EffectEntry> m_enemyEffectsList = new List<EffectEntry>();

	// Token: 0x040015BD RID: 5565
	[SerializeField]
	private List<EffectEntry> m_propsEffectList = new List<EffectEntry>();

	// Token: 0x040015BE RID: 5566
	[SerializeField]
	private List<EffectEntry> m_projectilesEffectList = new List<EffectEntry>();

	// Token: 0x040015BF RID: 5567
	[SerializeField]
	private List<EffectEntry> m_selfAnimationEffectList = new List<EffectEntry>();

	// Token: 0x040015C0 RID: 5568
	[SerializeField]
	private List<EffectEntry> m_uiEffectList = new List<EffectEntry>();

	// Token: 0x040015C1 RID: 5569
	private static EffectLibrary m_instance;
}
