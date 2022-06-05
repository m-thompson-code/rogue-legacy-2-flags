using System;
using UnityEngine;
using UnityEngine.SceneManagement;

// Token: 0x02000215 RID: 533
public class LetterboxController : MonoBehaviour
{
	// Token: 0x0600163F RID: 5695 RVA: 0x0004572A File Offset: 0x0004392A
	private void Awake()
	{
		this.SetPillarBoxesEnabled(false, -1f);
		this.SetLetterBoxesEnabled(false, -1f);
		SceneManager.sceneLoaded += this.OnSceneLoaded;
		this.m_onAspectRatioChanged = new Action<MonoBehaviour, EventArgs>(this.OnAspectRatioChanged);
	}

	// Token: 0x06001640 RID: 5696 RVA: 0x00045767 File Offset: 0x00043967
	private void OnDestroy()
	{
		SceneManager.sceneLoaded -= this.OnSceneLoaded;
	}

	// Token: 0x06001641 RID: 5697 RVA: 0x0004577A File Offset: 0x0004397A
	private void OnSceneLoaded(Scene arg0, LoadSceneMode arg1)
	{
		if (AspectRatioManager.Disable_16_9_Aspect)
		{
			this.SetLetterBoxesEnabled(false, -1f);
			this.SetPillarBoxesEnabled(false, -1f);
		}
	}

	// Token: 0x06001642 RID: 5698 RVA: 0x0004579B File Offset: 0x0004399B
	private void OnEnable()
	{
		Messenger<SceneMessenger, SceneEvent>.AddListener(SceneEvent.AspectRatioChanged, this.m_onAspectRatioChanged);
	}

	// Token: 0x06001643 RID: 5699 RVA: 0x000457A9 File Offset: 0x000439A9
	private void OnDisable()
	{
		Messenger<SceneMessenger, SceneEvent>.RemoveListener(SceneEvent.AspectRatioChanged, this.m_onAspectRatioChanged);
	}

	// Token: 0x06001644 RID: 5700 RVA: 0x000457B8 File Offset: 0x000439B8
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

	// Token: 0x06001645 RID: 5701 RVA: 0x00045814 File Offset: 0x00043A14
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

	// Token: 0x06001646 RID: 5702 RVA: 0x000458EC File Offset: 0x00043AEC
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

	// Token: 0x0400157F RID: 5503
	[SerializeField]
	private RectTransform m_leftPillarBox;

	// Token: 0x04001580 RID: 5504
	[SerializeField]
	private RectTransform m_rightPillarBox;

	// Token: 0x04001581 RID: 5505
	[SerializeField]
	private RectTransform m_topLetterBox;

	// Token: 0x04001582 RID: 5506
	[SerializeField]
	private RectTransform m_bottomLetterBox;

	// Token: 0x04001583 RID: 5507
	private Action<MonoBehaviour, EventArgs> m_onAspectRatioChanged;
}
