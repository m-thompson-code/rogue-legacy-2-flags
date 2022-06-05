using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000455 RID: 1109
public class RaycastTurret_Hazard : Hazard, IHasProjectileNameArray, IMultiHazardConsumer
{
	// Token: 0x17001008 RID: 4104
	// (get) Token: 0x060028E6 RID: 10470 RVA: 0x000873FC File Offset: 0x000855FC
	public string[] ProjectileNameArray
	{
		get
		{
			if (this.m_projectileNameArray == null)
			{
				this.m_projectileNameArray = new string[]
				{
					this.m_projectileName
				};
			}
			return this.m_projectileNameArray;
		}
	}

	// Token: 0x17001009 RID: 4105
	// (get) Token: 0x060028E7 RID: 10471 RVA: 0x00087421 File Offset: 0x00085621
	protected virtual Vector2 FireOffset
	{
		get
		{
			return Vector2.zero;
		}
	}

	// Token: 0x1700100A RID: 4106
	// (get) Token: 0x060028E8 RID: 10472 RVA: 0x00087428 File Offset: 0x00085628
	protected virtual bool AllTurretsFire
	{
		get
		{
			return true;
		}
	}

	// Token: 0x1700100B RID: 4107
	// (get) Token: 0x060028E9 RID: 10473 RVA: 0x0008742B File Offset: 0x0008562B
	protected virtual float InitializationDelay
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x1700100C RID: 4108
	// (get) Token: 0x060028EA RID: 10474 RVA: 0x00087432 File Offset: 0x00085632
	protected virtual float FireDelay
	{
		get
		{
			return 0.5f;
		}
	}

	// Token: 0x1700100D RID: 4109
	// (get) Token: 0x060028EB RID: 10475 RVA: 0x00087439 File Offset: 0x00085639
	protected virtual float DelayBetweenShots
	{
		get
		{
			return 4f;
		}
	}

	// Token: 0x1700100E RID: 4110
	// (get) Token: 0x060028EC RID: 10476 RVA: 0x00087440 File Offset: 0x00085640
	protected virtual float DetectedRange
	{
		get
		{
			return 25f;
		}
	}

	// Token: 0x1700100F RID: 4111
	// (get) Token: 0x060028ED RID: 10477 RVA: 0x00087447 File Offset: 0x00085647
	// (set) Token: 0x060028EE RID: 10478 RVA: 0x0008744F File Offset: 0x0008564F
	public Multi_Hazard MultiHazard { get; set; }

	// Token: 0x17001010 RID: 4112
	// (get) Token: 0x060028EF RID: 10479 RVA: 0x00087458 File Offset: 0x00085658
	private bool IsShotWithinRange
	{
		get
		{
			BaseCharacterController playerController = PlayerManager.GetPlayerController();
			float num = base.transform.localEulerAngles.z;
			num = (float)((int)CDGHelper.WrapAngleDegrees(num, true));
			Vector2 vector = playerController.Midpoint;
			vector = CDGHelper.RotatedPoint(vector, base.transform.position, 90f - num);
			Rect rect = new Rect(base.transform.position.x - 1f, base.transform.position.y, 2f, this.DetectedRange);
			return rect.Contains(vector);
		}
	}

	// Token: 0x060028F0 RID: 10480 RVA: 0x000874EE File Offset: 0x000856EE
	protected override void Awake()
	{
		base.Awake();
		this.m_triggerShotMask = 256;
		this.m_initialDelay = new WaitForSeconds(this.FireDelay);
	}

	// Token: 0x060028F1 RID: 10481 RVA: 0x00087512 File Offset: 0x00085712
	private void OnEnable()
	{
		this.m_onEnterTime = Time.time;
	}

	// Token: 0x060028F2 RID: 10482 RVA: 0x0008751F File Offset: 0x0008571F
	protected override void OnDisable()
	{
		base.OnDisable();
		this.m_shotDelayTimer = 0f;
	}

