using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// Token: 0x02000356 RID: 854
[RequireComponent(typeof(Rigidbody2D))]
public class HitboxControllerLite : MonoBehaviour, IHitboxController
{
	// Token: 0x17000D0E RID: 3342
	// (get) Token: 0x06001BC2 RID: 7106 RVA: 0x0000E68A File Offset: 0x0000C88A
	public bool ResponseMethodsInitialized
	{
		get
		{
			return this.m_responseMethodsInitialized;
		}
	}

	// Token: 0x17000D0F RID: 3343
	// (get) Token: 0x06001BC3 RID: 7107 RVA: 0x0000E692 File Offset: 0x0000C892
	// (set) Token: 0x06001BC4 RID: 7108 RVA: 0x0000E69A File Offset: 0x0000C89A
	public Collider2D LastCollidedWith { get; set; }

	// Token: 0x17000D10 RID: 3344
	// (get) Token: 0x06001BC5 RID: 7109 RVA: 0x0000E6A3 File Offset: 0x0000C8A3
	// (set) Token: 0x06001BC6 RID: 7110 RVA: 0x0000E6AB File Offset: 0x0000C8AB
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

	// Token: 0x17000D11 RID: 3345
	// (get) Token: 0x06001BC7 RID: 7111 RVA: 0x0000E6E2 File Offset: 0x0000C8E2
	public IBodyOnEnterHitResponse[] BodyOnEnterHitResponseArray
	{
		get
		{
			return this.m_bodyOnEnterHitResponse;
		}
	}

	// Token: 0x17000D12 RID: 3346
	// (get) Token: 0x06001BC8 RID: 7112 RVA: 0x0000E6EA File Offset: 0x0000C8EA
	public IWeaponOnEnterHitResponse[] WeaponOnEnterHitResponseArray
	{
		get
		{
			return this.m_weaponOnEnterHitResponse;
		}
	}

	// Token: 0x17000D13 RID: 3347
	// (get) Token: 0x06001BC9 RID: 7113 RVA: 0x0000E6F2 File Offset: 0x0000C8F2
	public ITerrainOnEnterHitResponse[] TerrainOnEnterHitResponseArray
	{
		get
		{
			return this.m_terrainOnEnterHitResponse;
		}
	}

	// Token: 0x17000D14 RID: 3348
	// (get) Token: 0x06001BCA RID: 7114 RVA: 0x0000E6FA File Offset: 0x0000C8FA
	public IBodyOnStayHitResponse[] BodyOnStayHitResponseArray
	{
		get
		{
			return this.m_bodyOnStayHitResponse;
		}
	}

	// Token: 0x17000D15 RID: 3349
	// (get) Token: 0x06001BCB RID: 7115 RVA: 0x0000E702 File Offset: 0x0000C902
	public IWeaponOnStayHitResponse[] WeaponOnStayHitResponseArray
	{
		get
		{
			return this.m_weaponOnStayHitResponse;
		}
	}

	// Token: 0x17000D16 RID: 3350
	// (get) Token: 0x06001BCC RID: 7116 RVA: 0x0000E70A File Offset: 0x0000C90A
	public ITerrainOnStayHitResponse[] TerrainOnStayHitResponseArray
	{
		get
		{
			return this.m_terrainOnStayHitResponse;
		}
	}

	// Token: 0x17000D17 RID: 3351
	// (get) Token: 0x06001BCD RID: 7117 RVA: 0x0000E712 File Offset: 0x0000C912
	public IBodyOnExitHitResponse[] BodyOnExitHitResponseList
	{
		get
		{
			return this.m_bodyOnExitHitResponse;
		}
	}

	// Token: 0x17000D18 RID: 3352
	// (get) Token: 0x06001BCE RID: 7118 RVA: 0x0000E71A File Offset: 0x0000C91A
	public IWeaponOnExitHitResponse[] WeaponOnExitHitResponseArray
	{
		get
		{
			return this.m_weaponOnExitHitResponse;
		}
	}

	// Token: 0x17000D19 RID: 3353
	// (get) Token: 0x06001BCF RID: 7119 RVA: 0x0000E722 File Offset: 0x0000C922
	public ITerrainOnExitHitResponse[] TerrainOnExitHitResponseArray
	{
		get
		{
			return this.m_terrainOnExitHitResponse;
		}
	}

	// Token: 0x17000D1A RID: 3354
	// (get) Token: 0x06001BD0 RID: 7120 RVA: 0x0000E72A File Offset: 0x0000C92A
	// (set) Token: 0x06001BD1 RID: 7121 RVA: 0x0000E732 File Offset: 0x0000C932
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

