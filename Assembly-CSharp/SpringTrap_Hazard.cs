using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200045D RID: 1117
public class SpringTrap_Hazard : Hazard
{
	// Token: 0x06002938 RID: 10552 RVA: 0x00088523 File Offset: 0x00086723
	public void BouncePlayer()
	{
		if (!this.m_trapTriggered && PlayerManager.IsInstantiated)
		{
			this.m_trapTriggered = true;
			base.StopAllCoroutines();
			base.StartCoroutine(this.SpringTrapCoroutine());
		}
	}

	// Token: 0x06002939 RID: 10553 RVA: 0x0008854E File Offset: 0x0008674E
	private IEnumerator SpringTrapCoroutine()
	{
		base.Animator.SetBool("SpikesOut", true);
		if (PlayerManager.IsInstantiated)
		{
			PlayerManager.GetPlayerController().SetVelocityY(42f, false);
		}
		float startTime = Time.time;
		while (Time.time < startTime + 0.25f)
		{
			yield return null;
		}
		base.Animator.SetBool("SpikesOut", false);
		this.m_trapTriggered = false;
		yield break;
	}

	// Token: 0x0600293A RID: 10554 RVA: 0x0008855D File Offset: 0x0008675D
	public override void ResetHazard()
	{
		base.Animator.SetBool("SpikesOut", false);
		base.Animator.SetTrigger("Reset");
		this.m_trapTriggered = false;
	}

	// Token: 0x040021FB RID: 8699
	private bool m_trapTriggered;
}
