using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200074E RID: 1870
public class SpringTrap_Hazard : Hazard
{
	// Token: 0x0600392A RID: 14634 RVA: 0x0001F621 File Offset: 0x0001D821
	public void BouncePlayer()
	{
		if (!this.m_trapTriggered && PlayerManager.IsInstantiated)
		{
			this.m_trapTriggered = true;
			base.StopAllCoroutines();
			base.StartCoroutine(this.SpringTrapCoroutine());
		}
	}

	// Token: 0x0600392B RID: 14635 RVA: 0x0001F64C File Offset: 0x0001D84C
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

	// Token: 0x0600392C RID: 14636 RVA: 0x0001F65B File Offset: 0x0001D85B
	public override void ResetHazard()
	{
		base.Animator.SetBool("SpikesOut", false);
		base.Animator.SetTrigger("Reset");
		this.m_trapTriggered = false;
	}

	// Token: 0x04002DC5 RID: 11717
	private bool m_trapTriggered;
}
