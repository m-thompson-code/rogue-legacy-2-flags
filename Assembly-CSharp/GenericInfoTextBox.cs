using System;
using TMPro;
using UnityEngine;

// Token: 0x020005FD RID: 1533
public class GenericInfoTextBox : MonoBehaviour
{
	// Token: 0x17001293 RID: 4755
	// (get) Token: 0x06002F36 RID: 12086 RVA: 0x00019D79 File Offset: 0x00017F79
	public CanvasGroup CanvasGroup
	{
		get
		{
			return this.m_canvasGroup;
		}
	}

	// Token: 0x17001294 RID: 4756
	// (get) Token: 0x06002F37 RID: 12087 RVA: 0x00019D81 File Offset: 0x00017F81
	public TMP_Text HeaderText
	{
		get
		{
			return this.m_headerText;
		}
	}

	// Token: 0x17001295 RID: 4757
	// (get) Token: 0x06002F38 RID: 12088 RVA: 0x00019D89 File Offset: 0x00017F89
	public TMP_Text SubHeaderText
	{
		get
		{
			return this.m_subHeaderText;
		}
	}

	// Token: 0x17001296 RID: 4758
	// (get) Token: 0x06002F39 RID: 12089 RVA: 0x00019D91 File Offset: 0x00017F91
	public TMP_Text DescriptionText
	{
		get
		{
			return this.m_descriptionText;
		}
	}

	// Token: 0x17001297 RID: 4759
	// (get) Token: 0x06002F3A RID: 12090 RVA: 0x00019D99 File Offset: 0x00017F99
	// (set) Token: 0x06002F3B RID: 12091 RVA: 0x00019DA1 File Offset: 0x00017FA1
	public bool HideOnPause { get; set; }

	// Token: 0x06002F3C RID: 12092 RVA: 0x00019DAA File Offset: 0x00017FAA
	private void Awake()
	{
		this.m_onPauseChange = new Action<MonoBehaviour, EventArgs>(this.OnPauseChange);
	}

	// Token: 0x06002F3D RID: 12093 RVA: 0x00019DBE File Offset: 0x00017FBE
	private void OnEnable()
	{
		if (this.HideOnPause)
		{
			Messenger<GameMessenger, GameEvent>.AddListener(GameEvent.GamePauseStateChange, this.m_onPauseChange);
		}
	}

	// Token: 0x06002F3E RID: 12094 RVA: 0x00019DD5 File Offset: 0x00017FD5
	private void OnDisable()
	{
		if (this.HideOnPause)
		{
			Messenger<GameMessenger, GameEvent>.RemoveListener(GameEvent.GamePauseStateChange, this.m_onPauseChange);
		}
	}

	// Token: 0x06002F3F RID: 12095 RVA: 0x000C9EAC File Offset: 0x000C80AC
	private void OnPauseChange(object sender, EventArgs args)
	{
		if ((args as GamePauseStateChangeEventArgs).IsPaused)
		{
			if (this.CanvasGroup)
			{
				this.m_storedCanvasAlpha = this.CanvasGroup.alpha;
				this.CanvasGroup.alpha = 0f;
				TweenManager.SetPauseAllTweensContaining(this.CanvasGroup, true);
				return;
			}
		}
		else if (this.CanvasGroup)
		{
			this.CanvasGroup.alpha = this.m_storedCanvasAlpha;
			TweenManager.SetPauseAllTweensContaining(this.CanvasGroup, false);
		}
	}

	// Token: 0x040026A7 RID: 9895
	[SerializeField]
	private TMP_Text m_headerText;

	// Token: 0x040026A8 RID: 9896
	[SerializeField]
	private TMP_Text m_subHeaderText;

	// Token: 0x040026A9 RID: 9897
	[SerializeField]
	private TMP_Text m_descriptionText;

	// Token: 0x040026AA RID: 9898
	[SerializeField]
	private CanvasGroup m_canvasGroup;

	// Token: 0x040026AB RID: 9899
	private float m_storedCanvasAlpha;

	// Token: 0x040026AC RID: 9900
	private Action<MonoBehaviour, EventArgs> m_onPauseChange;
}
