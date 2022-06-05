using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020001DB RID: 475
public class CooldownTimerUI : MonoBehaviour
{
	// Token: 0x17000A60 RID: 2656
	// (get) Token: 0x060013AD RID: 5037 RVA: 0x0003BD29 File Offset: 0x00039F29
	// (set) Token: 0x060013AE RID: 5038 RVA: 0x0003BD31 File Offset: 0x00039F31
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

	// Token: 0x060013AF RID: 5039 RVA: 0x0003BD60 File Offset: 0x00039F60
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

	// Token: 0x060013B0 RID: 5040 RVA: 0x0003BDBA File Offset: 0x00039FBA
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

	// Token: 0x060013B1 RID: 5041 RVA: 0x0003BDC9 File Offset: 0x00039FC9
	private void OnDisable()
	{
		base.StopAllCoroutines();
		if (this.Cooldown != null)
		{
			this.Cooldown.OnBeginCooldownRelay.RemoveListener(new Action<object, CooldownEventArgs>(this.OnBeginCooldown));
		}
	}

	// Token: 0x0400138E RID: 5006
	[SerializeField]
	private Image m_image;

	// Token: 0x0400138F RID: 5007
	private ICooldown m_cooldown;
}
