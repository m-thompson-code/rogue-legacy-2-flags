using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000686 RID: 1670
public class TeleporterEntry : MonoBehaviour
{
	// Token: 0x1700137E RID: 4990
	// (get) Token: 0x06003309 RID: 13065 RVA: 0x0001BF96 File Offset: 0x0001A196
	public List<TeleporterSubEntry> SubEntriesList
	{
		get
		{
			return this.m_subEntriesList;
		}
	}

	// Token: 0x1700137F RID: 4991
	// (get) Token: 0x0600330A RID: 13066 RVA: 0x0001BF9E File Offset: 0x0001A19E
	// (set) Token: 0x0600330B RID: 13067 RVA: 0x0001BFA6 File Offset: 0x0001A1A6
	public BiomeType BiomeType { get; private set; }

	// Token: 0x0600330C RID: 13068 RVA: 0x0001BFAF File Offset: 0x0001A1AF
	public void Initialize()
	{
		this.m_subEntriesList = new List<TeleporterSubEntry>();
		this.m_locItem = this.m_entryTitleText.GetComponent<LocalizationItem>();
	}

	// Token: 0x0600330D RID: 13069 RVA: 0x000DA694 File Offset: 0x000D8894
	public void SetBiomeType(BiomeType biomeType)
	{
		this.BiomeType = biomeType;
		BiomeData data = BiomeDataLibrary.GetData(biomeType);
		if (data)
		{
			this.m_locItem.SetString(data.BiomeNameLocID);
		}
		BiomeArtData artData = BiomeArtDataLibrary.GetArtData(biomeType);
		if (artData)
		{
			this.m_entryIcon.color = artData.Ferr2DBiomeArtData.MapColor;
		}
	}

	// Token: 0x0600330E RID: 13070 RVA: 0x000DA6F0 File Offset: 0x000D88F0
	public void AddSubEntry(GlobalTeleporterType subEntryType, GridPointManager gridPointManager)
	{
		TeleporterSubEntry teleporterSubEntry = UnityEngine.Object.Instantiate<TeleporterSubEntry>(this.m_teleporterSubEntryPrefab, this.m_teleporterSubEntries.transform.transform);
		teleporterSubEntry.SetSubEntryType(subEntryType, gridPointManager.BiomeControllerIndex, this.BiomeType, gridPointManager);
		teleporterSubEntry.gameObject.SetActive(false);
		this.m_subEntriesList.Add(teleporterSubEntry);
	}

	// Token: 0x040029A9 RID: 10665
	[SerializeField]
	private TeleporterSubEntry m_teleporterSubEntryPrefab;

	// Token: 0x040029AA RID: 10666
	[SerializeField]
	private TMP_Text m_entryTitleText;

	// Token: 0x040029AB RID: 10667
	[SerializeField]
	private Image m_entryIcon;

	// Token: 0x040029AC RID: 10668
	[SerializeField]
	private RectTransform m_teleporterSubEntries;

	// Token: 0x040029AD RID: 10669
	private LocalizationItem m_locItem;

	// Token: 0x040029AE RID: 10670
	private List<TeleporterSubEntry> m_subEntriesList;
}
