using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// Token: 0x02000353 RID: 851
[RequireComponent(typeof(Rigidbody2D))]
public class HitboxController : MonoBehaviour, IHitboxController
{
	// Token: 0x17000CF1 RID: 3313
	// (get) Token: 0x06001B6D RID: 7021 RVA: 0x0000E3E3 File Offset: 0x0000C5E3
	public bool ResponseMethodsInitialized
	{
		get
		{
			return this.m_responseMethodsInitialized;
		}
	}

	// Token: 0x17000CF2 RID: 3314
	// (get) Token: 0x06001B6E RID: 7022 RVA: 0x0000E3EB File Offset: 0x0000C5EB
	// (set) Token: 0x06001B6F RID: 7023 RVA: 0x0000E3F3 File Offset: 0x0000C5F3
	public Collider2D LastCollidedWith { get; set; }

	// Token: 0x17000CF3 RID: 3315
	// (get) Token: 0x06001B70 RID: 7024 RVA: 0x0000E3FC File Offset: 0x0000C5FC
	// (set) Token: 0x06001B71 RID: 7025 RVA: 0x0000E404 File Offset: 0x0000C604
	public GameObject RootGameObject { get; private set; }

	// Token: 0x17000CF4 RID: 3316
	// (get) Token: 0x06001B72 RID: 7026 RVA: 0x0000E40D File Offset: 0x0000C60D
	// (set) Token: 0x06001B73 RID: 7027 RVA: 0x0000E415 File Offset: 0x0000C615
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

	// Token: 0x17000CF5 RID: 3317
	// (get) Token: 0x06001B74 RID: 7028 RVA: 0x0000E44C File Offset: 0x0000C64C
	// (set) Token: 0x06001B75 RID: 7029 RVA: 0x0000E454 File Offset: 0x0000C654
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

	// Token: 0x17000CF6 RID: 3318
	// (get) Token: 0x06001B76 RID: 7030 RVA: 0x0000E45D File Offset: 0x0000C65D
	// (set) Token: 0x06001B77 RID: 7031 RVA: 0x0000E465 File Offset: 0x0000C665
	public CollisionType WeaponCollidesWithType
	{
		get
		{
			return this.m_weaponCollisionType;
		}
		set
		{
			this.m_weaponCollisionType = value;
		}
	}

	// Token: 0x17000CF7 RID: 3319
	// (get) Token: 0x06001B78 RID: 7032 RVA: 0x0000E46E File Offset: 0x0000C66E
	// (set) Token: 0x06001B79 RID: 7033 RVA: 0x0000E476 File Offset: 0x0000C676
	public CollisionType TerrainCollidesWithType
	{
		get
		{
			return this.m_terrainCollisionType;
		}
		set
		{
			this.m_terrainCollisionType = value;
		}
	}

	// Token: 0x17000CF8 RID: 3320
	// (get) Token: 0x06001B7A RID: 7034 RVA: 0x0000E47F File Offset: 0x0000C67F
	// (set) Token: 0x06001B7B RID: 7035 RVA: 0x0000E487 File Offset: 0x0000C687
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

	// Token: 0x17000CF9 RID: 3321
	// (get) Token: 0x06001B7C RID: 7036 RVA: 0x0000E490 File Offset: 0x0000C690
	public IDamageObj DamageObj
	{
		get
		{
			return this.m_damageObj;
		}
	}

	// Token: 0x17000CFA RID: 3322
	// (get) Token: 0x06001B7D RID: 7037 RVA: 0x0000E498 File Offset: 0x0000C698
	// (set) Token: 0x06001B7E RID: 7038 RVA: 0x0009553C File Offset: 0x0009373C
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

	// Token: 0x17000CFB RID: 3323
	// (get) Token: 0x06001B7F RID: 7039 RVA: 0x0000E4A0 File Offset: 0x0000C6A0
	// (set) Token: 0x06001B80 RID: 7040 RVA: 0x0000E4A8 File Offset: 0x0000C6A8
	public bool IsInitialized { get; private set; }

	// Token: 0x17000CFC RID: 3324
	// (get) Token: 0x06001B81 RID: 7041 RVA: 0x0000E4B1 File Offset: 0x0000C6B1
	// (set) Token: 0x06001B82 RID: 7042 RVA: 0x0000E4B9 File Offset: 0x0000C6B9
	public GameObject[] WeaponHitboxList
	{
		get
		{
			return this.m_weaponHitboxList;
		}
		set
		{
			this.m_weaponHitboxList = value;
		}
	}

	// Token: 0x17000CFD RID: 3325
	// (get) Token: 0x06001B83 RID: 7043 RVA: 0x0000E4C2 File Offset: 0x0000C6C2
	// (set) Token: 0x06001B84 RID: 7044 RVA: 0x0000E4CA File Offset: 0x0000C6CA
	public GameObject[] BodyHitboxList
	{
		get
		{
			return this.m_bodyHitboxList;
		}
		set
		{
			this.m_bodyHitboxList = value;
		}
	}

	// Token: 0x17000CFE RID: 3326
	// (get) Token: 0x06001B85 RID: 7045 RVA: 0x0000E4D3 File Offset: 0x0000C6D3
	// (set) Token: 0x06001B86 RID: 7046 RVA: 0x0000E4DB File Offset: 0x0000C6DB
	public GameObject[] TerrainHitboxList
	{
		get
		{
			return this.m_terrainHitboxList;
		}
		set
		{
			this.m_terrainHitboxList = value;
		}
	}

	// Token: 0x17000CFF RID: 3327
	// (get) Token: 0x06001B87 RID: 7047 RVA: 0x0000E4E4 File Offset: 0x0000C6E4
	// (set) Token: 0x06001B88 RID: 7048 RVA: 0x0000E4EC File Offset: 0x0000C6EC
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

	// Token: 0x17000D00 RID: 3328
	// (get) Token: 0x06001B89 RID: 7049 RVA: 0x0000E4F5 File Offset: 0x0000C6F5
	public Collider2D[] BodyColliderList
	{
		get
		{
			return this.m_bodyColliderList;
		}
	}

	// Token: 0x17000D01 RID: 3329
	// (get) Token: 0x06001B8A RID: 7050 RVA: 0x0000E4FD File Offset: 0x0000C6FD
	public Collider2D[] WeaponColliderList
	{
		get
		{
			return this.m_weaponColliderList;
		}
	}

	// Token: 0x17000D02 RID: 3330
	// (get) Token: 0x06001B8B RID: 7051 RVA: 0x0000E505 File Offset: 0x0000C705
	public Collider2D[] TerrainColliderList
	{
		get
		{
			return this.m_terrainColliderList;
		}
	}

	// Token: 0x17000D03 RID: 3331
	// (get) Token: 0x06001B8C RID: 7052 RVA: 0x0000E50D File Offset: 0x0000C70D
	public IBodyOnEnterHitResponse[] BodyOnEnterHitResponseArray
	{
		get
		{
			return this.m_bodyOnEnterResponseArray;
		}
	}

	// Token: 0x17000D04 RID: 3332
	// (get) Token: 0x06001B8D RID: 7053 RVA: 0x0000E515 File Offset: 0x0000C715
	public IWeaponOnEnterHitResponse[] WeaponOnEnterHitResponseArray
	{
		get
		{
			return this.m_weaponOnEnterResponseArray;
		}
	}

