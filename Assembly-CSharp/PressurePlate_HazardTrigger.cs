using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000730 RID: 1840
public class PressurePlate_HazardTrigger : Multi_Hazard, ITerrainOnStayHitResponse, IHitResponse, ITerrainOnExitHitResponse
{
	// Token: 0x06003859 RID: 14425 RVA: 0x0001EEC7 File Offset: 0x0001D0C7
	protected override void Awake()
	{
		base.Awake();
		this.m_waitYield = new WaitRL_Yield(0f, false);
		this.m_hbController = base.GetComponentInChildren<IHitboxController>();
	}

	// Token: 0x0600385A RID: 14426 RVA: 0x0001EEEC File Offset: 0x0001D0EC
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

	// Token: 0x0600385B RID: 14427 RVA: 0x000E7FD4 File Offset: 0x000E61D4
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

	// Token: 0x0600385C RID: 14428 RVA: 0x000E80F4 File Offset: 0x000E62F4
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

	// Token: 0x0600385D RID: 14429 RVA: 0x0001EF02 File Offset: 0x0001D102
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

	// Token: 0x0600385E RID: 14430 RVA: 0x0001EF3B File Offset: 0x0001D13B
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

	// Token: 0x0600385F RID: 14431 RVA: 0x000E8180 File Offset: 0x000E6380
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

	// Token: 0x04002D31 RID: 11569
	[SerializeField]
	private GameObject m_visuals;

	// Token: 0x04002D32 RID: 11570
	[SerializeField]
	private SpriteRenderer m_triggerSprite;

	// Token: 0x04002D33 RID: 11571
	private WaitRL_Yield m_waitYield;

	// Token: 0x04002D34 RID: 11572
	private bool m_plateTriggered;

	// Token: 0x04002D35 RID: 11573
	private bool m_isShooting;

	// Token: 0x04002D36 RID: 11574
	private BoxCollider2D m_triggerCollider;

	// Token: 0x04002D37 RID: 11575
	private IHitboxController m_hbController;

	// Token: 0x04002D38 RID: 11576
	private Vector3 m_spawnPosition;
}
