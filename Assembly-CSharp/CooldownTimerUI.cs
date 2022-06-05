using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x0200035E RID: 862
public class CooldownTimerUI : MonoBehaviour
{
	// Token: 0x17000D38 RID: 3384
	// (get) Token: 0x06001C48 RID: 7240 RVA: 0x0000EA03 File Offset: 0x0000CC03
	// (set) Token: 0x06001C49 RID: 7241 RVA: 0x0000EA0B File Offset: 0x0000CC0B
	public ICooldown Cooldown
	{
		get
		{
			return this.m_cooldown;
		}
		set
		{
			this.m_cooldown = value;
			if (this.m_cooldown != null)
			{
				this.m_cooldown.OnBeginCooldownRelay.AddListener(new Action<object, CooldownEventArgs>(this.OnBeginCooldown), false);
			}
		}
	}

	// Token: 0x06001C4A RID: 7242 RVA: 0x00098DE0 File Offset: 0x00096FE0
	private void OnBeginCooldown(object sender, CooldownEventArgs eventArgs)
	{
		if (this.Cooldown.IsNativeNull())
		{
			return;
		}
		base.StopAllCoroutines();
		if (this.m_image.type == Image.Type.Filled)
		{
			base.StartCoroutine(this.CooldownRunning());
			return;
		}
		Debug.LogFormat("{0}: Image should be of Type (Filled) in order to work with Cooldown Timer", new object[]
		{
			Time.frameCount
		});
	}

	// Token: 0x06001C4B RID: 7243 RVA: 0x0000EA3A File Offset: 0x0000CC3A
	private IEnumerator CooldownRunning()
	{
		while (this.Cooldown.IsOnCooldown)
		{
			float fillAmount = 1f - this.Cooldown.CooldownTimer / this.Cooldown.ActualCooldownTime;
			this.m_image.fillAmount = fillAmount;
			yield return null;
		}
		this.m_image.fillAmount = 1f;
		yield break;
	}

	// Token: 0x06001C4C RID: 7244 RVA: 0x0000EA49 File Offset: 0x0000CC49
	private void OnDisable()
	{
		base.StopAllCoroutines();
		if (this.Cooldown != null)
		{
			this.Cooldown.OnBeginCooldownRelay.RemoveListener(new Action<object, CooldownEventArgs>(this.OnBeginCooldown));
		}
	}

	// Token: 0x040019DF RID: 6623
	[SerializeField]
	private Image m_image;

	// Token: 0x040019E0 RID: 6624
	private ICooldown m_cooldown;
}
