using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// Token: 0x020001D5 RID: 469
[RequireComponent(typeof(Rigidbody2D))]
public class HitboxController : MonoBehaviour, IHitboxController
{
	// Token: 0x17000A1D RID: 2589
	// (get) Token: 0x060012E0 RID: 4832 RVA: 0x00037FE6 File Offset: 0x000361E6
	public bool ResponseMethodsInitialized
	{
		get
		{
			return this.m_responseMethodsInitialized;
		}
	}

	// Token: 0x17000A1E RID: 2590
	// (get) Token: 0x060012E1 RID: 4833 RVA: 0x00037FEE File Offset: 0x000361EE
	// (set) Token: 0x060012E2 RID: 4834 RVA: 0x00037FF6 File Offset: 0x000361F6
	public Collider2D LastCollidedWith { get; set; }

	// Token: 0x17000A1F RID: 2591
	// (get) Token: 0x060012E3 RID: 4835 RVA: 0x00037FFF File Offset: 0x000361FF
	// (set) Token: 0x060012E4 RID: 4836 RVA: 0x00038007 File Offset: 0x00036207
	public GameObject RootGameObject { get; private set; }

	// Token: 0x17000A20 RID: 2592
	// (get) Token: 0x060012E5 RID: 4837 RVA: 0x00038010 File Offset: 0x00036210
	// (set) Token: 0x060012E6 RID: 4838 RVA: 0x00038018 File Offset: 0x00036218
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

	// Token: 0x17000A21 RID: 2593
	// (get) Token: 0x060012E7 RID: 4839 RVA: 0x0003804F File Offset: 0x0003624F
	// (set) Token: 0x060012E8 RID: 4840 RVA: 0x00038057 File Offset: 0x00036257
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

	// Token: 0x17000A22 RID: 2594
	// (get) Token: 0x060012E9 RID: 4841 RVA: 0x00038060 File Offset: 0x00036260
	// (set) Token: 0x060012EA RID: 4842 RVA: 0x00038068 File Offset: 0x00036268
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

	// Token: 0x17000A23 RID: 2595
	// (get) Token: 0x060012EB RID: 4843 RVA: 0x00038071 File Offset: 0x00036271
	// (set) Token: 0x060012EC RID: 4844 RVA: 0x00038079 File Offset: 0x00036279
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

	// Token: 0x17000A24 RID: 2596
	// (get) Token: 0x060012ED RID: 4845 RVA: 0x00038082 File Offset: 0x00036282
	// (set) Token: 0x060012EE RID: 4846 RVA: 0x0003808A File Offset: 0x0003628A
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

	// Token: 0x17000A25 RID: 2597
	// (get) Token: 0x060012EF RID: 4847 RVA: 0x00038093 File Offset: 0x00036293
	public IDamageObj DamageObj
	{
		get
		{
			return this.m_damageObj;
		}
	}

	// Token: 0x17000A26 RID: 2598
	// (get) Token: 0x060012F0 RID: 4848 RVA: 0x0003809B File Offset: 0x0003629B
	// (set) Token: 0x060012F1 RID: 4849 RVA: 0x000380A4 File Offset: 0x000362A4
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

	// Token: 0x17000A27 RID: 2599
	// (get) Token: 0x060012F2 RID: 4850 RVA: 0x00038105 File Offset: 0x00036305
	// (set) Token: 0x060012F3 RID: 4851 RVA: 0x0003810D File Offset: 0x0003630D
	public bool IsInitialized { get; private set; }

	// Token: 0x17000A28 RID: 2600
	// (get) Token: 0x060012F4 RID: 4852 RVA: 0x00038116 File Offset: 0x00036316
	// (set) Token: 0x060012F5 RID: 4853 RVA: 0x0003811E File Offset: 0x0003631E
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

	// Token: 0x17000A29 RID: 2601
	// (get) Token: 0x060012F6 RID: 4854 RVA: 0x00038127 File Offset: 0x00036327
	// (set) Token: 0x060012F7 RID: 4855 RVA: 0x0003812F File Offset: 0x0003632F
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

