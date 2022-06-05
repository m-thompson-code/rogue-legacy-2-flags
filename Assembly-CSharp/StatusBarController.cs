using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000682 RID: 1666
public class StatusBarController : MonoBehaviour
{
	// Token: 0x060032CF RID: 13007 RVA: 0x000DA01C File Offset: 0x000D821C
	public bool HasActiveStatusBarEntry(StatusBarEntryType uiType)
	{
		foreach (StatusBarEntry statusBarEntry in this.m_activeStatusBarList)
		{
			if (statusBarEntry.isActiveAndEnabled && statusBarEntry.StatusBarType == uiType)
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x060032D0 RID: 13008 RVA: 0x000DA080 File Offset: 0x000D8280
	public StatusBarEntry GetActiveStatusBarEntry(StatusBarEntryType uiType)
	{
		foreach (StatusBarEntry statusBarEntry in this.m_activeStatusBarList)
		{
			if (statusBarEntry.isActiveAndEnabled && statusBarEntry.StatusBarType == uiType)
			{
				return statusBarEntry;
			}
		}
		return null;
	}

	// Token: 0x17001375 RID: 4981
	// (get) Token: 0x060032D1 RID: 13009 RVA: 0x0001BC67 File Offset: 0x00019E67
	// (set) Token: 0x060032D2 RID: 13010 RVA: 0x0001BC6F File Offset: 0x00019E6F
	public bool Active { get; set; } = true;

	// Token: 0x060032D3 RID: 13011 RVA: 0x0001BC78 File Offset: 0x00019E78
	private void Awake()
	{
		this.m_canvas = base.GetComponent<Canvas>();
		this.m_canvas.sortingOrder = 1;
		base.gameObject.SetActive(false);
	}

	// Token: 0x060032D4 RID: 13012 RVA: 0x000DA0E4 File Offset: 0x000D82E4
	public void ResetPositionAndScale(BaseCharacterController charController)
	{
		base.transform.localScale = Vector3.one;
		Vector3 lossyScale = base.transform.lossyScale;
		float num = this.m_baseScaleOverride;
		if (this.m_baseScaleOverride == 0f)
		{
			num = 1.15f;
		}
		base.transform.localScale = new Vector3(num / lossyScale.x, num / lossyScale.y, 1f);
		if (!this.m_disableAutoPosition && charController)
		{
			float y = charController.VisualBounds.max.y + 1f;
			base.transform.position = new Vector3(base.transform.position.x, y, base.transform.position.z);
		}
		base.transform.eulerAngles = Vector3.zero;
	}

	// Token: 0x060032D5 RID: 13013 RVA: 0x000DA1B8 File Offset: 0x000D83B8
	private void OnDisable()
	{
		foreach (StatusBarEntry statusBarEntry in this.m_activeStatusBarList)
		{
			statusBarEntry.gameObject.SetActive(false);
		}
		this.m_activeStatusBarList.Clear();
		this.SetCanvasVisible(true);
	}

	// Token: 0x060032D6 RID: 13014 RVA: 0x0001BC9E File Offset: 0x00019E9E
	public void ApplyUIEffect(StatusBarEntryType uiType)
	{
		if (!this.Active)
		{
			return;
		}
		if (!base.gameObject.activeSelf)
		{
			base.gameObject.SetActive(true);
		}
		this.GetInitializedStatusBarEntry(uiType).StartNoCounterOrTimer(uiType);
	}

	// Token: 0x060032D7 RID: 13015 RVA: 0x0001BCCF File Offset: 0x00019ECF
	public void ApplyUIEffect(StatusBarEntryType uiType, float duration)
	{
		if (!this.Active)
		{
			return;
		}
		if (!base.gameObject.activeSelf)
		{
			base.gameObject.SetActive(true);
		}
		if (base.gameObject.activeInHierarchy)
		{
			this.GetInitializedStatusBarEntry(uiType).StartTimer(uiType, duration);
		}
	}

	// Token: 0x060032D8 RID: 13016 RVA: 0x0001BD0E File Offset: 0x00019F0E
	public void ApplyUIEffect(StatusBarEntryType uiType, int maxCount, int currentCount)
	{
		if (!this.Active)
		{
			return;
		}
		if (!base.gameObject.activeSelf)
		{
			base.gameObject.SetActive(true);
		}
		this.GetInitializedStatusBarEntry(uiType).StartCounter(uiType, maxCount, currentCount);
	}

	// Token: 0x060032D9 RID: 13017 RVA: 0x000DA220 File Offset: 0x000D8420
	public void ApplyUIEffect(StatusBarEntryType uiType, float duration, int maxCount, int currentCount)
	{
		if (!this.Active)
		{
			return;
		}
		if (!base.gameObject.activeSelf)
		{
			base.gameObject.SetActive(true);
		}
		if (base.gameObject.activeInHierarchy)
		{
			this.GetInitializedStatusBarEntry(uiType).StartCounterAndTimer(uiType, duration, maxCount, currentCount);
		}
	}

	// Token: 0x060032DA RID: 13018 RVA: 0x000DA270 File Offset: 0x000D8470
	private StatusBarEntry GetInitializedStatusBarEntry(StatusBarEntryType uiType)
	{
		StatusBarEntry statusBarEntry = this.GetActiveStatusBarEntry(uiType);
		if (!statusBarEntry)
		{
			statusBarEntry = StatusBarEntryManager.GetFreeStatusBarEntry();
			statusBarEntry.gameObject.SetActive(true);
			statusBarEntry.transform.SetParent(base.transform);
			statusBarEntry.ResetScale();
			statusBarEntry.ResetPosition();
			this.m_activeStatusBarList.Add(statusBarEntry);
		}
		return statusBarEntry;
	}

	// Token: 0x060032DB RID: 13019 RVA: 0x000DA2CC File Offset: 0x000D84CC
	public void StopUIEffect(StatusBarEntryType uiType)
	{
		foreach (StatusBarEntry statusBarEntry in this.m_activeStatusBarList)
		{
			if (statusBarEntry.isActiveAndEnabled && statusBarEntry.StatusBarType == uiType)
			{
				statusBarEntry.StopTimer();
				statusBarEntry.gameObject.SetActive(false);
			}
		}
	}

	// Token: 0x060032DC RID: 13020 RVA: 0x000DA33C File Offset: 0x000D853C
	public void StopAllUIEffects()
	{
		foreach (StatusBarEntry statusBarEntry in this.m_activeStatusBarList)
		{
			if (statusBarEntry.isActiveAndEnabled)
			{
				statusBarEntry.StopTimer();
				statusBarEntry.gameObject.SetActive(false);
			}
		}
	}

	// Token: 0x060032DD RID: 13021 RVA: 0x000DA3A4 File Offset: 0x000D85A4
	private void Update()
	{
		if (this.m_activeStatusBarList.Count > 0)
		{
			for (int i = 0; i < this.m_activeStatusBarList.Count; i++)
			{
				if (!this.m_activeStatusBarList[i] || !this.m_activeStatusBarList[i].gameObject.activeSelf)
				{
					this.m_activeStatusBarList.RemoveAt(i);
					i--;
				}
			}
			return;
		}
		if (base.gameObject.activeSelf)
		{
			base.gameObject.SetActive(false);
		}
	}

	// Token: 0x060032DE RID: 13022 RVA: 0x0001BD41 File Offset: 0x00019F41
	public void RemoveStatusBarEntry(StatusBarEntry statusBar)
	{
		this.m_activeStatusBarList.Remove(statusBar);
	}

	// Token: 0x060032DF RID: 13023 RVA: 0x0001BD50 File Offset: 0x00019F50
	public void SetCanvasVisible(bool visible)
	{
		this.m_canvas.enabled = visible;
	}

	// Token: 0x0400298F RID: 10639
	[SerializeField]
	private bool m_disableAutoPosition;

	// Token: 0x04002990 RID: 10640
	[SerializeField]
	private float m_baseScaleOverride;

	// Token: 0x04002991 RID: 10641
	private Canvas m_canvas;

	// Token: 0x04002992 RID: 10642
	private List<StatusBarEntry> m_activeStatusBarList = new List<StatusBarEntry>();
}
