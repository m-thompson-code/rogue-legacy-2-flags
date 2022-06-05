using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020001E2 RID: 482
public class LevelEditorDebug_HUDController : MonoBehaviour
{
	// Token: 0x06001405 RID: 5125 RVA: 0x0003CB9C File Offset: 0x0003AD9C
	private void Start()
	{
		this.m_panel.SetActive(false);
		if (!Application.isEditor || !GameUtility.IsInLevelEditor)
		{
			base.enabled = false;
		}
	}

	// Token: 0x06001406 RID: 5126 RVA: 0x0003CBC0 File Offset: 0x0003ADC0
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

	// Token: 0x06001407 RID: 5127 RVA: 0x0003CCA0 File Offset: 0x0003AEA0
	private void Update()
	{
		if (Input.GetKeyDown("`"))
		{
			this.TogglePanel();
		}
	}

	// Token: 0x06001408 RID: 5128 RVA: 0x0003CCB4 File Offset: 0x0003AEB4
	public void SetBiome(int selectionIndex)
	{
		bool isDropdownInitialised = this.m_isDropdownInitialised;
	}

	// Token: 0x06001409 RID: 5129 RVA: 0x0003CCBD File Offset: 0x0003AEBD
	public void SetTransitionPointBiome(int selectionIndex)
	{
		bool isDropdownInitialised = this.m_isDropdownInitialised;
	}

	// Token: 0x0600140A RID: 5130 RVA: 0x0003CCC6 File Offset: 0x0003AEC6
	private void SetHotKeyText()
	{
	}

	// Token: 0x0600140B RID: 5131 RVA: 0x0003CCC8 File Offset: 0x0003AEC8
	public void ToggleCameraConstrainerIsEnabled()
	{
	}

	// Token: 0x0600140C RID: 5132 RVA: 0x0003CCCA File Offset: 0x0003AECA
	private void TogglePanel()
	{
		this.m_panel.SetActive(!this.m_panel.activeInHierarchy);
	}

	// Token: 0x040013CB RID: 5067
	[SerializeField]
	private GameObject m_panel;

	// Token: 0x040013CC RID: 5068
	[SerializeField]
	private Toggle m_cameraConstrainerToggle;

	// Token: 0x040013CD RID: 5069
	[SerializeField]
	private TMP_Dropdown m_dropdown;

	// Token: 0x040013CE RID: 5070
	[SerializeField]
	private TMP_Dropdown m_transitionPointDropdown;

	// Token: 0x040013CF RID: 5071
	[SerializeField]
	private TextMeshProUGUI m_cycleBiomeHotKey;

	// Token: 0x040013D0 RID: 5072
	[SerializeField]
	private TextMeshProUGUI m_cycleCloseDoorHotKey;

	// Token: 0x040013D1 RID: 5073
	[SerializeField]
	private TextMeshProUGUI m_cycleCloseAllDoorsHotKey;

	// Token: 0x040013D2 RID: 5074
	[SerializeField]
	private TextMeshProUGUI m_flipRoom;

	// Token: 0x040013D3 RID: 5075
	[SerializeField]
	private TextMeshProUGUI m_randomizeHazardsHotKey;

	// Token: 0x040013D4 RID: 5076
	[SerializeField]
	private TextMeshProUGUI m_difficultyHotKey;

	// Token: 0x040013D5 RID: 5077
	private Dictionary<int, BiomeType> m_indexToBiomeTable;

	// Token: 0x040013D6 RID: 5078
	private Dictionary<int, BiomeType> m_indexToTransitionPointBiomeTable = new Dictionary<int, BiomeType>
	{
		{
			0,
			BiomeType.None
		}
	};

	// Token: 0x040013D7 RID: 5079
	private bool m_isDropdownInitialised;
}
