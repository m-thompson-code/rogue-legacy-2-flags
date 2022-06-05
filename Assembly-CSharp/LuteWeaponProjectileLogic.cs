using System;
using System.Collections;
using System.Collections.Generic;
using FMOD.Studio;
using FMODUnity;
using RLAudio;
using UnityEngine;

// Token: 0x020007AA RID: 1962
public class LuteWeaponProjectileLogic : BaseProjectileLogic, IBodyOnEnterHitResponse, IHitResponse, IBodyOnStayHitResponse, IHasProjectileNameArray
{
	// Token: 0x170015EA RID: 5610
	// (get) Token: 0x06003BB9 RID: 15289 RVA: 0x00020D57 File Offset: 0x0001EF57
	public virtual string[] ProjectileNameArray
	{
		get
		{
			if (this.m_projectileNameArray == null)
			{
				this.m_projectileNameArray = new string[]
				{
					this.m_projectileToSpawn
				};
			}
			return this.m_projectileNameArray;
		}
	}

	// Token: 0x06003BBA RID: 15290 RVA: 0x00020D7C File Offset: 0x0001EF7C
	protected override void Awake()
	{
		base.Awake();
		if (!LuteWeaponProjectileLogic.m_bounceEventInstance.isValid())
		{
			LuteWeaponProjectileLogic.m_bounceEventInstance = AudioUtility.GetEventInstance(this.m_bounceAudioPath, base.transform);
		}
	}

	// Token: 0x06003BBB RID: 15291 RVA: 0x00020DA6 File Offset: 0x0001EFA6
	private void OnEnable()
	{
		base.StartCoroutine(this.EnableBodyHitboxCoroutine());
		base.StartCoroutine(this.MaxProjectileCheckCoroutine());
	}

	// Token: 0x06003BBC RID: 15292 RVA: 0x00020DC2 File Offset: 0x0001EFC2
	private IEnumerator MaxProjectileCheckCoroutine()
	{
		yield return null;
		if (base.SourceProjectile.CastAbilityType == CastAbilityType.Weapon)
		{
			LuteWeaponProjectileLogic.m_maxProjectileList_STATIC.Add(base.SourceProjectile);
			if (LuteWeaponProjectileLogic.m_maxProjectileList_STATIC.Count > 3)
			{
				if (LuteWeaponProjectileLogic.m_maxProjectileList_STATIC[0])
				{
					LuteWeaponProjectileLogic.m_maxProjectileList_STATIC[0].FlagForDestruction(null);
				}
				LuteWeaponProjectileLogic.m_maxProjectileList_STATIC.RemoveAt(0);
			}
		}
		yield break;
	}

	// Token: 0x06003BBD RID: 15293 RVA: 0x000F49CC File Offset: 0x000F2BCC
	private void OnDisable()
	{
		if (base.SourceProjectile.CastAbilityType == CastAbilityType.Weapon)
		{
			int num = LuteWeaponProjectileLogic.m_maxProjectileList_STATIC.IndexOf(base.SourceProjectile);
			if (num != -1)
			{
				LuteWeaponProjectileLogic.m_maxProjectileList_STATIC.RemoveAt(num);
			}
		}
	}

	// Token: 0x06003BBE RID: 15294 RVA: 0x00020DD1 File Offset: 0x0001EFD1
	private void OnDestroy()
	{
		if (LuteWeaponProjectileLogic.m_maxProjectileList_STATIC.Count > 0)
		{
			LuteWeaponProjectileLogic.m_maxProjectileList_STATIC.Clear();
		}
		if (LuteWeaponProjectileLogic.m_bounceEventInstance.isValid())
		{
			LuteWeaponProjectileLogic.m_bounceEventInstance.release();
		}
	}

	// Token: 0x06003BBF RID: 15295 RVA: 0x00020E01 File Offset: 0x0001F001
	private IEnumerator EnableBodyHitboxCoroutine()
	{
		base.SourceProjectile.HitboxController.SetHitboxActiveState(HitboxType.Body, false);
		float delayTime = Time.time + this.m_collisionDelayTime;
		while (Time.time < delayTime)
		{
			yield return null;
		}
		base.SourceProjectile.HitboxController.SetHitboxActiveState(HitboxType.Body, true);
		yield break;
	}

	// Token: 0x06003BC0 RID: 15296 RVA: 0x00020E10 File Offset: 0x0001F010
	public void BodyOnEnterHitResponse(IHitboxController otherHBController)
	{
		if (!base.enabled)
		{
			return;
		}
		if (otherHBController.CollisionType == CollisionType.PlayerProjectile && otherHBController.RootGameObject.GetComponent<DownstrikeProjectile_RL>())
		{
			LuteWeaponProjectileLogic.m_luteLogicHashset_STATIC.Add(this);
		}
	}

	// Token: 0x06003BC1 RID: 15297 RVA: 0x00020E42 File Offset: 0x0001F042
	public void BodyOnStayHitResponse(IHitboxController otherHBController)
	{
		this.BodyOnEnterHitResponse(otherHBController);
	}

