using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020003E3 RID: 995
public class StatusBarEntry : MonoBehaviour, IGenericPoolObj
{
	// Token: 0x17000ED7 RID: 3799
	// (get) Token: 0x060024A5 RID: 9381 RVA: 0x0007A255 File Offset: 0x00078455
	// (set) Token: 0x060024A6 RID: 9382 RVA: 0x0007A25D File Offset: 0x0007845D
	public bool IsTimerPaused { get; private set; }

	// Token: 0x17000ED8 RID: 3800
	// (get) Token: 0x060024A7 RID: 9383 RVA: 0x0007A266 File Offset: 0x00078466
	// (set) Token: 0x060024A8 RID: 9384 RVA: 0x0007A26E File Offset: 0x0007846E
	public bool IsFreePoolObj { get; set; }

	// Token: 0x17000ED9 RID: 3801
	// (get) Token: 0x060024A9 RID: 9385 RVA: 0x0007A277 File Offset: 0x00078477
	// (set) Token: 0x060024AA RID: 9386 RVA: 0x0007A27F File Offset: 0x0007847F
	public bool IsAwakeCalled { get; protected set; } = true;

	// Token: 0x17000EDA RID: 3802
	// (get) Token: 0x060024AB RID: 9387 RVA: 0x0007A288 File Offset: 0x00078488
	// (set) Token: 0x060024AC RID: 9388 RVA: 0x0007A290 File Offset: 0x00078490
	public StatusBarEntryType StatusBarType { get; private set; }

	// Token: 0x060024AD RID: 9389 RVA: 0x0007A299 File Offset: 0x00078499
	private void Awake()
	{
		this.m_storedScale = base.transform.localScale;
	}

	// Token: 0x060024AE RID: 9390 RVA: 0x0007A2AC File Offset: 0x000784AC
	public void ResetScale()
	{
		base.transform.localScale = this.m_storedScale;
	}

	// Token: 0x060024AF RID: 9391 RVA: 0x0007A2C0 File Offset: 0x000784C0
	public void ResetPosition()
	{
		Vector3 localPosition = base.transform.localPosition;
		localPosition.z = 1f;
		base.transform.localPosition = localPosition;
	}

	// Token: 0x060024B0 RID: 9392 RVA: 0x0007A2F4 File Offset: 0x000784F4
	public void StartCounter(StatusBarEntryType statusBarType, int maxCount, int currentCount)
	{
		this.StopTimer();
		this.m_barBG.SetActive(false);
		this.m_counterText.gameObject.SetActive(true);
		this.StatusBarType = statusBarType;
		this.m_icon.sprite = IconLibrary.GetStatusEffectSprite(statusBarType);
		this.UpdateCounter(maxCount, currentCount);
	}

	// Token: 0x060024B1 RID: 9393 RVA: 0x0007A344 File Offset: 0x00078544
	public void UpdateCounter(int maxCount, int currentCount)
	{
		this.m_counterText.text = currentCount.ToString();
		if (currentCount >= maxCount)
		{
			this.m_counterText.color = Color.green;
			return;
		}
		if (currentCount == 0)
		{
			this.m_counterText.color = Color.red;
			return;
		}
		this.m_counterText.color = Color.white;
	}

	// Token: 0x060024B2 RID: 9394 RVA: 0x0007A39C File Offset: 0x0007859C
	public void StartNoCounterOrTimer(StatusBarEntryType uiType)
	{
		this.StopTimer();
		this.m_barBG.SetActive(false);
		this.m_counterText.gameObject.SetActive(false);
		this.StatusBarType = uiType;
		this.m_icon.sprite = IconLibrary.GetStatusEffectSprite(uiType);
	}

	// Token: 0x060024B3 RID: 9395 RVA: 0x0007A3DC File Offset: 0x000785DC
	public void StartTimer(StatusBarEntryType uiType, float duration)
	{
		this.StopTimer();
		this.m_barBG.SetActive(true);
		this.m_counterText.gameObject.SetActive(false);
		this.StatusBarType = uiType;
		this.m_icon.sprite = IconLibrary.GetStatusEffectSprite(uiType);
		base.StartCoroutine(this.StartTimerCoroutine(duration));
	}

	// Token: 0x060024B4 RID: 9396 RVA: 0x0007A434 File Offset: 0x00078634
	public void StartCounterAndTimer(StatusBarEntryType statusBarType, float duration, int maxCount, int currentCount)
	{
		this.StopTimer();
		this.m_counterText.gameObject.SetActive(true);
		this.m_barBG.SetActive(true);
		this.StatusBarType = statusBarType;
		this.m_icon.sprite = IconLibrary.GetStatusEffectSprite(statusBarType);
		base.StartCoroutine(this.StartTimerCoroutine(duration));
		this.UpdateCounter(maxCount, currentCount);
	}

	// Token: 0x060024B5 RID: 9397 RVA: 0x0007A493 File Offset: 0x00078693
	private IEnumerator StartTimerCoroutine(float duration)
	{
		this.m_barImage.fillAmount = 1f;
		float startTime = Time.time;
		float endTime = Time.time + duration;
		while (Time.time < endTime + this.m_pauseTime)
		{
			if (this.IsTimerPaused)
			{
				this.m_pauseTime += Time.deltaTime;
			}
			this.m_barImage.fillAmount = 1f - (Time.time - (startTime + this.m_pauseTime)) / duration;
			yield return null;
		}
		this.StopTimer();
		yield break;
	}

	// Token: 0x060024B6 RID: 9398 RVA: 0x0007A4A9 File Offset: 0x000786A9
	public void SetTimerPaused(bool paused)
	{
		this.IsTimerPaused = paused;
		this.m_pauseTime = 0f;
	}

	// Token: 0x060024B7 RID: 9399 RVA: 0x0007A4BD File Offset: 0x000786BD
	public void StopTimer()
	{
		base.StopAllCoroutines();
	}

	// Token: 0x060024B8 RID: 9400 RVA: 0x0007A4C5 File Offset: 0x000786C5
	public void ResetValues()
	{
		this.m_barImage.fillAmount = 1f;
		this.IsTimerPaused = false;
		this.m_pauseTime = 0f;
		base.transform.localEulerAngles = Vector3.zero;
	}

	// Token: 0x060024B9 RID: 9401 RVA: 0x0007A4F9 File Offset: 0x000786F9
	private void OnDisable()
	{
		DisablePooledObjectManager.DisablePooledObject(this, false);
	}

	// Token: 0x060024BB RID: 9403 RVA: 0x0007A511 File Offset: 0x00078711
	GameObject IGenericPoolObj.get_gameObject()
	{
		return base.gameObject;
	}

	// Token: 0x04001F1C RID: 7964
	[SerializeField]
	private GameObject m_barBG;

	// Token: 0x04001F1D RID: 7965
	[SerializeField]
	private Image m_barImage;

	// Token: 0x04001F1E RID: 7966
	[SerializeField]
	private TMP_Text m_counterText;

	// Token: 0x04001F1F RID: 7967
	[SerializeField]
	private Image m_icon;

	// Token: 0x04001F20 RID: 7968
	private Vector3 m_storedScale;

	// Token: 0x04001F21 RID: 7969
	private float m_pauseTime;
}
