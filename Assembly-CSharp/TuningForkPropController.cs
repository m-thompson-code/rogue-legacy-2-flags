using System;
using System.Collections;
using FMODUnity;
using RLAudio;
using UnityEngine;

// Token: 0x020004E6 RID: 1254
public class TuningForkPropController : BaseSpecialPropController, ITerrainOnEnterHitResponse, IHitResponse
{
	// Token: 0x06002EF1 RID: 12017 RVA: 0x000A00C6 File Offset: 0x0009E2C6
	protected override void Awake()
	{
		base.Awake();
		this.m_animator = base.GetComponent<Animator>();
		this.m_hbController = base.GetComponentInChildren<IHitboxController>();
		this.m_hbController.DisableAllCollisions = false;
	}

	// Token: 0x06002EF2 RID: 12018 RVA: 0x000A00F4 File Offset: 0x0009E2F4
	protected override void InitializePooledPropOnEnter()
	{
		if (!this.m_projectileAdded)
		{
			ProjectileManager.Instance.AddProjectileToPool(this.m_projectileName);
			this.m_projectileAdded = true;
		}
		this.m_hbController.DisableAllCollisions = false;
		if (SaveManager.PlayerSaveData.GetFlag(PlayerSaveFlag.CaveTuningForkTriggered))
		{
			this.m_hbController.SetHitboxActiveState(HitboxType.Terrain, false);
			return;
		}
		this.m_hbController.SetHitboxActiveState(HitboxType.Terrain, true);
	}

	// Token: 0x06002EF3 RID: 12019 RVA: 0x000A0158 File Offset: 0x0009E358
	public void TuningForkHit()
	{
		SaveManager.PlayerSaveData.SetFlag(PlayerSaveFlag.CaveTuningForkTriggered, true);
		MapController.SetCaveWhitePipVisibility(true);
		this.m_animator.SetTrigger("Vibrate");
		this.m_animator.SetTrigger("StopVibrateEase");
		this.m_hbController.SetHitboxActiveState(HitboxType.Terrain, false);
		AudioManager.Play(null, this.m_onHitEventEmitter);
		if (!this.m_textPopupPlayed)
		{
			this.m_textPopupPlayed = true;
			base.StartCoroutine(this.MapUpdatedTextPopupCoroutine());
		}
		if (!string.IsNullOrEmpty(this.m_projectileName))
		{
			ProjectileManager.FireProjectile(base.gameObject, this.m_projectileName, this.m_spawnPos.transform.position, false, 0f, 1f, true, true, true, true);
		}
	}

	// Token: 0x06002EF4 RID: 12020 RVA: 0x000A0213 File Offset: 0x0009E413
	private IEnumerator MapUpdatedTextPopupCoroutine()
	{
		Vector2 position = base.transform.position;
		position.y += 5f;
		TextPopupObj popup = TextPopupManager.DisplayLocIDText(TextPopupType.OutOfAmmo, "LOC_ID_STATUS_EFFECT_CAVE_MAP_UPDATED_1", StringGenderType.Male, position, 0f);
		popup.StopAllCoroutines();
		this.m_textPopup = popup;
		float speed = 1.05f;
		yield return TweenManager.TweenBy(popup.transform, speed, new EaseDelegate(Ease.Quad.EaseOut), new object[]
		{
			"localPosition.y",
			2
		}).TweenCoroutine;
		TweenManager.TweenBy(popup.transform, speed, new EaseDelegate(Ease.Quad.EaseIn), new object[]
		{
			"localPosition.y",
			-1
		});
		yield return TweenManager.TweenTo(popup.TMPText, speed / 2f, new EaseDelegate(Ease.None), new object[]
		{
			"delay",
			speed / 2f,
			"alpha",
			0
		}).TweenCoroutine;
		popup.gameObject.SetActive(false);
		this.m_textPopup = null;
		yield break;
	}

	// Token: 0x06002EF5 RID: 12021 RVA: 0x000A0222 File Offset: 0x0009E422
	private IEnumerator DisableHitbox(float secs)
	{
		this.m_hbController.DisableAllCollisions = true;
		float delay = Time.time + secs;
		while (Time.time < delay)
		{
			yield return null;
		}
		this.m_hbController.DisableAllCollisions = false;
		yield break;
	}

	// Token: 0x06002EF6 RID: 12022 RVA: 0x000A0238 File Offset: 0x0009E438
	public void TerrainOnEnterHitResponse(IHitboxController otherHBController)
	{
		if (!SaveManager.PlayerSaveData.GetFlag(PlayerSaveFlag.CaveTuningForkTriggered))
		{
			this.TuningForkHit();
		}
		this.m_hbController.SetHitboxActiveState(HitboxType.Terrain, false);
	}

	// Token: 0x06002EF7 RID: 12023 RVA: 0x000A025E File Offset: 0x0009E45E
	protected override void OnDisable()
	{
		base.OnDisable();
		if (this.m_textPopup)
		{
			this.m_textPopup.gameObject.SetActive(false);
			this.m_textPopup = null;
		}
	}

	// Token: 0x0400255E RID: 9566
	private const float HITBOX_DISABLE_DURATION = 2f;

	// Token: 0x0400255F RID: 9567
	[SerializeField]
	private string m_projectileName;

	// Token: 0x04002560 RID: 9568
	[SerializeField]
	private GameObject m_spawnPos;

	// Token: 0x04002561 RID: 9569
	[SerializeField]
	private StudioEventEmitter m_onHitEventEmitter;

	// Token: 0x04002562 RID: 9570
	private Animator m_animator;

	// Token: 0x04002563 RID: 9571
	private IHitboxController m_hbController;

	// Token: 0x04002564 RID: 9572
	private bool m_projectileAdded;

	// Token: 0x04002565 RID: 9573
	private bool m_textPopupPlayed;

	// Token: 0x04002566 RID: 9574
	private TextPopupObj m_textPopup;
}
