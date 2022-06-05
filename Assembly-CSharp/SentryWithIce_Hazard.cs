using System;
using UnityEngine;

// Token: 0x0200073D RID: 1853
public class SentryWithIce_Hazard : Hazard, IPointHazard, IHazard, IRootObj, IHasProjectileNameArray
{
	// Token: 0x17001529 RID: 5417
	// (get) Token: 0x060038AF RID: 14511 RVA: 0x0001F1F1 File Offset: 0x0001D3F1
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

	// Token: 0x060038B0 RID: 14512 RVA: 0x0001F215 File Offset: 0x0001D415
	protected override void Awake()
	{
		base.Awake();
		this.m_onSentryStateChange = new Action<Sentry_Hazard, bool>(this.OnSentryStateChange);
	}

	// Token: 0x060038B1 RID: 14513 RVA: 0x000E8D90 File Offset: 0x000E6F90
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

	// Token: 0x060038B2 RID: 14514 RVA: 0x0001F22F File Offset: 0x0001D42F
	private void OnSentryStateChange(Sentry_Hazard hazard, bool resting)
	{
		(this.m_hazards[1] as IceCrystal_Hazard).SetIceCrystalPaused(resting);
	}

	// Token: 0x060038B3 RID: 14515 RVA: 0x000E8E78 File Offset: 0x000E7078
	public override void ResetHazard()
	{
		for (int i = 0; i < this.m_hazards.Length; i++)
		{
			this.m_hazards[i].ResetHazard();
		}
	}

	// Token: 0x060038B4 RID: 14516 RVA: 0x000E8EA8 File Offset: 0x000E70A8
	public override void SetIsCulled(bool culled)
	{
		base.SetIsCulled(culled);
		for (int i = 0; i < this.m_hazards.Length; i++)
		{
			this.m_hazards[i].SetIsCulled(culled);
		}
	}

	// Token: 0x060038B5 RID: 14517 RVA: 0x000E8EE0 File Offset: 0x000E70E0
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

	// Token: 0x060038B7 RID: 14519 RVA: 0x00003713 File Offset: 0x00001913
	GameObject IRootObj.get_gameObject()
	{
		return base.gameObject;
	}

	// Token: 0x04002D70 RID: 11632
	private IHazard[] m_hazards = new IHazard[2];

	// Token: 0x04002D71 RID: 11633
	private Action<Sentry_Hazard, bool> m_onSentryStateChange;

	// Token: 0x04002D72 RID: 11634
	[NonSerialized]
	private string[] m_projectileNameArray;
}
