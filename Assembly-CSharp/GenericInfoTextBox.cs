using System;
using TMPro;
using UnityEngine;

// Token: 0x02000378 RID: 888
public class GenericInfoTextBox : MonoBehaviour
{
	// Token: 0x17000E10 RID: 3600
	// (get) Token: 0x0600214F RID: 8527 RVA: 0x00068FA4 File Offset: 0x000671A4
	public CanvasGroup CanvasGroup
	{
		get
		{
			return this.m_canvasGroup;
		}
	}

	// Token: 0x17000E11 RID: 3601
	// (get) Token: 0x06002150 RID: 8528 RVA: 0x00068FAC File Offset: 0x000671AC
	public TMP_Text HeaderText
	{
		get
		{
			return this.m_headerText;
		}
	}

	// Token: 0x17000E12 RID: 3602
	// (get) Token: 0x06002151 RID: 8529 RVA: 0x00068FB4 File Offset: 0x000671B4
	public TMP_Text SubHeaderText
	{
		get
		{
			return this.m_subHeaderText;
		}
	}

	// Token: 0x17000E13 RID: 3603
	// (get) Token: 0x06002152 RID: 8530 RVA: 0x00068FBC File Offset: 0x000671BC
	public TMP_Text DescriptionText
	{
		get
		{
			return this.m_descriptionText;
		}
	}

	// Token: 0x17000E14 RID: 3604
	// (get) Token: 0x06002153 RID: 8531 RVA: 0x00068FC4 File Offset: 0x000671C4
	// (set) Token: 0x06002154 RID: 8532 RVA: 0x00068FCC File Offset: 0x000671CC
	public bool HideOnPause { get; set; }

	// Token: 0x06002155 RID: 8533 RVA: 0x00068FD5 File Offset: 0x000671D5
	private void Awake()
	{
		this.m_onPauseChange = new Action<MonoBehaviour, EventArgs>(this.OnPauseChange);
	}

	// Token: 0x06002156 RID: 8534 RVA: 0x00068FE9 File Offset: 0x000671E9
	private void OnEnable()
	{
		if (this.HideOnPause)
		{
			Messenger<GameMessenger, GameEvent>.AddListener(GameEvent.GamePauseStateChange, this.m_onPauseChange);
		}
	}

	// Token: 0x06002157 RID: 8535 RVA: 0x00069000 File Offset: 0x00067200
	private void OnDisable()
	{
		if (this.HideOnPause)
		{
			Messenger<GameMessenger, GameEvent>.RemoveListener(GameEvent.GamePauseStateChange, this.m_onPauseChange);
		}
	}

	// Token: 0x06002158 RID: 8536 RVA: 0x00069018 File Offset: 0x00067218
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

	// Token: 0x04001CD7 RID: 7383
	[SerializeField]
	private TMP_Text m_headerText;

	// Token: 0x04001CD8 RID: 7384
	[SerializeField]
	private TMP_Text m_subHeaderText;

	// Token: 0x04001CD9 RID: 7385
	[SerializeField]
	private TMP_Text m_descriptionText;

	// Token: 0x04001CDA RID: 7386
	[SerializeField]
	private CanvasGroup m_canvasGroup;

	// Token: 0x04001CDB RID: 7387
	private float m_storedCanvasAlpha;

	// Token: 0x04001CDC RID: 7388
	private Action<MonoBehaviour, EventArgs> m_onPauseChange;
}
