using System;
using UnityEngine;

// Token: 0x02000457 RID: 1111
public class SentryWithIce_Hazard : Hazard, IPointHazard, IHazard, IRootObj, IHasProjectileNameArray
{
	// Token: 0x17001012 RID: 4114
	// (get) Token: 0x060028FF RID: 10495 RVA: 0x000877A7 File Offset: 0x000859A7
	public string[] ProjectileNameArray
	{
		get
		{
			if (this.m_projectileNameArray == null)
			{
				this.m_projectileNameArray = new string[]
				{
					"SentryBounceBoltProjectile"
				};
			}
			return this.m_projectileNameArray;
		}
	}

	// Token: 0x06002900 RID: 10496 RVA: 0x000877CB File Offset: 0x000859CB
	protected override void Awake()
	{
		base.Awake();
		this.m_onSentryStateChange = new Action<Sentry_Hazard, bool>(this.OnSentryStateChange);
	}

	// Token: 0x06002901 RID: 10497 RVA: 0x000877E8 File Offset: 0x000859E8
	public override void Initialize(HazardArgs hazardArgs)
	{
		for (int i = 0; i < this.m_hazards.Length; i++)
		{
			if (i == 0)
			{
				this.m_hazards[i] = HazardManager.GetHazard(HazardType.Sentry);
				(this.m_hazards[i] as Sentry_Hazard).SentryRestingRelay.AddListener(this.m_onSentryStateChange, false);
			}
			else if (i == 1)
			{
				this.m_hazards[i] = HazardManager.GetHazard(HazardType.IceCrystal);
			}
			this.m_hazards[i].gameObject.SetActive(true);
			this.m_hazards[i].gameObject.transform.SetPositionAndRotation(base.transform.position, base.transform.rotation);
			this.m_hazards[i].Initialize(hazardArgs);
			this.m_hazards[i].ResetHazard();
			(this.m_hazards[i] as Hazard).SetRoom(base.Room);
		}
	}

	// Token: 0x06002902 RID: 10498 RVA: 0x000878CD File Offset: 0x00085ACD
	private void OnSentryStateChange(Sentry_Hazard hazard, bool resting)
	{
		(this.m_hazards[1] as IceCrystal_Hazard).SetIceCrystalPaused(resting);
	}

	// Token: 0x06002903 RID: 10499 RVA: 0x000878E4 File Offset: 0x00085AE4
	public override void ResetHazard()
	{
		for (int i = 0; i < this.m_hazards.Length; i++)
		{
			this.m_hazards[i].ResetHazard();
		}
	}

	// Token: 0x06002904 RID: 10500 RVA: 0x00087914 File Offset: 0x00085B14
	public override void SetIsCulled(bool culled)
	{
		base.SetIsCulled(culled);
		for (int i = 0; i < this.m_hazards.Length; i++)
		{
			this.m_hazards[i].SetIsCulled(culled);
		}
	}

	// Token: 0x06002905 RID: 10501 RVA: 0x0008794C File Offset: 0x00085B4C
	protected override void OnDisable()
	{
		for (int i = 0; i < this.m_hazards.Length; i++)
		{
			if (!this.m_hazards[i].IsNativeNull())
			{
				if (i == 0)
				{
					(this.m_hazards[i] as Sentry_Hazard).SentryRestingRelay.RemoveListener(this.m_onSentryStateChange);
				}
				if (this.m_hazards[i].gameObject)
				{
					this.m_hazards[i].gameObject.SetActive(false);
				}
			}
		}
		base.OnDisable();
	}

	// Token: 0x06002907 RID: 10503 RVA: 0x000879DD File Offset: 0x00085BDD
	GameObject IRootObj.get_gameObject()
	{
		return base.gameObject;
	}

	// Token: 0x040021CD RID: 8653
	private IHazard[] m_hazards = new IHazard[2];

	// Token: 0x040021CE RID: 8654
	private Action<Sentry_Hazard, bool> m_onSentryStateChange;

	// Token: 0x040021CF RID: 8655
	[NonSerialized]
	private string[] m_projectileNameArray;
}
