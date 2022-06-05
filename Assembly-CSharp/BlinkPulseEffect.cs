using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// Token: 0x02000337 RID: 823
public class BlinkPulseEffect : MonoBehaviour
{
	// Token: 0x17000CB4 RID: 3252
	// (get) Token: 0x06001A8A RID: 6794 RVA: 0x0000DA5F File Offset: 0x0000BC5F
	// (set) Token: 0x06001A8B RID: 6795 RVA: 0x0000DA67 File Offset: 0x0000BC67
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

	// Token: 0x17000CB5 RID: 3253
	// (get) Token: 0x06001A8C RID: 6796 RVA: 0x0000DA70 File Offset: 0x0000BC70
	// (set) Token: 0x06001A8D RID: 6797 RVA: 0x0000DA91 File Offset: 0x0000BC91
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

	// Token: 0x17000CB6 RID: 3254
	// (get) Token: 0x06001A8E RID: 6798 RVA: 0x0000DAB4 File Offset: 0x0000BCB4
	// (set) Token: 0x06001A8F RID: 6799 RVA: 0x0000DAD5 File Offset: 0x0000BCD5
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

	// Token: 0x17000CB7 RID: 3255
	// (get) Token: 0x06001A90 RID: 6800 RVA: 0x0000DAF8 File Offset: 0x0000BCF8
	// (set) Token: 0x06001A91 RID: 6801 RVA: 0x0000DB00 File Offset: 0x0000BD00
	public bool UseUnscaledTime { get; set; }

	// Token: 0x17000CB8 RID: 3256
	// (get) Token: 0x06001A92 RID: 6802 RVA: 0x0000DB09 File Offset: 0x0000BD09
	// (set) Token: 0x06001A93 RID: 6803 RVA: 0x0000DB11 File Offset: 0x0000BD11
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

	// Token: 0x17000CB9 RID: 3257
	// (get) Token: 0x06001A94 RID: 6804 RVA: 0x0000DB1A File Offset: 0x0000BD1A
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

	// Token: 0x17000CBA RID: 3258
	// (get) Token: 0x06001A95 RID: 6805 RVA: 0x0000DB50 File Offset: 0x0000BD50
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

	// Token: 0x06001A96 RID: 6806 RVA: 0x000924C4 File Offset: 0x000906C4
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

	// Token: 0x06001A97 RID: 6807 RVA: 0x0000DB86 File Offset: 0x0000BD86
	private void OnEnable()
	{
		if (!this.m_isInitialized)
		{
			this.Initialize();
		}
	}

	// Token: 0x06001A98 RID: 6808 RVA: 0x0009254C File Offset: 0x0009074C
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

	// Token: 0x06001A99 RID: 6809 RVA: 0x00092698 File Offset: 0x00090898
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

	// Token: 0x06001A9A RID: 6810 RVA: 0x0009276C File Offset: 0x0009096C
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

	// Token: 0x06001A9B RID: 6811 RVA: 0x0000DB96 File Offset: 0x0000BD96
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

	// Token: 0x06001A9C RID: 6812 RVA: 0x00092834 File Offset: 0x00090A34
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

	// Token: 0x06001A9D RID: 6813 RVA: 0x00092880 File Offset: 0x00090A80
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

	// Token: 0x06001A9E RID: 6814 RVA: 0x000928FC File Offset: 0x00090AFC
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

	// Token: 0x06001A9F RID: 6815 RVA: 0x000929C4 File Offset: 0x00090BC4
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

	// Token: 0x06001AA0 RID: 6816 RVA: 0x00092ABC File Offset: 0x00090CBC
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

	// Token: 0x06001AA1 RID: 6817 RVA: 0x00092BB4 File Offset: 0x00090DB4
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

	// Token: 0x06001AA2 RID: 6818 RVA: 0x0000DBB3 File Offset: 0x0000BDB3
	private void OnDisable()
	{
		this.ResetBlackFillState();
		base.StopAllCoroutines();
		this.StopInvincibilityEffect();
	}

