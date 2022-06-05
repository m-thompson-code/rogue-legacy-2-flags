using System;
using System.Collections;
using Sigtrap.Relays;
using TMPro;
using UnityEngine;

// Token: 0x0200036E RID: 878
[RequireComponent(typeof(TMP_Text))]
public class Typewrite_RL : MonoBehaviour
{
	// Token: 0x17000DFF RID: 3583
	// (get) Token: 0x060020D1 RID: 8401 RVA: 0x000672DA File Offset: 0x000654DA
	public IRelayLink OnTypewriteCompleteRelay
	{
		get
		{
			return this.m_onTypewriteCompleteRelay.link;
		}
	}

	// Token: 0x17000E00 RID: 3584
	// (get) Token: 0x060020D2 RID: 8402 RVA: 0x000672E7 File Offset: 0x000654E7
	public IRelayLink OnTypewriteLongDelayRelay
	{
		get
		{
			return this.m_onTypewriteLongDelayRelay.link;
		}
	}

	// Token: 0x17000E01 RID: 3585
	// (get) Token: 0x060020D3 RID: 8403 RVA: 0x000672F4 File Offset: 0x000654F4
	public IRelayLink OnTypewriteShortDelayRelay
	{
		get
		{
			return this.m_onTypewriteShortDelayRelay.link;
		}
	}

	// Token: 0x17000E02 RID: 3586
	// (get) Token: 0x060020D4 RID: 8404 RVA: 0x00067301 File Offset: 0x00065501
	// (set) Token: 0x060020D5 RID: 8405 RVA: 0x00067309 File Offset: 0x00065509
	public bool UseUnscaledTime
	{
		get
		{
			return this.m_useUnscaledTime;
		}
		set
		{
			this.m_useUnscaledTime = value;
		}
	}

	// Token: 0x17000E03 RID: 3587
	// (get) Token: 0x060020D6 RID: 8406 RVA: 0x00067312 File Offset: 0x00065512
	// (set) Token: 0x060020D7 RID: 8407 RVA: 0x0006731A File Offset: 0x0006551A
	public bool TypewriteOnStart
	{
		get
		{
			return this.m_typewriteOnStart;
		}
		set
		{
			this.m_typewriteOnStart = value;
		}
	}

	// Token: 0x17000E04 RID: 3588
	// (get) Token: 0x060020D8 RID: 8408 RVA: 0x00067323 File Offset: 0x00065523
	// (set) Token: 0x060020D9 RID: 8409 RVA: 0x0006732B File Offset: 0x0006552B
	public bool IsPaused { get; private set; }

	// Token: 0x17000E05 RID: 3589
	// (get) Token: 0x060020DA RID: 8410 RVA: 0x00067334 File Offset: 0x00065534
	// (set) Token: 0x060020DB RID: 8411 RVA: 0x0006733C File Offset: 0x0006553C
	public bool IsTypewriting { get; private set; }

	// Token: 0x17000E06 RID: 3590
	// (get) Token: 0x060020DC RID: 8412 RVA: 0x00067345 File Offset: 0x00065545
	// (set) Token: 0x060020DD RID: 8413 RVA: 0x0006734D File Offset: 0x0006554D
	public float TypewriteCharDelay
	{
		get
		{
			return this.m_typewriteCharDelay;
		}
		set
		{
			this.m_typewriteCharDelay = value;
		}
	}

	// Token: 0x17000E07 RID: 3591
	// (get) Token: 0x060020DE RID: 8414 RVA: 0x00067356 File Offset: 0x00065556
	// (set) Token: 0x060020DF RID: 8415 RVA: 0x0006735E File Offset: 0x0006555E
	public float TypewriteCharLongDelay
	{
		get
		{
			return this.m_typewriteCharLongDelay;
		}
		set
		{
			this.m_typewriteCharLongDelay = value;
		}
	}

	// Token: 0x060020E0 RID: 8416 RVA: 0x00067367 File Offset: 0x00065567
	private void Awake()
	{
		this.m_tmpText = base.GetComponent<TMP_Text>();
		this.m_typewriteWaitYield = new WaitRL_Yield(this.TypewriteCharDelay, this.UseUnscaledTime);
		this.m_typewriteLongWaitYield = new WaitRL_Yield(this.TypewriteCharLongDelay, this.UseUnscaledTime);
	}

