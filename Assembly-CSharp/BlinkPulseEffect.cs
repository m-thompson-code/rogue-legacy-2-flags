using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// Token: 0x020001C4 RID: 452
public class BlinkPulseEffect : MonoBehaviour
{
	// Token: 0x170009EE RID: 2542
	// (get) Token: 0x0600122C RID: 4652 RVA: 0x00034BC3 File Offset: 0x00032DC3
	// (set) Token: 0x0600122D RID: 4653 RVA: 0x00034BCB File Offset: 0x00032DCB
	public Color BlinkOnHitTint
	{
		get
		{
			return this.m_blinkOnHitTint;
		}
		set
		{
			this.m_blinkOnHitTint = value;
		}
	}

	// Token: 0x170009EF RID: 2543
	// (get) Token: 0x0600122E RID: 4654 RVA: 0x00034BD4 File Offset: 0x00032DD4
	// (set) Token: 0x0600122F RID: 4655 RVA: 0x00034BF5 File Offset: 0x00032DF5
	public bool UseOverrideDefaultTint
	{
		get
		{
			if (this.m_charController)
			{
				return this.m_charController.UseOverrideDefaultTint;
			}
			return this.m_useOverrideDefaultTint;
		}
		set
		{
			if (this.m_charController)
			{
				this.m_charController.UseOverrideDefaultTint = value;
				return;
			}
			this.m_useOverrideDefaultTint = value;
		}
	}

	// Token: 0x170009F0 RID: 2544
	// (get) Token: 0x06001230 RID: 4656 RVA: 0x00034C18 File Offset: 0x00032E18
	// (set) Token: 0x06001231 RID: 4657 RVA: 0x00034C39 File Offset: 0x00032E39
	public Color DefaultTintOverrideColor
	{
		get
		{
			if (this.m_charController)
			{
				return this.m_charController.DefaultTintOverrideColor;
			}
			return this.m_defaultTintOverrideColor;
		}
		set
		{
			if (this.m_charController)
			{
				this.m_charController.DefaultTintOverrideColor = value;
				return;
			}
			this.m_defaultTintOverrideColor = value;
		}
	}

	// Token: 0x170009F1 RID: 2545
	// (get) Token: 0x06001232 RID: 4658 RVA: 0x00034C5C File Offset: 0x00032E5C
	// (set) Token: 0x06001233 RID: 4659 RVA: 0x00034C64 File Offset: 0x00032E64
	public bool UseUnscaledTime { get; set; }

	// Token: 0x170009F2 RID: 2546
	// (get) Token: 0x06001234 RID: 4660 RVA: 0x00034C6D File Offset: 0x00032E6D
	// (set) Token: 0x06001235 RID: 4661 RVA: 0x00034C75 File Offset: 0x00032E75
	public float SingleBlinkDuration
	{
		get
		{
			return this.m_singleBlinkDuration;
		}
		set
		{
			this.m_singleBlinkDuration = value;
		}
	}

	// Token: 0x170009F3 RID: 2547
	// (get) Token: 0x06001236 RID: 4662 RVA: 0x00034C7E File Offset: 0x00032E7E
	public List<RendererArrayEntry> RendererArrayDefaultTint
	{
		get
		{
			if (this.m_charController && (this.m_rendererArray == null || this.m_rendererArray.Count == 0))
			{
				return this.m_charController.RendererArrayDefaultTint;
			}
			return this.m_rendererArrayDefaultTint;
		}
	}

	// Token: 0x170009F4 RID: 2548
	// (get) Token: 0x06001237 RID: 4663 RVA: 0x00034CB4 File Offset: 0x00032EB4
	public List<Renderer> RendererArray
	{
		get
		{
			if (this.m_charController && (this.m_rendererArray == null || this.m_rendererArray.Count == 0))
			{
				return this.m_charController.RendererArray;
			}
			return this.m_rendererArray;
		}
	}

