using System;
using System.Collections;
using FMODUnity;
using RLAudio;
using UnityEngine;

// Token: 0x02000831 RID: 2097
public class TuningForkPropController : BaseSpecialPropController, ITerrainOnEnterHitResponse, IHitResponse
{
	// Token: 0x060040BD RID: 16573 RVA: 0x00023C74 File Offset: 0x00021E74
	protected override void Awake()
	{
		base.Awake();
		this.m_animator = base.GetComponent<Animator>();
		this.m_hbController = base.GetComponentInChildren<IHitboxController>();
		this.m_hbController.DisableAllCollisions = false;
	}

	// Token: 0x060040BE RID: 16574 RVA: 0x00103D0C File Offset: 0x00101F0C
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

	// Token: 0x060040BF RID: 16575 RVA: 0x00103D70 File Offset: 0x00101F70
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

	// Token: 0x060040C0 RID: 16576 RVA: 0x00023CA0 File Offset: 0x00021EA0
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

	// Token: 0x060040C1 RID: 16577 RVA: 0x00023CAF File Offset: 0x00021EAF
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

	// Token: 0x060040C2 RID: 16578 RVA: 0x00023CC5 File Offset: 0x00021EC5
	public void TerrainOnEnterHitResponse(IHitboxController otherHBController)
	{
		if (!SaveManager.PlayerSaveData.GetFlag(PlayerSaveFlag.CaveTuningForkTriggered))
		{
			this.TuningForkHit();
		}
		this.m_hbController.SetHitboxActiveState(HitboxType.Terrain, false);
	}

	// Token: 0x060040C3 RID: 16579 RVA: 0x00023CEB File Offset: 0x00021EEB
	protected override void OnDisable()
	{
		base.OnDisable();
		if (this.m_textPopup)
		{
			this.m_textPopup.gameObject.SetActive(false);
			this.m_textPopup = null;
		}
	}

	// Token: 0x0400329C RID: 12956
	private const float HITBOX_DISABLE_DURATION = 2f;

	// Token: 0x0400329D RID: 12957
	[SerializeField]
	private string m_projectileName;

	// Token: 0x0400329E RID: 12958
	[SerializeField]
	private GameObject m_spawnPos;

	// Token: 0x0400329F RID: 12959
	[SerializeField]
	private StudioEventEmitter m_onHitEventEmitter;

	// Token: 0x040032A0 RID: 12960
	private Animator m_animator;

	// Token: 0x040032A1 RID: 12961
	private IHitboxController m_hbController;

	// Token: 0x040032A2 RID: 12962
	private bool m_projectileAdded;

	// Token: 0x040032A3 RID: 12963
	private bool m_textPopupPlayed;

	// Token: 0x040032A4 RID: 12964
	private TextPopupObj m_textPopup;
}
