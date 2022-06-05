using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200044C RID: 1100
public class ChangeResolutionOptionItem : SelectionListOptionItem
{
	// Token: 0x17000F36 RID: 3894
	// (get) Token: 0x06002341 RID: 9025 RVA: 0x00003CD2 File Offset: 0x00001ED2
	protected override bool UsesLocID
	{
		get
		{
			return false;
		}
	}

	// Token: 0x06002342 RID: 9026 RVA: 0x00013010 File Offset: 0x00011210
	protected override void Awake()
	{
		base.Awake();
		this.m_onPrimaryDisplayChanged = new Action<MonoBehaviour, EventArgs>(this.OnPrimaryDisplayChanged);
	}

	// Token: 0x06002343 RID: 9027 RVA: 0x000ACB8C File Offset: 0x000AAD8C
	public override void Initialize()
	{
		Resolution[] resolutions = Screen.resolutions;
		this.m_resolutionsToUse.Clear();
		foreach (Resolution item in resolutions)
		{
			bool flag = true;
			foreach (Resolution resolution in this.m_resolutionsToUse)
			{
				if (item.width == resolution.width && item.height == resolution.height)
				{
					flag = false;
					break;
				}
			}
			if (flag)
			{
				this.m_resolutionsToUse.Add(item);
			}
		}
		this.m_selectionLocIDArray = new string[this.m_resolutionsToUse.Count];
		for (int j = 0; j < this.m_resolutionsToUse.Count; j++)
		{
			this.m_selectionLocIDArray[j] = this.m_resolutionsToUse[j].width.ToString() + "x" + this.m_resolutionsToUse[j].height.ToString();
		}
		this.m_selectedIndex = 0;
		for (int k = 0; k < this.m_resolutionsToUse.Count; k++)
		{
			if (this.m_resolutionsToUse[k].width == SaveManager.ConfigData.ScreenWidth && this.m_resolutionsToUse[k].height == SaveManager.ConfigData.ScreenHeight)
			{
				this.m_selectedIndex = k;
				break;
			}
		}
		base.Initialize();
	}

	// Token: 0x06002344 RID: 9028 RVA: 0x000129D0 File Offset: 0x00010BD0
	private void OnPrimaryDisplayChanged(object sender, EventArgs args)
	{
		this.Initialize();
	}

	// Token: 0x06002345 RID: 9029 RVA: 0x000ACD2C File Offset: 0x000AAF2C
	protected override void OnEnable()
	{
		base.OnEnable();
		if (base.IsInitialized)
		{
			this.m_selectedIndex = 0;
			for (int i = 0; i < this.m_resolutionsToUse.Count; i++)
			{
				if (SaveManager.ConfigData.ScreenWidth == this.m_resolutionsToUse[i].width && SaveManager.ConfigData.ScreenHeight == this.m_resolutionsToUse[i].height)
				{
					this.m_selectedIndex = i;
				}
			}
			this.m_incrementValueText.SetText(base.CurrentSelectionString, true);
		}
		Messenger<SceneMessenger, SceneEvent>.AddListener(SceneEvent.PrimaryDisplayChanged, this.m_onPrimaryDisplayChanged);
	}

	// Token: 0x06002346 RID: 9030 RVA: 0x0001302A File Offset: 0x0001122A
	protected override void OnDisable()
	{
		base.OnDisable();
		Messenger<SceneMessenger, SceneEvent>.RemoveListener(SceneEvent.PrimaryDisplayChanged, this.m_onPrimaryDisplayChanged);
	}

	// Token: 0x06002347 RID: 9031 RVA: 0x0001303E File Offset: 0x0001123E
	public override void InvokeValueChange()
	{
		Debug.Log("Change resolution to: " + this.m_selectionLocIDArray[this.m_selectedIndex]);
	}

	// Token: 0x06002348 RID: 9032 RVA: 0x000ACDCC File Offset: 0x000AAFCC
	public override void ConfirmOptionChange()
	{
		Resolution resolution = this.m_resolutionsToUse[this.m_selectedIndex];
		GameResolutionManager.SetResolution(resolution.width, resolution.height, GameResolutionManager.FullscreenMode, true);
	}

	// Token: 0x04001F92 RID: 8082
	private List<Resolution> m_resolutionsToUse = new List<Resolution>();

	// Token: 0x04001F93 RID: 8083
	private Action<MonoBehaviour, EventArgs> m_onPrimaryDisplayChanged;
}