	// Token: 0x06001238 RID: 4664 RVA: 0x00034CEC File Offset: 0x00032EEC
	private void Awake()
	{
		this.m_blinkWaitYield = new WaitRL_Yield(this.m_singleBlinkDuration, false);
		this.m_invincibilityWaitYield = new WaitRL_Yield(this.m_invincibilityPulseRate, false);
		if (BlinkPulseEffect.m_matPropertyBlock_STATIC == null)
		{
			BlinkPulseEffect.m_matPropertyBlock_STATIC = new MaterialPropertyBlock();
		}
		GameObject root = this.GetRoot(false);
		if (this.m_rendererArray == null || this.m_rendererArray.Count == 0)
		{
			this.m_charController = root.GetComponent<BaseCharacterController>();
			if (!this.m_charController)
			{
				root.GetComponentsInChildren<Renderer>(true, this.m_rendererArray);
			}
		}
	}

	// Token: 0x06001239 RID: 4665 RVA: 0x00034D71 File Offset: 0x00032F71
	private void OnEnable()
	{
		if (!this.m_isInitialized)
		{
			this.Initialize();
		}
	}

	// Token: 0x0600123A RID: 4666 RVA: 0x00034D84 File Offset: 0x00032F84
	private void Initialize()
	{
		if (!this.m_charController)
		{
			CameraLayerController component = base.GetComponent<CameraLayerController>();
			if (component && component.IsSet && component.IsProp && component.CameraLayer != CameraLayer.Game)
			{
				return;
			}
			this.m_rendererArrayDefaultTint = new List<RendererArrayEntry>();
			for (int i = 0; i < this.m_rendererArray.Count; i++)
			{
				RendererArrayEntry rendererArrayEntry = default(RendererArrayEntry);
				Material sharedMaterial = this.RendererArray[i].sharedMaterial;
				rendererArrayEntry.HasAlphaColor = sharedMaterial.HasProperty(ShaderID_RL._AlphaBlendColor);
				rendererArrayEntry.HasRimColor = sharedMaterial.HasProperty(ShaderID_RL._RimLightColor);
				rendererArrayEntry.HasRimBias = sharedMaterial.HasProperty(ShaderID_RL._RimBias);
				rendererArrayEntry.HasRimScale = sharedMaterial.HasProperty(ShaderID_RL._RimScale);
				if (rendererArrayEntry.HasAlphaColor)
				{
					rendererArrayEntry.DefaultAlphaColor = sharedMaterial.GetColor(ShaderID_RL._AlphaBlendColor);
				}
				if (rendererArrayEntry.HasRimColor)
				{
					rendererArrayEntry.DefaultRimLightColor = sharedMaterial.GetColor(ShaderID_RL._RimLightColor);
				}
				if (rendererArrayEntry.HasRimBias)
				{
					rendererArrayEntry.DefaultRimBias = sharedMaterial.GetFloat(ShaderID_RL._RimBias);
				}
				if (rendererArrayEntry.HasRimScale)
				{
					rendererArrayEntry.DefaultRimScale = sharedMaterial.GetFloat(ShaderID_RL._RimScale);
				}
				this.m_rendererArrayDefaultTint.Add(rendererArrayEntry);
			}
		}
		this.m_isInitialized = true;
	}

	// Token: 0x0600123B RID: 4667 RVA: 0x00034ED0 File Offset: 0x000330D0
	public void ActivateBlackFill(BlackFillType fillType, float fadeInDuration)
	{
		if (this.m_charController && fillType == BlackFillType.EnemiesBlackFill_Trait)
		{
			EnemyController enemyController = this.m_charController as EnemyController;
			if (enemyController && Trait_EV.TRAIT_EFFECT_EXCEPTION_ARRAY.Contains(enemyController.EnemyType))
			{
				return;
			}
		}
		if (this.m_blackFillEffectTable.ContainsKey(fillType))
		{
			this.m_blackFillEffectTable[fillType] = true;
		}
		else
		{
			this.m_blackFillEffectTable.Add(fillType, true);
		}
		if (this.m_isFadedOut)
		{
			if (this.m_blackFillFadeCoroutine != null)
			{
				base.StopCoroutine(this.m_blackFillFadeCoroutine);
			}
			if (fadeInDuration > 0f)
			{
				this.m_blackFillFadeCoroutine = base.StartCoroutine(this.BlackFillFadeCoroutine(fadeInDuration, true));
				return;
			}
			Color black = Color.black;
			this.UseOverrideDefaultTint = true;
			this.m_isFadedIn = true;
			this.m_isFadedOut = false;
			this.DefaultTintOverrideColor = black;
			this.SetRendererArrayColor(black);
		}
	}

