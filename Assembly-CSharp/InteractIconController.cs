using System;
using UnityEngine;

// Token: 0x0200037E RID: 894
public class InteractIconController : MonoBehaviour
{
	// Token: 0x17000E1D RID: 3613
	// (get) Token: 0x0600217D RID: 8573 RVA: 0x000695C4 File Offset: 0x000677C4
	// (set) Token: 0x0600217E RID: 8574 RVA: 0x000695CC File Offset: 0x000677CC
	public bool IsIconVisible { get; private set; }

	// Token: 0x0600217F RID: 8575 RVA: 0x000695D5 File Offset: 0x000677D5
	private void Awake()
	{
		this.m_storedScale = this.m_interactIcon.transform.localScale;
		this.m_interactIcon.SetActive(false);
	}

	// Token: 0x06002180 RID: 8576 RVA: 0x000695FC File Offset: 0x000677FC
	public void SetIconVisible(bool visible)
	{
		if (visible)
		{
			if (!this.IsIconVisible)
			{
				this.m_interactIcon.SetActive(true);
				this.m_interactIcon.transform.localScale = new Vector3(this.m_storedScale.x * 0.5f, this.m_storedScale.y * 0.5f, this.m_storedScale.z * 0.5f);
				if (this.m_iconDisplayTween != null)
				{
					this.m_iconDisplayTween.StopTweenWithConditionChecks(this, this.m_interactIcon.transform, null);
				}
				this.m_iconDisplayTween = TweenManager.TweenTo(this.m_interactIcon.transform, 0.15f, new EaseDelegate(Ease.Back.EaseOutLarge), new object[]
				{
					"localScale.x",
					this.m_storedScale.x,
					"localScale.y",
					this.m_storedScale.y,
					"localScale.z",
					this.m_storedScale.z
				});
				this.IsIconVisible = true;
				return;
			}
		}
		else if (this.IsIconVisible)
		{
			if (this.m_iconDisplayTween != null)
			{
				this.m_iconDisplayTween.StopTweenWithConditionChecks(this, this.m_interactIcon.transform, null);
			}
			this.m_interactIcon.SetActive(false);
			this.IsIconVisible = false;
		}
	}

	// Token: 0x04001CF9 RID: 7417
	[SerializeField]
	private GameObject m_interactIcon;

	// Token: 0x04001CFA RID: 7418
	private Tween m_iconDisplayTween;

	// Token: 0x04001CFB RID: 7419
	private Vector3 m_storedScale;
}
