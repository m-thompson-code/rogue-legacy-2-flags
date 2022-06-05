using System;
using UnityEngine;
using UnityEngine.SceneManagement;

// Token: 0x020003C8 RID: 968
public class LetterboxController : MonoBehaviour
{
	// Token: 0x06001FDB RID: 8155 RVA: 0x00010D67 File Offset: 0x0000EF67
	private void Awake()
	{
		this.SetPillarBoxesEnabled(false, -1f);
		this.SetLetterBoxesEnabled(false, -1f);
		SceneManager.sceneLoaded += this.OnSceneLoaded;
		this.m_onAspectRatioChanged = new Action<MonoBehaviour, EventArgs>(this.OnAspectRatioChanged);
	}

	// Token: 0x06001FDC RID: 8156 RVA: 0x00010DA4 File Offset: 0x0000EFA4
	private void OnDestroy()
	{
		SceneManager.sceneLoaded -= this.OnSceneLoaded;
	}

	// Token: 0x06001FDD RID: 8157 RVA: 0x00010DB7 File Offset: 0x0000EFB7
	private void OnSceneLoaded(Scene arg0, LoadSceneMode arg1)
	{
		if (AspectRatioManager.Disable_16_9_Aspect)
		{
			this.SetLetterBoxesEnabled(false, -1f);
			this.SetPillarBoxesEnabled(false, -1f);
		}
	}

	// Token: 0x06001FDE RID: 8158 RVA: 0x00010DD8 File Offset: 0x0000EFD8
	private void OnEnable()
	{
		Messenger<SceneMessenger, SceneEvent>.AddListener(SceneEvent.AspectRatioChanged, this.m_onAspectRatioChanged);
	}

	// Token: 0x06001FDF RID: 8159 RVA: 0x00010DE6 File Offset: 0x0000EFE6
	private void OnDisable()
	{
		Messenger<SceneMessenger, SceneEvent>.RemoveListener(SceneEvent.AspectRatioChanged, this.m_onAspectRatioChanged);
	}

	// Token: 0x06001FE0 RID: 8160 RVA: 0x000A3DAC File Offset: 0x000A1FAC
	private void OnAspectRatioChanged(object sender, EventArgs args)
	{
		this.SetLetterBoxesEnabled(false, -1f);
		this.SetPillarBoxesEnabled(false, -1f);
		if (!AspectRatioManager.Disable_16_9_Aspect)
		{
			if (AspectRatioManager.IsScreen_16_9_AspectRatio)
			{
				return;
			}
			if (AspectRatioManager.CurrentScreenAspectRatio > 1.7777778f)
			{
				this.SetPillarBoxesEnabled(true, 1.7777778f);
				return;
			}
			this.SetLetterBoxesEnabled(true, 1.7777778f);
		}
	}

	// Token: 0x06001FE1 RID: 8161 RVA: 0x000A3E08 File Offset: 0x000A2008
	private void SetPillarBoxesEnabled(bool enable, float aspectRatio = -1f)
	{
		if (aspectRatio != -1f)
		{
			float num = (float)GameResolutionManager.Resolution.x;
			float num2 = (float)GameResolutionManager.Resolution.y;
			float num3 = aspectRatio * num2;
			float num4 = 1080f / num2;
			float num5 = (num - num3) * num4;
			num5 *= 0.5f;
			this.m_leftPillarBox.sizeDelta = new Vector2(num5, this.m_leftPillarBox.sizeDelta.y);
			this.m_rightPillarBox.sizeDelta = new Vector2(num5, this.m_rightPillarBox.sizeDelta.y);
		}
		if (this.m_leftPillarBox.gameObject.activeSelf != enable)
		{
			this.m_leftPillarBox.gameObject.SetActive(enable);
		}
		if (this.m_rightPillarBox.gameObject.activeSelf != enable)
		{
			this.m_rightPillarBox.gameObject.SetActive(enable);
		}
	}

	// Token: 0x06001FE2 RID: 8162 RVA: 0x000A3EE0 File Offset: 0x000A20E0
	private void SetLetterBoxesEnabled(bool enable, float aspectRatio = -1f)
	{
		if (aspectRatio != -1f)
		{
			float num = (float)GameResolutionManager.Resolution.x;
			float num2 = (float)GameResolutionManager.Resolution.y;
			float num3 = 1f / aspectRatio * num;
			float num4 = 1920f / num;
			float num5 = (num2 - num3) * num4;
			num5 *= 0.5f;
			this.m_topLetterBox.sizeDelta = new Vector2(this.m_topLetterBox.sizeDelta.x, num5);
			this.m_bottomLetterBox.sizeDelta = new Vector2(this.m_bottomLetterBox.sizeDelta.x, num5);
		}
		if (this.m_topLetterBox.gameObject.activeSelf != enable)
		{
			this.m_topLetterBox.gameObject.SetActive(enable);
		}
		if (this.m_bottomLetterBox.gameObject.activeSelf != enable)
		{
			this.m_bottomLetterBox.gameObject.SetActive(enable);
		}
	}

	// Token: 0x04001C82 RID: 7298
	[SerializeField]
	private RectTransform m_leftPillarBox;

	// Token: 0x04001C83 RID: 7299
	[SerializeField]
	private RectTransform m_rightPillarBox;

	// Token: 0x04001C84 RID: 7300
	[SerializeField]
	private RectTransform m_topLetterBox;

	// Token: 0x04001C85 RID: 7301
	[SerializeField]
	private RectTransform m_bottomLetterBox;

	// Token: 0x04001C86 RID: 7302
	private Action<MonoBehaviour, EventArgs> m_onAspectRatioChanged;
}