	// Token: 0x0600123C RID: 4668 RVA: 0x00034FA4 File Offset: 0x000331A4
	public void DisableBlackFill(BlackFillType fillType, float fadeOutDuration)
	{
		if (this.m_blackFillEffectTable.ContainsKey(fillType))
		{
			this.m_blackFillEffectTable[fillType] = false;
		}
		else
		{
			this.m_blackFillEffectTable.Add(fillType, false);
		}
		bool flag = true;
		foreach (KeyValuePair<BlackFillType, bool> keyValuePair in this.m_blackFillEffectTable)
		{
			if (keyValuePair.Value)
			{
				flag = false;
				break;
			}
		}
		if (flag && this.m_isFadedIn)
		{
			if (this.m_blackFillFadeCoroutine != null)
			{
				base.StopCoroutine(this.m_blackFillFadeCoroutine);
			}
			if (fadeOutDuration > 0f)
			{
				this.m_blackFillFadeCoroutine = base.StartCoroutine(this.BlackFillFadeCoroutine(fadeOutDuration, false));
				return;
			}
			this.ResetBlackFillState();
		}
	}

	// Token: 0x0600123D RID: 4669 RVA: 0x0003506C File Offset: 0x0003326C
	private IEnumerator BlackFillFadeCoroutine(float duration, bool fadeIn)
	{
		while (!this.m_isInitialized)
		{
			yield return null;
		}
		if (fadeIn)
		{
			this.m_isFadedIn = true;
			this.m_isFadedOut = false;
		}
		else
		{
			this.m_isFadedIn = false;
			this.m_isFadedOut = true;
		}
		float startTime = Time.time;
		float time = Time.time;
		Color fadeColor = Color.black;
		while (time < duration + startTime)
		{
			float lerpAmount = (time - startTime) / duration;
			if (fadeIn)
			{
				this.SetRendererArrayColorLerpFromDefault(fadeColor, lerpAmount);
			}
			else
			{
				this.SetRendererArrayColorLerpToDefault(fadeColor, lerpAmount);
			}
			yield return null;
			time = Time.time;
		}
		if (fadeIn)
		{
			this.UseOverrideDefaultTint = true;
			this.DefaultTintOverrideColor = fadeColor;
			this.SetRendererArrayColor(fadeColor);
		}
		else
		{
			this.ResetBlackFillState();
		}
		yield break;
	}

	// Token: 0x0600123E RID: 4670 RVA: 0x0003508C File Offset: 0x0003328C
	public void ResetAllBlackFills()
	{
		foreach (BlackFillType blackFillType in BlackFillType_RL.TypeArray)
		{
			if (blackFillType != BlackFillType.None && this.m_blackFillEffectTable.ContainsKey(blackFillType))
			{
				this.m_blackFillEffectTable[blackFillType] = false;
			}
		}
		this.ResetBlackFillState();
	}

	// Token: 0x0600123F RID: 4671 RVA: 0x000350D8 File Offset: 0x000332D8
	private void ResetBlackFillState()
	{
		if (!this.m_isInitialized)
		{
			return;
		}
		this.UseOverrideDefaultTint = false;
		this.ResetRendererArrayColor();
		this.m_isFadedIn = false;
		this.m_isFadedOut = true;
		if (TraitManager.IsTraitActive(TraitType.SmallHitbox) && base.CompareTag("Player"))
		{
			this.ActivateBlackFill(BlackFillType.SmallHitbox_Trait, 0f);
			return;
		}
		if (TraitManager.IsTraitActive(TraitType.EnemiesBlackFill) && base.CompareTag("Enemy"))
		{
			this.ActivateBlackFill(BlackFillType.EnemiesBlackFill_Trait, 0f);
		}
	}