	// Token: 0x17000A2A RID: 2602
	// (get) Token: 0x060012F8 RID: 4856 RVA: 0x00038138 File Offset: 0x00036338
	// (set) Token: 0x060012F9 RID: 4857 RVA: 0x00038140 File Offset: 0x00036340
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

	// Token: 0x17000A2B RID: 2603
	// (get) Token: 0x060012FA RID: 4858 RVA: 0x00038149 File Offset: 0x00036349
	// (set) Token: 0x060012FB RID: 4859 RVA: 0x00038151 File Offset: 0x00036351
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

	// Token: 0x17000A2C RID: 2604
	// (get) Token: 0x060012FC RID: 4860 RVA: 0x0003815A File Offset: 0x0003635A
	public Collider2D[] BodyColliderList
	{
		get
		{
			return this.m_bodyColliderList;
		}
	}

	// Token: 0x17000A2D RID: 2605
	// (get) Token: 0x060012FD RID: 4861 RVA: 0x00038162 File Offset: 0x00036362
	public Collider2D[] WeaponColliderList
	{
		get
		{
			return this.m_weaponColliderList;
		}
	}

	// Token: 0x17000A2E RID: 2606
	// (get) Token: 0x060012FE RID: 4862 RVA: 0x0003816A File Offset: 0x0003636A
	public Collider2D[] TerrainColliderList
	{
		get
		{
			return this.m_terrainColliderList;
		}
	}

	// Token: 0x17000A2F RID: 2607
	// (get) Token: 0x060012FF RID: 4863 RVA: 0x00038172 File Offset: 0x00036372
	public IBodyOnEnterHitResponse[] BodyOnEnterHitResponseArray
	{
		get
		{
			return this.m_bodyOnEnterResponseArray;
		}
	}

	// Token: 0x17000A30 RID: 2608
	// (get) Token: 0x06001300 RID: 4864 RVA: 0x0003817A File Offset: 0x0003637A
	public IWeaponOnEnterHitResponse[] WeaponOnEnterHitResponseArray
	{
		get
		{
			return this.m_weaponOnEnterResponseArray;
		}
	}

	// Token: 0x17000A31 RID: 2609
	// (get) Token: 0x06001301 RID: 4865 RVA: 0x00038182 File Offset: 0x00036382
	public ITerrainOnEnterHitResponse[] TerrainOnEnterHitResponseArray
	{
		get
		{
			return this.m_terrainOnEnterResponseArray;
		}
	}

	// Token: 0x17000A32 RID: 2610
	// (get) Token: 0x06001302 RID: 4866 RVA: 0x0003818A File Offset: 0x0003638A
	public IBodyOnStayHitResponse[] BodyOnStayHitResponseArray
	{
		get
		{
			return this.m_bodyOnStayResponseArray;
		}
	}

	// Token: 0x17000A33 RID: 2611
	// (get) Token: 0x06001303 RID: 4867 RVA: 0x00038192 File Offset: 0x00036392
	public IWeaponOnStayHitResponse[] WeaponOnStayHitResponseArray
	{
		get
		{
			return this.m_weaponOnStayResponseArray;
		}
	}

	// Token: 0x17000A34 RID: 2612
	// (get) Token: 0x06001304 RID: 4868 RVA: 0x0003819A File Offset: 0x0003639A
	public ITerrainOnStayHitResponse[] TerrainOnStayHitResponseArray
	{
		get
		{
			return this.m_terrainOnStayResponseArray;
		}
	}

	// Token: 0x17000A35 RID: 2613
	// (get) Token: 0x06001305 RID: 4869 RVA: 0x000381A2 File Offset: 0x000363A2
	public IBodyOnExitHitResponse[] BodyOnExitHitResponseList
	{
		get
		{
			return this.m_bodyOnExitResponseArray;
		}
	}

