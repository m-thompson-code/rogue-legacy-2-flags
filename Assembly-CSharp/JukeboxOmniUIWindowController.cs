using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000981 RID: 2433
public class JukeboxOmniUIWindowController : BaseOmniUIWindowController<BaseOmniUICategoryEntry, JukeboxOmniUIEntry>
{
	// Token: 0x170019E1 RID: 6625
	// (get) Token: 0x06004AB5 RID: 19125 RVA: 0x00028DD7 File Offset: 0x00026FD7
	public JukeboxSpectrumAnalyzer JukeboxSpectrum
	{
		get
		{
			return this.m_jukeboxSpectrum;
		}
	}

	// Token: 0x170019E2 RID: 6626
	// (get) Token: 0x06004AB6 RID: 19126 RVA: 0x00005315 File Offset: 0x00003515
	public override WindowID ID
	{
		get
		{
			return WindowID.Jukebox;
		}
	}

	// Token: 0x06004AB7 RID: 19127 RVA: 0x00002FCA File Offset: 0x000011CA
	protected override void CreateCategoryEntries()
	{
	}

	// Token: 0x06004AB8 RID: 19128 RVA: 0x001232FC File Offset: 0x001214FC
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

	// Token: 0x06004AB9 RID: 19129 RVA: 0x001233EC File Offset: 0x001215EC
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

	// Token: 0x06004ABA RID: 19130 RVA: 0x00028C7A File Offset: 0x00026E7A
	private void SetGameObjectActive(GameObject obj, bool active)
	{
		if (obj.activeSelf == !active)
		{
			obj.SetActive(active);
		}
	}

	// Token: 0x04003906 RID: 14598
	[SerializeField]
	private JukeboxSpectrumAnalyzer m_jukeboxSpectrum;
}