	// Token: 0x06001240 RID: 4672 RVA: 0x00035154 File Offset: 0x00033354
	public void SetRendererArrayColor(Color color)
	{
		if (!this.m_isInitialized)
		{
			return;
		}
		for (int i = 0; i < this.RendererArray.Count; i++)
		{
			Renderer renderer = this.RendererArray[i];
			renderer.GetPropertyBlock(BlinkPulseEffect.m_matPropertyBlock_STATIC);
			RendererArrayEntry rendererArrayEntry = this.RendererArrayDefaultTint[i];
			if (rendererArrayEntry.HasAlphaColor)
			{
				BlinkPulseEffect.m_matPropertyBlock_STATIC.SetColor(ShaderID_RL._AlphaBlendColor, color);
			}
			if (rendererArrayEntry.HasRimColor)
			{
				BlinkPulseEffect.m_matPropertyBlock_STATIC.SetColor(ShaderID_RL._RimLightColor, color);
			}
			if (rendererArrayEntry.HasRimBias)
			{
				BlinkPulseEffect.m_matPropertyBlock_STATIC.SetFloat(ShaderID_RL._RimBias, 0f);
			}
			if (rendererArrayEntry.HasRimScale)
			{
				BlinkPulseEffect.m_matPropertyBlock_STATIC.SetFloat(ShaderID_RL._RimScale, 0f);
			}
			renderer.SetPropertyBlock(BlinkPulseEffect.m_matPropertyBlock_STATIC);
		}
	}

	// Token: 0x06001241 RID: 4673 RVA: 0x0003521C File Offset: 0x0003341C
	public void SetRendererArrayColorLerpFromDefault(Color color, float lerpAmount)
	{
		if (!this.m_isInitialized)
		{
			return;
		}
		for (int i = 0; i < this.RendererArray.Count; i++)
		{
			Renderer renderer = this.RendererArray[i];
			renderer.GetPropertyBlock(BlinkPulseEffect.m_matPropertyBlock_STATIC);
			RendererArrayEntry rendererArrayEntry = this.RendererArrayDefaultTint[i];
			if (rendererArrayEntry.HasAlphaColor)
			{
				BlinkPulseEffect.m_matPropertyBlock_STATIC.SetColor(ShaderID_RL._AlphaBlendColor, Color.Lerp(rendererArrayEntry.DefaultAlphaColor, color, lerpAmount));
			}
			if (rendererArrayEntry.HasRimColor)
			{
				BlinkPulseEffect.m_matPropertyBlock_STATIC.SetColor(ShaderID_RL._RimLightColor, Color.Lerp(rendererArrayEntry.DefaultRimLightColor, color, lerpAmount));
			}
			if (rendererArrayEntry.HasRimBias)
			{
				BlinkPulseEffect.m_matPropertyBlock_STATIC.SetFloat(ShaderID_RL._RimBias, Mathf.Lerp(rendererArrayEntry.DefaultRimBias, 0f, lerpAmount));
			}
			if (rendererArrayEntry.HasRimScale)
			{
				BlinkPulseEffect.m_matPropertyBlock_STATIC.SetFloat(ShaderID_RL._RimScale, Mathf.Lerp(rendererArrayEntry.DefaultRimScale, 0f, lerpAmount));
			}
			renderer.SetPropertyBlock(BlinkPulseEffect.m_matPropertyBlock_STATIC);
		}
	}

	// Token: 0x06001242 RID: 4674 RVA: 0x00035314 File Offset: 0x00033514
	public void SetRendererArrayColorLerpToDefault(Color color, float lerpAmount)
	{
		if (!this.m_isInitialized)
		{
			return;
		}
		for (int i = 0; i < this.RendererArray.Count; i++)
		{
			Renderer renderer = this.RendererArray[i];
			renderer.GetPropertyBlock(BlinkPulseEffect.m_matPropertyBlock_STATIC);
			RendererArrayEntry rendererArrayEntry = this.RendererArrayDefaultTint[i];
			if (rendererArrayEntry.HasAlphaColor)
			{
				BlinkPulseEffect.m_matPropertyBlock_STATIC.SetColor(ShaderID_RL._AlphaBlendColor, Color.Lerp(color, rendererArrayEntry.DefaultAlphaColor, lerpAmount));
			}
			if (rendererArrayEntry.HasRimColor)
			{
				BlinkPulseEffect.m_matPropertyBlock_STATIC.SetColor(ShaderID_RL._RimLightColor, Color.Lerp(color, rendererArrayEntry.DefaultRimLightColor, lerpAmount));
			}
			if (rendererArrayEntry.HasRimBias)
			{
				BlinkPulseEffect.m_matPropertyBlock_STATIC.SetFloat(ShaderID_RL._RimBias, Mathf.Lerp(0f, rendererArrayEntry.DefaultRimBias, lerpAmount));
			}
			if (rendererArrayEntry.HasRimScale)
			{
				BlinkPulseEffect.m_matPropertyBlock_STATIC.SetFloat(ShaderID_RL._RimScale, Mathf.Lerp(0f, rendererArrayEntry.DefaultRimScale, lerpAmount));
			}
			renderer.SetPropertyBlock(BlinkPulseEffect.m_matPropertyBlock_STATIC);
		}
	}

