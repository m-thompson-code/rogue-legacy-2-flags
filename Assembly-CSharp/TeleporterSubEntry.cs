using System;
using TMPro;
using UnityEngine;

// Token: 0x020003E6 RID: 998
public class TeleporterSubEntry : MonoBehaviour
{
	// Token: 0x17000EDF RID: 3807
	// (get) Token: 0x060024CE RID: 9422 RVA: 0x0007A6F5 File Offset: 0x000788F5
	// (set) Token: 0x060024CF RID: 9423 RVA: 0x0007A6FD File Offset: 0x000788FD
	public int BiomeControllerIndex { get; private set; }

	// Token: 0x17000EE0 RID: 3808
	// (get) Token: 0x060024D0 RID: 9424 RVA: 0x0007A706 File Offset: 0x00078906
	// (set) Token: 0x060024D1 RID: 9425 RVA: 0x0007A70E File Offset: 0x0007890E
	public GlobalTeleporterType SubEntryType { get; private set; }

	// Token: 0x17000EE1 RID: 3809
	// (get) Token: 0x060024D2 RID: 9426 RVA: 0x0007A717 File Offset: 0x00078917
	// (set) Token: 0x060024D3 RID: 9427 RVA: 0x0007A71F File Offset: 0x0007891F
	public GridPointManager GridPointManager { get; private set; }

	// Token: 0x17000EE2 RID: 3810
	// (get) Token: 0x060024D4 RID: 9428 RVA: 0x0007A728 File Offset: 0x00078928
	// (set) Token: 0x060024D5 RID: 9429 RVA: 0x0007A730 File Offset: 0x00078930
	public BiomeType BiomeType { get; private set; }

	// Token: 0x060024D6 RID: 9430 RVA: 0x0007A739 File Offset: 0x00078939
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

	// Token: 0x060024D7 RID: 9431 RVA: 0x0007A774 File Offset: 0x00078974
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

	// Token: 0x060024D8 RID: 9432 RVA: 0x0007A7D5 File Offset: 0x000789D5
	public void SetSelected(bool selected)
	{
		if (selected)
		{
			this.m_text.color = TeleporterSubEntry.SELECTED_COLOR;
			return;
		}
		this.m_text.color = TeleporterSubEntry.NOT_SELECTED_COLOR;
	}

	// Token: 0x04001F32 RID: 7986
	private static Color NOT_SELECTED_COLOR = new Color(0.4627451f, 0.3882353f, 0.44705883f);

	// Token: 0x04001F33 RID: 7987
	private static Color SELECTED_COLOR = Color.white;

	// Token: 0x04001F34 RID: 7988
	[SerializeField]
	private TMP_Text m_text;

	// Token: 0x04001F35 RID: 7989
	private LocalizationItem m_locItem;
}
