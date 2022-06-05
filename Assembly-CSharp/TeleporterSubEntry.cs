using System;
using TMPro;
using UnityEngine;

// Token: 0x02000687 RID: 1671
public class TeleporterSubEntry : MonoBehaviour
{
	// Token: 0x17001380 RID: 4992
	// (get) Token: 0x06003310 RID: 13072 RVA: 0x0001BFCD File Offset: 0x0001A1CD
	// (set) Token: 0x06003311 RID: 13073 RVA: 0x0001BFD5 File Offset: 0x0001A1D5
	public int BiomeControllerIndex { get; private set; }

	// Token: 0x17001381 RID: 4993
	// (get) Token: 0x06003312 RID: 13074 RVA: 0x0001BFDE File Offset: 0x0001A1DE
	// (set) Token: 0x06003313 RID: 13075 RVA: 0x0001BFE6 File Offset: 0x0001A1E6
	public GlobalTeleporterType SubEntryType { get; private set; }

	// Token: 0x17001382 RID: 4994
	// (get) Token: 0x06003314 RID: 13076 RVA: 0x0001BFEF File Offset: 0x0001A1EF
	// (set) Token: 0x06003315 RID: 13077 RVA: 0x0001BFF7 File Offset: 0x0001A1F7
	public GridPointManager GridPointManager { get; private set; }

	// Token: 0x17001383 RID: 4995
	// (get) Token: 0x06003316 RID: 13078 RVA: 0x0001C000 File Offset: 0x0001A200
	// (set) Token: 0x06003317 RID: 13079 RVA: 0x0001C008 File Offset: 0x0001A208
	public BiomeType BiomeType { get; private set; }

	// Token: 0x06003318 RID: 13080 RVA: 0x0001C011 File Offset: 0x0001A211
	private string GetLocID(GlobalTeleporterType subEntryType)
	{
		if (subEntryType <= GlobalTeleporterType.BossEntrance)
		{
			if (subEntryType == GlobalTeleporterType.TransitionRoom)
			{
				return "LOC_ID_MAP_ICON_SUBENTRY_TRANSITION_TELEPORTER_1";
			}
			if (subEntryType == GlobalTeleporterType.BossEntrance)
			{
				return "LOC_ID_MAP_ICON_SUBENTRY_BOSS_TELEPORTER_1";
			}
		}
		else
		{
			if (subEntryType == GlobalTeleporterType.SpecialRoom)
			{
				return "LOC_ID_MAP_ICON_SUBENTRY_SPECIAL_TELEPORTER_1";
			}
			if (subEntryType == GlobalTeleporterType.HeirloomRoom)
			{
				return "LOC_ID_MAP_ICON_SUBENTRY_HEIRLOOM_TELEPORTER_1";
			}
		}
		return null;
	}

	// Token: 0x06003319 RID: 13081 RVA: 0x000DA748 File Offset: 0x000D8948
	public void SetSubEntryType(GlobalTeleporterType subEntryType, int biomeControllerIndex, BiomeType biomeType, GridPointManager gridPointManager)
	{
		if (!this.m_locItem)
		{
			this.m_locItem = base.GetComponent<LocalizationItem>();
		}
		this.SubEntryType = subEntryType;
		this.m_locItem.SetString(this.GetLocID(this.SubEntryType));
		this.BiomeControllerIndex = biomeControllerIndex;
		this.BiomeType = biomeType;
		this.GridPointManager = gridPointManager;
		this.SetSelected(false);
	}

	// Token: 0x0600331A RID: 13082 RVA: 0x0001C049 File Offset: 0x0001A249
	public void SetSelected(bool selected)
	{
		if (selected)
		{
			this.m_text.color = TeleporterSubEntry.SELECTED_COLOR;
			return;
		}
		this.m_text.color = TeleporterSubEntry.NOT_SELECTED_COLOR;
	}

	// Token: 0x040029B0 RID: 10672
	private static Color NOT_SELECTED_COLOR = new Color(0.4627451f, 0.3882353f, 0.44705883f);

	// Token: 0x040029B1 RID: 10673
	private static Color SELECTED_COLOR = Color.white;

	// Token: 0x040029B2 RID: 10674
	[SerializeField]
	private TMP_Text m_text;

	// Token: 0x040029B3 RID: 10675
	private LocalizationItem m_locItem;
}