	// Token: 0x06001AA3 RID: 6819 RVA: 0x0000DBC7 File Offset: 0x0000BDC7
	public void StartSingleBlinkEffect()
	{
		this.StartSingleBlinkEffect(this.m_blinkOnHitTint);
	}

	// Token: 0x06001AA4 RID: 6820 RVA: 0x0000DBD5 File Offset: 0x0000BDD5
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

	// Token: 0x06001AA5 RID: 6821 RVA: 0x0000DC11 File Offset: 0x0000BE11
	private IEnumerator SingleBlinkThenInvincibilityCoroutine(Color blinkColor)
	{
		yield return this.SingleBlinkCoroutine(blinkColor);
		this.StartInvincibilityEffect(-1f);
		yield break;
	}

	// Token: 0x06001AA6 RID: 6822 RVA: 0x0000DC27 File Offset: 0x0000BE27
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

	// Token: 0x06001AA7 RID: 6823 RVA: 0x0000DC5E File Offset: 0x0000BE5E
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

	// Token: 0x06001AA8 RID: 6824 RVA: 0x0000DC74 File Offset: 0x0000BE74
	public void StartInvincibilityEffect(float duration = -1f)
	{
		if (this.m_blinkCoroutine != null)
		{
			base.StopCoroutine(this.m_blinkCoroutine);
		}
		this.StopInvincibilityEffect();
		this.m_invincibilityCoroutine = base.StartCoroutine(this.InvincibilityCoroutine(duration));
	}

	// Token: 0x06001AA9 RID: 6825 RVA: 0x0000DCA3 File Offset: 0x0000BEA3
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

	// Token: 0x06001AAA RID: 6826 RVA: 0x0000DCB9 File Offset: 0x0000BEB9
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

	// Token: 0x040018C3 RID: 6339
	[SerializeField]
	private float m_invincibilityPulseRate = 0.1f;

	// Token: 0x040018C4 RID: 6340
	[SerializeField]
	private Color m_invincibilityTint = new Color(Color.black.r, Color.black.g, Color.black.b, 0.8f);

	// Token: 0x040018C5 RID: 6341
	[SerializeField]
	private float m_singleBlinkDuration = 0.1f;

	// Token: 0x040018C6 RID: 6342
	[SerializeField]
	private Color m_blinkOnHitTint = new Color(1f, 1f, 1f, 0.8f);

	// Token: 0x040018C7 RID: 6343
	[SerializeField]
	private List<Renderer> m_rendererArray;

	// Token: 0x040018C8 RID: 6344
	private bool m_invincibilityEffectOn;

	// Token: 0x040018C9 RID: 6345
	private WaitRL_Yield m_blinkWaitYield;

	// Token: 0x040018CA RID: 6346
	private WaitRL_Yield m_invincibilityWaitYield;

	// Token: 0x040018CB RID: 6347
	private Coroutine m_blinkCoroutine;

	// Token: 0x040018CC RID: 6348
	private Coroutine m_invincibilityCoroutine;

	// Token: 0x040018CD RID: 6349
	private bool m_isInitialized;

	// Token: 0x040018CE RID: 6350
	private BaseCharacterController m_charController;

	// Token: 0x040018CF RID: 6351
	private bool m_useOverrideDefaultTint;

	// Token: 0x040018D0 RID: 6352
	private Color m_defaultTintOverrideColor;

	// Token: 0x040018D1 RID: 6353
	private static MaterialPropertyBlock m_matPropertyBlock_STATIC;

	// Token: 0x040018D2 RID: 6354
	private Coroutine m_blackFillFadeCoroutine;

	// Token: 0x040018D3 RID: 6355
	private bool m_isFadedIn;

	// Token: 0x040018D4 RID: 6356
	private bool m_isFadedOut = true;

	// Token: 0x040018D5 RID: 6357
	private Dictionary<BlackFillType, bool> m_blackFillEffectTable = new Dictionary<BlackFillType, bool>();

	// Token: 0x040018D7 RID: 6359
	private List<RendererArrayEntry> m_rendererArrayDefaultTint;
}
