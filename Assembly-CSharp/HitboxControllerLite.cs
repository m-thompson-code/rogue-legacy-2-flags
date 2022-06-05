using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// Token: 0x020001D6 RID: 470
[RequireComponent(typeof(Rigidbody2D))]
public class HitboxControllerLite : MonoBehaviour, IHitboxController
{
	// Token: 0x17000A38 RID: 2616
	// (get) Token: 0x0600132F RID: 4911 RVA: 0x00039E7B File Offset: 0x0003807B
	public bool ResponseMethodsInitialized
	{
		get
		{
			return this.m_responseMethodsInitialized;
		}
	}

	// Token: 0x17000A39 RID: 2617
	// (get) Token: 0x06001330 RID: 4912 RVA: 0x00039E83 File Offset: 0x00038083
	// (set) Token: 0x06001331 RID: 4913 RVA: 0x00039E8B File Offset: 0x0003808B
	public Collider2D LastCollidedWith { get; set; }

	// Token: 0x17000A3A RID: 2618
	// (get) Token: 0x06001332 RID: 4914 RVA: 0x00039E94 File Offset: 0x00038094
	// (set) Token: 0x06001333 RID: 4915 RVA: 0x00039E9C File Offset: 0x0003809C
	public bool DisableAllCollisions
	{
		get
		{
			return this.m_disableAllCollisions;
		}
		set
		{
			this.m_disableAllCollisions = value;
			if (this.m_platformCollider)
			{
				if (this.m_disableAllCollisions)
				{
					this.m_platformCollider.enabled = false;
					return;
				}
				this.m_platformCollider.enabled = true;
			}
		}
	}

	// Token: 0x17000A3B RID: 2619
	// (get) Token: 0x06001334 RID: 4916 RVA: 0x00039ED3 File Offset: 0x000380D3
	public IBodyOnEnterHitResponse[] BodyOnEnterHitResponseArray
	{
		get
		{
			return this.m_bodyOnEnterHitResponse;
		}
	}

	// Token: 0x17000A3C RID: 2620
	// (get) Token: 0x06001335 RID: 4917 RVA: 0x00039EDB File Offset: 0x000380DB
	public IWeaponOnEnterHitResponse[] WeaponOnEnterHitResponseArray
	{
		get
		{
			return this.m_weaponOnEnterHitResponse;
		}
	}

	// Token: 0x17000A3D RID: 2621
	// (get) Token: 0x06001336 RID: 4918 RVA: 0x00039EE3 File Offset: 0x000380E3
	public ITerrainOnEnterHitResponse[] TerrainOnEnterHitResponseArray
	{
		get
		{
			return this.m_terrainOnEnterHitResponse;
		}
	}

	// Token: 0x17000A3E RID: 2622
	// (get) Token: 0x06001337 RID: 4919 RVA: 0x00039EEB File Offset: 0x000380EB
	public IBodyOnStayHitResponse[] BodyOnStayHitResponseArray
	{
		get
		{
			return this.m_bodyOnStayHitResponse;
		}
	}

	// Token: 0x17000A3F RID: 2623
	// (get) Token: 0x06001338 RID: 4920 RVA: 0x00039EF3 File Offset: 0x000380F3
	public IWeaponOnStayHitResponse[] WeaponOnStayHitResponseArray
	{
		get
		{
			return this.m_weaponOnStayHitResponse;
		}
	}

	// Token: 0x17000A40 RID: 2624
	// (get) Token: 0x06001339 RID: 4921 RVA: 0x00039EFB File Offset: 0x000380FB
	public ITerrainOnStayHitResponse[] TerrainOnStayHitResponseArray
	{
		get
		{
			return this.m_terrainOnStayHitResponse;
		}
	}

	// Token: 0x17000A41 RID: 2625
	// (get) Token: 0x0600133A RID: 4922 RVA: 0x00039F03 File Offset: 0x00038103
	public IBodyOnExitHitResponse[] BodyOnExitHitResponseList
	{
		get
		{
			return this.m_bodyOnExitHitResponse;
		}
	}

	// Token: 0x17000A42 RID: 2626
	// (get) Token: 0x0600133B RID: 4923 RVA: 0x00039F0B File Offset: 0x0003810B
	public IWeaponOnExitHitResponse[] WeaponOnExitHitResponseArray
	{
		get
		{
			return this.m_weaponOnExitHitResponse;
		}
	}

	// Token: 0x17000A43 RID: 2627
	// (get) Token: 0x0600133C RID: 4924 RVA: 0x00039F13 File Offset: 0x00038113
	public ITerrainOnExitHitResponse[] TerrainOnExitHitResponseArray
	{
		get
		{
			return this.m_terrainOnExitHitResponse;
		}
	}

	// Token: 0x17000A44 RID: 2628
	// (get) Token: 0x0600133D RID: 4925 RVA: 0x00039F1B File Offset: 0x0003811B
	// (set) Token: 0x0600133E RID: 4926 RVA: 0x00039F23 File Offset: 0x00038123
	public float RepeatHitDuration
	{
		get
		{
			return this.m_repeatHitDuration;
		}
		set
		{
			this.m_repeatHitDuration = value;
		}
	}

	// Token: 0x17000A45 RID: 2629
	// (get) Token: 0x0600133F RID: 4927 RVA: 0x00039F2C File Offset: 0x0003812C
	// (set) Token: 0x06001340 RID: 4928 RVA: 0x00039F34 File Offset: 0x00038134
	public GameObject RootGameObject { get; private set; }

	// Token: 0x17000A46 RID: 2630
	// (get) Token: 0x06001341 RID: 4929 RVA: 0x00039F3D File Offset: 0x0003813D
	public IDamageObj DamageObj
	{
		get
		{
			return this.m_damageObj;
		}
	}

	// Token: 0x17000A47 RID: 2631
	// (get) Token: 0x06001342 RID: 4930 RVA: 0x00039F45 File Offset: 0x00038145
	// (set) Token: 0x06001343 RID: 4931 RVA: 0x00039F4D File Offset: 0x0003814D
	public GameObject BodyHitbox
	{
		get
		{
			return this.m_bodyHitbox;
		}
		set
		{
			this.m_bodyHitbox = value;
		}
	}

	// Token: 0x17000A48 RID: 2632
	// (get) Token: 0x06001344 RID: 4932 RVA: 0x00039F56 File Offset: 0x00038156
	// (set) Token: 0x06001345 RID: 4933 RVA: 0x00039F5E File Offset: 0x0003815E
	public GameObject WeaponHitbox
	{
		get
		{
			return this.m_weaponHitbox;
		}
		set
		{
			this.m_weaponHitbox = value;
		}
	}

	// Token: 0x17000A49 RID: 2633
	// (get) Token: 0x06001346 RID: 4934 RVA: 0x00039F67 File Offset: 0x00038167
	// (set) Token: 0x06001347 RID: 4935 RVA: 0x00039F6F File Offset: 0x0003816F
	public GameObject TerrainHitbox
	{
		get
		{
			return this.m_terrainHitbox;
		}
		set
		{
			this.m_terrainHitbox = value;
		}
	}

	// Token: 0x17000A4A RID: 2634
	// (get) Token: 0x06001348 RID: 4936 RVA: 0x00039F78 File Offset: 0x00038178
	// (set) Token: 0x06001349 RID: 4937 RVA: 0x00039F80 File Offset: 0x00038180
	public GameObject PlatformHitbox
	{
		get
		{
			return this.m_platformHitbox;
		}
		set
		{
			this.m_platformHitbox = value;
		}
	}

	// Token: 0x17000A4B RID: 2635
	// (get) Token: 0x0600134A RID: 4938 RVA: 0x00039F89 File Offset: 0x00038189
	public Collider2D PlatformCollider
	{
		get
		{
			return this.m_platformCollider;
		}
	}

