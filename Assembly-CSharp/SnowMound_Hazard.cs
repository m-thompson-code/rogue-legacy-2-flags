using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200045B RID: 1115
public class SnowMound_Hazard : SingleLine_Multi_Hazard, ITerrainOnEnterHitResponse, IHitResponse, ITerrainOnExitHitResponse
{
	// Token: 0x06002925 RID: 10533 RVA: 0x000881A8 File Offset: 0x000863A8
	protected override void Awake()
	{
		base.Awake();
		this.m_hbController = base.GetComponentInChildren<IHitboxController>();
	}

	// Token: 0x06002926 RID: 10534 RVA: 0x000881BC File Offset: 0x000863BC
	public override void Initialize(PivotPoint pivot, int width, HazardArgs hazardArgs)
	{
		base.Initialize(pivot, width, hazardArgs);
		if (base.transform.localEulerAngles.z > 45f && base.transform.localEulerAngles.z < 315f)
		{
			base.gameObject.SetActive(false);
			return;
		}
		base.StartCoroutine(this.SetSnowMoundWidth(width));
	}

	// Token: 0x06002927 RID: 10535 RVA: 0x0008821B File Offset: 0x0008641B
	private IEnumerator SetSnowMoundWidth(int width)
	{
		while (!this.m_hbController.IsInitialized)
		{
			yield return null;
		}
		this.m_hbController.RepeatHitDuration = 0f;
		BoxCollider2D boxCollider2D = this.m_hbController.GetCollider(HitboxType.Terrain) as BoxCollider2D;
		Vector2 size = boxCollider2D.size;
		size.x = (float)width;
		boxCollider2D.size = size;
		this.m_lineSprite.size = new Vector2((float)width, this.m_lineSprite.size.y);
		yield break;
	}

	// Token: 0x06002928 RID: 10536 RVA: 0x00088231 File Offset: 0x00086431
	public void TerrainOnEnterHitResponse(IHitboxController otherHBController)
	{
		if (otherHBController.RootGameObject.CompareTag("Player") && !this.m_slowApplied)
		{
			this.m_groundCheckCoroutine = base.StartCoroutine(this.IsGroundedCheck());
		}
	}

	// Token: 0x06002929 RID: 10537 RVA: 0x0008825F File Offset: 0x0008645F
	private IEnumerator IsGroundedCheck()
	{
		PlayerController playerController = PlayerManager.GetPlayerController();
		while (!playerController.IsGrounded)
		{
			yield return null;
		}
		this.m_slowApplied = true;
		SnowMound_Hazard.m_numSnowMoundsCollided_STATIC++;
		if (SnowMound_Hazard.m_numSnowMoundsCollided_STATIC == 1)
		{
			playerController.MovementSpeedMod -= 0.35f;
			if (SnowMound_Hazard.m_snowMoundEffect_STATIC == null || !SnowMound_Hazard.m_snowMoundEffect_STATIC.IsPlaying)
			{
				if (SnowMound_Hazard.m_pauseSnowMoundEffectCoroutine_STATIC != null && SnowMound_Hazard.m_activeSnowMound_STATIC)
				{
					SnowMound_Hazard.m_activeSnowMound_STATIC.StopCoroutine(SnowMound_Hazard.m_pauseSnowMoundEffectCoroutine_STATIC);
					SnowMound_Hazard.m_pauseSnowMoundEffectCoroutine_STATIC = null;
					SnowMound_Hazard.m_activeSnowMound_STATIC = null;
				}
				SnowMound_Hazard.m_snowMoundEffect_STATIC = (EffectManager.PlayEffect(playerController.gameObject, playerController.Animator, "SnowDebris_Effect", Vector3.zero, 999999f, EffectStopType.Gracefully, EffectTriggerDirection.None) as GenericEffect);
				SnowMound_Hazard.m_snowMoundEffect_STATIC.transform.SetParent(playerController.Pivot.transform, false);
				if (playerController.transform.localScale.x > 1.4f)
				{
					Vector3 localScale = SnowMound_Hazard.m_snowMoundEffect_STATIC.transform.localScale;
					float num = localScale.x / 1.4f;
					localScale = new Vector3(num, num, num);
					SnowMound_Hazard.m_snowMoundEffect_STATIC.transform.localScale = localScale;
				}
				SnowMound_Hazard.m_pauseSnowMoundEffectCoroutine_STATIC = base.StartCoroutine(this.PauseSnowMoundEffectCoroutine());
				SnowMound_Hazard.m_activeSnowMound_STATIC = this;
			}
		}
		yield break;
	}

