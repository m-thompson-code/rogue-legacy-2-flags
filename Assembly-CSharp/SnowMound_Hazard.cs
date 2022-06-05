using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000745 RID: 1861
public class SnowMound_Hazard : SingleLine_Multi_Hazard, ITerrainOnEnterHitResponse, IHitResponse, ITerrainOnExitHitResponse
{
	// Token: 0x060038ED RID: 14573 RVA: 0x0001F481 File Offset: 0x0001D681
	protected override void Awake()
	{
		base.Awake();
		this.m_hbController = base.GetComponentInChildren<IHitboxController>();
	}

	// Token: 0x060038EE RID: 14574 RVA: 0x000E9DB0 File Offset: 0x000E7FB0
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

	// Token: 0x060038EF RID: 14575 RVA: 0x0001F495 File Offset: 0x0001D695
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

	// Token: 0x060038F0 RID: 14576 RVA: 0x0001F4AB File Offset: 0x0001D6AB
	public void TerrainOnEnterHitResponse(IHitboxController otherHBController)
	{
		if (otherHBController.RootGameObject.CompareTag("Player") && !this.m_slowApplied)
		{
			this.m_groundCheckCoroutine = base.StartCoroutine(this.IsGroundedCheck());
		}
	}

	// Token: 0x060038F1 RID: 14577 RVA: 0x0001F4D9 File Offset: 0x0001D6D9
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

	// Token: 0x060038F2 RID: 14578 RVA: 0x000E9E10 File Offset: 0x000E8010
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

	// Token: 0x060038F3 RID: 14579 RVA: 0x0001F4E8 File Offset: 0x0001D6E8
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

	// Token: 0x060038F4 RID: 14580 RVA: 0x000E9F20 File Offset: 0x000E8120
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

	// Token: 0x04002D9E RID: 11678
	[SerializeField]
	private SpriteRenderer m_lineSprite;

	// Token: 0x04002D9F RID: 11679
	private IHitboxController m_hbController;

	// Token: 0x04002DA0 RID: 11680
	private bool m_slowApplied;

	// Token: 0x04002DA1 RID: 11681
	private Coroutine m_groundCheckCoroutine;

	// Token: 0x04002DA2 RID: 11682
	private static int m_numSnowMoundsCollided_STATIC;

	// Token: 0x04002DA3 RID: 11683
	private static GenericEffect m_snowMoundEffect_STATIC;

	// Token: 0x04002DA4 RID: 11684
	private static Coroutine m_pauseSnowMoundEffectCoroutine_STATIC;

	// Token: 0x04002DA5 RID: 11685
	private static SnowMound_Hazard m_activeSnowMound_STATIC;
}