	// Token: 0x17000D05 RID: 3333
	// (get) Token: 0x06001B8E RID: 7054 RVA: 0x0000E51D File Offset: 0x0000C71D
	public ITerrainOnEnterHitResponse[] TerrainOnEnterHitResponseArray
	{
		get
		{
			return this.m_terrainOnEnterResponseArray;
		}
	}

	// Token: 0x17000D06 RID: 3334
	// (get) Token: 0x06001B8F RID: 7055 RVA: 0x0000E525 File Offset: 0x0000C725
	public IBodyOnStayHitResponse[] BodyOnStayHitResponseArray
	{
		get
		{
			return this.m_bodyOnStayResponseArray;
		}
	}

	// Token: 0x17000D07 RID: 3335
	// (get) Token: 0x06001B90 RID: 7056 RVA: 0x0000E52D File Offset: 0x0000C72D
	public IWeaponOnStayHitResponse[] WeaponOnStayHitResponseArray
	{
		get
		{
			return this.m_weaponOnStayResponseArray;
		}
	}

	// Token: 0x17000D08 RID: 3336
	// (get) Token: 0x06001B91 RID: 7057 RVA: 0x0000E535 File Offset: 0x0000C735
	public ITerrainOnStayHitResponse[] TerrainOnStayHitResponseArray
	{
		get
		{
			return this.m_terrainOnStayResponseArray;
		}
	}

	// Token: 0x17000D09 RID: 3337
	// (get) Token: 0x06001B92 RID: 7058 RVA: 0x0000E53D File Offset: 0x0000C73D
	public IBodyOnExitHitResponse[] BodyOnExitHitResponseList
	{
		get
		{
			return this.m_bodyOnExitResponseArray;
		}
	}

	// Token: 0x17000D0A RID: 3338
	// (get) Token: 0x06001B93 RID: 7059 RVA: 0x0000E545 File Offset: 0x0000C745
	public IWeaponOnExitHitResponse[] WeaponOnExitHitResponseArray
	{
		get
		{
			return this.m_weaponOnExitResponseArray;
		}
	}

	// Token: 0x17000D0B RID: 3339
	// (get) Token: 0x06001B94 RID: 7060 RVA: 0x0000E54D File Offset: 0x0000C74D
	public ITerrainOnExitHitResponse[] TerrainOnExitHitResponseArray
	{
		get
		{
			return this.m_terrainOnExitResponseArray;
		}
	}

	// Token: 0x06001B95 RID: 7061 RVA: 0x000955A0 File Offset: 0x000937A0
	public GameObject ContainsHitbox(HitboxType hitboxType, string hitboxName)
	{
		int length = hitboxName.Length;
		GameObject[] array = null;
		switch (hitboxType)
		{
		case HitboxType.Terrain:
			array = this.m_terrainHitboxList;
			break;
		case HitboxType.Body:
			array = this.m_bodyHitboxList;
			break;
		case HitboxType.Weapon:
			array = this.m_weaponHitboxList;
			break;
		}
		if (array != null)
		{
			foreach (GameObject gameObject in array)
			{
				if (gameObject.name.Substring(0, length).Equals(hitboxName))
				{
					return gameObject;
				}
			}
		}
		return null;
	}

