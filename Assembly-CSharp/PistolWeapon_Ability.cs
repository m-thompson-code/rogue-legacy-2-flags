using System;
using FMODUnity;
using RL_Windows;
using UnityEngine;

// Token: 0x02000308 RID: 776
public class PistolWeapon_Ability : AimedAbility_RL, IAttack, IAbility
{
	// Token: 0x17000BAB RID: 2987
	// (get) Token: 0x0600184E RID: 6222 RVA: 0x0000C3B4 File Offset: 0x0000A5B4
	public BaseAbility_RL ReloadAbility
	{
		get
		{
			return this.m_reloadAbility;
		}
	}

	// Token: 0x17000BAC RID: 2988
	// (get) Token: 0x0600184F RID: 6223 RVA: 0x0000C3BC File Offset: 0x0000A5BC
	public override Vector2 PushbackAmount
	{
		get
		{
			return this.BowPushbackAmount;
		}
	}

	// Token: 0x17000BAD RID: 2989
	// (get) Token: 0x06001850 RID: 6224 RVA: 0x0000C3C4 File Offset: 0x0000A5C4
	protected virtual Vector2 BowPushbackAmount
	{
		get
		{
			if (base.CurrentAmmo > 0)
			{
				return this.m_bowPushbackAmount;
			}
			return this.m_bowPushbackAmountNoAmmo;
		}
	}

	// Token: 0x17000BAE RID: 2990
	// (get) Token: 0x06001851 RID: 6225 RVA: 0x0000C3DC File Offset: 0x0000A5DC
	public override string ProjectileName
	{
		get
		{
			if (base.CurrentAmmo > 10)
			{
				return base.ProjectileName;
			}
			if (base.CurrentAmmo > 0)
			{
				return this.m_critHitProjectileName;
			}
			return this.m_emptyClipProjectileName;
		}
	}

	// Token: 0x17000BAF RID: 2991
	// (get) Token: 0x06001852 RID: 6226 RVA: 0x00003CD2 File Offset: 0x00001ED2
	protected override bool CancelTimeSlowOnFire
	{
		get
		{
			return false;
		}
	}

	// Token: 0x06001853 RID: 6227 RVA: 0x0000C405 File Offset: 0x0000A605
	protected override void InitializeProjectileNameArray()
	{
		this.m_projectileNameArray = new string[]
		{
			this.m_projectileName,
			this.m_critHitProjectileName,
			this.m_emptyClipProjectileName
		};
	}

	// Token: 0x17000BB0 RID: 2992
	// (get) Token: 0x06001854 RID: 6228 RVA: 0x00003DA1 File Offset: 0x00001FA1
	protected virtual bool CanManuallyReload
	{
		get
		{
			return true;
		}
	}

	// Token: 0x06001855 RID: 6229 RVA: 0x0008DC04 File Offset: 0x0008BE04
	public override void Initialize(CastAbility_RL abilityController, CastAbilityType castAbilityType)
	{
		base.Initialize(abilityController, castAbilityType);
		if (this.CanManuallyReload)
		{
			this.m_reloadAbility = AbilityLibrary.GetAbility(AbilityType.ReloadTalent);
			this.m_reloadAbility = UnityEngine.Object.Instantiate<BaseAbility_RL>(this.m_reloadAbility);
			this.m_reloadAbility.transform.SetParent(base.transform);
			this.m_reloadAbility.Initialize(abilityController, castAbilityType);
		}
	}

	// Token: 0x17000BB1 RID: 2993
	// (get) Token: 0x06001856 RID: 6230 RVA: 0x0000676B File Offset: 0x0000496B
	protected override float TellIntroAnimSpeed
	{
		get
		{
			return 3.25f;
		}
	}

	// Token: 0x17000BB2 RID: 2994
	// (get) Token: 0x06001857 RID: 6231 RVA: 0x00003CCB File Offset: 0x00001ECB
	protected override float TellIntroAnimExitDelay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x17000BB3 RID: 2995
	// (get) Token: 0x06001858 RID: 6232 RVA: 0x00003C62 File Offset: 0x00001E62
	protected override float TellAnimSpeed
	{
		get
		{
			return 5f;
		}
	}

	// Token: 0x17000BB4 RID: 2996
	// (get) Token: 0x06001859 RID: 6233 RVA: 0x00003CCB File Offset: 0x00001ECB
	protected override float TellAnimExitDelay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x17000BB5 RID: 2997
	// (get) Token: 0x0600185A RID: 6234 RVA: 0x00004536 File Offset: 0x00002736
	protected override float AttackIntroAnimSpeed
	{
		get
		{
			return 2f;
		}
	}

	// Token: 0x17000BB6 RID: 2998
	// (get) Token: 0x0600185B RID: 6235 RVA: 0x00003CCB File Offset: 0x00001ECB
	protected override float AttackIntroAnimExitDelay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x17000BB7 RID: 2999
	// (get) Token: 0x0600185C RID: 6236 RVA: 0x00003CE4 File Offset: 0x00001EE4
	protected override float AttackAnimSpeed
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x17000BB8 RID: 3000
	// (get) Token: 0x0600185D RID: 6237 RVA: 0x00003CCB File Offset: 0x00001ECB
	protected override float AttackAnimExitDelay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x17000BB9 RID: 3001
	// (get) Token: 0x0600185E RID: 6238 RVA: 0x00003C54 File Offset: 0x00001E54
	protected override float ExitAnimSpeed
	{
		get
		{
			return 3f;
		}
	}

