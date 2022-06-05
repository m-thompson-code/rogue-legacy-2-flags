using System;
using System.Collections;
using RLAudio;
using UnityEngine;

// Token: 0x020003ED RID: 1005
public class WispEnemyHitResponse : EnemyHitResponse
{
	// Token: 0x0600250E RID: 9486 RVA: 0x0007B168 File Offset: 0x00079368
	private void OnEnable()
	{
		this.m_sprite.SetActive(true);
	}

	// Token: 0x0600250F RID: 9487 RVA: 0x0007B178 File Offset: 0x00079378
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

	// Token: 0x06002510 RID: 9488 RVA: 0x0007B1DE File Offset: 0x000793DE
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

	// Token: 0x04001F56 RID: 8022
	[SerializeField]
	private GameObject m_sprite;

	// Token: 0x04001F57 RID: 8023
	[SerializeField]
	private DamageAudioController m_damageAudioController;

	// Token: 0x04001F58 RID: 8024
	private const float m_blinkInterval = 0.05f;

	// Token: 0x04001F59 RID: 8025
	private Coroutine m_blinkCoroutine;
}