	// Token: 0x06001B96 RID: 7062 RVA: 0x00095618 File Offset: 0x00093818
	public void ChangeCollisionType(HitboxType hitboxType, CollisionType collisionType)
	{
		string tag = TagType_RL.ToString(CollisionType_RL.GetEquivalentTag(collisionType));
		GameObject[] array = null;
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
			array = this.m_terrainHitboxList;
			break;
		case HitboxType.Body:
			array = this.m_bodyHitboxList;
			break;
		case HitboxType.Weapon:
			array = this.m_weaponHitboxList;
			break;
		}
		if (array != null)
		{
			GameObject[] array2 = array;
			for (int i = 0; i < array2.Length; i++)
			{
				array2[i].tag = tag;
			}
		}
	}

	// Token: 0x06001B97 RID: 7063 RVA: 0x000956A8 File Offset: 0x000938A8
	public void ChangeCanCollideWith(HitboxType hitboxType, CollisionType newCollisionType)
	{
		if (hitboxType == HitboxType.Terrain)
		{
			Collider2D[] array = this.m_terrainColliderList;
			for (int i = 0; i < array.Length; i++)
			{
				array[i].GetComponent<HitboxInfo>().CollidesWithType = newCollisionType;
			}
			return;
		}
		if (hitboxType == HitboxType.Weapon)
		{
			Collider2D[] array = this.m_weaponColliderList;
			for (int i = 0; i < array.Length; i++)
			{
				array[i].GetComponent<HitboxInfo>().CollidesWithType = newCollisionType;
			}
			return;
		}
		throw new Exception("Cannot change CollideWith type for any hitboxes other than Weapon or Terrain");
	}

	// Token: 0x06001B98 RID: 7064 RVA: 0x0000E555 File Offset: 0x0000C755
	private GameObject[] GetHitboxGOArray(HitboxType hbType)
	{
		switch (hbType)
		{
		case HitboxType.Terrain:
			return this.m_terrainHitboxList;
		case HitboxType.Body:
			return this.m_bodyHitboxList;
		case HitboxType.Weapon:
			return this.m_weaponHitboxList;
		default:
			return null;
		}
	}

	// Token: 0x06001B99 RID: 7065 RVA: 0x00095710 File Offset: 0x00093910
	public bool IsTouching(HitboxType hitbox, Collider2D collider)
	{
		Collider2D[] array;
		switch (hitbox)
		{
		case HitboxType.Platform:
			return this.GetCollider(HitboxType.Platform).IsTouching(collider);
		case HitboxType.Terrain:
			array = this.TerrainColliderList;
			break;
		case HitboxType.Body:
			array = this.BodyColliderList;
			break;
		case HitboxType.Weapon:
			array = this.WeaponColliderList;
			break;
		default:
			Debug.LogFormat("{0}: HitboxController.IsTouching has no switch case for Hitbox Type ({1})", new object[]
			{
				Time.frameCount,
				hitbox
			});
			return false;
		}
		Collider2D[] array2 = array;
		for (int i = 0; i < array2.Length; i++)
		{
			if (array2[i].IsTouching(collider))
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x06001B9A RID: 7066 RVA: 0x000957A8 File Offset: 0x000939A8
	public Collider2D GetCollider(HitboxType hitboxType)
	{
		switch (hitboxType)
		{
		case HitboxType.Platform:
			return this.m_platformCollider;
		case HitboxType.Terrain:
			if (this.m_terrainColliderList != null && this.m_terrainColliderList.Length != 0)
			{
				return this.m_terrainColliderList[0];
			}
			break;
		case HitboxType.Body:
			if (this.m_bodyColliderList != null && this.m_bodyColliderList.Length != 0)
			{
				return this.m_bodyColliderList[0];
			}
			break;
		case HitboxType.Weapon:
			if (this.m_weaponColliderList != null && this.m_weaponColliderList.Length != 0)
			{
				return this.m_weaponColliderList[0];
			}
			break;
		}
		return null;
	}

	// Token: 0x06001B9B RID: 7067 RVA: 0x0000E583 File Offset: 0x0000C783
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

	// Token: 0x06001B9C RID: 7068 RVA: 0x0000E5BF File Offset: 0x0000C7BF
	private void OnDisable()
	{
		this.ResetRepeatHitChecks();
	}

	// Token: 0x06001B9D RID: 7069 RVA: 0x0000E5C7 File Offset: 0x0000C7C7
	private void Awake()
	{
		if (this.m_initializeOnAwake && !this.IsInitialized)
		{
			this.Initialize();
		}
	}

	// Token: 0x06001B9E RID: 7070 RVA: 0x0000E5DF File Offset: 0x0000C7DF
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

	// Token: 0x06001B9F RID: 7071 RVA: 0x00095824 File Offset: 0x00093A24
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

	// Token: 0x06001BA0 RID: 7072 RVA: 0x00095888 File Offset: 0x00093A88
	private void InitializeHitboxes()
	{
		if (!this.m_hitboxesInitialized)
		{
			HitboxController.m_gameObjDisableList_STATIC.Clear();
			this.CreateHitboxes(HitboxType.Body, HitboxController.m_gameObjDisableList_STATIC);
			this.CreateHitboxes(HitboxType.Weapon, HitboxController.m_gameObjDisableList_STATIC);
			this.CreateHitboxes(HitboxType.Terrain, HitboxController.m_gameObjDisableList_STATIC);
			this.CreateHitboxes(HitboxType.Platform, HitboxController.m_gameObjDisableList_STATIC);
			foreach (GameObject gameObject in HitboxController.m_gameObjDisableList_STATIC)
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
			Array.Clear(this.m_bodyHitboxList, 0, this.m_bodyHitboxList.Length);
			this.m_bodyHitboxList = new GameObject[this.m_bodyColliderList.Length];
			for (int i = 0; i < this.m_bodyColliderList.Length; i++)
			{
				this.m_bodyHitboxList[i] = this.m_bodyColliderList[i].gameObject;
			}
			Array.Clear(this.m_weaponHitboxList, 0, this.m_weaponHitboxList.Length);
			this.m_weaponHitboxList = new GameObject[this.m_weaponColliderList.Length];
			for (int j = 0; j < this.m_weaponColliderList.Length; j++)
			{
				this.m_weaponHitboxList[j] = this.m_weaponColliderList[j].gameObject;
			}
			Array.Clear(this.m_terrainHitboxList, 0, this.m_terrainHitboxList.Length);
			this.m_terrainHitboxList = new GameObject[this.m_terrainColliderList.Length];
			for (int k = 0; k < this.m_terrainColliderList.Length; k++)
			{
				this.m_terrainHitboxList[k] = this.m_terrainColliderList[k].gameObject;
			}
			this.m_hitboxesInitialized = true;
			return;
		}
		if (GameUtility.IsInGame)
		{
			Debug.Log("<color=yellow>WARNING: HitboxController on: " + this.RootGameObject.name + " is being repopoulated, meaning the awake method on this HBController was called twice. This should only occur in level editor mode.</color>");
		}
		HitboxInfo[] componentsInChildren = this.RootGameObject.GetComponentsInChildren<HitboxInfo>(true);
		int l = 0;
		while (l < componentsInChildren.Length)
		{
			HitboxInfo hitboxInfo = componentsInChildren[l];
			hitboxInfo.HitboxController = this;
			int layer = hitboxInfo.gameObject.layer;
			if (layer == 7)
			{
				goto IL_233;
			}
			switch (layer)
			{
			case 13:
			case 18:
				goto IL_233;
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
					goto IL_233;
				}
				break;
			}
			IL_240:
			l++;
			continue;
			IL_233:
			hitboxInfo.CollidesWithType = this.TerrainCollidesWithType;
			goto IL_240;
		}
		this.RepopulateColliders(HitboxType.Body);
		this.RepopulateColliders(HitboxType.Weapon);
		this.RepopulateColliders(HitboxType.Terrain);
		this.RepopulateColliders(HitboxType.Platform);
	}

	// Token: 0x06001BA1 RID: 7073 RVA: 0x00095B10 File Offset: 0x00093D10
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

	// Token: 0x06001BA2 RID: 7074 RVA: 0x00095B74 File Offset: 0x00093D74
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
			foreach (Collider2D oldCollider in this.m_terrainColliderList)
			{
				this.RealignBox2DColliderToPolyCollider(oldCollider, component);
			}
			foreach (Collider2D oldCollider2 in this.m_weaponColliderList)
			{
				this.RealignBox2DColliderToPolyCollider(oldCollider2, component);
			}
			foreach (Collider2D oldCollider3 in this.m_bodyColliderList)
			{
				this.RealignBox2DColliderToPolyCollider(oldCollider3, component);
			}
			UnityEngine.Object.DestroyImmediate(component);
		}
	}

	// Token: 0x06001BA3 RID: 7075 RVA: 0x00095C28 File Offset: 0x00093E28
	private void RealignBox2DColliderToPolyCollider(Collider2D oldCollider, PolygonCollider2D polyCollider)
	{
		if (oldCollider == null || polyCollider == null)
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

	// Token: 0x06001BA4 RID: 7076 RVA: 0x00095CB0 File Offset: 0x00093EB0
	private void InitializeResponseMethods()
	{
		this.m_bodyOnEnterResponseArray = this.PopulateResponseMethods<IBodyOnEnterHitResponse>(this.RootGameObject);
		this.m_bodyOnStayResponseArray = this.PopulateResponseMethods<IBodyOnStayHitResponse>(this.RootGameObject);
		this.m_bodyOnExitResponseArray = this.PopulateResponseMethods<IBodyOnExitHitResponse>(this.RootGameObject);
		this.m_weaponOnEnterResponseArray = this.PopulateResponseMethods<IWeaponOnEnterHitResponse>(this.RootGameObject);
		this.m_weaponOnStayResponseArray = this.PopulateResponseMethods<IWeaponOnStayHitResponse>(this.RootGameObject);
		this.m_weaponOnExitResponseArray = this.PopulateResponseMethods<IWeaponOnExitHitResponse>(this.RootGameObject);
		this.m_terrainOnEnterResponseArray = this.PopulateResponseMethods<ITerrainOnEnterHitResponse>(this.RootGameObject);
		this.m_terrainOnStayResponseArray = this.PopulateResponseMethods<ITerrainOnStayHitResponse>(this.RootGameObject);
		this.m_terrainOnExitResponseArray = this.PopulateResponseMethods<ITerrainOnExitHitResponse>(this.RootGameObject);
		this.m_responseMethodsInitialized = true;
	}

	// Token: 0x06001BA5 RID: 7077 RVA: 0x00095D68 File Offset: 0x00093F68
	private T[] PopulateResponseMethods<T>(GameObject gameObj)
	{
		T[] componentsInChildren = gameObj.GetComponentsInChildren<T>(false);
		if (componentsInChildren.Length != 0)
		{
			this.m_responseMethodCount += componentsInChildren.Length;
			return componentsInChildren;
		}
		return null;
	}

	// Token: 0x06001BA6 RID: 7078 RVA: 0x00095D94 File Offset: 0x00093F94
	public IHitResponse[] GetHitResponseArray(HitboxType hbType, HitResponseType hbResponseType)
	{
		switch (hbType)
		{
		case HitboxType.Terrain:
			switch (hbResponseType)
			{
			case HitResponseType.OnEnter:
				return this.m_terrainOnEnterResponseArray;
			case HitResponseType.OnStay:
				return this.m_terrainOnStayResponseArray;
			case HitResponseType.OnExit:
				return this.m_terrainOnExitResponseArray;
			}
			break;
		case HitboxType.Body:
			switch (hbResponseType)
			{
			case HitResponseType.OnEnter:
				return this.m_bodyOnEnterResponseArray;
			case HitResponseType.OnStay:
				return this.m_bodyOnStayResponseArray;
			case HitResponseType.OnExit:
				return this.m_bodyOnExitResponseArray;
			}
			break;
		case HitboxType.Weapon:
			switch (hbResponseType)
			{
			case HitResponseType.OnEnter:
				return this.m_weaponOnEnterResponseArray;
			case HitResponseType.OnStay:
				return this.m_weaponOnStayResponseArray;
			case HitResponseType.OnExit:
				return this.m_weaponOnExitResponseArray;
			}
			break;
		}
		return null;
	}

	// Token: 0x06001BA7 RID: 7079 RVA: 0x00095E48 File Offset: 0x00094048
	public void AddHitboxResponseMethod(IHitResponse hitResponse)
	{
		if (this.m_responseMethodsInitialized)
		{
			IBodyOnEnterHitResponse bodyOnEnterHitResponse = hitResponse as IBodyOnEnterHitResponse;
			if (bodyOnEnterHitResponse != null && !this.m_bodyOnEnterResponseArray.Contains(bodyOnEnterHitResponse))
			{
				this.m_bodyOnEnterResponseArray = this.m_bodyOnEnterResponseArray.Add(bodyOnEnterHitResponse);
			}
			IBodyOnExitHitResponse bodyOnExitHitResponse = hitResponse as IBodyOnExitHitResponse;
			if (bodyOnExitHitResponse != null && !this.m_bodyOnExitResponseArray.Contains(bodyOnExitHitResponse))
			{
				this.m_bodyOnExitResponseArray = this.m_bodyOnExitResponseArray.Add(bodyOnExitHitResponse);
			}
			IBodyOnStayHitResponse bodyOnStayHitResponse = hitResponse as IBodyOnStayHitResponse;
			if (bodyOnStayHitResponse != null && !this.m_bodyOnStayResponseArray.Contains(bodyOnStayHitResponse))
			{
				this.m_bodyOnStayResponseArray = this.m_bodyOnStayResponseArray.Add(bodyOnStayHitResponse);
			}
			IWeaponOnEnterHitResponse weaponOnEnterHitResponse = hitResponse as IWeaponOnEnterHitResponse;
			if (weaponOnEnterHitResponse != null && !this.m_weaponOnEnterResponseArray.Contains(weaponOnEnterHitResponse))
			{
				this.m_weaponOnEnterResponseArray = this.m_weaponOnEnterResponseArray.Add(weaponOnEnterHitResponse);
			}
			IWeaponOnExitHitResponse weaponOnExitHitResponse = hitResponse as IWeaponOnExitHitResponse;
			if (weaponOnExitHitResponse != null && !this.m_weaponOnExitResponseArray.Contains(weaponOnExitHitResponse))
			{
				this.m_weaponOnExitResponseArray = this.m_weaponOnExitResponseArray.Add(weaponOnExitHitResponse);
			}
			IWeaponOnStayHitResponse weaponOnStayHitResponse = hitResponse as IWeaponOnStayHitResponse;
			if (weaponOnStayHitResponse != null && !this.m_weaponOnStayResponseArray.Contains(weaponOnStayHitResponse))
			{
				this.m_weaponOnStayResponseArray = this.m_weaponOnStayResponseArray.Add(weaponOnStayHitResponse);
			}
			ITerrainOnEnterHitResponse terrainOnEnterHitResponse = hitResponse as ITerrainOnEnterHitResponse;
			if (terrainOnEnterHitResponse != null && !this.m_terrainOnEnterResponseArray.Contains(terrainOnEnterHitResponse))
			{
				this.m_terrainOnEnterResponseArray = this.m_terrainOnEnterResponseArray.Add(terrainOnEnterHitResponse);
			}
			ITerrainOnExitHitResponse terrainOnExitHitResponse = hitResponse as ITerrainOnExitHitResponse;
			if (terrainOnExitHitResponse != null && !this.m_terrainOnExitResponseArray.Contains(terrainOnExitHitResponse))
			{
				this.m_terrainOnExitResponseArray = this.m_terrainOnExitResponseArray.Add(terrainOnExitHitResponse);
			}
			ITerrainOnStayHitResponse terrainOnStayHitResponse = hitResponse as ITerrainOnStayHitResponse;
			if (terrainOnStayHitResponse != null && !this.m_terrainOnStayResponseArray.Contains(terrainOnStayHitResponse))
			{
				this.m_terrainOnStayResponseArray = this.m_terrainOnStayResponseArray.Add(terrainOnStayHitResponse);
			}
		}
	}

	// Token: 0x06001BA8 RID: 7080 RVA: 0x00095FF0 File Offset: 0x000941F0
	public void RemoveHitboxResponseMethod(IHitResponse hitResponse)
	{
		if (this.m_responseMethodsInitialized)
		{
			IBodyOnEnterHitResponse bodyOnEnterHitResponse = hitResponse as IBodyOnEnterHitResponse;
			if (bodyOnEnterHitResponse != null)
			{
				this.m_bodyOnEnterResponseArray = this.m_bodyOnEnterResponseArray.Remove(bodyOnEnterHitResponse);
			}
			IBodyOnExitHitResponse bodyOnExitHitResponse = hitResponse as IBodyOnExitHitResponse;
			if (bodyOnExitHitResponse != null)
			{
				this.m_bodyOnExitResponseArray = this.m_bodyOnExitResponseArray.Remove(bodyOnExitHitResponse);
			}
			IBodyOnStayHitResponse bodyOnStayHitResponse = hitResponse as IBodyOnStayHitResponse;
			if (bodyOnStayHitResponse != null)
			{
				this.m_bodyOnStayResponseArray = this.m_bodyOnStayResponseArray.Remove(bodyOnStayHitResponse);
			}
			IWeaponOnEnterHitResponse weaponOnEnterHitResponse = hitResponse as IWeaponOnEnterHitResponse;
			if (weaponOnEnterHitResponse != null)
			{
				this.m_weaponOnEnterResponseArray = this.m_weaponOnEnterResponseArray.Remove(weaponOnEnterHitResponse);
			}
			IWeaponOnExitHitResponse weaponOnExitHitResponse = hitResponse as IWeaponOnExitHitResponse;
			if (weaponOnExitHitResponse != null)
			{
				this.m_weaponOnExitResponseArray = this.m_weaponOnExitResponseArray.Remove(weaponOnExitHitResponse);
			}
			IWeaponOnStayHitResponse weaponOnStayHitResponse = hitResponse as IWeaponOnStayHitResponse;
			if (weaponOnStayHitResponse != null)
			{
				this.m_weaponOnStayResponseArray = this.m_weaponOnStayResponseArray.Remove(weaponOnStayHitResponse);
			}
			ITerrainOnEnterHitResponse terrainOnEnterHitResponse = hitResponse as ITerrainOnEnterHitResponse;
			if (terrainOnEnterHitResponse != null)
			{
				this.m_terrainOnEnterResponseArray = this.m_terrainOnEnterResponseArray.Remove(terrainOnEnterHitResponse);
			}
			ITerrainOnExitHitResponse terrainOnExitHitResponse = hitResponse as ITerrainOnExitHitResponse;
			if (terrainOnExitHitResponse != null)
			{
				this.m_terrainOnExitResponseArray = this.m_terrainOnExitResponseArray.Remove(terrainOnExitHitResponse);
			}
			ITerrainOnStayHitResponse terrainOnStayHitResponse = hitResponse as ITerrainOnStayHitResponse;
			if (terrainOnStayHitResponse != null)
			{
				this.m_terrainOnStayResponseArray = this.m_terrainOnStayResponseArray.Remove(terrainOnStayHitResponse);
			}
		}
	}

	// Token: 0x06001BA9 RID: 7081 RVA: 0x00096114 File Offset: 0x00094314
	private void RepopulateColliders(HitboxType hbType)
	{
		List<Collider2D> list = new List<Collider2D>();
		switch (hbType)
		{
		case HitboxType.Platform:
			if (this.m_platformHitbox)
			{
				this.m_platformCollider = this.m_platformHitbox.GetComponent<Collider2D>();
				this.m_platformHitbox.GetComponent<HitboxInfo>().SetCollider(this.m_platformCollider);
			}
			return;
		case HitboxType.Terrain:
			if (this.m_terrainColliderList != null)
			{
				Array.Clear(this.m_terrainColliderList, 0, this.m_terrainColliderList.Length);
			}
			foreach (GameObject gameObject in this.m_terrainHitboxList)
			{
				Collider2D[] components = gameObject.GetComponents<Collider2D>();
				foreach (Collider2D item in components)
				{
					list.Add(item);
				}
				HitboxInfo component = gameObject.GetComponent<HitboxInfo>();
				if (components.Length != 0)
				{
					component.SetCollider(components[0]);
				}
			}
			this.m_terrainColliderList = list.ToArray();
			return;
		case HitboxType.Body:
			if (this.m_bodyColliderList != null)
			{
				Array.Clear(this.m_bodyColliderList, 0, this.m_bodyColliderList.Length);
			}
			foreach (GameObject gameObject2 in this.m_bodyHitboxList)
			{
				Collider2D[] components2 = gameObject2.GetComponents<Collider2D>();
				foreach (Collider2D item2 in components2)
				{
					list.Add(item2);
				}
				HitboxInfo component2 = gameObject2.GetComponent<HitboxInfo>();
				if (components2.Length != 0)
				{
					component2.SetCollider(components2[0]);
				}
			}
			this.m_bodyColliderList = list.ToArray();
			return;
		case HitboxType.Weapon:
			if (this.m_weaponColliderList != null)
			{
				Array.Clear(this.m_weaponColliderList, 0, this.m_weaponColliderList.Length);
			}
			foreach (GameObject gameObject3 in this.m_weaponHitboxList)
			{
				Collider2D[] components3 = gameObject3.GetComponents<Collider2D>();
				foreach (Collider2D item3 in components3)
				{
					list.Add(item3);
				}
				HitboxInfo component3 = gameObject3.GetComponent<HitboxInfo>();
				if (components3.Length != 0)
				{
					component3.SetCollider(components3[0]);
				}
			}
			this.m_weaponColliderList = list.ToArray();
			return;
		default:
			return;
		}
	}

	// Token: 0x06001BAA RID: 7082 RVA: 0x00096310 File Offset: 0x00094510
	private void CreateHitboxes(HitboxType hitboxType, List<GameObject> gameObjsToDisable)
	{
		if (hitboxType == HitboxType.Platform && !this.m_platformHitbox)
		{
			return;
		}
		HitboxController.m_colliderListHelper.Clear();
		GameObject[] array = null;
		CollisionType collisionType = CollisionType.None;
		LayerType layer = LayerType.Ignore_Raycast;
		string str = null;
		switch (hitboxType)
		{
		case HitboxType.Platform:
			str = " (Active - Platform)";
			HitboxController.m_platformArrayHelper[0] = this.m_platformHitbox;
			array = HitboxController.m_platformArrayHelper;
			break;
		case HitboxType.Terrain:
			str = " (Active - Terrain)";
			layer = LayerType.Terrain_Hitbox;
			array = this.m_terrainHitboxList;
			collisionType = this.TerrainCollidesWithType;
			if ((collisionType & CollisionType.Platform) != CollisionType.None)
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
			array = this.m_bodyHitboxList;
			if (this.CollisionType == CollisionType.Player)
			{
				layer = LayerType.Body_Hitbox_ForPlayerOnly;
			}
			break;
		case HitboxType.Weapon:
			str = " (Active - Weapon)";
			layer = LayerType.Weapon_Hitbox;
			array = this.m_weaponHitboxList;
			collisionType = this.WeaponCollidesWithType;
			if (((collisionType & CollisionType.Player) != CollisionType.None || (collisionType & CollisionType.Player_Dodging) != CollisionType.None) && (collisionType & CollisionType.Enemy) == CollisionType.None)
			{
				layer = LayerType.Weapon_Hitbox_HitsPlayerOnly;
			}
			break;
		}
		foreach (GameObject gameObject in array)
		{
			Collider2D component = gameObject.GetComponent<Collider2D>();
			if (component && component.isActiveAndEnabled)
			{
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
						Debug.Log(str2 + ((type != null) ? type.ToString() : null) + " found in HitboxController.  Cannot generate hitbox.</color>");
						goto IL_3AC;
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
				HitboxController.m_colliderListHelper.Add(component2);
			}
			IL_3AC:;
		}
		switch (hitboxType)
		{
		case HitboxType.Platform:
			this.m_platformCollider = HitboxController.m_colliderListHelper[0];
			this.m_platformHitbox = this.m_platformCollider.gameObject;
			break;
		case HitboxType.Terrain:
			this.m_terrainColliderList = HitboxController.m_colliderListHelper.ToArray();
			break;
		case HitboxType.Body:
			this.m_bodyColliderList = HitboxController.m_colliderListHelper.ToArray();
			break;
		case HitboxType.Weapon:
			this.m_weaponColliderList = HitboxController.m_colliderListHelper.ToArray();
			break;
		}
		if (hitboxType == HitboxType.Platform && this.m_platformHitbox)
		{
			switch (this.m_platformCollisionType)
			{
			case PlatformCollisionType.Character:
				this.m_platformHitbox.layer = LayerMask.NameToLayer("Ignore Raycast");
				break;
			case PlatformCollisionType.Platform_CollidesWithAll:
				this.m_platformHitbox.layer = LayerMask.NameToLayer("Platform_CollidesWithAll");
				return;
			case PlatformCollisionType.Platform_CollidesWithPlayer:
				this.m_platformHitbox.layer = LayerMask.NameToLayer("Platform_CollidesWithPlayer");
				return;
			case PlatformCollisionType.Platform_CollidesWithEnemy:
				this.m_platformHitbox.layer = LayerMask.NameToLayer("Platform_CollidesWithEnemy");
				return;
			case PlatformCollisionType.Platform_OneWay:
				this.m_platformHitbox.layer = LayerMask.NameToLayer("Platform_OneWay");
				return;
			default:
				return;
			}
		}
	}

	// Token: 0x06001BAB RID: 7083 RVA: 0x000967F4 File Offset: 0x000949F4
	public static void CopyColliderInfo(HBColliderType hbColliderType, Collider2D oldCollider, Collider2D newCollider)
	{
		newCollider.offset = oldCollider.offset;
		switch (hbColliderType)
		{
		case HBColliderType.Box:
		{
			BoxCollider2D boxCollider2D = oldCollider as BoxCollider2D;
			(newCollider as BoxCollider2D).size = boxCollider2D.size;
			return;
		}
		case HBColliderType.Circle:
		{
			CircleCollider2D circleCollider2D = oldCollider as CircleCollider2D;
			(newCollider as CircleCollider2D).radius = circleCollider2D.radius;
			return;
		}
		case HBColliderType.Capsule:
		{
			CapsuleCollider2D capsuleCollider2D = oldCollider as CapsuleCollider2D;
			CapsuleCollider2D capsuleCollider2D2 = newCollider as CapsuleCollider2D;
			capsuleCollider2D2.size = capsuleCollider2D.size;
			capsuleCollider2D2.direction = capsuleCollider2D.direction;
			return;
		}
		case HBColliderType.PolygonToBox:
		{
			PolygonCollider2D polygonCollider2D = oldCollider as PolygonCollider2D;
			BoxCollider2D boxCollider2D2 = newCollider as BoxCollider2D;
			Vector3 localEulerAngles = polygonCollider2D.transform.localEulerAngles;
			polygonCollider2D.transform.localEulerAngles = Vector3.zero;
			boxCollider2D2.size = polygonCollider2D.bounds.size;
			boxCollider2D2.offset = polygonCollider2D.bounds.center - polygonCollider2D.gameObject.transform.position;
			polygonCollider2D.transform.localEulerAngles = localEulerAngles;
			return;
		}
		case HBColliderType.Polygon:
		{
			PolygonCollider2D polygonCollider2D2 = oldCollider as PolygonCollider2D;
			PolygonCollider2D polygonCollider2D3 = newCollider as PolygonCollider2D;
			polygonCollider2D3.pathCount = polygonCollider2D2.pathCount;
			for (int i = 0; i < polygonCollider2D2.pathCount; i++)
			{
				polygonCollider2D3.SetPath(i, polygonCollider2D2.GetPath(i));
			}
			return;
		}
		default:
			return;
		}
	}

	// Token: 0x06001BAC RID: 7084 RVA: 0x00096948 File Offset: 0x00094B48
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

	// Token: 0x06001BAD RID: 7085 RVA: 0x00096A70 File Offset: 0x00094C70
	public void InvokeAllBodyHitResponseMethods(HitResponseType hitResponseType, IHitboxController otherHBController)
	{
		switch (hitResponseType)
		{
		case HitResponseType.OnEnter:
			if (this.BodyOnEnterHitResponseArray != null)
			{
				IBodyOnEnterHitResponse[] bodyOnEnterHitResponseArray = this.BodyOnEnterHitResponseArray;
				for (int i = 0; i < bodyOnEnterHitResponseArray.Length; i++)
				{
					bodyOnEnterHitResponseArray[i].BodyOnEnterHitResponse(otherHBController);
				}
				return;
			}
			break;
		case HitResponseType.OnStay:
			if (this.BodyOnStayHitResponseArray != null)
			{
				IBodyOnStayHitResponse[] bodyOnStayHitResponseArray = this.BodyOnStayHitResponseArray;
				for (int i = 0; i < bodyOnStayHitResponseArray.Length; i++)
				{
					bodyOnStayHitResponseArray[i].BodyOnStayHitResponse(otherHBController);
				}
				return;
			}
			break;
		case HitResponseType.OnExit:
			if (this.BodyOnExitHitResponseList != null)
			{
				IBodyOnExitHitResponse[] bodyOnExitHitResponseList = this.BodyOnExitHitResponseList;
				for (int i = 0; i < bodyOnExitHitResponseList.Length; i++)
				{
					bodyOnExitHitResponseList[i].BodyOnExitHitResponse(otherHBController);
				}
			}
			break;
		default:
			return;
		}
	}

	// Token: 0x06001BAE RID: 7086 RVA: 0x00096B04 File Offset: 0x00094D04
	public void InvokeAllWeaponHitResponseMethods(HitResponseType hitResponseType, IHitboxController otherHBController)
	{
		switch (hitResponseType)
		{
		case HitResponseType.OnEnter:
			if (this.WeaponOnEnterHitResponseArray != null)
			{
				IWeaponOnEnterHitResponse[] weaponOnEnterHitResponseArray = this.WeaponOnEnterHitResponseArray;
				for (int i = 0; i < weaponOnEnterHitResponseArray.Length; i++)
				{
					weaponOnEnterHitResponseArray[i].WeaponOnEnterHitResponse(otherHBController);
				}
				return;
			}
			break;
		case HitResponseType.OnStay:
			if (this.WeaponOnStayHitResponseArray != null)
			{
				IWeaponOnStayHitResponse[] weaponOnStayHitResponseArray = this.WeaponOnStayHitResponseArray;
				for (int i = 0; i < weaponOnStayHitResponseArray.Length; i++)
				{
					weaponOnStayHitResponseArray[i].WeaponOnStayHitResponse(otherHBController);
				}
				return;
			}
			break;
		case HitResponseType.OnExit:
			if (this.WeaponOnExitHitResponseArray != null)
			{
				IWeaponOnExitHitResponse[] weaponOnExitHitResponseArray = this.WeaponOnExitHitResponseArray;
				for (int i = 0; i < weaponOnExitHitResponseArray.Length; i++)
				{
					weaponOnExitHitResponseArray[i].WeaponOnExitHitResponse(otherHBController);
				}
			}
			break;
		default:
			return;
		}
	}

	// Token: 0x06001BAF RID: 7087 RVA: 0x00096B98 File Offset: 0x00094D98
	public void InvokeAllTerrainHitResponseMethods(HitResponseType hitResponseType, IHitboxController otherHBController)
	{
		switch (hitResponseType)
		{
		case HitResponseType.OnEnter:
			if (this.TerrainOnEnterHitResponseArray != null)
			{
				ITerrainOnEnterHitResponse[] terrainOnEnterHitResponseArray = this.TerrainOnEnterHitResponseArray;
				for (int i = 0; i < terrainOnEnterHitResponseArray.Length; i++)
				{
					terrainOnEnterHitResponseArray[i].TerrainOnEnterHitResponse(otherHBController);
				}
				return;
			}
			break;
		case HitResponseType.OnStay:
			if (this.TerrainOnStayHitResponseArray != null)
			{
				ITerrainOnStayHitResponse[] terrainOnStayHitResponseArray = this.TerrainOnStayHitResponseArray;
				for (int i = 0; i < terrainOnStayHitResponseArray.Length; i++)
				{
					terrainOnStayHitResponseArray[i].TerrainOnStayHitResponse(otherHBController);
				}
				return;
			}
			break;
		case HitResponseType.OnExit:
			if (this.TerrainOnExitHitResponseArray != null)
			{
				ITerrainOnExitHitResponse[] terrainOnExitHitResponseArray = this.TerrainOnExitHitResponseArray;
				for (int i = 0; i < terrainOnExitHitResponseArray.Length; i++)
				{
					terrainOnExitHitResponseArray[i].TerrainOnExitHitResponse(otherHBController);
				}
			}
			break;
		default:
			return;
		}
	}

	// Token: 0x06001BB0 RID: 7088 RVA: 0x00096C2C File Offset: 0x00094E2C
	public void SetHitboxActiveState(HitboxType hitboxType, bool active)
	{
		Collider2D[] array = null;
		int num = 0;
		switch (hitboxType)
		{
		case HitboxType.Platform:
			if (this.m_platformCollider)
			{
				this.m_platformCollider.enabled = active;
			}
			break;
		case HitboxType.Terrain:
			array = this.TerrainColliderList;
			num = this.m_cachedTerrainHitboxStates;
			if (!active)
			{
				if (this.m_terrainHitboxStatesDisabled)
				{
					this.SetHitboxActiveState(HitboxType.Terrain, true);
				}
				this.m_terrainHitboxStatesDisabled = true;
			}
			else
			{
				this.m_terrainHitboxStatesDisabled = false;
			}
			break;
		case HitboxType.Body:
			array = this.BodyColliderList;
			num = this.m_cachedBodyHitboxStates;
			if (!active)
			{
				if (this.m_bodyHitboxStatesDisabled)
				{
					this.SetHitboxActiveState(HitboxType.Body, true);
				}
				this.m_bodyHitboxStatesDisabled = true;
			}
			else
			{
				this.m_bodyHitboxStatesDisabled = false;
			}
			break;
		case HitboxType.Weapon:
			array = this.WeaponColliderList;
			num = this.m_cachedWeaponHitboxStates;
			if (!active)
			{
				if (this.m_weaponHitboxStatesDisabled)
				{
					this.SetHitboxActiveState(HitboxType.Weapon, true);
				}
				this.m_weaponHitboxStatesDisabled = true;
			}
			else
			{
				this.m_weaponHitboxStatesDisabled = false;
			}
			break;
		}
		if (array != null)
		{
			if (!active)
			{
				this.CacheHitboxState(hitboxType);
				for (int i = 0; i < array.Length; i++)
				{
					array[i].enabled = false;
				}
				return;
			}
			for (int j = 0; j < array.Length; j++)
			{
				Collider2D collider2D = array[j];
				if ((num & 1 << j) == 0)
				{
					collider2D.enabled = true;
				}
			}
		}
	}

	// Token: 0x06001BB1 RID: 7089 RVA: 0x00096D58 File Offset: 0x00094F58
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
			return !this.m_terrainHitboxStatesDisabled;
		case HitboxType.Body:
			return !this.m_bodyHitboxStatesDisabled;
		case HitboxType.Weapon:
			return !this.m_weaponHitboxStatesDisabled;
		}
		return false;
	}

	// Token: 0x06001BB2 RID: 7090 RVA: 0x00096DB8 File Offset: 0x00094FB8
	private void CacheHitboxState(HitboxType hitboxType)
	{
		Collider2D[] array = null;
		switch (hitboxType)
		{
		case HitboxType.Terrain:
			array = this.TerrainColliderList;
			break;
		case HitboxType.Body:
			array = this.BodyColliderList;
			break;
		case HitboxType.Weapon:
			array = this.WeaponColliderList;
			break;
		}
		int num = 0;
		for (int i = 0; i < array.Length; i++)
		{
			if (array[i].enabled)
			{
				num &= ~(1 << i);
			}
			else
			{
				num |= 1 << i;
			}
		}
		switch (hitboxType)
		{
		case HitboxType.Terrain:
			this.m_cachedTerrainHitboxStates = num;
			return;
		case HitboxType.Body:
			this.m_cachedBodyHitboxStates = num;
			return;
		case HitboxType.Weapon:
			this.m_cachedWeaponHitboxStates = num;
			return;
		default:
			return;
		}
	}

	// Token: 0x06001BB3 RID: 7091 RVA: 0x0000E5EE File Offset: 0x0000C7EE
	public void ResetAllHitboxStates()
	{
		this.SetHitboxActiveState(HitboxType.Weapon, false);
		this.SetHitboxActiveState(HitboxType.Body, false);
		this.SetHitboxActiveState(HitboxType.Terrain, false);
		this.SetHitboxActiveState(HitboxType.Platform, false);
		this.m_cachedTerrainHitboxStates = 0;
		this.m_cachedBodyHitboxStates = 0;
		this.m_cachedWeaponHitboxStates = 0;
	}

	// Token: 0x06001BB4 RID: 7092 RVA: 0x00096E54 File Offset: 0x00095054
	public void SetCulledState(bool culled, bool includeRigidbody)
	{
		if (culled == this.m_culled)
		{
			return;
		}
		this.m_culled = culled;
		if (culled)
		{
			this.m_bodyCullState = this.m_bodyHitboxStatesDisabled;
			this.SetHitboxActiveState(HitboxType.Body, false);
			this.m_weaponCullState = this.m_weaponHitboxStatesDisabled;
			this.SetHitboxActiveState(HitboxType.Weapon, false);
			this.m_terrainCullState = this.m_terrainHitboxStatesDisabled;
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

	// Token: 0x06001BB5 RID: 7093 RVA: 0x0000E625 File Offset: 0x0000C825
	private void InitializeRepeatHitCheck()
	{
		if (this.m_repeatHitArraySizeOverride == -1)
		{
			this.m_repeatHitArraySizeOverride = 4;
		}
		this.m_repeatHitCheckList = new List<HitboxController.RepeatHitEntry>(this.m_repeatHitArraySizeOverride);
	}

	// Token: 0x06001BB6 RID: 7094 RVA: 0x00096F38 File Offset: 0x00095138
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
			HitboxController.RepeatHitEntry repeatHitEntry = this.m_repeatHitCheckList[i];
			if (repeatHitEntry.ObjHashCode == obj.GetHashCode() && repeatHitEntry.Layer == layer && repeatHitEntry.HitDuration > Time.fixedTime)
			{
				return false;
			}
			if (repeatHitEntry.ObjHashCode == 0 || repeatHitEntry.HitDuration <= Time.fixedTime)
			{
				num = i;
			}
		}
		HitboxController.RepeatHitEntry repeatHitEntry2 = default(HitboxController.RepeatHitEntry);
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

	// Token: 0x06001BB7 RID: 7095 RVA: 0x00097010 File Offset: 0x00095210
	public void RemoveFromRepeatHitChecks(GameObject obj)
	{
		if (this.m_repeatHitCheckList != null)
		{
			int count = this.m_repeatHitCheckList.Count;
			for (int i = 0; i < count; i++)
			{
				HitboxController.RepeatHitEntry repeatHitEntry = this.m_repeatHitCheckList[i];
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

	// Token: 0x06001BB8 RID: 7096 RVA: 0x00097084 File Offset: 0x00095284
	public void ResetRepeatHitChecks()
	{
		if (this.m_repeatHitCheckList != null)
		{
			int count = this.m_repeatHitCheckList.Count;
			for (int i = 0; i < count; i++)
			{
				HitboxController.RepeatHitEntry value = this.m_repeatHitCheckList[i];
				value.ObjHashCode = 0;
				value.HitDuration = 0f;
				value.Layer = LayerType.None;
				this.m_repeatHitCheckList[i] = value;
			}
		}
	}

	// Token: 0x06001BBB RID: 7099 RVA: 0x00003713 File Offset: 0x00001913
	GameObject IHitboxController.get_gameObject()
	{
		return base.gameObject;
	}

	// Token: 0x04001963 RID: 6499
	public const int REPEAT_HIT_ARRAY_DEFAULT_SIZE = 4;

	// Token: 0x04001964 RID: 6500
	public const string PLATFORM_TITLE = " (Active - Platform)";

	// Token: 0x04001965 RID: 6501
	public const string BODY_TITLE = " (Active - Body)";

	// Token: 0x04001966 RID: 6502
	public const string WEAPON_TITLE = " (Active - Weapon)";

	// Token: 0x04001967 RID: 6503
	public const string TERRAIN_TITLE = " (Active - Terrain)";

	// Token: 0x04001968 RID: 6504
	[Space(5f)]
	[SerializeField]
	private CollisionType m_collisionType;

	// Token: 0x04001969 RID: 6505
	private float m_repeatHitDuration = 0.5f;

	// Token: 0x0400196A RID: 6506
	[Space(5f)]
	[Header("Body Hitboxes")]
	[SerializeField]
	private GameObject[] m_bodyHitboxList = new GameObject[0];

	// Token: 0x0400196B RID: 6507
	[Space(5f)]
	[Header("Weapon Hitboxes")]
	[SerializeField]
	private GameObject[] m_weaponHitboxList = new GameObject[0];

	// Token: 0x0400196C RID: 6508
	[Space(5f)]
	[Header("Terrain Hitboxes")]
	[SerializeField]
	private GameObject[] m_terrainHitboxList = new GameObject[0];

	// Token: 0x0400196D RID: 6509
	[Space(5f)]
	[Header("Platform Hitbox")]
	[SerializeField]
	private GameObject m_platformHitbox;

	// Token: 0x0400196E RID: 6510
	[SerializeField]
	private PlatformCollisionType m_platformCollisionType;

	// Token: 0x0400196F RID: 6511
	[SerializeField]
	[EnumFlag]
	private CollisionType m_weaponCollisionType;

	// Token: 0x04001970 RID: 6512
	[SerializeField]
	[EnumFlag]
	private CollisionType m_terrainCollisionType;

	// Token: 0x04001971 RID: 6513
	[SerializeField]
	[HideInInspector]
	private bool m_hitboxesInitialized;

	// Token: 0x04001972 RID: 6514
	[SerializeField]
	private bool m_initializeOnAwake = true;

	// Token: 0x04001973 RID: 6515
	[SerializeField]
	private int m_repeatHitArraySizeOverride = -1;

	// Token: 0x04001974 RID: 6516
	private Collider2D[] m_terrainColliderList;

	// Token: 0x04001975 RID: 6517
	private Collider2D[] m_bodyColliderList;

	// Token: 0x04001976 RID: 6518
	private Collider2D[] m_weaponColliderList;

	// Token: 0x04001977 RID: 6519
	private Collider2D m_platformCollider;

	// Token: 0x04001978 RID: 6520
	private Rigidbody2D m_rigidBody;

	// Token: 0x04001979 RID: 6521
	private ITerrainOnEnterHitResponse[] m_terrainOnEnterResponseArray;

	// Token: 0x0400197A RID: 6522
	private ITerrainOnStayHitResponse[] m_terrainOnStayResponseArray;

	// Token: 0x0400197B RID: 6523
	private ITerrainOnExitHitResponse[] m_terrainOnExitResponseArray;

	// Token: 0x0400197C RID: 6524
	private IBodyOnEnterHitResponse[] m_bodyOnEnterResponseArray;

	// Token: 0x0400197D RID: 6525
	private IBodyOnStayHitResponse[] m_bodyOnStayResponseArray;

	// Token: 0x0400197E RID: 6526
	private IBodyOnExitHitResponse[] m_bodyOnExitResponseArray;

	// Token: 0x0400197F RID: 6527
	private IWeaponOnEnterHitResponse[] m_weaponOnEnterResponseArray;

	// Token: 0x04001980 RID: 6528
	private IWeaponOnStayHitResponse[] m_weaponOnStayResponseArray;

	// Token: 0x04001981 RID: 6529
	private IWeaponOnExitHitResponse[] m_weaponOnExitResponseArray;

	// Token: 0x04001982 RID: 6530
	private IDamageObj m_damageObj;

	// Token: 0x04001983 RID: 6531
	private int m_responseMethodCount;

	// Token: 0x04001984 RID: 6532
	private bool m_responseMethodsInitialized;

	// Token: 0x04001985 RID: 6533
	private bool m_startExecuted;

	// Token: 0x04001986 RID: 6534
	private static List<string> m_warningIssuedTracker = new List<string>();

	// Token: 0x04001989 RID: 6537
	private bool m_disableAllCollisions;

	// Token: 0x0400198B RID: 6539
	private static List<GameObject> m_gameObjDisableList_STATIC = new List<GameObject>();

	// Token: 0x0400198C RID: 6540
	private static List<Collider2D> m_colliderListHelper = new List<Collider2D>();

	// Token: 0x0400198D RID: 6541
	private static GameObject[] m_platformArrayHelper = new GameObject[1];

	// Token: 0x0400198E RID: 6542
	private int m_cachedWeaponHitboxStates;

	// Token: 0x0400198F RID: 6543
	private int m_cachedBodyHitboxStates;

	// Token: 0x04001990 RID: 6544
	private int m_cachedTerrainHitboxStates;

	// Token: 0x04001991 RID: 6545
	private bool m_weaponHitboxStatesDisabled;

	// Token: 0x04001992 RID: 6546
	private bool m_bodyHitboxStatesDisabled;

	// Token: 0x04001993 RID: 6547
	private bool m_terrainHitboxStatesDisabled;

	// Token: 0x04001994 RID: 6548
	private bool m_culled;

	// Token: 0x04001995 RID: 6549
	private bool m_bodyCullState;

	// Token: 0x04001996 RID: 6550
	private bool m_weaponCullState;

	// Token: 0x04001997 RID: 6551
	private bool m_terrainCullState;

	// Token: 0x04001998 RID: 6552
	private bool m_rigidBodyCullState;

	// Token: 0x04001999 RID: 6553
	protected List<HitboxController.RepeatHitEntry> m_repeatHitCheckList;

	// Token: 0x02000354 RID: 852
	protected struct RepeatHitEntry
	{
		// Token: 0x0400199A RID: 6554
		public int ObjHashCode;

		// Token: 0x0400199B RID: 6555
		public float HitDuration;

		// Token: 0x0400199C RID: 6556
		public LayerType Layer;
	}
}
