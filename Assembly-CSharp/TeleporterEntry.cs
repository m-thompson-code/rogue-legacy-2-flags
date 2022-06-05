using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020003E5 RID: 997
public class TeleporterEntry : MonoBehaviour
{
	// Token: 0x17000EDD RID: 3805
	// (get) Token: 0x060024C7 RID: 9415 RVA: 0x0007A605 File Offset: 0x00078805
	public List<TeleporterSubEntry> SubEntriesList
	{
		get
		{
			return this.m_subEntriesList;
		}
	}

	// Token: 0x17000EDE RID: 3806
	// (get) Token: 0x060024C8 RID: 9416 RVA: 0x0007A60D File Offset: 0x0007880D
	// (set) Token: 0x060024C9 RID: 9417 RVA: 0x0007A615 File Offset: 0x00078815
	public BiomeType BiomeType { get; private set; }

	// Token: 0x060024CA RID: 9418 RVA: 0x0007A61E File Offset: 0x0007881E
	public void Initialize()
	{
		this.m_subEntriesList = new List<TeleporterSubEntry>();
		this.m_locItem = this.m_entryTitleText.GetComponent<LocalizationItem>();
	}

	// Token: 0x060024CB RID: 9419 RVA: 0x0007A63C File Offset: 0x0007883C
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

	// Token: 0x060024CC RID: 9420 RVA: 0x0007A698 File Offset: 0x00078898
	public void AddSubEntry(GlobalTeleporterType subEntryType, GridPointManager gridPointManager)
	{
		TeleporterSubEntry teleporterSubEntry = UnityEngine.Object.Instantiate<TeleporterSubEntry>(this.m_teleporterSubEntryPrefab, this.m_teleporterSubEntries.transform.transform);
		teleporterSubEntry.SetSubEntryType(subEntryType, gridPointManager.BiomeControllerIndex, this.BiomeType, gridPointManager);
		teleporterSubEntry.gameObject.SetActive(false);
		this.m_subEntriesList.Add(teleporterSubEntry);
	}

	// Token: 0x04001F2B RID: 7979
	[SerializeField]
	private TeleporterSubEntry m_teleporterSubEntryPrefab;

	// Token: 0x04001F2C RID: 7980
	[SerializeField]
	private TMP_Text m_entryTitleText;

	// Token: 0x04001F2D RID: 7981
	[SerializeField]
	private Image m_entryIcon;

	// Token: 0x04001F2E RID: 7982
	[SerializeField]
	private RectTransform m_teleporterSubEntries;

	// Token: 0x04001F2F RID: 7983
	private LocalizationItem m_locItem;

	// Token: 0x04001F30 RID: 7984
	private List<TeleporterSubEntry> m_subEntriesList;
}