	// Token: 0x17000A4C RID: 2636
	// (get) Token: 0x0600134B RID: 4939 RVA: 0x00039F91 File Offset: 0x00038191
	// (set) Token: 0x0600134C RID: 4940 RVA: 0x00039F99 File Offset: 0x00038199
	public PlatformCollisionType PlatformCollisionType
	{
		get
		{
			return this.m_platformCollisionType;
		}
		set
		{
			this.m_platformCollisionType = value;
		}
	}

	// Token: 0x17000A4D RID: 2637
	// (get) Token: 0x0600134D RID: 4941 RVA: 0x00039FA2 File Offset: 0x000381A2
	// (set) Token: 0x0600134E RID: 4942 RVA: 0x00039FAC File Offset: 0x000381AC
	public CollisionType CollisionType
	{
		get
		{
			return this.m_collisionType;
		}
		set
		{
			if (value != this.m_collisionType)
			{
				this.m_collisionType = value;
				if (Application.isPlaying && this.IsInitialized)
				{
					string tag = TagType_RL.ToString(CollisionType_RL.GetEquivalentTag(this.m_collisionType));
					HitboxInfo[] componentsInChildren = this.RootGameObject.GetComponentsInChildren<HitboxInfo>(true);
					for (int i = 0; i < componentsInChildren.Length; i++)
					{
						componentsInChildren[i].tag = tag;
					}
				}
			}
		}
	}

	// Token: 0x17000A4E RID: 2638
	// (get) Token: 0x0600134F RID: 4943 RVA: 0x0003A00D File Offset: 0x0003820D
	// (set) Token: 0x06001350 RID: 4944 RVA: 0x0003A015 File Offset: 0x00038215
	public CollisionType WeaponCollidesWithType
	{
		get
		{
			return this.m_weaponCollidesWithType;
		}
		set
		{
			this.m_weaponCollidesWithType = value;
		}
	}

	// Token: 0x17000A4F RID: 2639
	// (get) Token: 0x06001351 RID: 4945 RVA: 0x0003A01E File Offset: 0x0003821E
	// (set) Token: 0x06001352 RID: 4946 RVA: 0x0003A026 File Offset: 0x00038226
	public CollisionType TerrainCollidesWithType
	{
		get
		{
			return this.m_terrainCollidesWithType;
		}
		set
		{
			this.m_terrainCollidesWithType = value;
		}
	}

	// Token: 0x06001353 RID: 4947 RVA: 0x0003A030 File Offset: 0x00038230
	public GameObject ContainsHitbox(HitboxType hitboxType, string hitboxName)
	{
		int length = hitboxName.Length;
		GameObject gameObject = null;
		switch (hitboxType)
		{
		case HitboxType.Terrain:
			gameObject = this.m_terrainHitbox;
			break;
		case HitboxType.Body:
			gameObject = this.m_bodyHitbox;
			break;
		case HitboxType.Weapon:
			gameObject = this.m_weaponHitbox;
			break;
		}
		if (gameObject != null && gameObject.name.Substring(0, length).Equals(hitboxName))
		{
			return gameObject;
		}
		return null;
	}

	// Token: 0x06001354 RID: 4948 RVA: 0x0003A098 File Offset: 0x00038298
	public void ChangeCollisionType(HitboxType hitboxType, CollisionType collisionType)
	{
		string tag = TagType_RL.ToString(CollisionType_RL.GetEquivalentTag(collisionType));
		GameObject gameObject = null;
		switch (hitboxType)
		{
		case HitboxType.Platform:
			if (this.m_platformHitbox != null)
			{
				Debug.Log("WARNING: Changing collision type for platform. This is highly not recommended.");
				this.m_platformHitbox.tag = tag;
			}
			return;
		case HitboxType.Terrain:
			gameObject = this.m_terrainHitbox;
			break;
		case HitboxType.Body:
			gameObject = this.m_bodyHitbox;
			break;
		case HitboxType.Weapon:
			gameObject = this.m_weaponHitbox;
			break;
		}
		if (gameObject != null)
		{
			gameObject.tag = tag;
		}
	}

	// Token: 0x06001355 RID: 4949 RVA: 0x0003A11B File Offset: 0x0003831B
	public Collider2D GetCollider(HitboxType hitboxType)
	{
		switch (hitboxType)
		{
		case HitboxType.Platform:
			return this.m_platformCollider;
		case HitboxType.Terrain:
			return this.m_terrainHitboxCollider;
		case HitboxType.Body:
			return this.m_bodyHitboxCollider;
		case HitboxType.Weapon:
			return this.m_weaponHitboxCollider;
		default:
			return null;
		}
	}

	// Token: 0x06001356 RID: 4950 RVA: 0x0003A152 File Offset: 0x00038352
	private GameObject GetHitboxGO(HitboxType hbType)
	{
		switch (hbType)
		{
		case HitboxType.Platform:
			return this.m_platformHitbox;
		case HitboxType.Terrain:
			return this.m_terrainHitbox;
		case HitboxType.Body:
			return this.m_bodyHitbox;
		case HitboxType.Weapon:
			return this.m_weaponHitbox;
		default:
			return null;
		}
	}

	// Token: 0x06001357 RID: 4951 RVA: 0x0003A18C File Offset: 0x0003838C
	public void ChangeCanCollideWith(HitboxType hitboxType, CollisionType newCollisionType)
	{
		if (hitboxType != HitboxType.Terrain)
		{
			if (hitboxType != HitboxType.Weapon)
			{
				throw new Exception("Cannot change CollideWith type for any hitboxes other than Weapon or Terrain");
			}
			if (this.m_weaponHitboxCollider != null)
			{
				this.m_weaponHitboxCollider.GetComponent<HitboxInfo>().CollidesWithType = newCollisionType;
				return;
			}
		}
		else if (this.m_terrainHitboxCollider != null)
		{
			this.m_terrainHitboxCollider.GetComponent<HitboxInfo>().CollidesWithType = newCollisionType;
			return;
		}
	}

	// Token: 0x17000A50 RID: 2640
	// (get) Token: 0x06001358 RID: 4952 RVA: 0x0003A1EC File Offset: 0x000383EC
	// (set) Token: 0x06001359 RID: 4953 RVA: 0x0003A1F4 File Offset: 0x000383F4
	public bool IsInitialized { get; private set; }

	// Token: 0x0600135A RID: 4954 RVA: 0x0003A1FD File Offset: 0x000383FD
	private void OnEnable()
	{
		if (!Application.isPlaying)
		{
			this.m_hitboxesInitialized = false;
		}
		if (this.IsInitialized)
		{
			this.SetupIgnoreCollisions();
		}
		if (this.IsInitialized && this.m_startExecuted && !this.m_responseMethodsInitialized)
		{
			this.InitializeResponseMethods();
		}
	}

	// Token: 0x0600135B RID: 4955 RVA: 0x0003A239 File Offset: 0x00038439
	private void OnDisable()
	{
		this.ResetRepeatHitChecks();
	}

	// Token: 0x0600135C RID: 4956 RVA: 0x0003A241 File Offset: 0x00038441
	private void Awake()
	{
		if (this.m_initializeOnAwake && !this.IsInitialized)
		{
			this.Initialize();
		}
	}

	// Token: 0x0600135D RID: 4957 RVA: 0x0003A259 File Offset: 0x00038459
	private IEnumerator Start()
	{
		this.m_startExecuted = true;
		if (this.m_initializeOnAwake)
		{
			while (!this.IsInitialized)
			{
				yield return null;
			}
			this.InitializeResponseMethods();
		}
		yield break;
	}