	// Token: 0x17000D1B RID: 3355
	// (get) Token: 0x06001BD2 RID: 7122 RVA: 0x0000E73B File Offset: 0x0000C93B
	// (set) Token: 0x06001BD3 RID: 7123 RVA: 0x0000E743 File Offset: 0x0000C943
	public GameObject RootGameObject { get; private set; }

	// Token: 0x17000D1C RID: 3356
	// (get) Token: 0x06001BD4 RID: 7124 RVA: 0x0000E74C File Offset: 0x0000C94C
	public IDamageObj DamageObj
	{
		get
		{
			return this.m_damageObj;
		}
	}

	// Token: 0x17000D1D RID: 3357
	// (get) Token: 0x06001BD5 RID: 7125 RVA: 0x0000E754 File Offset: 0x0000C954
	// (set) Token: 0x06001BD6 RID: 7126 RVA: 0x0000E75C File Offset: 0x0000C95C
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

	// Token: 0x17000D1E RID: 3358
	// (get) Token: 0x06001BD7 RID: 7127 RVA: 0x0000E765 File Offset: 0x0000C965
	// (set) Token: 0x06001BD8 RID: 7128 RVA: 0x0000E76D File Offset: 0x0000C96D
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

	// Token: 0x17000D1F RID: 3359
	// (get) Token: 0x06001BD9 RID: 7129 RVA: 0x0000E776 File Offset: 0x0000C976
	// (set) Token: 0x06001BDA RID: 7130 RVA: 0x0000E77E File Offset: 0x0000C97E
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

	// Token: 0x17000D20 RID: 3360
	// (get) Token: 0x06001BDB RID: 7131 RVA: 0x0000E787 File Offset: 0x0000C987
	// (set) Token: 0x06001BDC RID: 7132 RVA: 0x0000E78F File Offset: 0x0000C98F
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

	// Token: 0x17000D21 RID: 3361
	// (get) Token: 0x06001BDD RID: 7133 RVA: 0x0000E798 File Offset: 0x0000C998
	public Collider2D PlatformCollider
	{
		get
		{
			return this.m_platformCollider;
		}
	}

	// Token: 0x17000D22 RID: 3362
	// (get) Token: 0x06001BDE RID: 7134 RVA: 0x0000E7A0 File Offset: 0x0000C9A0
	// (set) Token: 0x06001BDF RID: 7135 RVA: 0x0000E7A8 File Offset: 0x0000C9A8
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

	// Token: 0x17000D23 RID: 3363
	// (get) Token: 0x06001BE0 RID: 7136 RVA: 0x0000E7B1 File Offset: 0x0000C9B1
	// (set) Token: 0x06001BE1 RID: 7137 RVA: 0x0009719C File Offset: 0x0009539C
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

	// Token: 0x17000D24 RID: 3364
	// (get) Token: 0x06001BE2 RID: 7138 RVA: 0x0000E7B9 File Offset: 0x0000C9B9
	// (set) Token: 0x06001BE3 RID: 7139 RVA: 0x0000E7C1 File Offset: 0x0000C9C1
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

	// Token: 0x17000D25 RID: 3365
	// (get) Token: 0x06001BE4 RID: 7140 RVA: 0x0000E7CA File Offset: 0x0000C9CA
	// (set) Token: 0x06001BE5 RID: 7141 RVA: 0x0000E7D2 File Offset: 0x0000C9D2
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

	// Token: 0x06001BE6 RID: 7142 RVA: 0x00097200 File Offset: 0x00095400
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

	// Token: 0x06001BE7 RID: 7143 RVA: 0x00097268 File Offset: 0x00095468
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

	// Token: 0x06001BE8 RID: 7144 RVA: 0x0000E7DB File Offset: 0x0000C9DB
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

	// Token: 0x06001BE9 RID: 7145 RVA: 0x0000E812 File Offset: 0x0000CA12
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

	// Token: 0x06001BEA RID: 7146 RVA: 0x000972EC File Offset: 0x000954EC
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

	// Token: 0x17000D26 RID: 3366
	// (get) Token: 0x06001BEB RID: 7147 RVA: 0x0000E849 File Offset: 0x0000CA49
	// (set) Token: 0x06001BEC RID: 7148 RVA: 0x0000E851 File Offset: 0x0000CA51
	public bool IsInitialized { get; private set; }

	// Token: 0x06001BED RID: 7149 RVA: 0x0000E85A File Offset: 0x0000CA5A
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