	// Token: 0x17000A36 RID: 2614
	// (get) Token: 0x06001306 RID: 4870 RVA: 0x000381AA File Offset: 0x000363AA
	public IWeaponOnExitHitResponse[] WeaponOnExitHitResponseArray
	{
		get
		{
			return this.m_weaponOnExitResponseArray;
		}
	}

	// Token: 0x17000A37 RID: 2615
	// (get) Token: 0x06001307 RID: 4871 RVA: 0x000381B2 File Offset: 0x000363B2
	public ITerrainOnExitHitResponse[] TerrainOnExitHitResponseArray
	{
		get
		{
			return this.m_terrainOnExitResponseArray;
		}
	}

	// Token: 0x06001308 RID: 4872 RVA: 0x000381BC File Offset: 0x000363BC
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

	// Token: 0x06001309 RID: 4873 RVA: 0x00038234 File Offset: 0x00036434
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

	// Token: 0x0600130A RID: 4874 RVA: 0x000382C4 File Offset: 0x000364C4
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

	// Token: 0x0600130B RID: 4875 RVA: 0x0003832B File Offset: 0x0003652B
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

	// Token: 0x0600130C RID: 4876 RVA: 0x0003835C File Offset: 0x0003655C
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

	// Token: 0x0600130D RID: 4877 RVA: 0x000383F4 File Offset: 0x000365F4
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

	// Token: 0x0600130E RID: 4878 RVA: 0x0003846F File Offset: 0x0003666F
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

	// Token: 0x0600130F RID: 4879 RVA: 0x000384AB File Offset: 0x000366AB
	private void OnDisable()
	{
		this.ResetRepeatHitChecks();
	}

	// Token: 0x06001310 RID: 4880 RVA: 0x000384B3 File Offset: 0x000366B3
	private void Awake()
	{
		if (this.m_initializeOnAwake && !this.IsInitialized)
		{
			this.Initialize();
		}
	}

	// Token: 0x06001311 RID: 4881 RVA: 0x000384CB File Offset: 0x000366CB
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

	// Token: 0x06001312 RID: 4882 RVA: 0x000384DC File Offset: 0x000366DC
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

	// Token: 0x06001313 RID: 4883 RVA: 0x00038540 File Offset: 0x00036740
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

	// Token: 0x06001314 RID: 4884 RVA: 0x000387C8 File Offset: 0x000369C8
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

	// Token: 0x06001315 RID: 4885 RVA: 0x0003882C File Offset: 0x00036A2C
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

	// Token: 0x06001316 RID: 4886 RVA: 0x000388E0 File Offset: 0x00036AE0
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

	// Token: 0x06001317 RID: 4887 RVA: 0x00038968 File Offset: 0x00036B68
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

	// Token: 0x06001318 RID: 4888 RVA: 0x00038A20 File Offset: 0x00036C20
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

	// Token: 0x06001319 RID: 4889 RVA: 0x00038A4C File Offset: 0x00036C4C
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

	// Token: 0x0600131A RID: 4890 RVA: 0x00038B00 File Offset: 0x00036D00
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

	// Token: 0x0600131B RID: 4891 RVA: 0x00038CA8 File Offset: 0x00036EA8
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

	// Token: 0x0600131C RID: 4892 RVA: 0x00038DCC File Offset: 0x00036FCC
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

	// Token: 0x0600131D RID: 4893 RVA: 0x00038FC8 File Offset: 0x000371C8
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

	// Token: 0x0600131E RID: 4894 RVA: 0x000394AC File Offset: 0x000376AC
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

	// Token: 0x0600131F RID: 4895 RVA: 0x00039600 File Offset: 0x00037800
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

	// Token: 0x06001320 RID: 4896 RVA: 0x00039728 File Offset: 0x00037928
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

	// Token: 0x06001321 RID: 4897 RVA: 0x000397BC File Offset: 0x000379BC
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

	// Token: 0x06001322 RID: 4898 RVA: 0x00039850 File Offset: 0x00037A50
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

	// Token: 0x06001323 RID: 4899 RVA: 0x000398E4 File Offset: 0x00037AE4
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