	// Token: 0x0600135E RID: 4958 RVA: 0x0003A268 File Offset: 0x00038468
	public void Initialize()
	{
		this.RootGameObject = this.GetRoot(false);
		this.m_damageObj = this.RootGameObject.GetComponentInChildren<IDamageObj>();
		this.InitializeRepeatHitCheck();
		this.InitializeHitboxes();
		this.SetupIgnoreCollisions();
		if (!this.m_initializeOnAwake || (this.m_initializeOnAwake && this.m_startExecuted))
		{
			this.InitializeResponseMethods();
		}
		this.IsInitialized = true;
	}

	// Token: 0x0600135F RID: 4959 RVA: 0x0003A2CC File Offset: 0x000384CC
	private void InitializeHitboxes()
	{
		if (!this.m_hitboxesInitialized)
		{
			HitboxControllerLite.m_gameObjDisableList_STATIC.Clear();
			if (this.m_bodyHitbox)
			{
				this.CreateHitbox(HitboxType.Body, HitboxControllerLite.m_gameObjDisableList_STATIC);
			}
			if (this.m_weaponHitbox)
			{
				this.CreateHitbox(HitboxType.Weapon, HitboxControllerLite.m_gameObjDisableList_STATIC);
			}
			if (this.m_terrainHitbox)
			{
				this.CreateHitbox(HitboxType.Terrain, HitboxControllerLite.m_gameObjDisableList_STATIC);
			}
			if (this.m_platformHitbox)
			{
				this.CreateHitbox(HitboxType.Platform, HitboxControllerLite.m_gameObjDisableList_STATIC);
			}
			foreach (GameObject gameObject in HitboxControllerLite.m_gameObjDisableList_STATIC)
			{
				if (gameObject != base.gameObject)
				{
					UnityEngine.Object.Destroy(gameObject);
				}
				else
				{
					Collider2D component = gameObject.GetComponent<Collider2D>();
					if (component)
					{
						UnityEngine.Object.Destroy(component);
					}
				}
			}
			this.m_hitboxesInitialized = true;
			return;
		}
		HitboxInfo[] componentsInChildren = this.RootGameObject.GetComponentsInChildren<HitboxInfo>(true);
		int i = 0;
		while (i < componentsInChildren.Length)
		{
			HitboxInfo hitboxInfo = componentsInChildren[i];
			hitboxInfo.HitboxController = this;
			int layer = hitboxInfo.gameObject.layer;
			if (layer == 7)
			{
				goto IL_146;
			}
			switch (layer)
			{
			case 13:
			case 18:
				goto IL_146;
			case 14:
			case 15:
				break;
			case 16:
			case 17:
				hitboxInfo.CollidesWithType = this.WeaponCollidesWithType;
				break;
			default:
				if (layer == 30)
				{
					goto IL_146;
				}
				break;
			}
			IL_153:
			i++;
			continue;
			IL_146:
			hitboxInfo.CollidesWithType = this.TerrainCollidesWithType;
			goto IL_153;
		}
		this.RepopulateColliders(HitboxType.Body);
		this.RepopulateColliders(HitboxType.Weapon);
		this.RepopulateColliders(HitboxType.Terrain);
		this.RepopulateColliders(HitboxType.Platform);
	}