	// Token: 0x06001BEE RID: 7150 RVA: 0x0000E896 File Offset: 0x0000CA96
	private void OnDisable()
	{
		this.ResetRepeatHitChecks();
	}

	// Token: 0x06001BEF RID: 7151 RVA: 0x0000E89E File Offset: 0x0000CA9E
	private void Awake()
	{
		if (this.m_initializeOnAwake && !this.IsInitialized)
		{
			this.Initialize();
		}
	}

	// Token: 0x06001BF0 RID: 7152 RVA: 0x0000E8B6 File Offset: 0x0000CAB6
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

	// Token: 0x06001BF1 RID: 7153 RVA: 0x0009734C File Offset: 0x0009554C
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

	// Token: 0x06001BF2 RID: 7154 RVA: 0x000973B0 File Offset: 0x000955B0
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

	// Token: 0x06001BF3 RID: 7155 RVA: 0x0009754C File Offset: 0x0009574C
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

	// Token: 0x06001BF4 RID: 7156 RVA: 0x000975B0 File Offset: 0x000957B0
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

	// Token: 0x06001BF5 RID: 7157 RVA: 0x00097624 File Offset: 0x00095824
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

	// Token: 0x06001BF6 RID: 7158 RVA: 0x000976A8 File Offset: 0x000958A8
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

	// Token: 0x06001BF7 RID: 7159 RVA: 0x000977D0 File Offset: 0x000959D0
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

	// Token: 0x06001BF8 RID: 7160 RVA: 0x00097838 File Offset: 0x00095A38
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

	// Token: 0x06001BF9 RID: 7161 RVA: 0x000978EC File Offset: 0x00095AEC
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

	// Token: 0x06001BFA RID: 7162 RVA: 0x00097A94 File Offset: 0x00095C94
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

	// Token: 0x06001BFB RID: 7163 RVA: 0x00097BB8 File Offset: 0x00095DB8
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

	// Token: 0x06001BFC RID: 7164 RVA: 0x00098018 File Offset: 0x00096218
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

	// Token: 0x06001BFD RID: 7165 RVA: 0x0009815C File Offset: 0x0009635C
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

	// Token: 0x06001BFE RID: 7166 RVA: 0x00098284 File Offset: 0x00096484
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

	// Token: 0x06001BFF RID: 7167 RVA: 0x00098318 File Offset: 0x00096518
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

	// Token: 0x06001C00 RID: 7168 RVA: 0x000983AC File Offset: 0x000965AC
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

	// Token: 0x06001C01 RID: 7169 RVA: 0x00098440 File Offset: 0x00096640
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

	// Token: 0x06001C02 RID: 7170 RVA: 0x00098498 File Offset: 0x00096698
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

	// Token: 0x06001C03 RID: 7171 RVA: 0x0000E8C5 File Offset: 0x0000CAC5
	public void ResetAllHitboxStates()
	{
		this.SetHitboxActiveState(HitboxType.Weapon, true);
		this.SetHitboxActiveState(HitboxType.Body, true);
		this.SetHitboxActiveState(HitboxType.Terrain, true);
		this.SetHitboxActiveState(HitboxType.Platform, true);
	}

	// Token: 0x06001C04 RID: 7172 RVA: 0x00098524 File Offset: 0x00096724
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

	// Token: 0x06001C05 RID: 7173 RVA: 0x0000E8E7 File Offset: 0x0000CAE7
	private void InitializeRepeatHitCheck()
	{
		if (this.m_repeatHitArraySizeOverride == -1)
		{
			this.m_repeatHitArraySizeOverride = 4;
		}
		this.m_repeatHitCheckList = new List<HitboxControllerLite.RepeatHitEntry>(this.m_repeatHitArraySizeOverride);
	}

	// Token: 0x06001C06 RID: 7174 RVA: 0x00098654 File Offset: 0x00096854
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

	// Token: 0x06001C07 RID: 7175 RVA: 0x0009872C File Offset: 0x0009692C
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

	// Token: 0x06001C08 RID: 7176 RVA: 0x000987A0 File Offset: 0x000969A0
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

	// Token: 0x06001C0B RID: 7179 RVA: 0x00003713 File Offset: 0x00001913
	GameObject IHitboxController.get_gameObject()
	{
		return base.gameObject;
	}

	// Token: 0x040019A0 RID: 6560
	[SerializeField]
	private CollisionType m_collisionType;

	// Token: 0x040019A1 RID: 6561
	[Space(5f)]
	[SerializeField]
	private GameObject m_bodyHitbox;

