using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000589 RID: 1417
public class JukeboxOmniUIWindowController : BaseOmniUIWindowController<BaseOmniUICategoryEntry, JukeboxOmniUIEntry>
{
	// Token: 0x170012D0 RID: 4816
	// (get) Token: 0x060034F8 RID: 13560 RVA: 0x000B66ED File Offset: 0x000B48ED
	public JukeboxSpectrumAnalyzer JukeboxSpectrum
	{
		get
		{
			return this.m_jukeboxSpectrum;
		}
	}

	// Token: 0x170012D1 RID: 4817
	// (get) Token: 0x060034F9 RID: 13561 RVA: 0x000B66F5 File Offset: 0x000B48F5
	public override WindowID ID
	{
		get
		{
			return WindowID.Jukebox;
		}
	}

	// Token: 0x060034FA RID: 13562 RVA: 0x000B66F9 File Offset: 0x000B48F9
	protected override void CreateCategoryEntries()
	{
	}

	// Token: 0x060034FB RID: 13563 RVA: 0x000B66FC File Offset: 0x000B48FC
	protected override void CreateEntries()
	{
		if (base.EntryArray != null)
		{
			Array.Clear(base.EntryArray, 0, base.EntryArray.Length);
			base.EntryArray = null;
		}
		int count = Jukebox_EV.JukeboxDataDict.Count;
		int num = 0;
		base.EntryArray = new JukeboxOmniUIEntry[count];
		foreach (KeyValuePair<SongID, JukeboxData> keyValuePair in Jukebox_EV.JukeboxDataDict)
		{
			SongID key = keyValuePair.Key;
			JukeboxData value = keyValuePair.Value;
			base.EntryArray[num] = UnityEngine.Object.Instantiate<JukeboxOmniUIEntry>(this.m_entryPrefab, base.EntryLayoutGroup.transform);
			base.EntryArray[num].transform.localScale = Vector3.one;
			base.EntryArray[num].Initialize(key, this);
			base.EntryArray[num].SetEntryIndex(num);
			num++;
		}
	}

	// Token: 0x060034FC RID: 13564 RVA: 0x000B67EC File Offset: 0x000B49EC
	protected override void UpdateScrollArrows(float scrollAmount)
	{
		base.UpdateScrollArrows(scrollAmount);
		if (base.TopScrollArrow.activeSelf || base.BottomScrollArrow.activeSelf)
		{
			float num = (float)base.ActiveEntryArray.Length * this.m_entryHeight;
			float num2 = Mathf.Clamp(1f - base.ScrollBar.value, 0f, 1f);
			float height = base.ContentViewport.rect.height;
			float num3 = height;
			float num4 = num3 - height;
			if (num2 > 0f)
			{
				num3 = height + (num - height) * num2;
				num4 = num3 - height;
			}
			int num5 = Mathf.CeilToInt(num4 / this.m_entryHeight);
			int num6 = Mathf.FloorToInt(num3 / this.m_entryHeight);
			if (base.TopScrollArrow.activeSelf)
			{
				bool flag = false;
				bool flag2 = false;
				for (int i = 0; i < num5; i++)
				{
					JukeboxOmniUIEntry jukeboxOmniUIEntry = base.ActiveEntryArray[i];
					if (SaveManager.PlayerSaveData.GetSongFoundState(jukeboxOmniUIEntry.SongType) == FoundState.FoundButNotViewed)
					{
						flag = true;
						break;
					}
				}
				if (flag)
				{
					this.SetGameObjectActive(base.TopScrollNewSymbol.gameObject, true);
					this.SetGameObjectActive(base.TopScrollUpgradeSymbol.gameObject, false);
				}
				else if (flag2)
				{
					this.SetGameObjectActive(base.TopScrollNewSymbol.gameObject, false);
					this.SetGameObjectActive(base.TopScrollUpgradeSymbol.gameObject, true);
				}
				else
				{
					this.SetGameObjectActive(base.TopScrollNewSymbol.gameObject, false);
					this.SetGameObjectActive(base.TopScrollUpgradeSymbol.gameObject, false);
				}
			}
			if (base.BottomScrollArrow.activeSelf)
			{
				bool flag3 = false;
				bool flag4 = false;
				for (int j = num6; j < base.ActiveEntryArray.Length; j++)
				{
					JukeboxOmniUIEntry jukeboxOmniUIEntry2 = base.ActiveEntryArray[j];
					if (SaveManager.PlayerSaveData.GetSongFoundState(jukeboxOmniUIEntry2.SongType) == FoundState.FoundButNotViewed)
					{
						flag3 = true;
						break;
					}
				}
				if (flag3)
				{
					this.SetGameObjectActive(base.BottomScrollNewSymbol.gameObject, true);
					this.SetGameObjectActive(base.BottomScrollUpgradeSymbol.gameObject, false);
					return;
				}
				if (flag4)
				{
					this.SetGameObjectActive(base.BottomScrollNewSymbol.gameObject, false);
					this.SetGameObjectActive(base.BottomScrollUpgradeSymbol.gameObject, true);
					return;
				}
				this.SetGameObjectActive(base.BottomScrollNewSymbol.gameObject, false);
				this.SetGameObjectActive(base.BottomScrollUpgradeSymbol.gameObject, false);
			}
		}
	}

	// Token: 0x060034FD RID: 13565 RVA: 0x000B6A2C File Offset: 0x000B4C2C
	private void SetGameObjectActive(GameObject obj, bool active)
	{
		if (obj.activeSelf == !active)
		{
			obj.SetActive(active);
		}
	}

	// Token: 0x0400294B RID: 10571
	[SerializeField]
	private JukeboxSpectrumAnalyzer m_jukeboxSpectrum;
}
