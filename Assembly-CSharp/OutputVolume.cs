using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000042 RID: 66
public class OutputVolume : MonoBehaviour
{
	// Token: 0x17000004 RID: 4
	// (set) Token: 0x060000AC RID: 172 RVA: 0x00003201 File Offset: 0x00001401
	public float inputValue
	{
		set
		{
			if (this.sourceType == OutputVolume.SourceType.Custom)
			{
				this.newValue = value;
				return;
			}
			Debug.LogError("Error from OutputVolume: inputValue cannot be set while sourceType is not Custom.");
		}
	}

	// Token: 0x17000005 RID: 5
	// (get) Token: 0x060000AD RID: 173 RVA: 0x0000321E File Offset: 0x0000141E
	public float outputValue
	{
		get
		{
			return this.oldScale;
		}
	}

	// Token: 0x060000AE RID: 174 RVA: 0x00042EDC File Offset: 0x000410DC
	private void Start()
	{
		if (this.outputType == OutputVolume.OutputType.PrefabBar)
		{
			this.bar = UnityEngine.Object.Instantiate<GameObject>(this.prefab);
			this.barT = this.bar.transform;
			this.barT.SetParent(base.transform, false);
			this.barT.localPosition = Vector3.zero;
			Renderer component = this.barT.GetChild(0).GetComponent<Renderer>();
			if (component != null)
			{
				this.mat = component.material;
			}
			else
			{
				Image component2 = this.barT.GetChild(0).GetComponent<Image>();
				if (component2 != null)
				{
					component2.material = new Material(component2.material);
					this.mat = component2.material;
				}
				else
				{
					Debug.LogWarning("Warning from OutputVolume: The Bar Prefab you're using doesn't have a Renderer or Image component as its first child. Dynamic colouring will not work.");
					this.materialColourCanBeUsed = false;
				}
			}
			this.mat_ValId = Shader.PropertyToID("_Val");
			this.mat.SetColor("_Color1", this.MinColor);
			this.mat.SetColor("_Color2", this.MaxColor);
		}
	}

	// Token: 0x060000AF RID: 175 RVA: 0x00042FE8 File Offset: 0x000411E8
	private void Update()
	{
		if (this.isEnabled && this.sourceType != OutputVolume.SourceType.Custom)
		{
			if (this.sourceType == OutputVolume.SourceType.AudioListener)
			{
				this.newValue = OutputVolume.GetRMS(this.sampleAmount, this.channel);
			}
			else
			{
				this.newValue = OutputVolume.GetRMS(this.audioSource, this.sampleAmount, this.channel);
			}
		}
		float num = (this.newValue > this.oldScale) ? Mathf.Lerp(this.oldScale, this.newValue, this.attackDamp) : Mathf.Lerp(this.oldScale, this.newValue, this.decayDamp);
		this.oldScale = num;
		switch (this.outputType)
		{
		case OutputVolume.OutputType.PrefabBar:
			if (this.scalePrefab)
			{
				this.barT.localScale = new Vector3(1f, num, 1f);
			}
			if (this.useColorGradient && this.materialColourCanBeUsed)
			{
				float num2 = this.colorCurve.Evaluate(num);
				if (num2 > this.oldColorVal)
				{
					if (this.colorAttackDamp != 1f)
					{
						num2 = Mathf.Lerp(this.oldColorVal, num2, this.colorAttackDamp);
					}
				}
				else if (this.colorDecayDamp != 1f)
				{
					num2 = Mathf.Lerp(this.oldColorVal, num2, this.colorDecayDamp);
				}
				this.mat.SetFloat(this.mat_ValId, num2);
				this.oldColorVal = num2;
				return;
			}
			break;
		case OutputVolume.OutputType.ObjectPosition:
			base.transform.localPosition = this.valueMultiplier * num;
			return;
		case OutputVolume.OutputType.ObjectRotation:
			base.transform.localEulerAngles = this.valueMultiplier * num;
			return;
		case OutputVolume.OutputType.ObjectScale:
		{
			float num3 = Mathf.Lerp(this.outputScaleMin, this.outputScaleMax, num);
			base.transform.localScale = new Vector3(num3, num3, num3);
			break;
		}
		default:
			return;
		}
	}

	// Token: 0x060000B0 RID: 176 RVA: 0x000431AC File Offset: 0x000413AC
	public static float GetRMS(AudioSource aSource, int sampleSize, int channelUsed = 0)
	{
		sampleSize = Mathf.ClosestPowerOfTwo(sampleSize);
		float[] array = new float[sampleSize];
		aSource.GetOutputData(array, channelUsed);
		float num = 0f;
		foreach (float num2 in array)
		{
			num += num2 * num2;
		}
		return Mathf.Sqrt(num / (float)array.Length);
	}