	// Token: 0x06003BC2 RID: 15298 RVA: 0x000F4A08 File Offset: 0x000F2C08
	private void LateUpdate()
	{
		if (PlayerManager.IsInstantiated && PlayerManager.GetPlayerController().IsGrounded)
		{
			LuteWeaponProjectileLogic.m_bounceCount = 0;
		}
		if (LuteWeaponProjectileLogic.m_luteLogicHashset_STATIC.Count > 0)
		{
			LuteWeaponProjectileLogic luteWeaponProjectileLogic = null;
			PlayerController playerController = PlayerManager.GetPlayerController();
			float num = float.MaxValue;
			foreach (LuteWeaponProjectileLogic luteWeaponProjectileLogic2 in LuteWeaponProjectileLogic.m_luteLogicHashset_STATIC)
			{
				float num2 = CDGHelper.DistanceBetweenPts(playerController.Midpoint, luteWeaponProjectileLogic2.SourceProjectile.Midpoint);
				if (num2 < num)
				{
					num = num2;
					luteWeaponProjectileLogic = luteWeaponProjectileLogic2;
				}
			}
			if (luteWeaponProjectileLogic)
			{
				Projectile_RL sourceProjectile = luteWeaponProjectileLogic.SourceProjectile;
				if (luteWeaponProjectileLogic.m_destroyOnKick)
				{
					luteWeaponProjectileLogic.SourceProjectile.FlagForDestruction(playerController.gameObject);
				}
				if (luteWeaponProjectileLogic.m_explodeOnKick)
				{
					Projectile_RL projectile_RL = ProjectileManager.FireProjectile(sourceProjectile.Owner, luteWeaponProjectileLogic.m_projectileToSpawn, sourceProjectile.Midpoint, false, 0f, 1f, true, true, true, true);
					playerController.CastAbility.InitializeProjectile(projectile_RL, sourceProjectile.CastAbilityType);
					projectile_RL.Strength = sourceProjectile.Strength;
					projectile_RL.Magic = sourceProjectile.Magic;
					projectile_RL.DamageMod = sourceProjectile.DamageMod;
					projectile_RL.ActualCritChance = sourceProjectile.ActualCritChance;
					projectile_RL.RelicDamageTypeString = base.SourceProjectile.RelicDamageTypeString;
					if (base.SourceProjectile.StatusEffectTypes != null)
					{
						foreach (StatusEffectType type in base.SourceProjectile.StatusEffectTypes)
						{
							projectile_RL.AttachStatusEffect(type, 0f);
						}
					}
					LuteWeaponProjectileLogic.m_bounceCount++;
					if (LuteWeaponProjectileLogic.m_bounceEventInstance.isValid())
					{
						float playerGenderAudioParameterValue = AudioUtility.GetPlayerGenderAudioParameterValue();
						LuteWeaponProjectileLogic.m_bounceEventInstance.setParameterByName("gender", playerGenderAudioParameterValue, false);
						LuteWeaponProjectileLogic.m_bounceCount = Mathf.Clamp(LuteWeaponProjectileLogic.m_bounceCount, 1, 8);
						LuteWeaponProjectileLogic.m_bounceEventInstance.setParameterByName("bardNoteBounce", (float)LuteWeaponProjectileLogic.m_bounceCount, false);
						AudioManager.PlayAttached(null, LuteWeaponProjectileLogic.m_bounceEventInstance, PlayerManager.GetPlayer());
					}
					projectile_RL.OnCollisionRelay.AddListener(new Action<Projectile_RL, GameObject>(LuteWeaponProjectileLogic.OnExplosionCollision), false);
				}
			}
			LuteWeaponProjectileLogic.m_luteLogicHashset_STATIC.Clear();
		}
	}

	// Token: 0x06003BC3 RID: 15299 RVA: 0x00020E4B File Offset: 0x0001F04B
	private static void OnExplosionCollision(Projectile_RL projectile, GameObject colliderObj)
	{
		if (PlayerManager.GetPlayerController().CharacterClass.ClassType == ClassType.LuteClass && colliderObj.CompareTag("Enemy"))
		{
			PlayerManager.GetPlayerController().StatusEffectController.StartStatusEffect(StatusEffectType.Player_Dance, 0f, projectile);
		}
	}

	// Token: 0x04002F70 RID: 12144
	[SerializeField]
	[EventRef]
	private string m_bounceAudioPath = string.Empty;

	// Token: 0x04002F71 RID: 12145
	private static HashSet<LuteWeaponProjectileLogic> m_luteLogicHashset_STATIC = new HashSet<LuteWeaponProjectileLogic>();

	// Token: 0x04002F72 RID: 12146
	private static List<Projectile_RL> m_maxProjectileList_STATIC = new List<Projectile_RL>();

	// Token: 0x04002F73 RID: 12147
	[SerializeField]
	private string m_projectileToSpawn;

	// Token: 0x04002F74 RID: 12148
	[SerializeField]
	private bool m_destroyOnKick;

	// Token: 0x04002F75 RID: 12149
	[SerializeField]
	private bool m_explodeOnKick;

	// Token: 0x04002F76 RID: 12150
	[SerializeField]
	private float m_collisionDelayTime;

	// Token: 0x04002F77 RID: 12151
	[NonSerialized]
	protected string[] m_projectileNameArray;

	// Token: 0x04002F78 RID: 12152
	private static EventInstance m_bounceEventInstance = default(EventInstance);

	// Token: 0x04002F79 RID: 12153
	private static int m_bounceCount = 0;
}
