using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000755 RID: 1877
public class VoidTrap_Hazard : Hazard, IPointHazard, IHazard, IRootObj, IHasProjectileNameArray, ITerrainOnEnterHitResponse, IHitResponse
{
	// Token: 0x1700154F RID: 5455
	// (get) Token: 0x0600395F RID: 14687 RVA: 0x0001F8B1 File Offset: 0x0001DAB1
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

	// Token: 0x06003960 RID: 14688 RVA: 0x000EAD50 File Offset: 0x000E8F50
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

	// Token: 0x06003961 RID: 14689 RVA: 0x000EAE08 File Offset: 0x000E9008
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

	// Token: 0x06003962 RID: 14690 RVA: 0x0001F8D5 File Offset: 0x0001DAD5
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

	// Token: 0x06003963 RID: 14691 RVA: 0x000EAE60 File Offset: 0x000E9060
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

	// Token: 0x06003964 RID: 14692 RVA: 0x0001F8E4 File Offset: 0x0001DAE4
	public void TerrainOnEnterHitResponse(IHitboxController otherHBController)
	{
		if (this.m_state == VoidTrap_Hazard.VoidTrapState.Idle)
		{
			base.StartCoroutine(this.FireProjectile());
		}
	}

	// Token: 0x06003965 RID: 14693 RVA: 0x000EAEF4 File Offset: 0x000E90F4
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

	// Token: 0x06003966 RID: 14694 RVA: 0x0001F8FB File Offset: 0x0001DAFB
	public override void ResetHazard()
	{
		if (this.m_state == VoidTrap_Hazard.VoidTrapState.Attacking)
		{
			this.StopCarousel();
		}
		this.m_state = VoidTrap_Hazard.VoidTrapState.Idle;
		this.m_renderer.color = Color.white;
	}

	// Token: 0x06003968 RID: 14696 RVA: 0x00003713 File Offset: 0x00001913
	GameObject IRootObj.get_gameObject()
	{
		return base.gameObject;
	}

	// Token: 0x04002DE8 RID: 11752
	[SerializeField]
	private SpriteRenderer m_renderer;

	// Token: 0x04002DE9 RID: 11753
	[SerializeField]
	private CircleCollider2D m_triggerCollider;

	// Token: 0x04002DEA RID: 11754
	private Projectile_RL[] m_projectileArray;

	// Token: 0x04002DEB RID: 11755
	private GameObject m_carouselGO;

	// Token: 0x04002DEC RID: 11756
	private float m_rotationDiff;

	// Token: 0x04002DED RID: 11757
	private float m_ringRadius;

	// Token: 0x04002DEE RID: 11758
	private VoidTrap_Hazard.VoidTrapState m_state;

	// Token: 0x04002DEF RID: 11759
	private WaitRL_Yield m_waitYield;

	// Token: 0x04002DF0 RID: 11760
	[NonSerialized]
	private string[] m_projectileNameArray;

	// Token: 0x02000756 RID: 1878
	private enum VoidTrapState
	{
		// Token: 0x04002DF2 RID: 11762
		Idle,
		// Token: 0x04002DF3 RID: 11763
		Activated,
		// Token: 0x04002DF4 RID: 11764
		Attacking,
		// Token: 0x04002DF5 RID: 11765
		CoolingDown
	}
}