	// Token: 0x06001243 RID: 4675 RVA: 0x0003540C File Offset: 0x0003360C
	public void ResetRendererArrayColor()
	{
		if (!this.m_isInitialized)
		{
			return;
		}
		if (this.UseOverrideDefaultTint)
		{
			this.SetRendererArrayColor(this.DefaultTintOverrideColor);
			return;
		}
		for (int i = 0; i < this.RendererArray.Count; i++)
		{
			Renderer renderer = this.RendererArray[i];
			renderer.GetPropertyBlock(BlinkPulseEffect.m_matPropertyBlock_STATIC);
			RendererArrayEntry rendererArrayEntry = this.RendererArrayDefaultTint[i];
			if (rendererArrayEntry.HasAlphaColor)
			{
				BlinkPulseEffect.m_matPropertyBlock_STATIC.SetColor(ShaderID_RL._AlphaBlendColor, rendererArrayEntry.DefaultAlphaColor);
			}
			if (rendererArrayEntry.HasRimColor)
			{
				BlinkPulseEffect.m_matPropertyBlock_STATIC.SetColor(ShaderID_RL._RimLightColor, rendererArrayEntry.DefaultRimLightColor);
			}
			if (rendererArrayEntry.HasRimBias)
			{
				BlinkPulseEffect.m_matPropertyBlock_STATIC.SetFloat(ShaderID_RL._RimBias, rendererArrayEntry.DefaultRimBias);
			}
			if (rendererArrayEntry.HasRimScale)
			{
				BlinkPulseEffect.m_matPropertyBlock_STATIC.SetFloat(ShaderID_RL._RimScale, rendererArrayEntry.DefaultRimScale);
			}
			renderer.SetPropertyBlock(BlinkPulseEffect.m_matPropertyBlock_STATIC);
		}
	}

	// Token: 0x06001244 RID: 4676 RVA: 0x000354F5 File Offset: 0x000336F5
	private void OnDisable()
	{
		this.ResetBlackFillState();
		base.StopAllCoroutines();
		this.StopInvincibilityEffect();
	}

	// Token: 0x06001245 RID: 4677 RVA: 0x00035509 File Offset: 0x00033709
	public void StartSingleBlinkEffect()
	{
		this.StartSingleBlinkEffect(this.m_blinkOnHitTint);
	}

	// Token: 0x06001246 RID: 4678 RVA: 0x00035517 File Offset: 0x00033717
	public void StartSingleBlinkThenInvincibility()
	{
		if (!base.gameObject.activeInHierarchy)
		{
			return;
		}
		if (this.m_blinkCoroutine != null)
		{
			base.StopCoroutine(this.m_blinkCoroutine);
		}
		this.m_blinkCoroutine = base.StartCoroutine(this.SingleBlinkThenInvincibilityCoroutine(this.m_blinkOnHitTint));
	}

	// Token: 0x06001247 RID: 4679 RVA: 0x00035553 File Offset: 0x00033753
	private IEnumerator SingleBlinkThenInvincibilityCoroutine(Color blinkColor)
	{
		yield return this.SingleBlinkCoroutine(blinkColor);
		this.StartInvincibilityEffect(-1f);
		yield break;
	}

	// Token: 0x06001248 RID: 4680 RVA: 0x00035569 File Offset: 0x00033769
	public void StartSingleBlinkEffect(Color blinkColor)
	{
		if (!base.gameObject.activeInHierarchy)
		{
			return;
		}
		if (this.m_blinkCoroutine != null)
		{
			base.StopCoroutine(this.m_blinkCoroutine);
		}
		this.m_blinkCoroutine = base.StartCoroutine(this.SingleBlinkCoroutine(blinkColor));
	}

	// Token: 0x06001249 RID: 4681 RVA: 0x000355A0 File Offset: 0x000337A0
	private IEnumerator SingleBlinkCoroutine(Color blinkColor)
	{
		this.m_blinkWaitYield.CreateNew(this.m_singleBlinkDuration, this.UseUnscaledTime);
		if (this.RendererArray != null)
		{
			this.SetRendererArrayColor(blinkColor);
			yield return this.m_blinkWaitYield;
			this.ResetRendererArrayColor();
		}
		yield break;
	}