	// Token: 0x060028F3 RID: 10483 RVA: 0x00087534 File Offset: 0x00085734
	private void FixedUpdate()
	{
		if (Time.time < this.m_onEnterTime + this.InitializationDelay)
		{
			return;
		}
		if (this.m_shotDelayTimer <= 0f)
		{
			if (PlayerManager.IsInstantiated)
			{
				if (this.m_needsReloading)
				{
					base.Animator.SetTrigger("Reload");
					this.m_needsReloading = false;
				}
				PlayerController playerController = PlayerManager.GetPlayerController();
				float num = 0.5f;
				float z = base.transform.localEulerAngles.z;
				Vector3 v = base.transform.position;
				v.y += num;
				v = CDGHelper.RotatedPoint(v, base.transform.position, z - 90f);
				v.z = base.transform.position.z;
				if (this.IsShotWithinRange)
				{
					Vector2 direction = CDGHelper.VectorBetweenPts(v, playerController.Midpoint);
					float magnitude = direction.magnitude;
					direction.Normalize();
					if (!Physics2D.Raycast(v, direction, magnitude, this.m_triggerShotMask))
					{
						if (this.AllTurretsFire)
						{
							if (!this.MultiHazard || this.MultiHazard.Hazards == null)
							{
								return;
							}
							using (List<IHazard>.Enumerator enumerator = this.MultiHazard.Hazards.GetEnumerator())
							{
								while (enumerator.MoveNext())
								{
									IHazard hazard = enumerator.Current;
									(hazard as RaycastTurret_Hazard).FireRaycastArrow();
								}
								return;
							}
						}
						this.FireRaycastArrow();
						return;
					}
				}
			}
		}
		else
		{
			this.m_shotDelayTimer -= Time.deltaTime;
		}
	}

	// Token: 0x060028F4 RID: 10484 RVA: 0x000876DC File Offset: 0x000858DC
	public void FireRaycastArrow()
	{
		if (this.m_initialDelayCoroutine == null)
		{
			this.m_initialDelayCoroutine = base.StartCoroutine(this.FireAfterInitialDelay());
		}
	}

	// Token: 0x060028F5 RID: 10485 RVA: 0x000876F8 File Offset: 0x000858F8
	private IEnumerator FireAfterInitialDelay()
	{
		base.Animator.SetTrigger("Tell");
		yield return this.m_initialDelay;
		base.Animator.SetTrigger("Fire");
		this.m_needsReloading = true;
		ProjectileManager.FireProjectile(base.gameObject, this.m_projectileName, this.FireOffset, false, base.transform.localEulerAngles.z, 1f, false, true, true, true);
		this.m_shotDelayTimer = this.DelayBetweenShots;
		this.m_initialDelayCoroutine = null;
		yield break;
	}

	// Token: 0x060028F6 RID: 10486 RVA: 0x00087707 File Offset: 0x00085907
	public override void ResetHazard()
	{
		this.m_initialDelayCoroutine = null;
		this.m_needsReloading = false;
	}

	// Token: 0x040021C3 RID: 8643
	[SerializeField]
	private string m_projectileName;

	// Token: 0x040021C4 RID: 8644
	private const int SHOT_TRIGGER_WIDTH = 2;

	// Token: 0x040021C5 RID: 8645
	private float m_shotDelayTimer;

	// Token: 0x040021C6 RID: 8646
	private int m_triggerShotMask;

	// Token: 0x040021C7 RID: 8647
	private Coroutine m_initialDelayCoroutine;

	// Token: 0x040021C8 RID: 8648
	private WaitForSeconds m_initialDelay;

	// Token: 0x040021C9 RID: 8649
	private float m_onEnterTime;

	// Token: 0x040021CA RID: 8650
	private bool m_needsReloading;

	// Token: 0x040021CB RID: 8651
	[NonSerialized]
	private string[] m_projectileNameArray;
}
