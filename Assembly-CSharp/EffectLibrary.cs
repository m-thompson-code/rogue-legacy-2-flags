using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020003DC RID: 988
[CreateAssetMenu(menuName = "Custom/Libraries/Effect Library")]
public class EffectLibrary : ScriptableObject
{
	// Token: 0x17000E4C RID: 3660
	// (get) Token: 0x0600201E RID: 8222 RVA: 0x0001106F File Offset: 0x0000F26F
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

	// Token: 0x0600201F RID: 8223 RVA: 0x00011098 File Offset: 0x0000F298
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

	// Token: 0x06002020 RID: 8224 RVA: 0x000A4600 File Offset: 0x000A2800
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

	// Token: 0x06002021 RID: 8225 RVA: 0x000110A0 File Offset: 0x0000F2A0
	public static EffectEntry GetEffectEntry(EffectCategoryType category, int index)
	{
		return EffectLibrary.GetEffectEntryList(category)[index];
	}

	// Token: 0x06002022 RID: 8226 RVA: 0x000110AE File Offset: 0x0000F2AE
	public static void AddEffectEntry(EffectCategoryType category, EffectEntry newEffectEntry)
	{
		EffectLibrary.GetEffectEntryList(category).Add(newEffectEntry);
	}

	// Token: 0x04001CC5 RID: 7365
	public const string RESOURCES_PATH = "Scriptable Objects/Libraries/EffectLibrary";

	// Token: 0x04001CC6 RID: 7366
	[SerializeField]
	private List<EffectEntry> m_genericEffectsList = new List<EffectEntry>();

	// Token: 0x04001CC7 RID: 7367
	[SerializeField]
	private List<EffectEntry> m_playerEffectsList = new List<EffectEntry>();

	// Token: 0x04001CC8 RID: 7368
	[SerializeField]
	private List<EffectEntry> m_playerInteractionEffectsList = new List<EffectEntry>();

	// Token: 0x04001CC9 RID: 7369
	[SerializeField]
	private List<EffectEntry> m_enemyEffectsList = new List<EffectEntry>();

	// Token: 0x04001CCA RID: 7370
	[SerializeField]
	private List<EffectEntry> m_propsEffectList = new List<EffectEntry>();

	// Token: 0x04001CCB RID: 7371
	[SerializeField]
	private List<EffectEntry> m_projectilesEffectList = new List<EffectEntry>();

	// Token: 0x04001CCC RID: 7372
	[SerializeField]
	private List<EffectEntry> m_selfAnimationEffectList = new List<EffectEntry>();

	// Token: 0x04001CCD RID: 7373
	[SerializeField]
	private List<EffectEntry> m_uiEffectList = new List<EffectEntry>();

	// Token: 0x04001CCE RID: 7374
	private static EffectLibrary m_instance;
}
