using System;
using UnityEngine;

// Token: 0x02000605 RID: 1541
public class InteractIconController : MonoBehaviour
{
	// Token: 0x170012A4 RID: 4772
	// (get) Token: 0x06002F71 RID: 12145 RVA: 0x00019F3A File Offset: 0x0001813A
	// (set) Token: 0x06002F72 RID: 12146 RVA: 0x00019F42 File Offset: 0x00018142
	public bool IsIconVisible { get; private set; }

	// Token: 0x06002F73 RID: 12147 RVA: 0x00019F4B File Offset: 0x0001814B
	private void Awake()
	{
		this.m_storedScale = this.m_interactIcon.transform.localScale;
		this.m_interactIcon.SetActive(false);
	}

	// Token: 0x06002F74 RID: 12148 RVA: 0x000CA4F0 File Offset: 0x000C86F0
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

	// Token: 0x040026D1 RID: 9937
	[SerializeField]
	private GameObject m_interactIcon;

	// Token: 0x040026D2 RID: 9938
	private Tween m_iconDisplayTween;

	// Token: 0x040026D3 RID: 9939
	private Vector3 m_storedScale;
}