	// Token: 0x0600292A RID: 10538 RVA: 0x00088270 File Offset: 0x00086470
	public void TerrainOnExitHitResponse(IHitboxController otherHBController)
	{
		if (otherHBController.RootGameObject.CompareTag("Player"))
		{
			if (this.m_groundCheckCoroutine != null)
			{
				base.StopCoroutine(this.m_groundCheckCoroutine);
			}
			if (this.m_slowApplied)
			{
				this.m_slowApplied = false;
				SnowMound_Hazard.m_numSnowMoundsCollided_STATIC--;
				if (SnowMound_Hazard.m_numSnowMoundsCollided_STATIC == 0)
				{
					PlayerController playerController = PlayerManager.GetPlayerController();
					playerController.MovementSpeedMod += 0.35f;
					if (SnowMound_Hazard.m_pauseSnowMoundEffectCoroutine_STATIC != null && SnowMound_Hazard.m_activeSnowMound_STATIC)
					{
						SnowMound_Hazard.m_activeSnowMound_STATIC.StopCoroutine(SnowMound_Hazard.m_pauseSnowMoundEffectCoroutine_STATIC);
						SnowMound_Hazard.m_pauseSnowMoundEffectCoroutine_STATIC = null;
						SnowMound_Hazard.m_activeSnowMound_STATIC = null;
					}
					if (SnowMound_Hazard.m_snowMoundEffect_STATIC)
					{
						if (SnowMound_Hazard.m_snowMoundEffect_STATIC.IsPlaying)
						{
							if (SnowMound_Hazard.m_snowMoundEffect_STATIC.isActiveAndEnabled)
							{
								SnowMound_Hazard.m_snowMoundEffect_STATIC.Stop(EffectStopType.Gracefully);
							}
							else
							{
								SnowMound_Hazard.m_snowMoundEffect_STATIC.Stop(EffectStopType.Immediate);
							}
						}
						SnowMound_Hazard.m_snowMoundEffect_STATIC = null;
					}
					EffectManager.PlayEffect(playerController.gameObject, playerController.Animator, "SnowDebrisExit_Effect", playerController.transform.position, 0.1f, EffectStopType.Gracefully, EffectTriggerDirection.None);
				}
			}
		}
	}

	// Token: 0x0600292B RID: 10539 RVA: 0x0008837F File Offset: 0x0008657F
	private IEnumerator PauseSnowMoundEffectCoroutine()
	{
		PlayerController playerController = PlayerManager.GetPlayerController();
		while (SnowMound_Hazard.m_snowMoundEffect_STATIC)
		{
			if (SnowMound_Hazard.m_snowMoundEffect_STATIC.ParticleSystem.isPlaying && Mathf.Abs(playerController.Velocity.x) < 0.01f)
			{
				SnowMound_Hazard.m_snowMoundEffect_STATIC.ParticleSystem.Stop();
			}
			else if (!SnowMound_Hazard.m_snowMoundEffect_STATIC.ParticleSystem.isPlaying && Mathf.Abs(playerController.Velocity.x) >= 0.01f)
			{
				SnowMound_Hazard.m_snowMoundEffect_STATIC.ParticleSystem.Play();
			}
			yield return null;
		}
		yield break;
		yield break;
	}

	// Token: 0x0600292C RID: 10540 RVA: 0x00088388 File Offset: 0x00086588
	public override void ResetHazard()
	{
		if (this.m_slowApplied)
		{
			PlayerManager.GetPlayerController().MovementSpeedMod += 0.35f;
			if (SnowMound_Hazard.m_pauseSnowMoundEffectCoroutine_STATIC != null && SnowMound_Hazard.m_activeSnowMound_STATIC)
			{
				SnowMound_Hazard.m_activeSnowMound_STATIC.StopCoroutine(SnowMound_Hazard.m_pauseSnowMoundEffectCoroutine_STATIC);
				SnowMound_Hazard.m_pauseSnowMoundEffectCoroutine_STATIC = null;
				SnowMound_Hazard.m_activeSnowMound_STATIC = null;
			}
			if (SnowMound_Hazard.m_snowMoundEffect_STATIC != null)
			{
				if (SnowMound_Hazard.m_snowMoundEffect_STATIC.isActiveAndEnabled)
				{
					SnowMound_Hazard.m_snowMoundEffect_STATIC.Stop(EffectStopType.Gracefully);
				}
				else
				{
					SnowMound_Hazard.m_snowMoundEffect_STATIC.Stop(EffectStopType.Immediate);
				}
				SnowMound_Hazard.m_snowMoundEffect_STATIC = null;
			}
		}
		base.StopAllCoroutines();
		this.m_slowApplied = false;
	}

	// Token: 0x040021ED RID: 8685
	[SerializeField]
	private SpriteRenderer m_lineSprite;

	// Token: 0x040021EE RID: 8686
	private IHitboxController m_hbController;

	// Token: 0x040021EF RID: 8687
	private bool m_slowApplied;

	// Token: 0x040021F0 RID: 8688
	private Coroutine m_groundCheckCoroutine;

	// Token: 0x040021F1 RID: 8689
	private static int m_numSnowMoundsCollided_STATIC;

	// Token: 0x040021F2 RID: 8690
	private static GenericEffect m_snowMoundEffect_STATIC;

	// Token: 0x040021F3 RID: 8691
	private static Coroutine m_pauseSnowMoundEffectCoroutine_STATIC;

	// Token: 0x040021F4 RID: 8692
	private static SnowMound_Hazard m_activeSnowMound_STATIC;
}
