using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000683 RID: 1667
public class StatusBarEntry : MonoBehaviour, IGenericPoolObj
{
	// Token: 0x17001376 RID: 4982
	// (get) Token: 0x060032E1 RID: 13025 RVA: 0x0001BD78 File Offset: 0x00019F78
	// (set) Token: 0x060032E2 RID: 13026 RVA: 0x0001BD80 File Offset: 0x00019F80
	public bool IsTimerPaused { get; private set; }

	// Token: 0x17001377 RID: 4983
	// (get) Token: 0x060032E3 RID: 13027 RVA: 0x0001BD89 File Offset: 0x00019F89
	// (set) Token: 0x060032E4 RID: 13028 RVA: 0x0001BD91 File Offset: 0x00019F91
	public bool IsFreePoolObj { get; set; }

	// Token: 0x17001378 RID: 4984
	// (get) Token: 0x060032E5 RID: 13029 RVA: 0x0001BD9A File Offset: 0x00019F9A
	// (set) Token: 0x060032E6 RID: 13030 RVA: 0x0001BDA2 File Offset: 0x00019FA2
	public bool IsAwakeCalled { get; protected set; } = true;

	// Token: 0x17001379 RID: 4985
	// (get) Token: 0x060032E7 RID: 13031 RVA: 0x0001BDAB File Offset: 0x00019FAB
	// (set) Token: 0x060032E8 RID: 13032 RVA: 0x0001BDB3 File Offset: 0x00019FB3
	public StatusBarEntryType StatusBarType { get; private set; }

	// Token: 0x060032E9 RID: 13033 RVA: 0x0001BDBC File Offset: 0x00019FBC
	private void Awake()
	{
		this.m_storedScale = base.transform.localScale;
	}

	// Token: 0x060032EA RID: 13034 RVA: 0x0001BDCF File Offset: 0x00019FCF
	public void ResetScale()
	{
		base.transform.localScale = this.m_storedScale;
	}

	// Token: 0x060032EB RID: 13035 RVA: 0x000DA42C File Offset: 0x000D862C
	public void ResetPosition()
	{
		Vector3 localPosition = base.transform.localPosition;
		localPosition.z = 1f;
		base.transform.localPosition = localPosition;
	}

	// Token: 0x060032EC RID: 13036 RVA: 0x000DA460 File Offset: 0x000D8660
	public void StartCounter(StatusBarEntryType statusBarType, int maxCount, int currentCount)
	{
		this.StopTimer();
		this.m_barBG.SetActive(false);
		this.m_counterText.gameObject.SetActive(true);
		this.StatusBarType = statusBarType;
		this.m_icon.sprite = IconLibrary.GetStatusEffectSprite(statusBarType);
		this.UpdateCounter(maxCount, currentCount);
	}

	// Token: 0x060032ED RID: 13037 RVA: 0x000DA4B0 File Offset: 0x000D86B0
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

	// Token: 0x060032EE RID: 13038 RVA: 0x0001BDE2 File Offset: 0x00019FE2
	public void StartNoCounterOrTimer(StatusBarEntryType uiType)
	{
		this.StopTimer();
		this.m_barBG.SetActive(false);
		this.m_counterText.gameObject.SetActive(false);
		this.StatusBarType = uiType;
		this.m_icon.sprite = IconLibrary.GetStatusEffectSprite(uiType);
	}

	// Token: 0x060032EF RID: 13039 RVA: 0x000DA508 File Offset: 0x000D8708
	public void StartTimer(StatusBarEntryType uiType, float duration)
	{
		this.StopTimer();
		this.m_barBG.SetActive(true);
		this.m_counterText.gameObject.SetActive(false);
		this.StatusBarType = uiType;
		this.m_icon.sprite = IconLibrary.GetStatusEffectSprite(uiType);
		base.StartCoroutine(this.StartTimerCoroutine(duration));
	}

	// Token: 0x060032F0 RID: 13040 RVA: 0x000DA560 File Offset: 0x000D8760
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

	// Token: 0x060032F1 RID: 13041 RVA: 0x0001BE1F File Offset: 0x0001A01F
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

	// Token: 0x060032F2 RID: 13042 RVA: 0x0001BE35 File Offset: 0x0001A035
	public void SetTimerPaused(bool paused)
	{
		this.IsTimerPaused = paused;
		this.m_pauseTime = 0f;
	}

	// Token: 0x060032F3 RID: 13043 RVA: 0x0001BE49 File Offset: 0x0001A049
	public void StopTimer()
	{
		base.StopAllCoroutines();
	}

	// Token: 0x060032F4 RID: 13044 RVA: 0x0001BE51 File Offset: 0x0001A051
	public void ResetValues()
	{
		this.m_barImage.fillAmount = 1f;
		this.IsTimerPaused = false;
		this.m_pauseTime = 0f;
		base.transform.localEulerAngles = Vector3.zero;
	}

	// Token: 0x060032F5 RID: 13045 RVA: 0x0001BE85 File Offset: 0x0001A085
	private void OnDisable()
	{
		DisablePooledObjectManager.DisablePooledObject(this, false);
	}

	// Token: 0x060032F7 RID: 13047 RVA: 0x00003713 File Offset: 0x00001913
	GameObject IGenericPoolObj.get_gameObject()
	{
		return base.gameObject;
	}

	// Token: 0x04002994 RID: 10644
	[SerializeField]
	private GameObject m_barBG;

	// Token: 0x04002995 RID: 10645
	[SerializeField]
	private Image m_barImage;

	// Token: 0x04002996 RID: 10646
	[SerializeField]
	private TMP_Text m_counterText;

	// Token: 0x04002997 RID: 10647
	[SerializeField]
	private Image m_icon;

	// Token: 0x04002998 RID: 10648
	private Vector3 m_storedScale;

	// Token: 0x04002999 RID: 10649
	private float m_pauseTime;
}
