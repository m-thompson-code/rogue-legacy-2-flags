using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000462 RID: 1122
public class VoidTrap_Hazard : Hazard, IPointHazard, IHazard, IRootObj, IHasProjectileNameArray, ITerrainOnEnterHitResponse, IHitResponse
{
	// Token: 0x1700101E RID: 4126
	// (get) Token: 0x06002961 RID: 10593 RVA: 0x00088DCB File Offset: 0x00086FCB
	public string[] ProjectileNameArray
	{
		get
		{
			if (this.m_projectileNameArray == null)
			{
				this.m_projectileNameArray = new string[]
				{
					"VoidTrapHazardProjectile"
				};
			}
			return this.m_projectileNameArray;
		}
	}

	// Token: 0x06002962 RID: 10594 RVA: 0x00088DF0 File Offset: 0x00086FF0
	protected override void Awake()
	{
		base.Awake();
		this.m_projectileArray = new Projectile_RL[8];
		this.m_rotationDiff = 45f;
		this.m_carouselGO = new GameObject();
		this.m_carouselGO.name = "CarouselGO";
		this.m_carouselGO.transform.SetParent(base.transform);
		Vector3 position = this.m_carouselGO.transform.position;
		position.x += Hazard_EV.VOID_TRAP_CAROUSEL_OFFSET.x;
		position.y += Hazard_EV.VOID_TRAP_CAROUSEL_OFFSET.y;
		this.m_carouselGO.transform.position = position;
		this.m_waitYield = new WaitRL_Yield(0f, false);
	}

	// Token: 0x06002963 RID: 10595 RVA: 0x00088EA8 File Offset: 0x000870A8
	public override void Initialize(HazardArgs hazardArgs)
	{
		base.Initialize(hazardArgs);
		PointHazardArgs pointHazardArgs = hazardArgs as PointHazardArgs;
		if (pointHazardArgs != null)
		{
			this.m_ringRadius = pointHazardArgs.Radius * 1.17f;
		}
		else
		{
			Debug.LogFormat("<color=red>| {0} | Failed to cast hazardArgs as PointHazardArgs. If you see this message please bug it on Pivotal.</color>", Array.Empty<object>());
		}
		this.m_triggerCollider.radius = 3f;
		this.ResetHazard();
	}

	// Token: 0x06002964 RID: 10596 RVA: 0x00088EFF File Offset: 0x000870FF
	private IEnumerator FireProjectile()
	{
		this.m_state = VoidTrap_Hazard.VoidTrapState.Activated;
		this.m_renderer.color = Color.red;
		this.m_waitYield.CreateNew(0.25f, false);
		yield return this.m_waitYield;
		this.m_state = VoidTrap_Hazard.VoidTrapState.Attacking;
		Vector2 thePoint = new Vector2(this.m_ringRadius, 0f);
		for (int i = 0; i < 8; i++)
		{
			float theRotation = (float)i * this.m_rotationDiff;
			Vector2 vector = CDGHelper.RotatedPoint(thePoint, Vector2.zero, theRotation);
			this.m_projectileArray[i] = ProjectileManager.FireProjectile(base.gameObject, "VoidTrapHazardProjectile", vector, false, 0f, 1f, false, true, true, true);
			this.m_projectileArray[i].transform.SetParent(this.m_carouselGO.transform, false);
			this.m_projectileArray[i].transform.localPosition = vector;
		}
		this.m_waitYield.CreateNew(5f, false);
		yield return this.m_waitYield;
		this.m_state = VoidTrap_Hazard.VoidTrapState.CoolingDown;
		this.m_renderer.color = Color.blue;
		this.StopCarousel();
		this.m_waitYield.CreateNew(10f, false);
		yield return this.m_waitYield;
		this.ResetHazard();
		yield break;
	}

	// Token: 0x06002965 RID: 10597 RVA: 0x00088F10 File Offset: 0x00087110
	private void FixedUpdate()
	{
		if (this.m_state == VoidTrap_Hazard.VoidTrapState.Attacking && this.m_projectileArray[0] != null && this.m_projectileArray[0].isActiveAndEnabled)
		{
			Vector3 eulerAngles = this.m_carouselGO.transform.eulerAngles;
			eulerAngles.z += 360f * Time.deltaTime;
			this.m_carouselGO.transform.eulerAngles = eulerAngles;
			Projectile_RL[] projectileArray = this.m_projectileArray;
			for (int i = 0; i < projectileArray.Length; i++)
			{
				projectileArray[i].transform.eulerAngles = Vector3.zero;
			}
		}
	}

	// Token: 0x06002966 RID: 10598 RVA: 0x00088FA4 File Offset: 0x000871A4
	public void TerrainOnEnterHitResponse(IHitboxController otherHBController)
	{
		if (this.m_state == VoidTrap_Hazard.VoidTrapState.Idle)
		{
			base.StartCoroutine(this.FireProjectile());
		}
	}

	// Token: 0x06002967 RID: 10599 RVA: 0x00088FBC File Offset: 0x000871BC
	private void StopCarousel()
	{
		foreach (Projectile_RL projectile_RL in this.m_projectileArray)
		{
			if (projectile_RL != null && projectile_RL.isActiveAndEnabled)
			{
				projectile_RL.FlagForDestruction(null);
			}
		}
	}

	// Token: 0x06002968 RID: 10600 RVA: 0x00088FFA File Offset: 0x000871FA
	public override void ResetHazard()
	{
		if (this.m_state == VoidTrap_Hazard.VoidTrapState.Attacking)
		{
			this.StopCarousel();
		}
		this.m_state = VoidTrap_Hazard.VoidTrapState.Idle;
		this.m_renderer.color = Color.white;
	}

	// Token: 0x0600296A RID: 10602 RVA: 0x0008902A File Offset: 0x0008722A
	GameObject IRootObj.get_gameObject()
	{
		return base.gameObject;
	}

	// Token: 0x04002217 RID: 8727
	[SerializeField]
	private SpriteRenderer m_renderer;

	// Token: 0x04002218 RID: 8728
	[SerializeField]
	private CircleCollider2D m_triggerCollider;

	// Token: 0x04002219 RID: 8729
	private Projectile_RL[] m_projectileArray;

	// Token: 0x0400221A RID: 8730
	private GameObject m_carouselGO;

	// Token: 0x0400221B RID: 8731
	private float m_rotationDiff;

	// Token: 0x0400221C RID: 8732
	private float m_ringRadius;

	// Token: 0x0400221D RID: 8733
	private VoidTrap_Hazard.VoidTrapState m_state;

	// Token: 0x0400221E RID: 8734
	private WaitRL_Yield m_waitYield;

	// Token: 0x0400221F RID: 8735
	[NonSerialized]
	private string[] m_projectileNameArray;

	// Token: 0x02000C6C RID: 3180
	private enum VoidTrapState
	{
		// Token: 0x04005066 RID: 20582
		Idle,
		// Token: 0x04005067 RID: 20583
		Activated,
		// Token: 0x04005068 RID: 20584
		Attacking,
		// Token: 0x04005069 RID: 20585
		CoolingDown
	}
}