	// Token: 0x06001360 RID: 4960 RVA: 0x0003A468 File Offset: 0x00038668
	private void SetupIgnoreCollisions()
	{
		Collider2D[] componentsInChildren = this.RootGameObject.GetComponentsInChildren<Collider2D>();
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			for (int j = i; j < componentsInChildren.Length; j++)
			{
				Collider2D collider2D = componentsInChildren[i];
				Collider2D collider2D2 = componentsInChildren[j];
				if (collider2D && collider2D2 && collider2D != collider2D2)
				{
					Physics2D.IgnoreCollision(collider2D, collider2D2);
				}
			}
		}
	}

	// Token: 0x06001361 RID: 4961 RVA: 0x0003A4CC File Offset: 0x000386CC
	public void RealignHitboxColliders()
	{
		PolygonCollider2D component;
		if (this.IsInitialized)
		{
			component = this.RootGameObject.GetComponent<PolygonCollider2D>();
		}
		else
		{
			component = this.GetRoot(false).GetComponent<PolygonCollider2D>();
		}
		if (component)
		{
			this.RealignBox2DColliderToPolyCollider(this.m_platformCollider, component);
			this.RealignBox2DColliderToPolyCollider(this.m_terrainHitboxCollider, component);
			this.RealignBox2DColliderToPolyCollider(this.m_weaponHitboxCollider, component);
			this.RealignBox2DColliderToPolyCollider(this.m_bodyHitboxCollider, component);
			UnityEngine.Object.DestroyImmediate(component);
		}
	}

	// Token: 0x06001362 RID: 4962 RVA: 0x0003A540 File Offset: 0x00038740
	private void RealignBox2DColliderToPolyCollider(Collider2D oldCollider, PolygonCollider2D polyCollider)
	{
		if (!oldCollider || !polyCollider)
		{
			return;
		}
		BoxCollider2D boxCollider2D = oldCollider as BoxCollider2D;
		if (boxCollider2D && boxCollider2D.size == polyCollider.bounds.size)
		{
			boxCollider2D.offset = Vector2.zero;
			boxCollider2D.offset = polyCollider.bounds.center - boxCollider2D.bounds.center;
		}
	}

	// Token: 0x06001363 RID: 4963 RVA: 0x0003A5C4 File Offset: 0x000387C4
	private void RepopulateColliders(HitboxType hbType)
	{
		if (GameUtility.IsInGame)
		{
			Debug.Log("<color=yellow>WARNING: HitboxController on: " + this.RootGameObject.name + " is being repopoulated, meaning the awake method on this HBController was called twice. This should only occur in level editor mode.</color>");
		}
		switch (hbType)
		{
		case HitboxType.Platform:
			if (this.m_platformHitbox != null)
			{
				this.m_platformCollider = this.m_platformHitbox.GetComponent<Collider2D>();
				this.m_platformHitbox.GetComponent<HitboxInfo>().SetCollider(this.m_platformCollider);
			}
			break;
		case HitboxType.Terrain:
			if (this.TerrainHitbox != null)
			{
				this.m_terrainHitboxCollider = this.TerrainHitbox.GetComponent<Collider2D>();
				this.TerrainHitbox.GetComponent<HitboxInfo>().SetCollider(this.m_terrainHitboxCollider);
				return;
			}
			break;
		case HitboxType.Body:
			if (this.BodyHitbox != null)
			{
				this.m_bodyHitboxCollider = this.BodyHitbox.GetComponent<Collider2D>();
				this.BodyHitbox.GetComponent<HitboxInfo>().SetCollider(this.m_bodyHitboxCollider);
				return;
			}
			break;
		case HitboxType.Weapon:
			if (this.WeaponHitbox != null)
			{
				this.m_weaponHitboxCollider = this.WeaponHitbox.GetComponent<Collider2D>();
				this.WeaponHitbox.GetComponent<HitboxInfo>().SetCollider(this.m_weaponHitboxCollider);
				return;
			}
			break;
		default:
			return;
		}
	}

	// Token: 0x06001364 RID: 4964 RVA: 0x0003A6EC File Offset: 0x000388EC
	private void InitializeResponseMethods()
	{
		if (this.m_bodyHitbox != null)
		{
			this.CreateHitResponseMethods(HitboxType.Body, this.RootGameObject);
		}
		if (this.m_weaponHitbox != null)
		{
			this.CreateHitResponseMethods(HitboxType.Weapon, this.RootGameObject);
		}
		if (this.m_terrainHitbox != null)
		{
			this.CreateHitResponseMethods(HitboxType.Terrain, this.RootGameObject);
		}
		this.m_responseMethodsInitialized = true;
	}

	// Token: 0x06001365 RID: 4965 RVA: 0x0003A754 File Offset: 0x00038954
	public IHitResponse[] GetHitResponseArray(HitboxType hbType, HitResponseType hbResponseType)
	{
		switch (hbType)
		{
		case HitboxType.Terrain:
			switch (hbResponseType)
			{
			case HitResponseType.OnEnter:
				return this.m_terrainOnEnterHitResponse;
			case HitResponseType.OnStay:
				return this.m_terrainOnStayHitResponse;
			case HitResponseType.OnExit:
				return this.m_terrainOnExitHitResponse;
			}
			break;
		case HitboxType.Body:
			switch (hbResponseType)
			{
			case HitResponseType.OnEnter:
				return this.m_bodyOnEnterHitResponse;
			case HitResponseType.OnStay:
				return this.m_bodyOnStayHitResponse;
			case HitResponseType.OnExit:
				return this.m_bodyOnExitHitResponse;
			}
			break;
		case HitboxType.Weapon:
			switch (hbResponseType)
			{
			case HitResponseType.OnEnter:
				return this.m_weaponOnEnterHitResponse;
			case HitResponseType.OnStay:
				return this.m_weaponOnStayHitResponse;
			case HitResponseType.OnExit:
				return this.m_weaponOnExitHitResponse;
			}
			break;
		}
		return null;
	}

	// Token: 0x06001366 RID: 4966 RVA: 0x0003A808 File Offset: 0x00038A08
	public void AddHitboxResponseMethod(IHitResponse hitResponse)
	{
		if (this.m_responseMethodsInitialized)
		{
			IBodyOnEnterHitResponse bodyOnEnterHitResponse = hitResponse as IBodyOnEnterHitResponse;
			if (bodyOnEnterHitResponse != null && !this.m_bodyOnEnterHitResponse.Contains(bodyOnEnterHitResponse))
			{
				this.m_bodyOnEnterHitResponse = this.m_bodyOnEnterHitResponse.Add(bodyOnEnterHitResponse);
			}
			IBodyOnExitHitResponse bodyOnExitHitResponse = hitResponse as IBodyOnExitHitResponse;
			if (bodyOnExitHitResponse != null && !this.m_bodyOnExitHitResponse.Contains(bodyOnExitHitResponse))
			{
				this.m_bodyOnExitHitResponse = this.m_bodyOnExitHitResponse.Add(bodyOnExitHitResponse);
			}
			IBodyOnStayHitResponse bodyOnStayHitResponse = hitResponse as IBodyOnStayHitResponse;
			if (bodyOnStayHitResponse != null && !this.m_bodyOnStayHitResponse.Contains(bodyOnStayHitResponse))
			{
				this.m_bodyOnStayHitResponse = this.m_bodyOnStayHitResponse.Add(bodyOnStayHitResponse);
			}
			IWeaponOnEnterHitResponse weaponOnEnterHitResponse = hitResponse as IWeaponOnEnterHitResponse;
			if (weaponOnEnterHitResponse != null && !this.m_weaponOnEnterHitResponse.Contains(weaponOnEnterHitResponse))
			{
				this.m_weaponOnEnterHitResponse = this.m_weaponOnEnterHitResponse.Add(weaponOnEnterHitResponse);
			}
			IWeaponOnExitHitResponse weaponOnExitHitResponse = hitResponse as IWeaponOnExitHitResponse;
			if (weaponOnExitHitResponse != null && !this.m_weaponOnExitHitResponse.Contains(weaponOnExitHitResponse))
			{
				this.m_weaponOnExitHitResponse = this.m_weaponOnExitHitResponse.Add(weaponOnExitHitResponse);
			}
			IWeaponOnStayHitResponse weaponOnStayHitResponse = hitResponse as IWeaponOnStayHitResponse;
			if (weaponOnStayHitResponse != null && !this.m_weaponOnStayHitResponse.Contains(weaponOnStayHitResponse))
			{
				this.m_weaponOnStayHitResponse = this.m_weaponOnStayHitResponse.Add(weaponOnStayHitResponse);
			}
			ITerrainOnEnterHitResponse terrainOnEnterHitResponse = hitResponse as ITerrainOnEnterHitResponse;
			if (terrainOnEnterHitResponse != null && !this.m_terrainOnEnterHitResponse.Contains(terrainOnEnterHitResponse))
			{
				this.m_terrainOnEnterHitResponse = this.m_terrainOnEnterHitResponse.Add(terrainOnEnterHitResponse);
			}
			ITerrainOnExitHitResponse terrainOnExitHitResponse = hitResponse as ITerrainOnExitHitResponse;
			if (terrainOnExitHitResponse != null && !this.m_terrainOnExitHitResponse.Contains(terrainOnExitHitResponse))
			{
				this.m_terrainOnExitHitResponse = this.m_terrainOnExitHitResponse.Add(terrainOnExitHitResponse);
			}
			ITerrainOnStayHitResponse terrainOnStayHitResponse = hitResponse as ITerrainOnStayHitResponse;
			if (terrainOnStayHitResponse != null && !this.m_terrainOnStayHitResponse.Contains(terrainOnStayHitResponse))
			{
				this.m_terrainOnStayHitResponse = this.m_terrainOnStayHitResponse.Add(terrainOnStayHitResponse);
			}
		}
	}

	// Token: 0x06001367 RID: 4967 RVA: 0x0003A9B0 File Offset: 0x00038BB0
	public void RemoveHitboxResponseMethod(IHitResponse hitResponse)
	{
		if (this.m_responseMethodsInitialized)
		{
			IBodyOnEnterHitResponse bodyOnEnterHitResponse = hitResponse as IBodyOnEnterHitResponse;
			if (bodyOnEnterHitResponse != null)
			{
				this.m_bodyOnEnterHitResponse = this.m_bodyOnEnterHitResponse.Remove(bodyOnEnterHitResponse);
			}
			IBodyOnExitHitResponse bodyOnExitHitResponse = hitResponse as IBodyOnExitHitResponse;
			if (bodyOnExitHitResponse != null)
			{
				this.m_bodyOnExitHitResponse = this.m_bodyOnExitHitResponse.Remove(bodyOnExitHitResponse);
			}
			IBodyOnStayHitResponse bodyOnStayHitResponse = hitResponse as IBodyOnStayHitResponse;
			if (bodyOnStayHitResponse != null)
			{
				this.m_bodyOnStayHitResponse = this.m_bodyOnStayHitResponse.Remove(bodyOnStayHitResponse);
			}
			IWeaponOnEnterHitResponse weaponOnEnterHitResponse = hitResponse as IWeaponOnEnterHitResponse;
			if (weaponOnEnterHitResponse != null)
			{
				this.m_weaponOnEnterHitResponse = this.m_weaponOnEnterHitResponse.Remove(weaponOnEnterHitResponse);
			}
			IWeaponOnExitHitResponse weaponOnExitHitResponse = hitResponse as IWeaponOnExitHitResponse;
			if (weaponOnExitHitResponse != null)
			{
				this.m_weaponOnExitHitResponse = this.m_weaponOnExitHitResponse.Remove(weaponOnExitHitResponse);
			}
			IWeaponOnStayHitResponse weaponOnStayHitResponse = hitResponse as IWeaponOnStayHitResponse;
			if (weaponOnStayHitResponse != null)
			{
				this.m_weaponOnStayHitResponse = this.m_weaponOnStayHitResponse.Remove(weaponOnStayHitResponse);
			}
			ITerrainOnEnterHitResponse terrainOnEnterHitResponse = hitResponse as ITerrainOnEnterHitResponse;
			if (terrainOnEnterHitResponse != null)
			{
				this.m_terrainOnEnterHitResponse = this.m_terrainOnEnterHitResponse.Remove(terrainOnEnterHitResponse);
			}
			ITerrainOnExitHitResponse terrainOnExitHitResponse = hitResponse as ITerrainOnExitHitResponse;
			if (terrainOnExitHitResponse != null)
			{
				this.m_terrainOnExitHitResponse = this.m_terrainOnExitHitResponse.Remove(terrainOnExitHitResponse);
			}
			ITerrainOnStayHitResponse terrainOnStayHitResponse = hitResponse as ITerrainOnStayHitResponse;
			if (terrainOnStayHitResponse != null)
			{
				this.m_terrainOnStayHitResponse = this.m_terrainOnStayHitResponse.Remove(terrainOnStayHitResponse);
			}
		}
	}

	// Token: 0x06001368 RID: 4968 RVA: 0x0003AAD4 File Offset: 0x00038CD4
	private void CreateHitbox(HitboxType hitboxType, List<GameObject> gameObjsToDisable)
	{
		if (hitboxType == HitboxType.Platform && !this.m_platformHitbox)
		{
			return;
		}
		LayerType layer = LayerType.Ignore_Raycast;
		GameObject gameObject = null;
		CollisionType collisionType = CollisionType.None;
		string str = null;
		switch (hitboxType)
		{
		case HitboxType.Platform:
			str = " (Active - Platform)";
			layer = (LayerType)this.m_platformHitbox.layer;
			gameObject = this.m_platformHitbox;
			break;
		case HitboxType.Terrain:
			str = " (Active - Terrain)";
			layer = LayerType.Terrain_Hitbox;
			gameObject = this.m_terrainHitbox;
			collisionType = this.m_terrainCollidesWithType;
			if ((this.m_terrainCollidesWithType & CollisionType.Platform) != CollisionType.None)
			{
				layer = LayerType.Terrain_Hitbox_HitsPlatform;
			}
			if (this.CollisionType == CollisionType.Hazard || this.CollisionType == CollisionType.TriggerHazard)
			{
				layer = LayerType.TerrainHazard_Hitbox;
			}
			if (this.CollisionType == CollisionType.ItemDrop)
			{
				layer = LayerType.Terrain_Hitbox_ItemDrop;
			}
			break;
		case HitboxType.Body:
			str = " (Active - Body)";
			layer = LayerType.Body_Hitbox;
			gameObject = this.m_bodyHitbox;
			if (this.CollisionType == CollisionType.Player)
			{
				layer = LayerType.Body_Hitbox_ForPlayerOnly;
			}
			break;
		case HitboxType.Weapon:
			str = " (Active - Weapon)";
			layer = LayerType.Weapon_Hitbox;
			gameObject = this.m_weaponHitbox;
			collisionType = this.m_weaponCollidesWithType;
			if (((collisionType & CollisionType.Player) != CollisionType.None || (collisionType & CollisionType.Player_Dodging) != CollisionType.None) && (collisionType & CollisionType.Enemy) == CollisionType.None)
			{
				layer = LayerType.Weapon_Hitbox_HitsPlayerOnly;
			}
			break;
		}
		Collider2D component = gameObject.GetComponent<Collider2D>();
		if (!component || !component.isActiveAndEnabled)
		{
			return;
		}
		Transform parent;
		if (!gameObject.transform.parent || gameObject == this.RootGameObject)
		{
			parent = gameObject.transform;
		}
		else
		{
			parent = gameObject.transform.parent;
		}
		GameObject gameObject2;
		HBColliderType hbColliderType;
		if (component is BoxCollider2D)
		{
			gameObject2 = UnityEngine.Object.Instantiate<GameObject>(UtilityLibrary.GetHitControllerBoxColliderGO(), parent, false);
			hbColliderType = HBColliderType.Box;
		}
		else if (component is CircleCollider2D)
		{
			gameObject2 = UnityEngine.Object.Instantiate<GameObject>(UtilityLibrary.GetHitControllerCircleColliderGO(), parent, false);
			hbColliderType = HBColliderType.Circle;
		}
		else if (component is CapsuleCollider2D)
		{
			gameObject2 = UnityEngine.Object.Instantiate<GameObject>(UtilityLibrary.GetHitControllerCapsuleColliderGO(), parent, false);
			hbColliderType = HBColliderType.Capsule;
		}
		else
		{
			if (!(component is PolygonCollider2D))
			{
				string str2 = "<color=red>ERROR: Invalid collider type: ";
				Type type = component.GetType();
				Debug.Log(str2 + ((type != null) ? type.ToString() : null) + " found in HitboxControllerLite.  Cannot generate hitbox.</color>");
				return;
			}
			if (CDGHelper.CanPolyColliderBeBoxCollider(component as PolygonCollider2D))
			{
				gameObject2 = UnityEngine.Object.Instantiate<GameObject>(UtilityLibrary.GetHitControllerBoxColliderGO(), parent, false);
				hbColliderType = HBColliderType.PolygonToBox;
			}
			else
			{
				gameObject2 = UnityEngine.Object.Instantiate<GameObject>(UtilityLibrary.GetHitControllerPolygonColliderGO(), parent, false);
				hbColliderType = HBColliderType.Polygon;
			}
		}
		if (gameObject.gameObject != this.RootGameObject)
		{
			gameObject2.transform.localPosition = gameObject.transform.localPosition;
			gameObject2.transform.localScale = gameObject.transform.localScale;
			gameObject2.transform.localRotation = gameObject.transform.localRotation;
			if (hitboxType == HitboxType.Platform)
			{
				this.m_platformHitbox.tag = this.RootGameObject.tag;
			}
			else
			{
				gameObject2.tag = gameObject.tag;
			}
			if (!gameObjsToDisable.Contains(gameObject))
			{
				gameObjsToDisable.Add(gameObject);
			}
		}
		Collider2D component2 = gameObject2.GetComponent<Collider2D>();
		HitboxController.CopyColliderInfo(hbColliderType, component, component2);
		HitboxInfo component3 = gameObject2.GetComponent<HitboxInfo>();
		component3.HitboxController = this;
		component3.CollidesWithType = collisionType;
		TagType equivalentTag = CollisionType_RL.GetEquivalentTag(this.CollisionType);
		component3.tag = TagType_RL.ToString(equivalentTag);
		component3.SetCollider(component2);
		gameObject2.layer = (int)layer;
		gameObject2.name = gameObject.name + str;
		this.m_rigidBody = base.GetComponent<Rigidbody2D>();
		if (gameObject2.transform.parent == base.transform)
		{
			if (!this.m_rigidBody)
			{
				this.m_rigidBody = base.gameObject.AddComponent<Rigidbody2D>();
				this.m_rigidBody.bodyType = RigidbodyType2D.Kinematic;
				this.m_rigidBody.sleepMode = RigidbodySleepMode2D.NeverSleep;
			}
		}
		else
		{
			Rigidbody2D rigidbody2D = gameObject2.AddComponent<Rigidbody2D>();
			rigidbody2D.bodyType = RigidbodyType2D.Kinematic;
			rigidbody2D.sleepMode = RigidbodySleepMode2D.NeverSleep;
		}
		switch (hitboxType)
		{
		case HitboxType.Platform:
			this.m_platformCollider = component2;
			this.m_platformHitbox = this.m_platformCollider.gameObject;
			break;
		case HitboxType.Terrain:
			this.m_terrainHitboxCollider = component2;
			this.m_terrainHitbox = gameObject2;
			break;
		case HitboxType.Body:
			this.m_bodyHitboxCollider = component2;
			this.m_bodyHitbox = gameObject2;
			break;
		case HitboxType.Weapon:
			this.m_weaponHitboxCollider = component2;
			this.m_weaponHitbox = gameObject2;
			break;
		}
		if (hitboxType == HitboxType.Platform && this.m_platformHitbox)
		{
			switch (this.m_platformCollisionType)
			{
			case PlatformCollisionType.Character:
				this.m_platformHitbox.layer = 2;
				break;
			case PlatformCollisionType.Platform_CollidesWithAll:
				this.m_platformHitbox.layer = 8;
				return;
			case PlatformCollisionType.Platform_CollidesWithPlayer:
				this.m_platformHitbox.layer = 9;
				return;
			case PlatformCollisionType.Platform_CollidesWithEnemy:
				this.m_platformHitbox.layer = 10;
				return;
			case PlatformCollisionType.Platform_OneWay:
				this.m_platformHitbox.layer = 11;
				return;
			default:
				return;
			}
		}
	}

	// Token: 0x06001369 RID: 4969 RVA: 0x0003AF34 File Offset: 0x00039134
	private void CreateHitResponseMethods(HitboxType hitboxType, GameObject rootGameObj)
	{
		switch (hitboxType)
		{
		case HitboxType.Terrain:
		{
			ITerrainOnEnterHitResponse[] componentsInChildren = rootGameObj.GetComponentsInChildren<ITerrainOnEnterHitResponse>();
			this.m_terrainOnEnterHitResponse = componentsInChildren;
			this.m_responseMethodCount += componentsInChildren.Length;
			ITerrainOnStayHitResponse[] componentsInChildren2 = rootGameObj.GetComponentsInChildren<ITerrainOnStayHitResponse>();
			this.m_terrainOnStayHitResponse = componentsInChildren2;
			this.m_responseMethodCount += componentsInChildren2.Length;
			ITerrainOnExitHitResponse[] componentsInChildren3 = rootGameObj.GetComponentsInChildren<ITerrainOnExitHitResponse>();
			this.m_terrainOnExitHitResponse = componentsInChildren3;
			this.m_responseMethodCount += componentsInChildren3.Length;
			return;
		}
		case HitboxType.Body:
		{
			IBodyOnEnterHitResponse[] componentsInChildren4 = rootGameObj.GetComponentsInChildren<IBodyOnEnterHitResponse>();
			this.m_bodyOnEnterHitResponse = componentsInChildren4;
			this.m_responseMethodCount += componentsInChildren4.Length;
			IBodyOnStayHitResponse[] componentsInChildren5 = rootGameObj.GetComponentsInChildren<IBodyOnStayHitResponse>();
			this.m_bodyOnStayHitResponse = componentsInChildren5;
			this.m_responseMethodCount += componentsInChildren5.Length;
			IBodyOnExitHitResponse[] componentsInChildren6 = rootGameObj.GetComponentsInChildren<IBodyOnExitHitResponse>();
			this.m_bodyOnExitHitResponse = componentsInChildren6;
			this.m_responseMethodCount += componentsInChildren6.Length;
			return;
		}
		case HitboxType.Weapon:
		{
			IWeaponOnEnterHitResponse[] componentsInChildren7 = rootGameObj.GetComponentsInChildren<IWeaponOnEnterHitResponse>();
			this.m_weaponOnEnterHitResponse = componentsInChildren7;
			this.m_responseMethodCount += componentsInChildren7.Length;
			IWeaponOnStayHitResponse[] componentsInChildren8 = rootGameObj.GetComponentsInChildren<IWeaponOnStayHitResponse>();
			this.m_weaponOnStayHitResponse = componentsInChildren8;
			this.m_responseMethodCount += componentsInChildren8.Length;
			IWeaponOnExitHitResponse[] componentsInChildren9 = rootGameObj.GetComponentsInChildren<IWeaponOnExitHitResponse>();
			this.m_weaponOnExitHitResponse = componentsInChildren9;
			this.m_responseMethodCount += componentsInChildren9.Length;
			return;
		}
		default:
			return;
		}
	}

	// Token: 0x0600136A RID: 4970 RVA: 0x0003B078 File Offset: 0x00039278
	public void HandleCollision(HitResponseType hitResponseType, HitboxInfo hbInfo, Collider2D otherCollider)
	{
		if (!this.ResponseMethodsInitialized)
		{
			return;
		}
		HitboxInfo component = otherCollider.GetComponent<HitboxInfo>();
		if (component != null && (component.HitboxController == this || component.HitboxController.DisableAllCollisions))
		{
			return;
		}
		LayerType layer = (LayerType)otherCollider.gameObject.layer;
		switch (layer)
		{
		case LayerType.Terrain_Hitbox_ItemDrop:
		case LayerType.Platform_CollidesWithAll:
		case LayerType.Platform_CollidesWithPlayer:
		case LayerType.Platform_CollidesWithEnemy:
		case LayerType.Platform_OneWay:
		case LayerType.Terrain_Hitbox:
		case LayerType.Terrain_Hitbox_HitsPlatform:
			break;
		case LayerType.Prop_Hitbox:
		case LayerType.LevelBounds:
		case LayerType.HELPER_NOTOUCH:
		case LayerType.Weapon_Hitbox:
		case LayerType.Weapon_Hitbox_HitsPlayerOnly:
		case LayerType.Foreground_Persp:
			return;
		case LayerType.Body_Hitbox_ForPlayerOnly:
		case LayerType.Body_Hitbox:
			if (component == null)
			{
				return;
			}
			if (hitResponseType != HitResponseType.OnExit && !this.AllowRepeatHitCheckCollision(component.RootGameObj, layer))
			{
				return;
			}
			this.LastCollidedWith = otherCollider;
			this.InvokeAllWeaponHitResponseMethods(hitResponseType, component.HitboxController);
			component.HitboxController.LastCollidedWith = hbInfo.Collider;
			component.HitboxController.InvokeAllBodyHitResponseMethods(hitResponseType, this);
			return;
		default:
			if (layer != LayerType.TerrainHazard_Hitbox)
			{
				return;
			}
			break;
		}
		IHitboxController otherHBController = null;
		GameObject obj;
		if (component != null)
		{
			otherHBController = component.HitboxController;
			obj = component.RootGameObj;
		}
		else
		{
			obj = otherCollider.gameObject;
		}
		if (hitResponseType != HitResponseType.OnExit && !this.AllowRepeatHitCheckCollision(obj, layer))
		{
			return;
		}
		this.LastCollidedWith = otherCollider;
		this.InvokeAllTerrainHitResponseMethods(hitResponseType, otherHBController);
	}

	// Token: 0x0600136B RID: 4971 RVA: 0x0003B1A0 File Offset: 0x000393A0
	public void InvokeAllBodyHitResponseMethods(HitResponseType hitResponseType, IHitboxController otherHBController)
	{
		switch (hitResponseType)
		{
		case HitResponseType.OnEnter:
			if (this.m_bodyOnEnterHitResponse != null)
			{
				IBodyOnEnterHitResponse[] bodyOnEnterHitResponse = this.m_bodyOnEnterHitResponse;
				for (int i = 0; i < bodyOnEnterHitResponse.Length; i++)
				{
					bodyOnEnterHitResponse[i].BodyOnEnterHitResponse(otherHBController);
				}
				return;
			}
			break;
		case HitResponseType.OnStay:
			if (this.m_bodyOnStayHitResponse != null)
			{
				IBodyOnStayHitResponse[] bodyOnStayHitResponse = this.m_bodyOnStayHitResponse;
				for (int i = 0; i < bodyOnStayHitResponse.Length; i++)
				{
					bodyOnStayHitResponse[i].BodyOnStayHitResponse(otherHBController);
				}
				return;
			}
			break;
		case HitResponseType.OnExit:
			if (this.m_bodyOnExitHitResponse != null)
			{
				IBodyOnExitHitResponse[] bodyOnExitHitResponse = this.m_bodyOnExitHitResponse;
				for (int i = 0; i < bodyOnExitHitResponse.Length; i++)
				{
					bodyOnExitHitResponse[i].BodyOnExitHitResponse(otherHBController);
				}
			}
			break;
		default:
			return;
		}
	}

	// Token: 0x0600136C RID: 4972 RVA: 0x0003B234 File Offset: 0x00039434
	public void InvokeAllWeaponHitResponseMethods(HitResponseType hitResponseType, IHitboxController otherHBController)
	{
		switch (hitResponseType)
		{
		case HitResponseType.OnEnter:
			if (this.m_weaponOnEnterHitResponse != null)
			{
				IWeaponOnEnterHitResponse[] weaponOnEnterHitResponse = this.m_weaponOnEnterHitResponse;
				for (int i = 0; i < weaponOnEnterHitResponse.Length; i++)
				{
					weaponOnEnterHitResponse[i].WeaponOnEnterHitResponse(otherHBController);
				}
				return;
			}
			break;
		case HitResponseType.OnStay:
			if (this.m_weaponOnStayHitResponse != null)
			{
				IWeaponOnStayHitResponse[] weaponOnStayHitResponse = this.m_weaponOnStayHitResponse;
				for (int i = 0; i < weaponOnStayHitResponse.Length; i++)
				{
					weaponOnStayHitResponse[i].WeaponOnStayHitResponse(otherHBController);
				}
				return;
			}
			break;
		case HitResponseType.OnExit:
			if (this.m_weaponOnExitHitResponse != null)
			{
				IWeaponOnExitHitResponse[] weaponOnExitHitResponse = this.m_weaponOnExitHitResponse;
				for (int i = 0; i < weaponOnExitHitResponse.Length; i++)
				{
					weaponOnExitHitResponse[i].WeaponOnExitHitResponse(otherHBController);
				}
			}
			break;
		default:
			return;
		}
	}

	// Token: 0x0600136D RID: 4973 RVA: 0x0003B2C8 File Offset: 0x000394C8
	public void InvokeAllTerrainHitResponseMethods(HitResponseType hitResponseType, IHitboxController otherHBController)
	{
		switch (hitResponseType)
		{
		case HitResponseType.OnEnter:
			if (this.m_terrainOnEnterHitResponse != null)
			{
				ITerrainOnEnterHitResponse[] terrainOnEnterHitResponse = this.m_terrainOnEnterHitResponse;
				for (int i = 0; i < terrainOnEnterHitResponse.Length; i++)
				{
					terrainOnEnterHitResponse[i].TerrainOnEnterHitResponse(otherHBController);
				}
				return;
			}
			break;
		case HitResponseType.OnStay:
			if (this.m_terrainOnStayHitResponse != null)
			{
				ITerrainOnStayHitResponse[] terrainOnStayHitResponse = this.m_terrainOnStayHitResponse;
				for (int i = 0; i < terrainOnStayHitResponse.Length; i++)
				{
					terrainOnStayHitResponse[i].TerrainOnStayHitResponse(otherHBController);
				}
				return;
			}
			break;
		case HitResponseType.OnExit:
			if (this.m_terrainOnExitHitResponse != null)
			{
				ITerrainOnExitHitResponse[] terrainOnExitHitResponse = this.m_terrainOnExitHitResponse;
				for (int i = 0; i < terrainOnExitHitResponse.Length; i++)
				{
					terrainOnExitHitResponse[i].TerrainOnExitHitResponse(otherHBController);
				}
			}
			break;
		default:
			return;
		}
	}

	// Token: 0x0600136E RID: 4974 RVA: 0x0003B35C File Offset: 0x0003955C
	public void SetHitboxActiveState(HitboxType hitboxType, bool active)
	{
		Collider2D collider2D = null;
		switch (hitboxType)
		{
		case HitboxType.Platform:
			collider2D = this.m_platformCollider;
			break;
		case HitboxType.Terrain:
			collider2D = this.m_terrainHitboxCollider;
			break;
		case HitboxType.Body:
			collider2D = this.m_bodyHitboxCollider;
			break;
		case HitboxType.Weapon:
			collider2D = this.m_weaponHitboxCollider;
			break;
		}
		if (collider2D)
		{
			collider2D.enabled = active;
		}
	}

	// Token: 0x0600136F RID: 4975 RVA: 0x0003B3B4 File Offset: 0x000395B4
	public bool GetHitboxActiveState(HitboxType hitboxType)
	{
		switch (hitboxType)
		{
		case HitboxType.Platform:
			if (this.m_platformCollider)
			{
				return this.m_platformCollider.enabled;
			}
			break;
		case HitboxType.Terrain:
			if (this.m_terrainHitboxCollider)
			{
				return this.m_terrainHitboxCollider.enabled;
			}
			break;
		case HitboxType.Body:
			if (this.m_bodyHitboxCollider)
			{
				return this.m_bodyHitboxCollider.enabled;
			}
			break;
		case HitboxType.Weapon:
			if (this.m_weaponHitboxCollider)
			{
				return this.m_weaponHitboxCollider.enabled;
			}
			break;
		}
		return false;
	}

	// Token: 0x06001370 RID: 4976 RVA: 0x0003B43E File Offset: 0x0003963E
	public void ResetAllHitboxStates()
	{
		this.SetHitboxActiveState(HitboxType.Weapon, true);
		this.SetHitboxActiveState(HitboxType.Body, true);
		this.SetHitboxActiveState(HitboxType.Terrain, true);
		this.SetHitboxActiveState(HitboxType.Platform, true);
	}

	// Token: 0x06001371 RID: 4977 RVA: 0x0003B460 File Offset: 0x00039660
	public void SetCulledState(bool culled, bool includeRigidbody)
	{
		if (culled == this.m_culled)
		{
			return;
		}
		this.m_culled = culled;
		if (culled)
		{
			this.m_bodyCullState = (this.m_bodyHitboxCollider && !this.m_bodyHitboxCollider.enabled);
			this.SetHitboxActiveState(HitboxType.Body, false);
			this.m_weaponCullState = (this.m_weaponHitboxCollider && !this.m_weaponHitboxCollider.enabled);
			this.SetHitboxActiveState(HitboxType.Weapon, false);
			this.m_terrainCullState = (this.m_terrainHitboxCollider && !this.m_terrainHitboxCollider.enabled);
			this.SetHitboxActiveState(HitboxType.Terrain, false);
			if (this.m_rigidBody && includeRigidbody)
			{
				this.m_rigidBodyCullState = !this.m_rigidBody.simulated;
				this.m_rigidBody.simulated = false;
				return;
			}
		}
		else
		{
			if (!this.m_bodyCullState)
			{
				this.SetHitboxActiveState(HitboxType.Body, true);
			}
			if (!this.m_weaponCullState)
			{
				this.SetHitboxActiveState(HitboxType.Weapon, true);
			}
			if (!this.m_terrainCullState)
			{
				this.SetHitboxActiveState(HitboxType.Terrain, true);
			}
			if (this.m_rigidBody && includeRigidbody && !this.m_rigidBodyCullState)
			{
				this.m_rigidBody.simulated = true;
			}
		}
	}

	// Token: 0x06001372 RID: 4978 RVA: 0x0003B58D File Offset: 0x0003978D
	private void InitializeRepeatHitCheck()
	{
		if (this.m_repeatHitArraySizeOverride == -1)
		{
			this.m_repeatHitArraySizeOverride = 4;
		}
		this.m_repeatHitCheckList = new List<HitboxControllerLite.RepeatHitEntry>(this.m_repeatHitArraySizeOverride);
	}

	// Token: 0x06001373 RID: 4979 RVA: 0x0003B5B0 File Offset: 0x000397B0
	private bool AllowRepeatHitCheckCollision(GameObject obj, LayerType layer)
	{
		if (this.RepeatHitDuration <= 0f)
		{
			return true;
		}
		int num = -1;
		int count = this.m_repeatHitCheckList.Count;
		for (int i = 0; i < count; i++)
		{
			HitboxControllerLite.RepeatHitEntry repeatHitEntry = this.m_repeatHitCheckList[i];
			if (repeatHitEntry.ObjHashCode == obj.GetHashCode() && repeatHitEntry.Layer == layer && repeatHitEntry.HitDuration > Time.fixedTime)
			{
				return false;
			}
			if (repeatHitEntry.ObjHashCode == 0 || repeatHitEntry.HitDuration <= Time.fixedTime)
			{
				num = i;
			}
		}
		HitboxControllerLite.RepeatHitEntry repeatHitEntry2 = default(HitboxControllerLite.RepeatHitEntry);
		repeatHitEntry2.ObjHashCode = obj.GetHashCode();
		repeatHitEntry2.HitDuration = Time.fixedTime + this.RepeatHitDuration;
		repeatHitEntry2.Layer = layer;
		if (num > -1)
		{
			this.m_repeatHitCheckList[num] = repeatHitEntry2;
		}
		else
		{
			this.m_repeatHitCheckList.Add(repeatHitEntry2);
		}
		return true;
	}

	// Token: 0x06001374 RID: 4980 RVA: 0x0003B688 File Offset: 0x00039888
	public void RemoveFromRepeatHitChecks(GameObject obj)
	{
		if (this.m_repeatHitCheckList != null)
		{
			int count = this.m_repeatHitCheckList.Count;
			for (int i = 0; i < count; i++)
			{
				HitboxControllerLite.RepeatHitEntry repeatHitEntry = this.m_repeatHitCheckList[i];
				if (repeatHitEntry.ObjHashCode == obj.GetHashCode())
				{
					repeatHitEntry.ObjHashCode = 0;
					repeatHitEntry.HitDuration = 0f;
					repeatHitEntry.Layer = LayerType.None;
					this.m_repeatHitCheckList[i] = repeatHitEntry;
				}
			}
		}
	}

	// Token: 0x06001375 RID: 4981 RVA: 0x0003B6FC File Offset: 0x000398FC
	public void ResetRepeatHitChecks()
	{
		if (this.m_repeatHitCheckList != null)
		{
			int count = this.m_repeatHitCheckList.Count;
			for (int i = 0; i < count; i++)
			{
				HitboxControllerLite.RepeatHitEntry value = this.m_repeatHitCheckList[i];
				value.ObjHashCode = 0;
				value.HitDuration = 0f;
				value.Layer = LayerType.None;
				this.m_repeatHitCheckList[i] = value;
			}
		}
	}

	// Token: 0x06001378 RID: 4984 RVA: 0x0003B78C File Offset: 0x0003998C
	GameObject IHitboxController.get_gameObject()
	{
		return base.gameObject;
	}

	// Token: 0x0400135A RID: 4954
	[SerializeField]
	private CollisionType m_collisionType;

	// Token: 0x0400135B RID: 4955
	[Space(5f)]
	[SerializeField]
	private GameObject m_bodyHitbox;

	// Token: 0x0400135C RID: 4956
	[SerializeField]
	private GameObject m_weaponHitbox;

	// Token: 0x0400135D RID: 4957
	[SerializeField]
	private GameObject m_terrainHitbox;

	// Token: 0x0400135E RID: 4958
	[SerializeField]
	private GameObject m_platformHitbox;

	// Token: 0x0400135F RID: 4959
	[SerializeField]
	[EnumFlag]
	private CollisionType m_weaponCollidesWithType;

	// Token: 0x04001360 RID: 4960
	[SerializeField]
	[EnumFlag]
	private CollisionType m_terrainCollidesWithType;

	// Token: 0x04001361 RID: 4961
	[Space(5f)]
	[SerializeField]
	private PlatformCollisionType m_platformCollisionType;

	// Token: 0x04001362 RID: 4962
	[SerializeField]
	[HideInInspector]
	private bool m_hitboxesInitialized;

	// Token: 0x04001363 RID: 4963
	[SerializeField]
	private bool m_initializeOnAwake = true;

	// Token: 0x04001364 RID: 4964
	[SerializeField]
	private int m_repeatHitArraySizeOverride = -1;

	// Token: 0x04001365 RID: 4965
	private Collider2D m_bodyHitboxCollider;

	// Token: 0x04001366 RID: 4966
	private Collider2D m_weaponHitboxCollider;

	// Token: 0x04001367 RID: 4967
	private Collider2D m_terrainHitboxCollider;

	// Token: 0x04001368 RID: 4968
	private Collider2D m_platformCollider;

	// Token: 0x04001369 RID: 4969
	private float m_repeatHitDuration = 0.5f;

	// Token: 0x0400136A RID: 4970
	private Rigidbody2D m_rigidBody;

	// Token: 0x0400136B RID: 4971
	private IDamageObj m_damageObj;

	// Token: 0x0400136C RID: 4972
	private bool m_responseMethodsInitialized;

	// Token: 0x0400136D RID: 4973
	private bool m_startExecuted;

	// Token: 0x0400136E RID: 4974
	private int m_responseMethodCount;

	// Token: 0x0400136F RID: 4975
	private IBodyOnEnterHitResponse[] m_bodyOnEnterHitResponse;

	// Token: 0x04001370 RID: 4976
	private IBodyOnStayHitResponse[] m_bodyOnStayHitResponse;

	// Token: 0x04001371 RID: 4977
	private IBodyOnExitHitResponse[] m_bodyOnExitHitResponse;

	// Token: 0x04001372 RID: 4978
	private IWeaponOnEnterHitResponse[] m_weaponOnEnterHitResponse;

	// Token: 0x04001373 RID: 4979
	private IWeaponOnStayHitResponse[] m_weaponOnStayHitResponse;

	// Token: 0x04001374 RID: 4980
	private IWeaponOnExitHitResponse[] m_weaponOnExitHitResponse;

	// Token: 0x04001375 RID: 4981
	private ITerrainOnEnterHitResponse[] m_terrainOnEnterHitResponse;

	// Token: 0x04001376 RID: 4982
	private ITerrainOnStayHitResponse[] m_terrainOnStayHitResponse;

	// Token: 0x04001377 RID: 4983
	private ITerrainOnExitHitResponse[] m_terrainOnExitHitResponse;

	// Token: 0x04001379 RID: 4985
	private bool m_disableAllCollisions;

	// Token: 0x0400137C RID: 4988
	private static List<GameObject> m_gameObjDisableList_STATIC = new List<GameObject>();

	// Token: 0x0400137D RID: 4989
	private bool m_culled;

	// Token: 0x0400137E RID: 4990
	private bool m_bodyCullState;

	// Token: 0x0400137F RID: 4991
	private bool m_weaponCullState;

	// Token: 0x04001380 RID: 4992
	private bool m_terrainCullState;

	// Token: 0x04001381 RID: 4993
	private bool m_rigidBodyCullState;

	// Token: 0x04001382 RID: 4994
	protected List<HitboxControllerLite.RepeatHitEntry> m_repeatHitCheckList;

	// Token: 0x02000AF9 RID: 2809
	protected struct RepeatHitEntry
	{
		// Token: 0x04004ADB RID: 19163
		public int ObjHashCode;

		// Token: 0x04004ADC RID: 19164
		public float HitDuration;

		// Token: 0x04004ADD RID: 19165
		public LayerType Layer;
	}
}
