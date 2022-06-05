using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020003E2 RID: 994
public class StatusBarController : MonoBehaviour
{
	// Token: 0x06002493 RID: 9363 RVA: 0x00079D34 File Offset: 0x00077F34
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

	// Token: 0x06002494 RID: 9364 RVA: 0x00079D98 File Offset: 0x00077F98
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

	// Token: 0x17000ED6 RID: 3798
	// (get) Token: 0x06002495 RID: 9365 RVA: 0x00079DFC File Offset: 0x00077FFC
	// (set) Token: 0x06002496 RID: 9366 RVA: 0x00079E04 File Offset: 0x00078004
	public bool Active { get; set; } = true;

	// Token: 0x06002497 RID: 9367 RVA: 0x00079E0D File Offset: 0x0007800D
	private void Awake()
	{
		this.m_canvas = base.GetComponent<Canvas>();
		this.m_canvas.sortingOrder = 1;
		base.gameObject.SetActive(false);
	}

	// Token: 0x06002498 RID: 9368 RVA: 0x00079E34 File Offset: 0x00078034
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

	// Token: 0x06002499 RID: 9369 RVA: 0x00079F08 File Offset: 0x00078108
	private void OnDisable()
	{
		foreach (StatusBarEntry statusBarEntry in this.m_activeStatusBarList)
		{
			statusBarEntry.gameObject.SetActive(false);
		}
		this.m_activeStatusBarList.Clear();
		this.SetCanvasVisible(true);
	}

	// Token: 0x0600249A RID: 9370 RVA: 0x00079F70 File Offset: 0x00078170
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

	// Token: 0x0600249B RID: 9371 RVA: 0x00079FA1 File Offset: 0x000781A1
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

	// Token: 0x0600249C RID: 9372 RVA: 0x00079FE0 File Offset: 0x000781E0
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

	// Token: 0x0600249D RID: 9373 RVA: 0x0007A014 File Offset: 0x00078214
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

	// Token: 0x0600249E RID: 9374 RVA: 0x0007A064 File Offset: 0x00078264
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

	// Token: 0x0600249F RID: 9375 RVA: 0x0007A0C0 File Offset: 0x000782C0
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

	// Token: 0x060024A0 RID: 9376 RVA: 0x0007A130 File Offset: 0x00078330
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

	// Token: 0x060024A1 RID: 9377 RVA: 0x0007A198 File Offset: 0x00078398
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

	// Token: 0x060024A2 RID: 9378 RVA: 0x0007A21E File Offset: 0x0007841E
	public void RemoveStatusBarEntry(StatusBarEntry statusBar)
	{
		this.m_activeStatusBarList.Remove(statusBar);
	}

	// Token: 0x060024A3 RID: 9379 RVA: 0x0007A22D File Offset: 0x0007842D
	public void SetCanvasVisible(bool visible)
	{
		this.m_canvas.enabled = visible;
	}

	// Token: 0x04001F17 RID: 7959
	[SerializeField]
	private bool m_disableAutoPosition;

	// Token: 0x04001F18 RID: 7960
	[SerializeField]
	private float m_baseScaleOverride;

	// Token: 0x04001F19 RID: 7961
	private Canvas m_canvas;

	// Token: 0x04001F1A RID: 7962
	private List<StatusBarEntry> m_activeStatusBarList = new List<StatusBarEntry>();
}
