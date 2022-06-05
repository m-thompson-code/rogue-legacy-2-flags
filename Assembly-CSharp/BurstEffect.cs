using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x02000408 RID: 1032
public class BurstEffect : BaseEffect
{
	// Token: 0x17000F6E RID: 3950
	// (get) Token: 0x06002694 RID: 9876 RVA: 0x0007FBB9 File Offset: 0x0007DDB9
	// (set) Token: 0x06002695 RID: 9877 RVA: 0x0007FBC4 File Offset: 0x0007DDC4
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

	// Token: 0x17000F6F RID: 3951
	// (get) Token: 0x06002696 RID: 9878 RVA: 0x0007FC03 File Offset: 0x0007DE03
	// (set) Token: 0x06002697 RID: 9879 RVA: 0x0007FC0C File Offset: 0x0007DE0C
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

	// Token: 0x06002698 RID: 9880 RVA: 0x0007FC4C File Offset: 0x0007DE4C
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

	// Token: 0x06002699 RID: 9881 RVA: 0x0007FCA0 File Offset: 0x0007DEA0
	public override void Play(float duration = 0f, EffectStopType stopType = EffectStopType.Gracefully)
	{
		base.Play(duration, stopType);
		base.StartCoroutine(this.PlayBurstCoroutine());
	}

	// Token: 0x0600269A RID: 9882 RVA: 0x0007FCB7 File Offset: 0x0007DEB7
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

	// Token: 0x0600269B RID: 9883 RVA: 0x0007FCC8 File Offset: 0x0007DEC8
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

	// Token: 0x0600269C RID: 9884 RVA: 0x0007FCFC File Offset: 0x0007DEFC
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

	// Token: 0x04002054 RID: 8276
	[SerializeField]
	private Vector2 m_burstMinMaxMagnitude;

	// Token: 0x04002055 RID: 8277
	[SerializeField]
	private Vector2 m_randDelayBetweenBursts;

	// Token: 0x04002056 RID: 8278
	[SerializeField]
	private bool m_moveTowardsImmediately;

	// Token: 0x04002057 RID: 8279
	[SerializeField]
	private UnityEvent m_effectCompleteUnityEvent;

	// Token: 0x04002058 RID: 8280
	private BurstLogic[] m_particleArray;

	// Token: 0x04002059 RID: 8281
	private WaitRL_Yield m_burstDelayYield;

	// Token: 0x0400205A RID: 8282
	private Transform m_destinationOverride;

	// Token: 0x0400205B RID: 8283
	private IMidpointObj m_destinationMidpointOverride;
}