	// Token: 0x17000BBA RID: 3002
	// (get) Token: 0x0600185F RID: 6239 RVA: 0x0000C42E File Offset: 0x0000A62E
	protected override float ExitAnimExitDelay
	{
		get
		{
			if (SaveManager.PlayerSaveData.GetRelic(RelicType.AttackCooldown).Level > 0)
			{
				return (float)SaveManager.PlayerSaveData.GetRelic(RelicType.AttackCooldown).Level * 0.125f;
			}
			return 0f;
		}
	}

	// Token: 0x06001860 RID: 6240 RVA: 0x0008DC68 File Offset: 0x0008BE68
	protected override void UpdateAimLine()
	{
		Vector3 localEulerAngles = this.m_aimSprite.transform.localEulerAngles;
		localEulerAngles.z = this.m_unmoddedAngle;
		this.m_aimSprite.transform.localEulerAngles = localEulerAngles;
	}

	// Token: 0x06001861 RID: 6241 RVA: 0x0008DCA4 File Offset: 0x0008BEA4
	protected override void Update()
	{
		if (!base.AbilityActive && Rewired_RL.Player.GetButton(this.m_abilityController.GetAbilityInputString(base.CastAbilityType)))
		{
			this.m_abilityController.StartAbility(base.CastAbilityType, false, false);
		}
		base.Update();
		if (this.CanManuallyReload && !Interactable.PlayerIsInteracting && !base.AbilityActive && Rewired_RL.Player.GetButtonDown("Interact") && this.m_reloadAbility && this.m_abilityController.IsAbilityPermitted(this.m_reloadAbility))
		{
			this.m_abilityController.StartAbility(CastAbilityType.Weapon, true, false);
		}
		if (this.m_aimSprite)
		{
			if (!this.m_aimSpriteInitialized)
			{
				this.m_aimSprite.transform.localScale = new Vector3(1f / this.m_abilityController.PlayerController.transform.lossyScale.x, 1f / this.m_abilityController.PlayerController.transform.lossyScale.y, 1f);
				float num = this.m_abilityController.PlayerController.transform.lossyScale.x / this.m_abilityController.PlayerController.BaseScaleToOffsetWith;
				Vector2 v = new Vector2(0f, this.m_projectileOffset.y * num);
				this.m_aimSprite.transform.position = this.m_abilityController.transform.localPosition + v;
				this.m_aimSpriteInitialized = true;
			}
			if ((this.m_abilityController.AnyAbilityInProgress && !this.m_abilityController.AbilityInProgress(CastAbilityType.Weapon)) || WindowManager.IsAnyWindowOpen || GameManager.IsGamePaused || (!GameManager.IsGamePaused && !RewiredMapController.IsCurrentMapEnabled))
			{
				if (this.m_aimSprite.gameObject.activeSelf)
				{
					this.m_aimSprite.gameObject.SetActive(false);
				}
			}
			else if (!this.m_aimSprite.gameObject.activeSelf)
			{
				this.m_aimSprite.gameObject.SetActive(true);
			}
			if (!base.AbilityActive)
			{
				this.UpdateArrowAim(true);
				this.UpdateAimLine();
			}
		}
	}

	// Token: 0x06001862 RID: 6242 RVA: 0x0000C468 File Offset: 0x0000A668
	protected void Base_FireProjectile()
	{
		base.FireProjectile();
	}

	// Token: 0x06001863 RID: 6243 RVA: 0x0008DEC8 File Offset: 0x0008C0C8
	protected override void FireProjectile()
	{
		if (base.CurrentAmmo > 0)
		{
			this.m_shootAudioEmitter.Play();
		}
		else
		{
			this.m_emptyClipAudioEmitter.Play();
		}
		if (base.CurrentAmmo <= 10)
		{
			this.m_shootAudioEmitter.SetParameter("ammoFull", 0.1f, false);
		}
		base.FireProjectile();
		if (base.CurrentAmmo <= 0)
		{
			this.m_firedProjectile.RemoveAllStatusEffects();
		}
		if (base.CurrentAmmo < 10)
		{
			this.m_firedProjectile.ActualCritChance += 100f;
		}
		int level = SaveManager.PlayerSaveData.GetRelic(RelicType.AttackCooldown).Level;
		if (level > 0)
		{
			this.m_firedProjectile.LifespanTimer += (float)level * 0f;
		}
	}

	// Token: 0x0400178E RID: 6030
	[SerializeField]
	private GameObject m_aimSprite;

	// Token: 0x0400178F RID: 6031
	[SerializeField]
	protected StudioEventEmitter m_shootAudioEmitter;

	// Token: 0x04001790 RID: 6032
	[SerializeField]
	protected StudioEventEmitter m_emptyClipAudioEmitter;

	// Token: 0x04001791 RID: 6033
	[SerializeField]
	protected string m_emptyClipProjectileName;

	// Token: 0x04001792 RID: 6034
	[SerializeField]
	private string m_critHitProjectileName;

	// Token: 0x04001793 RID: 6035
	private BaseAbility_RL m_reloadAbility;

	// Token: 0x04001794 RID: 6036
	private const float LOW_AMMO_AUDIO_PARAMETER_VALUE = 0.1f;

	// Token: 0x04001795 RID: 6037
	private bool m_aimSpriteInitialized;

	// Token: 0x04001796 RID: 6038
	private Vector2 m_bowPushbackAmount = new Vector2(0f, 3.25f);

	// Token: 0x04001797 RID: 6039
	private Vector2 m_bowPushbackAmountNoAmmo = new Vector2(0f, 2.4f);
}
