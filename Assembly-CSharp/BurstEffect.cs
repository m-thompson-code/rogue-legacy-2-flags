using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x020006BE RID: 1726
public class BurstEffect : BaseEffect
{
	// Token: 0x1700142F RID: 5167
	// (get) Token: 0x0600353D RID: 13629 RVA: 0x0001D392 File Offset: 0x0001B592
	// (set) Token: 0x0600353E RID: 13630 RVA: 0x000E01EC File Offset: 0x000DE3EC
	public Transform DestinationOverride
	{
		get
		{
			return this.m_destinationOverride;
		}
		set
		{
			this.m_destinationOverride = value;
			if (this.m_particleArray != null)
			{
				BurstLogic[] particleArray = this.m_particleArray;
				for (int i = 0; i < particleArray.Length; i++)
				{
					particleArray[i].DestinationOverride = this.m_destinationOverride;
				}
			}
		}
	}

	// Token: 0x17001430 RID: 5168
	// (get) Token: 0x0600353F RID: 13631 RVA: 0x0001D39A File Offset: 0x0001B59A
	// (set) Token: 0x06003540 RID: 13632 RVA: 0x000E022C File Offset: 0x000DE42C
	public IMidpointObj DestinationMidpointOverride
	{
		get
		{
			return this.m_destinationMidpointOverride;
		}
		set
		{
			this.m_destinationMidpointOverride = value;
			if (this.m_particleArray != null)
			{
				BurstLogic[] particleArray = this.m_particleArray;
				for (int i = 0; i < particleArray.Length; i++)
				{
					particleArray[i].DestinationMidpointOverride = this.m_destinationMidpointOverride;
				}
			}
		}
	}

	// Token: 0x06003541 RID: 13633 RVA: 0x000E026C File Offset: 0x000DE46C
	protected override void Awake()
	{
		base.Awake();
		this.m_particleArray = base.GetComponentsInChildren<BurstLogic>(true);
		BurstLogic[] particleArray = this.m_particleArray;
		for (int i = 0; i < particleArray.Length; i++)
		{
			particleArray[i].gameObject.SetActive(false);
		}
		this.m_burstDelayYield = new WaitRL_Yield(0f, false);
	}

	// Token: 0x06003542 RID: 13634 RVA: 0x0001D3A2 File Offset: 0x0001B5A2
	public override void Play(float duration = 0f, EffectStopType stopType = EffectStopType.Gracefully)
	{
		base.Play(duration, stopType);
		base.StartCoroutine(this.PlayBurstCoroutine());
	}

	// Token: 0x06003543 RID: 13635 RVA: 0x0001D3B9 File Offset: 0x0001B5B9
	private IEnumerator PlayBurstCoroutine()
	{
		Vector3 spawnPos = base.Source.transform.position;
		IMidpointObj component = base.Source.GetComponent<IMidpointObj>();
		if (component != null)
		{
			spawnPos = component.Midpoint;
		}
		foreach (BurstLogic burstLogic in this.m_particleArray)
		{
			burstLogic.transform.position = spawnPos;
			burstLogic.ClearTrail();
			burstLogic.gameObject.SetActive(true);
			if (!this.m_moveTowardsImmediately)
			{
				burstLogic.Burst((float)UnityEngine.Random.Range(10, 20), -1f);
			}
			else
			{
				burstLogic.BurstTowards((float)UnityEngine.Random.Range(10, 20));
			}
			if (this.m_randDelayBetweenBursts.y > 0f)
			{
				this.m_burstDelayYield.CreateNew(UnityEngine.Random.Range(this.m_randDelayBetweenBursts.x, this.m_randDelayBetweenBursts.y), false);
				yield return this.m_burstDelayYield;
			}
		}
		BurstLogic[] array = null;
		while (!this.AllParticlesComplete())
		{
			yield return null;
		}
		this.Stop(EffectStopType.Gracefully);
		yield break;
	}

	// Token: 0x06003544 RID: 13636 RVA: 0x000E02C0 File Offset: 0x000DE4C0
	private bool AllParticlesComplete()
	{
		BurstLogic[] particleArray = this.m_particleArray;
		for (int i = 0; i < particleArray.Length; i++)
		{
			if (particleArray[i].gameObject.activeSelf)
			{
				return false;
			}
		}
		return true;
	}

	// Token: 0x06003545 RID: 13637 RVA: 0x000E02F4 File Offset: 0x000DE4F4
	public override void Stop(EffectStopType stopType)
	{
		foreach (BurstLogic burstLogic in this.m_particleArray)
		{
			if (burstLogic.gameObject.activeSelf)
			{
				burstLogic.gameObject.SetActive(false);
			}
		}
		this.DestinationOverride = null;
		this.DestinationMidpointOverride = null;
		if (this.m_effectCompleteUnityEvent != null)
		{
			this.m_effectCompleteUnityEvent.Invoke();
		}
		this.PlayComplete();
	}

	// Token: 0x04002B2E RID: 11054
	[SerializeField]
	private Vector2 m_burstMinMaxMagnitude;

	// Token: 0x04002B2F RID: 11055
	[SerializeField]
	private Vector2 m_randDelayBetweenBursts;

	// Token: 0x04002B30 RID: 11056
	[SerializeField]
	private bool m_moveTowardsImmediately;

	// Token: 0x04002B31 RID: 11057
	[SerializeField]
	private UnityEvent m_effectCompleteUnityEvent;

	// Token: 0x04002B32 RID: 11058
	private BurstLogic[] m_particleArray;

	// Token: 0x04002B33 RID: 11059
	private WaitRL_Yield m_burstDelayYield;

	// Token: 0x04002B34 RID: 11060
	private Transform m_destinationOverride;

	// Token: 0x04002B35 RID: 11061
	private IMidpointObj m_destinationMidpointOverride;
}