	// Token: 0x060000B1 RID: 177 RVA: 0x00043200 File Offset: 0x00041400
	public static float GetRMS(int sampleSize, int channelUsed = 0)
	{
		sampleSize = Mathf.ClosestPowerOfTwo(sampleSize);
		float[] array = new float[sampleSize];
		AudioListener.GetOutputData(array, channelUsed);
		float num = 0f;
		foreach (float num2 in array)
		{
			num += num2 * num2;
		}
		return Mathf.Sqrt(num / (float)array.Length);
	}

	// Token: 0x040000FF RID: 255
	[Tooltip("Enables or disables the processing and display of volume data.")]
	public bool isEnabled = true;

	// Token: 0x04000100 RID: 256
	[Tooltip("The type of source for volume data.")]
	public OutputVolume.SourceType sourceType;

	// Token: 0x04000101 RID: 257
	[Tooltip("The AudioSource to take data from.")]
	public AudioSource audioSource;

	// Token: 0x04000102 RID: 258
	[Tooltip("The number of samples to use when sampling. Must be a power of two.")]
	public int sampleAmount = 256;

	// Token: 0x04000103 RID: 259
	[Tooltip("The audio channel to take data from when sampling.")]
	public int channel;

	// Token: 0x04000104 RID: 260
	[Range(0f, 1f)]
	[Tooltip("The amount of dampening used when the new scale is higher than the bar's existing scale.")]
	public float attackDamp = 0.75f;

	// Token: 0x04000105 RID: 261
	[Range(0f, 1f)]
	[Tooltip("The amount of dampening used when the new scale is lower than the bar's existing scale.")]
	public float decayDamp = 0.25f;

	// Token: 0x04000106 RID: 262
	[Tooltip("How the volume data should be presented to the user.")]
	public OutputVolume.OutputType outputType;

	// Token: 0x04000107 RID: 263
	[Tooltip("A multiplier / mask for positioning or rotating. The volume data is multiplied by this vector, so 0 will mask that dimension out.")]
	public Vector3 valueMultiplier = new Vector3(0f, 0f, -90f);

	// Token: 0x04000108 RID: 264
	[Tooltip("The scale used when output volume is lowest (0).")]
	public float outputScaleMin = 1f;

	// Token: 0x04000109 RID: 265
	[Tooltip("The scale used when output volume is highest (1).")]
	public float outputScaleMax = 1.5f;

	// Token: 0x0400010A RID: 266
	[Tooltip("The prefab of bar to use. Use a prefab from SimpleSpectrum/Bar Prefabs or refer to the documentation to use a custom prefab.")]
	public GameObject prefab;

	// Token: 0x0400010B RID: 267
	[Tooltip("Determines whether to scale the bar prefab (i.e. disable for just colouring).")]
	public bool scalePrefab = true;

	// Token: 0x0400010C RID: 268
	[Tooltip("Determines whether to apply a color gradient on the bar.")]
	public bool useColorGradient;

	// Token: 0x0400010D RID: 269
	[Tooltip("The minimum (low value) color.")]
	public Color MinColor = Color.black;

	// Token: 0x0400010E RID: 270
	[Tooltip("The maximum (high value) color.")]
	public Color MaxColor = Color.white;

	// Token: 0x0400010F RID: 271
	[Tooltip("The curve that determines the interpolation between Color Min and Color Max")]
	public AnimationCurve colorCurve;

	// Token: 0x04000110 RID: 272
	[Range(0f, 1f)]
	[Tooltip("The amount of dampening used when the new color value is higher than the existing color value.")]
	public float colorAttackDamp = 1f;

	// Token: 0x04000111 RID: 273
	[Range(0f, 1f)]
	[Tooltip("The amount of dampening used when the new color value is lower than the existing color value.")]
	public float colorDecayDamp = 1f;

	// Token: 0x04000112 RID: 274
	private GameObject bar;

	// Token: 0x04000113 RID: 275
	private Transform barT;

	// Token: 0x04000114 RID: 276
	private float newValue;

	// Token: 0x04000115 RID: 277
	private float oldScale;

	// Token: 0x04000116 RID: 278
	private float oldColorVal;

	// Token: 0x04000117 RID: 279
	private Material mat;

	// Token: 0x04000118 RID: 280
	private int mat_ValId;

	// Token: 0x04000119 RID: 281
	private bool materialColourCanBeUsed = true;

	// Token: 0x02000043 RID: 67
	public enum SourceType
	{
		// Token: 0x0400011B RID: 283
		AudioSource,
		// Token: 0x0400011C RID: 284
		AudioListener,
		// Token: 0x0400011D RID: 285
		Custom
	}

	// Token: 0x02000044 RID: 68
	public enum OutputType
	{
		// Token: 0x0400011F RID: 287
		PrefabBar,
		// Token: 0x04000120 RID: 288
		ObjectPosition,
		// Token: 0x04000121 RID: 289
		ObjectRotation,
		// Token: 0x04000122 RID: 290
		ObjectScale
	}
}
