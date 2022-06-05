using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000281 RID: 641
public class ChangeResolutionOptionItem : SelectionListOptionItem
{
	// Token: 0x17000BF5 RID: 3061
	// (get) Token: 0x06001952 RID: 6482 RVA: 0x0004F686 File Offset: 0x0004D886
	protected override bool UsesLocID
	{
		get
		{
			return false;
		}
	}

	// Token: 0x06001953 RID: 6483 RVA: 0x0004F689 File Offset: 0x0004D889
	protected override void Awake()
	{
		base.Awake();
		this.m_onPrimaryDisplayChanged = new Action<MonoBehaviour, EventArgs>(this.OnPrimaryDisplayChanged);
	}

	// Token: 0x06001954 RID: 6484 RVA: 0x0004F6A4 File Offset: 0x0004D8A4
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

	// Token: 0x06001955 RID: 6485 RVA: 0x0004F844 File Offset: 0x0004DA44
	private void OnPrimaryDisplayChanged(object sender, EventArgs args)
	{
		this.Initialize();
	}

	// Token: 0x06001956 RID: 6486 RVA: 0x0004F84C File Offset: 0x0004DA4C
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

	// Token: 0x06001957 RID: 6487 RVA: 0x0004F8E9 File Offset: 0x0004DAE9
	protected override void OnDisable()
	{
		base.OnDisable();
		Messenger<SceneMessenger, SceneEvent>.RemoveListener(SceneEvent.PrimaryDisplayChanged, this.m_onPrimaryDisplayChanged);
	}

	// Token: 0x06001958 RID: 6488 RVA: 0x0004F8FD File Offset: 0x0004DAFD
	public override void InvokeValueChange()
	{
		Debug.Log("Change resolution to: " + this.m_selectionLocIDArray[this.m_selectedIndex]);
	}

	// Token: 0x06001959 RID: 6489 RVA: 0x0004F91C File Offset: 0x0004DB1C
	public override void ConfirmOptionChange()
	{
		Resolution resolution = this.m_resolutionsToUse[this.m_selectedIndex];
		GameResolutionManager.SetResolution(resolution.width, resolution.height, GameResolutionManager.FullscreenMode, true);
	}

	// Token: 0x04001845 RID: 6213
	private List<Resolution> m_resolutionsToUse = new List<Resolution>();

	// Token: 0x04001846 RID: 6214
	private Action<MonoBehaviour, EventArgs> m_onPrimaryDisplayChanged;
}
