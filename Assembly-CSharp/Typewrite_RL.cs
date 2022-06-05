using System;
using System.Collections;
using Sigtrap.Relays;
using TMPro;
using UnityEngine;

// Token: 0x020005ED RID: 1517
[RequireComponent(typeof(TMP_Text))]
public class Typewrite_RL : MonoBehaviour
{
	// Token: 0x17001278 RID: 4728
	// (get) Token: 0x06002E9A RID: 11930 RVA: 0x000196F6 File Offset: 0x000178F6
	public IRelayLink OnTypewriteCompleteRelay
	{
		get
		{
			return this.m_onTypewriteCompleteRelay.link;
		}
	}

	// Token: 0x17001279 RID: 4729
	// (get) Token: 0x06002E9B RID: 11931 RVA: 0x00019703 File Offset: 0x00017903
	public IRelayLink OnTypewriteLongDelayRelay
	{
		get
		{
			return this.m_onTypewriteLongDelayRelay.link;
		}
	}

	// Token: 0x1700127A RID: 4730
	// (get) Token: 0x06002E9C RID: 11932 RVA: 0x00019710 File Offset: 0x00017910
	public IRelayLink OnTypewriteShortDelayRelay
	{
		get
		{
			return this.m_onTypewriteShortDelayRelay.link;
		}
	}

	// Token: 0x1700127B RID: 4731
	// (get) Token: 0x06002E9D RID: 11933 RVA: 0x0001971D File Offset: 0x0001791D
	// (set) Token: 0x06002E9E RID: 11934 RVA: 0x00019725 File Offset: 0x00017925
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

	// Token: 0x1700127C RID: 4732
	// (get) Token: 0x06002E9F RID: 11935 RVA: 0x0001972E File Offset: 0x0001792E
	// (set) Token: 0x06002EA0 RID: 11936 RVA: 0x00019736 File Offset: 0x00017936
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

	// Token: 0x1700127D RID: 4733
	// (get) Token: 0x06002EA1 RID: 11937 RVA: 0x0001973F File Offset: 0x0001793F
	// (set) Token: 0x06002EA2 RID: 11938 RVA: 0x00019747 File Offset: 0x00017947
	public bool IsPaused { get; private set; }

	// Token: 0x1700127E RID: 4734
	// (get) Token: 0x06002EA3 RID: 11939 RVA: 0x00019750 File Offset: 0x00017950
	// (set) Token: 0x06002EA4 RID: 11940 RVA: 0x00019758 File Offset: 0x00017958
	public bool IsTypewriting { get; private set; }

	// Token: 0x1700127F RID: 4735
	// (get) Token: 0x06002EA5 RID: 11941 RVA: 0x00019761 File Offset: 0x00017961
	// (set) Token: 0x06002EA6 RID: 11942 RVA: 0x00019769 File Offset: 0x00017969
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

	// Token: 0x17001280 RID: 4736
	// (get) Token: 0x06002EA7 RID: 11943 RVA: 0x00019772 File Offset: 0x00017972
	// (set) Token: 0x06002EA8 RID: 11944 RVA: 0x0001977A File Offset: 0x0001797A
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

	// Token: 0x06002EA9 RID: 11945 RVA: 0x00019783 File Offset: 0x00017983
	private void Awake()
	{
		this.m_tmpText = base.GetComponent<TMP_Text>();
		this.m_typewriteWaitYield = new WaitRL_Yield(this.TypewriteCharDelay, this.UseUnscaledTime);
		this.m_typewriteLongWaitYield = new WaitRL_Yield(this.TypewriteCharLongDelay, this.UseUnscaledTime);
	}

	// Token: 0x06002EAA RID: 11946 RVA: 0x000197BF File Offset: 0x000179BF
	private void Start()
	{
		if (this.TypewriteOnStart)
		{
			this.StartTypewriting();
		}
	}

	// Token: 0x06002EAB RID: 11947 RVA: 0x000197CF File Offset: 0x000179CF
	public void StartTypewriting()
	{
		this.m_typewriteCoroutine = base.StartCoroutine(this.TypewriterCoroutine());
	}

	// Token: 0x06002EAC RID: 11948 RVA: 0x000197E3 File Offset: 0x000179E3
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

	// Token: 0x06002EAD RID: 11949 RVA: 0x000C8308 File Offset: 0x000C6508
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

	// Token: 0x06002EAE RID: 11950 RVA: 0x000197F2 File Offset: 0x000179F2
	public void PauseTypewriting()
	{
		this.IsPaused = true;
	}

	// Token: 0x06002EAF RID: 11951 RVA: 0x000197FB File Offset: 0x000179FB
	public void ResumeTypewriting()
	{
		this.IsPaused = false;
	}

	// Token: 0x04002628 RID: 9768
	[SerializeField]
	private float m_typewriteCharDelay = 0.015f;

	// Token: 0x04002629 RID: 9769
	[SerializeField]
	private float m_typewriteCharLongDelay = 0.1f;

	// Token: 0x0400262A RID: 9770
	[SerializeField]
	private bool m_typewriteOnStart = true;

	// Token: 0x0400262B RID: 9771
	[SerializeField]
	private bool m_useUnscaledTime;

	// Token: 0x0400262C RID: 9772
	private TMP_Text m_tmpText;

	// Token: 0x0400262D RID: 9773
	private WaitRL_Yield m_typewriteWaitYield;

	// Token: 0x0400262E RID: 9774
	private WaitRL_Yield m_typewriteLongWaitYield;

	// Token: 0x0400262F RID: 9775
	private Coroutine m_typewriteCoroutine;

	// Token: 0x04002630 RID: 9776
	private Relay m_onTypewriteCompleteRelay = new Relay();

	// Token: 0x04002631 RID: 9777
	private Relay m_onTypewriteLongDelayRelay = new Relay();

	// Token: 0x04002632 RID: 9778
	private Relay m_onTypewriteShortDelayRelay = new Relay();
}