	// Token: 0x06001324 RID: 4900 RVA: 0x00039A10 File Offset: 0x00037C10
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

	// Token: 0x06001325 RID: 4901 RVA: 0x00039A70 File Offset: 0x00037C70
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

	// Token: 0x06001326 RID: 4902 RVA: 0x00039B09 File Offset: 0x00037D09
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

	// Token: 0x06001327 RID: 4903 RVA: 0x00039B40 File Offset: 0x00037D40
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

	// Token: 0x06001328 RID: 4904 RVA: 0x00039C22 File Offset: 0x00037E22
	private void InitializeRepeatHitCheck()
	{
		if (this.m_repeatHitArraySizeOverride == -1)
		{
			this.m_repeatHitArraySizeOverride = 4;
		}
		this.m_repeatHitCheckList = new List<HitboxController.RepeatHitEntry>(this.m_repeatHitArraySizeOverride);
	}

	// Token: 0x06001329 RID: 4905 RVA: 0x00039C48 File Offset: 0x00037E48
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

	// Token: 0x0600132A RID: 4906 RVA: 0x00039D20 File Offset: 0x00037F20
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

	// Token: 0x0600132B RID: 4907 RVA: 0x00039D94 File Offset: 0x00037F94
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

	// Token: 0x0600132E RID: 4910 RVA: 0x00039E73 File Offset: 0x00038073
	GameObject IHitboxController.get_gameObject()
	{
		return base.gameObject;
	}

	// Token: 0x04001323 RID: 4899
	public const int REPEAT_HIT_ARRAY_DEFAULT_SIZE = 4;

	// Token: 0x04001324 RID: 4900
	public const string PLATFORM_TITLE = " (Active - Platform)";

	// Token: 0x04001325 RID: 4901
	public const string BODY_TITLE = " (Active - Body)";

	// Token: 0x04001326 RID: 4902
	public const string WEAPON_TITLE = " (Active - Weapon)";

	// Token: 0x04001327 RID: 4903
	public const string TERRAIN_TITLE = " (Active - Terrain)";

	// Token: 0x04001328 RID: 4904
	[Space(5f)]
	[SerializeField]
	private CollisionType m_collisionType;

	// Token: 0x04001329 RID: 4905
	private float m_repeatHitDuration = 0.5f;

	// Token: 0x0400132A RID: 4906
	[Space(5f)]
	[Header("Body Hitboxes")]
	[SerializeField]
	private GameObject[] m_bodyHitboxList = new GameObject[0];

	// Token: 0x0400132B RID: 4907
	[Space(5f)]
	[Header("Weapon Hitboxes")]
	[SerializeField]
	private GameObject[] m_weaponHitboxList = new GameObject[0];

	// Token: 0x0400132C RID: 4908
	[Space(5f)]
	[Header("Terrain Hitboxes")]
	[SerializeField]
	private GameObject[] m_terrainHitboxList = new GameObject[0];

	// Token: 0x0400132D RID: 4909
	[Space(5f)]
	[Header("Platform Hitbox")]
	[SerializeField]
	private GameObject m_platformHitbox;

	// Token: 0x0400132E RID: 4910
	[SerializeField]
	private PlatformCollisionType m_platformCollisionType;

	// Token: 0x0400132F RID: 4911
	[SerializeField]
	[EnumFlag]
	private CollisionType m_weaponCollisionType;

	// Token: 0x04001330 RID: 4912
	[SerializeField]
	[EnumFlag]
	private CollisionType m_terrainCollisionType;

	// Token: 0x04001331 RID: 4913
	[SerializeField]
	[HideInInspector]
	private bool m_hitboxesInitialized;

	// Token: 0x04001332 RID: 4914
	[SerializeField]
	private bool m_initializeOnAwake = true;

	// Token: 0x04001333 RID: 4915
	[SerializeField]
	private int m_repeatHitArraySizeOverride = -1;

	// Token: 0x04001334 RID: 4916
	private Collider2D[] m_terrainColliderList;

