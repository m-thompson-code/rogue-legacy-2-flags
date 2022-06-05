using System;
using System.Collections;
using RLAudio;
using UnityEngine;

// Token: 0x02000691 RID: 1681
public class WispEnemyHitResponse : EnemyHitResponse
{
	// Token: 0x06003362 RID: 13154 RVA: 0x0001C29D File Offset: 0x0001A49D
	private void OnEnable()
	{
		this.m_sprite.SetActive(true);
	}

	// Token: 0x06003363 RID: 13155 RVA: 0x000DB228 File Offset: 0x000D9428
	public override void StartHitResponse(GameObject otherRootGameObj, IDamageObj damageObj, float damageOverride = -1f, bool trueDamage = false, bool fireEvents = true)
	{
		if (damageObj is DownstrikeProjectile_RL)
		{
			base.StartHitResponse(otherRootGameObj, damageObj, damageOverride, trueDamage, fireEvents);
			return;
		}
		if (this.m_blinkCoroutine != null)
		{
			base.StopCoroutine(this.m_blinkCoroutine);
		}
		this.m_blinkCoroutine = base.StartCoroutine(this.BlinkCoroutine(0.3f));
		if (this.m_damageAudioController)
		{
			this.m_damageAudioController.PlayImmuneHit();
		}
	}

	// Token: 0x06003364 RID: 13156 RVA: 0x0001C2AB File Offset: 0x0001A4AB
	private IEnumerator BlinkCoroutine(float duration)
	{
		this.m_sprite.SetActive(false);
		float timer = Time.time + duration;
		float intervalCounter = Time.time + 0.05f;
		while (Time.time < timer)
		{
			if (Time.time >= intervalCounter)
			{
				intervalCounter = Time.time + 0.05f;
				if (this.m_sprite.activeSelf)
				{
					this.m_sprite.SetActive(false);
				}
				else
				{
					this.m_sprite.SetActive(true);
				}
			}
			yield return null;
		}
		this.m_sprite.SetActive(true);
		yield break;
	}

	// Token: 0x040029E2 RID: 10722
	[SerializeField]
	private GameObject m_sprite;

	// Token: 0x040029E3 RID: 10723
	[SerializeField]
	private DamageAudioController m_damageAudioController;

	// Token: 0x040029E4 RID: 10724
	private const float m_blinkInterval = 0.05f;

	// Token: 0x040029E5 RID: 10725
	private Coroutine m_blinkCoroutine;
}