	// Token: 0x0600124A RID: 4682 RVA: 0x000355B6 File Offset: 0x000337B6
	public void StartInvincibilityEffect(float duration = -1f)
	{
		if (this.m_blinkCoroutine != null)
		{
			base.StopCoroutine(this.m_blinkCoroutine);
		}
		this.StopInvincibilityEffect();
		this.m_invincibilityCoroutine = base.StartCoroutine(this.InvincibilityCoroutine(duration));
	}

	// Token: 0x0600124B RID: 4683 RVA: 0x000355E5 File Offset: 0x000337E5
	private IEnumerator InvincibilityCoroutine(float duration = -1f)
	{
		if (duration == -1f)
		{
			duration = 999999f;
		}
		float startTime = Time.time;
		while (Time.time < startTime + duration)
		{
			this.m_invincibilityWaitYield.CreateNew(this.m_invincibilityPulseRate, this.UseUnscaledTime);
			if (!this.m_invincibilityEffectOn)
			{
				this.ResetRendererArrayColor();
			}
			else
			{
				this.SetRendererArrayColor(this.m_invincibilityTint);
			}
			this.m_invincibilityEffectOn = !this.m_invincibilityEffectOn;
			yield return this.m_invincibilityWaitYield;
		}
		if (this.RendererArray != null)
		{
			this.ResetRendererArrayColor();
		}
		this.m_invincibilityEffectOn = false;
		yield break;
	}

	// Token: 0x0600124C RID: 4684 RVA: 0x000355FB File Offset: 0x000337FB
	public void StopInvincibilityEffect()
	{
		if (this.m_invincibilityCoroutine != null)
		{
			base.StopCoroutine(this.m_invincibilityCoroutine);
		}
		if (this.RendererArray != null)
		{
			this.ResetRendererArrayColor();
		}
		this.m_invincibilityEffectOn = false;
	}

	// Token: 0x040012AD RID: 4781
	[SerializeField]
	private float m_invincibilityPulseRate = 0.1f;

	// Token: 0x040012AE RID: 4782
	[SerializeField]
	private Color m_invincibilityTint = new Color(Color.black.r, Color.black.g, Color.black.b, 0.8f);

	// Token: 0x040012AF RID: 4783
	[SerializeField]
	private float m_singleBlinkDuration = 0.1f;

	// Token: 0x040012B0 RID: 4784
	[SerializeField]
	private Color m_blinkOnHitTint = new Color(1f, 1f, 1f, 0.8f);

	// Token: 0x040012B1 RID: 4785
	[SerializeField]
	private List<Renderer> m_rendererArray;

	// Token: 0x040012B2 RID: 4786
	private bool m_invincibilityEffectOn;

	// Token: 0x040012B3 RID: 4787
	private WaitRL_Yield m_blinkWaitYield;

	// Token: 0x040012B4 RID: 4788
	private WaitRL_Yield m_invincibilityWaitYield;

	// Token: 0x040012B5 RID: 4789
	private Coroutine m_blinkCoroutine;

	// Token: 0x040012B6 RID: 4790
	private Coroutine m_invincibilityCoroutine;

	// Token: 0x040012B7 RID: 4791
	private bool m_isInitialized;

	// Token: 0x040012B8 RID: 4792
	private BaseCharacterController m_charController;

	// Token: 0x040012B9 RID: 4793
	private bool m_useOverrideDefaultTint;

	// Token: 0x040012BA RID: 4794
	private Color m_defaultTintOverrideColor;

	// Token: 0x040012BB RID: 4795
	private static MaterialPropertyBlock m_matPropertyBlock_STATIC;

	// Token: 0x040012BC RID: 4796
	private Coroutine m_blackFillFadeCoroutine;

	// Token: 0x040012BD RID: 4797
	private bool m_isFadedIn;

	// Token: 0x040012BE RID: 4798
	private bool m_isFadedOut = true;

	// Token: 0x040012BF RID: 4799
	private Dictionary<BlackFillType, bool> m_blackFillEffectTable = new Dictionary<BlackFillType, bool>();

	// Token: 0x040012C1 RID: 4801
	private List<RendererArrayEntry> m_rendererArrayDefaultTint;
}