	// Token: 0x04001335 RID: 4917
	private Collider2D[] m_bodyColliderList;

	// Token: 0x04001336 RID: 4918
	private Collider2D[] m_weaponColliderList;

	// Token: 0x04001337 RID: 4919
	private Collider2D m_platformCollider;

	// Token: 0x04001338 RID: 4920
	private Rigidbody2D m_rigidBody;

	// Token: 0x04001339 RID: 4921
	private ITerrainOnEnterHitResponse[] m_terrainOnEnterResponseArray;

	// Token: 0x0400133A RID: 4922
	private ITerrainOnStayHitResponse[] m_terrainOnStayResponseArray;

	// Token: 0x0400133B RID: 4923
	private ITerrainOnExitHitResponse[] m_terrainOnExitResponseArray;

	// Token: 0x0400133C RID: 4924
	private IBodyOnEnterHitResponse[] m_bodyOnEnterResponseArray;

	// Token: 0x0400133D RID: 4925
	private IBodyOnStayHitResponse[] m_bodyOnStayResponseArray;

	// Token: 0x0400133E RID: 4926
	private IBodyOnExitHitResponse[] m_bodyOnExitResponseArray;

	// Token: 0x0400133F RID: 4927
	private IWeaponOnEnterHitResponse[] m_weaponOnEnterResponseArray;

	// Token: 0x04001340 RID: 4928
	private IWeaponOnStayHitResponse[] m_weaponOnStayResponseArray;

	// Token: 0x04001341 RID: 4929
	private IWeaponOnExitHitResponse[] m_weaponOnExitResponseArray;

	// Token: 0x04001342 RID: 4930
	private IDamageObj m_damageObj;

	// Token: 0x04001343 RID: 4931
	private int m_responseMethodCount;

	// Token: 0x04001344 RID: 4932
	private bool m_responseMethodsInitialized;

	// Token: 0x04001345 RID: 4933
	private bool m_startExecuted;

	// Token: 0x04001346 RID: 4934
	private static List<string> m_warningIssuedTracker = new List<string>();

	// Token: 0x04001349 RID: 4937
	private bool m_disableAllCollisions;

	// Token: 0x0400134B RID: 4939
	private static List<GameObject> m_gameObjDisableList_STATIC = new List<GameObject>();

	// Token: 0x0400134C RID: 4940
	private static List<Collider2D> m_colliderListHelper = new List<Collider2D>();

	// Token: 0x0400134D RID: 4941
	private static GameObject[] m_platformArrayHelper = new GameObject[1];

	// Token: 0x0400134E RID: 4942
	private int m_cachedWeaponHitboxStates;

	// Token: 0x0400134F RID: 4943
	private int m_cachedBodyHitboxStates;

	// Token: 0x04001350 RID: 4944
	private int m_cachedTerrainHitboxStates;

	// Token: 0x04001351 RID: 4945
	private bool m_weaponHitboxStatesDisabled;

	// Token: 0x04001352 RID: 4946
	private bool m_bodyHitboxStatesDisabled;

	// Token: 0x04001353 RID: 4947
	private bool m_terrainHitboxStatesDisabled;

	// Token: 0x04001354 RID: 4948
	private bool m_culled;

	// Token: 0x04001355 RID: 4949
	private bool m_bodyCullState;

	// Token: 0x04001356 RID: 4950
	private bool m_weaponCullState;

	// Token: 0x04001357 RID: 4951
	private bool m_terrainCullState;

	// Token: 0x04001358 RID: 4952
	private bool m_rigidBodyCullState;

	// Token: 0x04001359 RID: 4953
	protected List<HitboxController.RepeatHitEntry> m_repeatHitCheckList;

	// Token: 0x02000AF7 RID: 2807
	protected struct RepeatHitEntry
	{
		// Token: 0x04004AD5 RID: 19157
		public int ObjHashCode;

		// Token: 0x04004AD6 RID: 19158
		public float HitDuration;

		// Token: 0x04004AD7 RID: 19159
		public LayerType Layer;
	}
}