	// Token: 0x040019A2 RID: 6562
	[SerializeField]
	private GameObject m_weaponHitbox;

	// Token: 0x040019A3 RID: 6563
	[SerializeField]
	private GameObject m_terrainHitbox;

	// Token: 0x040019A4 RID: 6564
	[SerializeField]
	private GameObject m_platformHitbox;

	// Token: 0x040019A5 RID: 6565
	[SerializeField]
	[EnumFlag]
	private CollisionType m_weaponCollidesWithType;

	// Token: 0x040019A6 RID: 6566
	[SerializeField]
	[EnumFlag]
	private CollisionType m_terrainCollidesWithType;

	// Token: 0x040019A7 RID: 6567
	[Space(5f)]
	[SerializeField]
	private PlatformCollisionType m_platformCollisionType;

	// Token: 0x040019A8 RID: 6568
	[SerializeField]
	[HideInInspector]
	private bool m_hitboxesInitialized;

	// Token: 0x040019A9 RID: 6569
	[SerializeField]
	private bool m_initializeOnAwake = true;

	// Token: 0x040019AA RID: 6570
	[SerializeField]
	private int m_repeatHitArraySizeOverride = -1;

	// Token: 0x040019AB RID: 6571
	private Collider2D m_bodyHitboxCollider;

	// Token: 0x040019AC RID: 6572
	private Collider2D m_weaponHitboxCollider;

	// Token: 0x040019AD RID: 6573
	private Collider2D m_terrainHitboxCollider;

	// Token: 0x040019AE RID: 6574
	private Collider2D m_platformCollider;

	// Token: 0x040019AF RID: 6575
	private float m_repeatHitDuration = 0.5f;

	// Token: 0x040019B0 RID: 6576
	private Rigidbody2D m_rigidBody;

	// Token: 0x040019B1 RID: 6577
	private IDamageObj m_damageObj;

	// Token: 0x040019B2 RID: 6578
	private bool m_responseMethodsInitialized;

	// Token: 0x040019B3 RID: 6579
	private bool m_startExecuted;

	// Token: 0x040019B4 RID: 6580
	private int m_responseMethodCount;

	// Token: 0x040019B5 RID: 6581
	private IBodyOnEnterHitResponse[] m_bodyOnEnterHitResponse;

	// Token: 0x040019B6 RID: 6582
	private IBodyOnStayHitResponse[] m_bodyOnStayHitResponse;

	// Token: 0x040019B7 RID: 6583
	private IBodyOnExitHitResponse[] m_bodyOnExitHitResponse;

	// Token: 0x040019B8 RID: 6584
	private IWeaponOnEnterHitResponse[] m_weaponOnEnterHitResponse;

	// Token: 0x040019B9 RID: 6585
	private IWeaponOnStayHitResponse[] m_weaponOnStayHitResponse;

	// Token: 0x040019BA RID: 6586
	private IWeaponOnExitHitResponse[] m_weaponOnExitHitResponse;

	// Token: 0x040019BB RID: 6587
	private ITerrainOnEnterHitResponse[] m_terrainOnEnterHitResponse;

	// Token: 0x040019BC RID: 6588
	private ITerrainOnStayHitResponse[] m_terrainOnStayHitResponse;

	// Token: 0x040019BD RID: 6589
	private ITerrainOnExitHitResponse[] m_terrainOnExitHitResponse;

	// Token: 0x040019BF RID: 6591
	private bool m_disableAllCollisions;

	// Token: 0x040019C2 RID: 6594
	private static List<GameObject> m_gameObjDisableList_STATIC = new List<GameObject>();

	// Token: 0x040019C3 RID: 6595
	private bool m_culled;

	// Token: 0x040019C4 RID: 6596
	private bool m_bodyCullState;

	// Token: 0x040019C5 RID: 6597
	private bool m_weaponCullState;

	// Token: 0x040019C6 RID: 6598
	private bool m_terrainCullState;

	// Token: 0x040019C7 RID: 6599
	private bool m_rigidBodyCullState;

	// Token: 0x040019C8 RID: 6600
	protected List<HitboxControllerLite.RepeatHitEntry> m_repeatHitCheckList;

	// Token: 0x02000357 RID: 855
	protected struct RepeatHitEntry
	{
		// Token: 0x040019C9 RID: 6601
		public int ObjHashCode;

		// Token: 0x040019CA RID: 6602
		public float HitDuration;

		// Token: 0x040019CB RID: 6603
		public LayerType Layer;
	}
}