	// Token: 0x060020E1 RID: 8417 RVA: 0x000673A3 File Offset: 0x000655A3
	private void Start()
	{
		if (this.TypewriteOnStart)
		{
			this.StartTypewriting();
		}
	}

	// Token: 0x060020E2 RID: 8418 RVA: 0x000673B3 File Offset: 0x000655B3
	public void StartTypewriting()
	{
		this.m_typewriteCoroutine = base.StartCoroutine(this.TypewriterCoroutine());
	}

	// Token: 0x060020E3 RID: 8419 RVA: 0x000673C7 File Offset: 0x000655C7
	private IEnumerator TypewriterCoroutine()
	{
		this.IsTypewriting = true;
		this.m_tmpText.ForceMeshUpdate(false, false);
		int charCount = this.m_tmpText.textInfo.characterCount;
		int counter = 0;
		this.m_tmpText.maxVisibleCharacters = 0;
		yield return this.m_typewriteWaitYield;
		while (counter < charCount)
		{
			while (this.IsPaused)
			{
				yield return null;
			}
			int num = counter;
			counter = num + 1;
			if (counter < charCount)
			{
				char character = this.m_tmpText.textInfo.characterInfo[counter - 1].character;
				this.m_tmpText.maxVisibleCharacters = counter;
				if (character == ',' || character == '.' || character == '!')
				{
					this.m_onTypewriteLongDelayRelay.Dispatch();
					this.m_typewriteLongWaitYield.CreateNew(this.TypewriteCharLongDelay, this.UseUnscaledTime);
					yield return this.m_typewriteLongWaitYield;
				}
				else
				{
					this.m_onTypewriteShortDelayRelay.Dispatch();
					this.m_typewriteWaitYield.CreateNew(this.TypewriteCharDelay, this.UseUnscaledTime);
					yield return this.m_typewriteWaitYield;
				}
			}
		}
		yield return null;
		this.StopTypewriting();
		yield break;
	}

	// Token: 0x060020E4 RID: 8420 RVA: 0x000673D8 File Offset: 0x000655D8
	public void StopTypewriting()
	{
		if (this.m_typewriteCoroutine != null)
		{
			base.StopCoroutine(this.m_typewriteCoroutine);
		}
		this.IsTypewriting = false;
		this.m_tmpText.maxVisibleCharacters = this.m_tmpText.text.Length;
		this.m_onTypewriteCompleteRelay.Dispatch();
	}

	// Token: 0x060020E5 RID: 8421 RVA: 0x00067426 File Offset: 0x00065626
	public void PauseTypewriting()
	{
		this.IsPaused = true;
	}

	// Token: 0x060020E6 RID: 8422 RVA: 0x0006742F File Offset: 0x0006562F
	public void ResumeTypewriting()
	{
		this.IsPaused = false;
	}

	// Token: 0x04001C75 RID: 7285
	[SerializeField]
	private float m_typewriteCharDelay = 0.015f;

	// Token: 0x04001C76 RID: 7286
	[SerializeField]
	private float m_typewriteCharLongDelay = 0.1f;

	// Token: 0x04001C77 RID: 7287
	[SerializeField]
	private bool m_typewriteOnStart = true;

	// Token: 0x04001C78 RID: 7288
	[SerializeField]
	private bool m_useUnscaledTime;

	// Token: 0x04001C79 RID: 7289
	private TMP_Text m_tmpText;

	// Token: 0x04001C7A RID: 7290
	private WaitRL_Yield m_typewriteWaitYield;

	// Token: 0x04001C7B RID: 7291
	private WaitRL_Yield m_typewriteLongWaitYield;

	// Token: 0x04001C7C RID: 7292
	private Coroutine m_typewriteCoroutine;

	// Token: 0x04001C7D RID: 7293
	private Relay m_onTypewriteCompleteRelay = new Relay();

	// Token: 0x04001C7E RID: 7294
	private Relay m_onTypewriteLongDelayRelay = new Relay();

	// Token: 0x04001C7F RID: 7295
	private Relay m_onTypewriteShortDelayRelay = new Relay();
}
