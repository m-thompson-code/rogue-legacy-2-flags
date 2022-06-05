using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000452 RID: 1106
public class PressurePlate_HazardTrigger : Multi_Hazard, ITerrainOnStayHitResponse, IHitResponse, ITerrainOnExitHitResponse
{
	// Token: 0x060028CD RID: 10445 RVA: 0x00086E8B File Offset: 0x0008508B
	protected override void Awake()
	{
		base.Awake();
		this.m_waitYield = new WaitRL_Yield(0f, false);
		this.m_hbController = base.GetComponentInChildren<IHitboxController>();
	}

	// Token: 0x060028CE RID: 10446 RVA: 0x00086EB0 File Offset: 0x000850B0
	private IEnumerator InitializeHitboxes(float width)
	{
		while (!this.m_hbController.IsInitialized)
		{
			yield return null;
		}
		this.m_hbController.RepeatHitDuration = 0f;
		this.m_triggerCollider = (this.m_hbController.GetCollider(HitboxType.Terrain) as BoxCollider2D);
		this.m_triggerCollider.size = new Vector2(width, this.m_triggerCollider.size.y);
		this.m_triggerCollider.gameObject.transform.position = this.m_spawnPosition;
		yield break;
	}

	// Token: 0x060028CF RID: 10447 RVA: 0x00086EC8 File Offset: 0x000850C8
	public override void Initialize(PivotPoint pivot, int width, HazardArgs hazardArgs)
	{
		base.Initialize(pivot, width, hazardArgs);
		this.m_spawnPosition = base.transform.position;
		if (pivot != PivotPoint.Center)
		{
			if (pivot != PivotPoint.Left)
			{
				if (pivot == PivotPoint.Right)
				{
					this.m_spawnPosition -= 0.5f * (float)width * base.transform.right;
				}
			}
			else
			{
				this.m_spawnPosition += 0.5f * (float)width * base.transform.right;
			}
		}
		this.m_visuals.transform.position = this.m_spawnPosition;
		this.m_triggerSprite.size = new Vector2((float)width, this.m_triggerSprite.size.y);
		if (this.m_triggerCollider == null)
		{
			base.StartCoroutine(this.InitializeHitboxes((float)width));
			return;
		}
		this.m_triggerCollider.size = new Vector2((float)width, this.m_triggerCollider.size.y);
		this.m_triggerCollider.gameObject.transform.position = this.m_spawnPosition;
	}

	// Token: 0x060028D0 RID: 10448 RVA: 0x00086FE8 File Offset: 0x000851E8
	public void TerrainOnStayHitResponse(IHitboxController otherHBController)
	{
		if (!this.m_plateTriggered && PlayerManager.GetPlayerController().IsGrounded && !this.m_isShooting)
		{
			if (this.m_hazards != null)
			{
				foreach (IHazard hazard in this.m_hazards)
				{
					((PressurePlate_Hazard)hazard).Animator.SetTrigger("ButtonDown");
				}
			}
			this.m_plateTriggered = true;
		}
	}

	// Token: 0x060028D1 RID: 10449 RVA: 0x00087074 File Offset: 0x00085274
	public void TerrainOnExitHitResponse(IHitboxController otherHBController)
	{
		if (this.m_plateTriggered && !this.m_isShooting)
		{
			base.StopAllCoroutines();
			if (base.gameObject.activeInHierarchy)
			{
				base.StartCoroutine(this.TriggerPressurePlate());
			}
		}
		this.m_plateTriggered = false;
	}

	// Token: 0x060028D2 RID: 10450 RVA: 0x000870AD File Offset: 0x000852AD
	private IEnumerator TriggerPressurePlate()
	{
		this.m_isShooting = true;
		foreach (IHazard hazard in this.m_hazards)
		{
			((PressurePlate_Hazard)hazard).Animator.SetTrigger("Open");
		}
		this.m_waitYield.CreateNew(0.2f, false);
		yield return this.m_waitYield;
		foreach (IHazard hazard2 in this.m_hazards)
		{
			((PressurePlate_Hazard)hazard2).Shoot();
		}
		this.m_waitYield.CreateNew(0.6f, false);
		yield return this.m_waitYield;
		foreach (IHazard hazard3 in this.m_hazards)
		{
			((PressurePlate_Hazard)hazard3).StopShooting();
		}
		this.m_waitYield.CreateNew(0.25f, false);
		yield return this.m_waitYield;
		foreach (IHazard hazard4 in this.m_hazards)
		{
			((PressurePlate_Hazard)hazard4).Animator.SetTrigger("Close");
		}
		this.m_isShooting = false;
		yield break;
	}

	// Token: 0x060028D3 RID: 10451 RVA: 0x000870BC File Offset: 0x000852BC
	public override void ResetHazard()
	{
		if (this.m_hazards != null && this.m_isShooting)
		{
			foreach (IHazard hazard in this.m_hazards)
			{
				PressurePlate_Hazard pressurePlate_Hazard = (PressurePlate_Hazard)hazard;
				pressurePlate_Hazard.StopShooting();
				pressurePlate_Hazard.Animator.ResetTrigger("Close");
				pressurePlate_Hazard.Animator.ResetTrigger("Open");
				pressurePlate_Hazard.Animator.ResetTrigger("ButtonDown");
			}
		}
		this.m_plateTriggered = false;
		this.m_isShooting = false;
		base.ResetHazard();
	}

	// Token: 0x040021AA RID: 8618
	[SerializeField]
	private GameObject m_visuals;

	// Token: 0x040021AB RID: 8619
	[SerializeField]
	private SpriteRenderer m_triggerSprite;

	// Token: 0x040021AC RID: 8620
	private WaitRL_Yield m_waitYield;

	// Token: 0x040021AD RID: 8621
	private bool m_plateTriggered;

	// Token: 0x040021AE RID: 8622
	private bool m_isShooting;

	// Token: 0x040021AF RID: 8623
	private BoxCollider2D m_triggerCollider;

	// Token: 0x040021B0 RID: 8624
	private IHitboxController m_hbController;

	// Token: 0x040021B1 RID: 8625
	private Vector3 m_spawnPosition;
}
