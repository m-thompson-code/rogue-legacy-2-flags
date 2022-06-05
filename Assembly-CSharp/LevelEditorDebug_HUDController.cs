using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000379 RID: 889
public class LevelEditorDebug_HUDController : MonoBehaviour
{
	// Token: 0x06001D16 RID: 7446 RVA: 0x0000EFED File Offset: 0x0000D1ED
	private void Start()
	{
		this.m_panel.SetActive(false);
		if (!Application.isEditor || !GameUtility.IsInLevelEditor)
		{
			base.enabled = false;
		}
	}

	// Token: 0x06001D17 RID: 7447 RVA: 0x0009BA58 File Offset: 0x00099C58
	private void OnWorldSetup(object sender, EventArgs e)
	{
		this.m_isDropdownInitialised = false;
		int num = 0;
		int valueWithoutNotify = 0;
		foreach (KeyValuePair<int, BiomeType> keyValuePair in this.m_indexToBiomeTable)
		{
			if (keyValuePair.Value == OnPlayManager.CurrentBiome)
			{
				valueWithoutNotify = num;
			}
			num++;
		}
		this.m_dropdown.SetValueWithoutNotify(valueWithoutNotify);
		num = 0;
		int valueWithoutNotify2 = 0;
		foreach (KeyValuePair<int, BiomeType> keyValuePair2 in this.m_indexToTransitionPointBiomeTable)
		{
			if (keyValuePair2.Value == OnPlayManager.TransitionPointBiome)
			{
				valueWithoutNotify2 = num;
			}
			num++;
		}
		this.m_transitionPointDropdown.SetValueWithoutNotify(valueWithoutNotify2);
		this.m_isDropdownInitialised = true;
	}

	// Token: 0x06001D18 RID: 7448 RVA: 0x0000F010 File Offset: 0x0000D210
	private void Update()
	{
		if (Input.GetKeyDown("`"))
		{
			this.TogglePanel();
		}
	}

	// Token: 0x06001D19 RID: 7449 RVA: 0x0000F024 File Offset: 0x0000D224
	public void SetBiome(int selectionIndex)
	{
		bool isDropdownInitialised = this.m_isDropdownInitialised;
	}

	// Token: 0x06001D1A RID: 7450 RVA: 0x0000F024 File Offset: 0x0000D224
	public void SetTransitionPointBiome(int selectionIndex)
	{
		bool isDropdownInitialised = this.m_isDropdownInitialised;
	}

	// Token: 0x06001D1B RID: 7451 RVA: 0x00002FCA File Offset: 0x000011CA
	private void SetHotKeyText()
	{
	}

	// Token: 0x06001D1C RID: 7452 RVA: 0x00002FCA File Offset: 0x000011CA
	public void ToggleCameraConstrainerIsEnabled()
	{
	}

	// Token: 0x06001D1D RID: 7453 RVA: 0x0000F02D File Offset: 0x0000D22D
	private void TogglePanel()
	{
		this.m_panel.SetActive(!this.m_panel.activeInHierarchy);
	}

	// Token: 0x04001A68 RID: 6760
	[SerializeField]
	private GameObject m_panel;

	// Token: 0x04001A69 RID: 6761
	[SerializeField]
	private Toggle m_cameraConstrainerToggle;

	// Token: 0x04001A6A RID: 6762
	[SerializeField]
	private TMP_Dropdown m_dropdown;

	// Token: 0x04001A6B RID: 6763
	[SerializeField]
	private TMP_Dropdown m_transitionPointDropdown;

	// Token: 0x04001A6C RID: 6764
	[SerializeField]
	private TextMeshProUGUI m_cycleBiomeHotKey;

	// Token: 0x04001A6D RID: 6765
	[SerializeField]
	private TextMeshProUGUI m_cycleCloseDoorHotKey;

	// Token: 0x04001A6E RID: 6766
	[SerializeField]
	private TextMeshProUGUI m_cycleCloseAllDoorsHotKey;

	// Token: 0x04001A6F RID: 6767
	[SerializeField]
	private TextMeshProUGUI m_flipRoom;

	// Token: 0x04001A70 RID: 6768
	[SerializeField]
	private TextMeshProUGUI m_randomizeHazardsHotKey;

	// Token: 0x04001A71 RID: 6769
	[SerializeField]
	private TextMeshProUGUI m_difficultyHotKey;

	// Token: 0x04001A72 RID: 6770
	private Dictionary<int, BiomeType> m_indexToBiomeTable;

	// Token: 0x04001A73 RID: 6771
	private Dictionary<int, BiomeType> m_indexToTransitionPointBiomeTable = new Dictionary<int, BiomeType>
	{
		{
			0,
			BiomeType.None
		}
	};

	// Token: 0x04001A74 RID: 6772
	private bool m_isDropdownInitialised;
}
