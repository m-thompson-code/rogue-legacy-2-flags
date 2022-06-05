using System;
using System.Collections;
using System.Collections.Generic;
using FMOD.Studio;
using FMODUnity;
using RLAudio;
using UnityEngine;

// Token: 0x020004A1 RID: 1185
public class LuteWeaponProjectileLogic : BaseProjectileLogic, IBodyOnEnterHitResponse, IHitResponse, IBodyOnStayHitResponse, IHasProjectileNameArray
{
	// Token: 0x1700109B RID: 4251
	// (get) Token: 0x06002B58 RID: 11096 RVA: 0x00092FE3 File Offset: 0x000911E3
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

	// Token: 0x06002B59 RID: 11097 RVA: 0x00093008 File Offset: 0x00091208
	protected override void Awake()
	{
		base.Awake();
		if (!LuteWeaponProjectileLogic.m_bounceEventInstance.isValid())
		{
			LuteWeaponProjectileLogic.m_bounceEventInstance = AudioUtility.GetEventInstance(this.m_bounceAudioPath, base.transform);
		}
	}

	// Token: 0x06002B5A RID: 11098 RVA: 0x00093032 File Offset: 0x00091232
	private void OnEnable()
	{
		base.StartCoroutine(this.EnableBodyHitboxCoroutine());
		base.StartCoroutine(this.MaxProjectileCheckCoroutine());
	}

	// Token: 0x06002B5B RID: 11099 RVA: 0x0009304E File Offset: 0x0009124E
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

	// Token: 0x06002B5C RID: 11100 RVA: 0x00093060 File Offset: 0x00091260
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

	// Token: 0x06002B5D RID: 11101 RVA: 0x0009309A File Offset: 0x0009129A
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

	// Token: 0x06002B5E RID: 11102 RVA: 0x000930CA File Offset: 0x000912CA
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

	// Token: 0x06002B5F RID: 11103 RVA: 0x000930D9 File Offset: 0x000912D9
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

	// Token: 0x06002B60 RID: 11104 RVA: 0x0009310B File Offset: 0x0009130B
	public void BodyOnStayHitResponse(IHitboxController otherHBController)
	{
		this.BodyOnEnterHitResponse(otherHBController);
	}

	// Token: 0x06002B61 RID: 11105 RVA: 0x00093114 File Offset: 0x00091314
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

	// Token: 0x06002B62 RID: 11106 RVA: 0x0009335C File Offset: 0x0009155C
	private static void OnExplosionCollision(Projectile_RL projectile, GameObject colliderObj)
	{
		if (PlayerManager.GetPlayerController().CharacterClass.ClassType == ClassType.LuteClass && colliderObj.CompareTag("Enemy"))
		{
			PlayerManager.GetPlayerController().StatusEffectController.StartStatusEffect(StatusEffectType.Player_Dance, 0f, projectile);
		}
	}

	// Token: 0x04002345 RID: 9029
	[SerializeField]
	[EventRef]
	private string m_bounceAudioPath = string.Empty;

	// Token: 0x04002346 RID: 9030
	private static HashSet<LuteWeaponProjectileLogic> m_luteLogicHashset_STATIC = new HashSet<LuteWeaponProjectileLogic>();

	// Token: 0x04002347 RID: 9031
	private static List<Projectile_RL> m_maxProjectileList_STATIC = new List<Projectile_RL>();

	// Token: 0x04002348 RID: 9032
	[SerializeField]
	private string m_projectileToSpawn;

	// Token: 0x04002349 RID: 9033
	[SerializeField]
	private bool m_destroyOnKick;

	// Token: 0x0400234A RID: 9034
	[SerializeField]
	private bool m_explodeOnKick;

	// Token: 0x0400234B RID: 9035
	[SerializeField]
	private float m_collisionDelayTime;

	// Token: 0x0400234C RID: 9036
	[NonSerialized]
	protected string[] m_projectileNameArray;

	// Token: 0x0400234D RID: 9037
	private static EventInstance m_bounceEventInstance = default(EventInstance);

	// Token: 0x0400234E RID: 9038
	private static int m_bounceCount = 0;
}
