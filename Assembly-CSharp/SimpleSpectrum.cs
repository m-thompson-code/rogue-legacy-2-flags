using System;
using FMOD;
using FMODUnity;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

// Token: 0x02000040 RID: 64
public class SimpleSpectrum : MonoBehaviour
{
	// Token: 0x17000004 RID: 4
	// (get) Token: 0x060000AB RID: 171 RVA: 0x000066A1 File Offset: 0x000048A1
	// (set) Token: 0x060000AC RID: 172 RVA: 0x000066A9 File Offset: 0x000048A9
	public float[] spectrumInputData
	{
		get
		{
			return this.spectrum;
		}
		set
		{
			if (this.sourceType == SimpleSpectrum.SourceType.Custom)
			{
				this.spectrum = value;
				return;
			}
			UnityEngine.Debug.LogError("Error from SimpleSpectrum: spectrumInputData cannot be set while sourceType is not Custom.");
		}
	}

	// Token: 0x17000005 RID: 5
	// (get) Token: 0x060000AD RID: 173 RVA: 0x000066C6 File Offset: 0x000048C6
	public float[] spectrumOutputData
	{
		get
		{
			return this.oldYScales;
		}
	}

	// Token: 0x060000AE RID: 174 RVA: 0x000066CE File Offset: 0x000048CE
	private void Start()
	{
		if (this.audioSource == null && this.sourceType == SimpleSpectrum.SourceType.AudioSource)
		{
			UnityEngine.Debug.LogError("An audio source has not been assigned. Please assign a reference to a source, or set useAudioListener instead.");
		}
		this.RebuildSpectrum();
	}

	// Token: 0x060000AF RID: 175 RVA: 0x000066F8 File Offset: 0x000048F8
	public void RebuildSpectrum()
	{
		this.isEnabled = false;
		int childCount = base.transform.childCount;
		for (int i = 0; i < childCount; i++)
		{
			UnityEngine.Object.Destroy(base.transform.GetChild(i).gameObject);
		}
		this.numSamples = Mathf.ClosestPowerOfTwo(this.numSamples);
		this.spectrum = new float[this.numSamples];
		this.bars = new Transform[this.barAmount];
		this.barMaterials = new Material[this.barAmount];
		this.oldYScales = new float[this.barAmount];
		this.oldColorValues = new float[this.barAmount];
		this.materialColourCanBeUsed = true;
		float num = (float)this.barAmount * (1f + this.barXSpacing);
		float num2 = num / 2f;
		float num3 = 0f;
		float num4 = 0f;
		float num5 = 0f;
		float num6 = 0f;
		Vector3 zero = Vector3.zero;
		if (this.barCurveAngle > 0f)
		{
			num3 = this.barCurveAngle / 360f * 6.2831855f;
			num4 = num / num3;
			num5 = num3 / 2f;
			num6 = this.barCurveAngle / 2f;
			zero = new Vector3(0f, 0f, 1f * -num4);
			if (this.barCurveAngle == 360f)
			{
				zero = new Vector3(0f, 0f, 0f);
			}
		}
		for (int j = 0; j < this.barAmount; j++)
		{
			GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.barPrefab, base.transform, false);
			gameObject.transform.localScale = new Vector3(this.barXScale, this.barMinYScale, 1f);
			if (this.barCurveAngle > 0f)
			{
				float num7 = (float)j / (float)this.barAmount;
				float f = num7 * num3 - num5;
				float y = num7 * this.barCurveAngle - num6;
				gameObject.transform.localPosition = new Vector3(Mathf.Sin(f) * num4, 0f, Mathf.Cos(f) * num4) + zero;
				gameObject.transform.localRotation = Quaternion.Euler(this.barXRotation, y, 0f);
			}
			else
			{
				gameObject.transform.localPosition = new Vector3((float)j * (1f + this.barXSpacing) - num2, 0f, 0f);
			}
			this.bars[j] = gameObject.transform;
			Renderer component = gameObject.transform.GetChild(0).GetComponent<Renderer>();
			if (component != null)
			{
				this.barMaterials[j] = component.material;
			}
			else
			{
				Image component2 = gameObject.transform.GetChild(0).GetComponent<Image>();
				if (component2 != null)
				{
					component2.material = new Material(component2.material);
					this.barMaterials[j] = component2.material;
				}
				else if (this.materialColourCanBeUsed)
				{
					UnityEngine.Debug.LogWarning("Warning from SimpleSpectrum: The Bar Prefab you're using doesn't have a Renderer or Image component as its first child. Dynamic colouring will not work.");
					this.materialColourCanBeUsed = false;
				}
			}
			int nameID = Shader.PropertyToID("_Color1");
			int nameID2 = Shader.PropertyToID("_Color2");
			if (!this.useRainbowGradient)
			{
				this.barMaterials[j].SetColor(nameID, this.colorMin);
				this.barMaterials[j].SetColor(nameID2, this.colorMax);
			}
			else
			{
				int count = Song_EV.SpectrumAnalysisGradientList.Count;
				float num8 = Mathf.Lerp(0f, (float)(count - 1), (float)j / (float)(this.barAmount - 1));
				int num9 = (int)num8;
				float t = num8 - (float)num9;
				Color value;
				if (num9 == count - 1)
				{
					value = Song_EV.SpectrumAnalysisGradientList[num9];
				}
				else
				{
					value = Color.Lerp(Song_EV.SpectrumAnalysisGradientList[num9], Song_EV.SpectrumAnalysisGradientList[num9 + 1], t);
				}
				this.barMaterials[j].SetColor(nameID, value);
				this.barMaterials[j].SetColor(nameID2, value);
			}
		}
		this.materialValId = Shader.PropertyToID("_Val");
		this.highestLogFreq = Mathf.Log((float)(this.barAmount + 1), 2f);
		int num10;
		SPEAKERMODE speakermode;
		int num11;
		RuntimeManager.CoreSystem.getSoftwareFormat(out num10, out speakermode, out num11);
		this.frequencyScaleFactor = 1f / (float)(num10 / 2) * (float)this.numSamples;
		this.isEnabled = true;
	}

	// Token: 0x060000B0 RID: 176 RVA: 0x00006B3C File Offset: 0x00004D3C
	public void RestartMicrophone()
	{
		Microphone.End(this.microphoneName);
		if (this.sourceType == SimpleSpectrum.SourceType.MicrophoneInput || this.sourceType == SimpleSpectrum.SourceType.StereoMix)
		{
			this.audioSource = base.GetComponent<AudioSource>();
			if (this.audioSource == null)
			{
				this.audioSource = base.gameObject.AddComponent<AudioSource>();
			}
			if (Microphone.devices.Length == 0)
			{
				UnityEngine.Debug.LogError("Error from SimpleSpectrum: Microphone or Stereo Mix is being used, but no Microphones are found!");
			}
			this.microphoneName = null;
			if (this.sourceType == SimpleSpectrum.SourceType.StereoMix)
			{
				foreach (string text in Microphone.devices)
				{
					if (text.StartsWith("Stereo Mix"))
					{
						this.microphoneName = text;
					}
				}
				if (this.microphoneName == null)
				{
					UnityEngine.Debug.LogError("Error from SimpleSpectrum: Stereo Mix not found. Reverting to default microphone.");
				}
			}
			this.audioSource.loop = true;
			this.audioSource.outputAudioMixerGroup = this.muteGroup;
			AudioClip clip = this.audioSource.clip = Microphone.Start(this.microphoneName, true, 5, 44100);
			this.audioSource.clip = clip;
			while (Microphone.GetPosition(this.microphoneName) <= 0)
			{
			}
			this.audioSource.Play();
			this.lastMicRestartTime = Time.unscaledTime;
			return;
		}
		UnityEngine.Object.Destroy(base.GetComponent<AudioSource>());
	}

	// Token: 0x060000B1 RID: 177 RVA: 0x00006C70 File Offset: 0x00004E70
	private void Update()
	{
		if (this.isEnabled)
		{
			if (this.sourceType != SimpleSpectrum.SourceType.Custom)
			{
				if (this.sourceType == SimpleSpectrum.SourceType.AudioListener)
				{
					AudioListener.GetSpectrumData(this.spectrum, this.sampleChannel, this.windowUsed);
				}
				else
				{
					this.audioSource.GetSpectrumData(this.spectrum, this.sampleChannel, this.windowUsed);
				}
			}
			float b = this.frequencyLimitHigh;
			for (int i = 0; i < this.bars.Length; i++)
			{
				Transform transform = this.bars[i];
				float num;
				if (this.useLogarithmicFrequency)
				{
					num = Mathf.Lerp(this.frequencyLimitLow, b, (this.highestLogFreq - Mathf.Log((float)(this.barAmount + 1 - i), 2f)) / this.highestLogFreq) * this.frequencyScaleFactor;
				}
				else
				{
					num = Mathf.Lerp(this.frequencyLimitLow, b, (float)i / (float)this.barAmount) * this.frequencyScaleFactor;
				}
				int num2 = Mathf.FloorToInt(num);
				num2 = Mathf.Clamp(num2, 0, this.spectrum.Length - 2);
				float num3 = Mathf.SmoothStep(this.spectrum[num2], this.spectrum[num2 + 1], num - (float)num2);
				if (this.multiplyByFrequency)
				{
					num3 *= num + 1f;
				}
				num3 = Mathf.Sqrt(num3);
				float num4 = this.oldYScales[i];
				float num5;
				if (num3 * this.barYScale > num4)
				{
					num5 = Mathf.Lerp(num4, Mathf.Max(num3 * this.barYScale, this.barMinYScale), this.attackDamp);
				}
				else
				{
					num5 = Mathf.Lerp(num4, Mathf.Max(num3 * this.barYScale, this.barMinYScale), this.decayDamp);
				}
				transform.localScale = new Vector3(this.barXScale, num5, 1f);
				this.oldYScales[i] = num5;
				if (this.useColorGradient && this.materialColourCanBeUsed)
				{
					float num6 = this.colorValueCurve.Evaluate(num3);
					float num7 = this.oldColorValues[i];
					if (num6 > num7)
					{
						if (this.colorAttackDamp != 1f)
						{
							num6 = Mathf.Lerp(num7, num6, this.colorAttackDamp);
						}
					}
					else if (this.colorDecayDamp != 1f)
					{
						num6 = Mathf.Lerp(num7, num6, this.colorDecayDamp);
					}
					this.barMaterials[i].SetFloat(this.materialValId, num6);
					this.oldColorValues[i] = num6;
				}
			}
			return;
		}
		foreach (Transform transform2 in this.bars)
		{
			transform2.localScale = Vector3.Lerp(transform2.localScale, new Vector3(1f, this.barMinYScale, 1f), this.decayDamp);
		}
	}

	// Token: 0x060000B2 RID: 178 RVA: 0x00006F04 File Offset: 0x00005104
	public static float[] GetLogarithmicSpectrumData(AudioSource source, int spectrumSize, int sampleSize, FFTWindow windowUsed = FFTWindow.BlackmanHarris, int channelUsed = 0)
	{
		float[] array = new float[spectrumSize];
		channelUsed = Mathf.Clamp(channelUsed, 0, 1);
		float[] array2 = new float[Mathf.ClosestPowerOfTwo(sampleSize)];
		source.GetSpectrumData(array2, channelUsed, windowUsed);
		float num = Mathf.Log((float)(array.Length + 1), 2f);
		float num2 = (float)sampleSize / num;
		for (int i = 0; i < array.Length; i++)
		{
			float num3 = (num - Mathf.Log((float)(array.Length + 1 - i), 2f)) * num2;
			int num4 = Mathf.FloorToInt(num3);
			num4 = Mathf.Clamp(num4, 0, array2.Length - 2);
			float num5 = Mathf.SmoothStep(array[num4], array[num4 + 1], num3 - (float)num4);
			num5 *= num3;
			num5 = Mathf.Sqrt(num5);
			array[i] = num5;
		}
		return array;
	}

	// Token: 0x060000B3 RID: 179 RVA: 0x00006FC0 File Offset: 0x000051C0
	public static float[] GetLogarithmicSpectrumData(int spectrumSize, int sampleSize, FFTWindow windowUsed = FFTWindow.BlackmanHarris, int channelUsed = 0)
	{
		float[] array = new float[spectrumSize];
		channelUsed = Mathf.Clamp(channelUsed, 0, 1);
		float[] array2 = new float[Mathf.ClosestPowerOfTwo(sampleSize)];
		AudioListener.GetSpectrumData(array2, channelUsed, windowUsed);
		float num = Mathf.Log((float)(array.Length + 1), 2f);
		float num2 = (float)sampleSize / num;
		for (int i = 0; i < array.Length; i++)
		{
			float num3 = (num - Mathf.Log((float)(array.Length + 1 - i), 2f)) * num2;
			int num4 = Mathf.FloorToInt(num3);
			num4 = Mathf.Clamp(num4, 0, array2.Length - 2);
			float num5 = Mathf.SmoothStep(array[num4], array[num4 + 1], num3 - (float)num4);
			num5 *= num3 + 1f;
			num5 = Mathf.Sqrt(num5);
			array[i] = num5;
		}
		return array;
	}

	// Token: 0x04000111 RID: 273
	[SerializeField]
	public AudioMixerGroup muteGroup;

	// Token: 0x04000112 RID: 274
	[Tooltip("Enables or disables the processing and display of spectrum data. ")]
	public bool isEnabled = true;

	// Token: 0x04000113 RID: 275
	[Tooltip("The type of source for spectrum data.")]
	public SimpleSpectrum.SourceType sourceType;

	// Token: 0x04000114 RID: 276
	[Tooltip("The AudioSource to take data from.")]
	public AudioSource audioSource;

	// Token: 0x04000115 RID: 277
	[Tooltip("The audio channel to use when sampling.")]
	public int sampleChannel;

	// Token: 0x04000116 RID: 278
	[Tooltip("The number of samples to use when sampling. Must be a power of two.")]
	public int numSamples = 256;

	// Token: 0x04000117 RID: 279
	[Tooltip("The FFTWindow to use when sampling.")]
	public FFTWindow windowUsed = FFTWindow.BlackmanHarris;

	// Token: 0x04000118 RID: 280
	[Tooltip("If true, audio data is scaled logarithmically.")]
	public bool useLogarithmicFrequency = true;

	// Token: 0x04000119 RID: 281
	[Tooltip("If true, the values of the spectrum are multiplied based on their frequency, to keep the values proportionate.")]
	public bool multiplyByFrequency = true;

	// Token: 0x0400011A RID: 282
	[Tooltip("The lower bound of the freuqnecy range to sample from. Leave at 0 when unused.")]
	public float frequencyLimitLow;

	// Token: 0x0400011B RID: 283
	[Tooltip("The upper bound of the freuqnecy range to sample from. Leave at 22050 (44100/2) when unused.")]
	public float frequencyLimitHigh = 22050f;

	// Token: 0x0400011C RID: 284
	[Tooltip("The amount of bars to use. Does not have to be equal to Num Samples, but probably should be lower.")]
	public int barAmount = 32;

	// Token: 0x0400011D RID: 285
	[Tooltip("Stretches the values of the bars.")]
	public float barYScale = 50f;

	// Token: 0x0400011E RID: 286
	[Tooltip("Sets a minimum scale for the bars.")]
	public float barMinYScale = 0.1f;

	// Token: 0x0400011F RID: 287
	[Tooltip("The prefab of bar to use when building. Choose one from SimpleSpectrum/Bar Prefabs, or refer to the documentation to use a custom prefab.")]
	public GameObject barPrefab;

	// Token: 0x04000120 RID: 288
	[Tooltip("Stretches the bars sideways.")]
	public float barXScale = 1f;

	// Token: 0x04000121 RID: 289
	[Tooltip("Increases the spacing between bars.")]
	public float barXSpacing;

	// Token: 0x04000122 RID: 290
	[Range(0f, 360f)]
	[Tooltip("Bends the Spectrum using a given angle. Set to 360 for a circle.")]
	public float barCurveAngle;

	// Token: 0x04000123 RID: 291
	[Tooltip("Rotates the Spectrum inwards or outwards. Especially useful when using barCurveAngle.")]
	public float barXRotation;

	// Token: 0x04000124 RID: 292
	[Range(0f, 1f)]
	[Tooltip("The amount of dampening used when the new scale is higher than the bar's existing scale.")]
	public float attackDamp = 0.3f;

	// Token: 0x04000125 RID: 293
	[Range(0f, 1f)]
	[Tooltip("The amount of dampening used when the new scale is lower than the bar's existing scale.")]
	public float decayDamp = 0.15f;

	// Token: 0x04000126 RID: 294
	[Tooltip("Determines whether to apply a color gradient on the bars, or just use a solid color.")]
	public bool useColorGradient;

	// Token: 0x04000127 RID: 295
	[Tooltip("Special colour gradient added for RL2.")]
	public bool useRainbowGradient;

	// Token: 0x04000128 RID: 296
	[Tooltip("The minimum (low value) color if useColorGradient is true, else the solid color to use.")]
	public Color colorMin = Color.black;

	// Token: 0x04000129 RID: 297
	[Tooltip("The maximum (high value) color.")]
	public Color colorMax = Color.white;

	// Token: 0x0400012A RID: 298
	[Tooltip("The curve that determines the interpolation between colorMin and colorMax.")]
	public AnimationCurve colorValueCurve = new AnimationCurve(new Keyframe[]
	{
		new Keyframe(0f, 0f),
		new Keyframe(1f, 1f)
	});

	// Token: 0x0400012B RID: 299
	[Range(0f, 1f)]
	[Tooltip("The amount of dampening used when the new color value is higher than the existing color value.")]
	public float colorAttackDamp = 1f;

	// Token: 0x0400012C RID: 300
	[Range(0f, 1f)]
	[Tooltip("The amount of dampening used when the new color value is lower than the existing color value.")]
	public float colorDecayDamp = 1f;

	// Token: 0x0400012D RID: 301
	private float[] spectrum;

	// Token: 0x0400012E RID: 302
	private Transform[] bars;

	// Token: 0x0400012F RID: 303
	private Material[] barMaterials;

	// Token: 0x04000130 RID: 304
	private float[] oldYScales;

	// Token: 0x04000131 RID: 305
	private float[] oldColorValues;

	// Token: 0x04000132 RID: 306
	private int materialValId;

	// Token: 0x04000133 RID: 307
	private bool materialColourCanBeUsed = true;

	// Token: 0x04000134 RID: 308
	private float highestLogFreq;

	// Token: 0x04000135 RID: 309
	private float frequencyScaleFactor;

	// Token: 0x04000136 RID: 310
	private string microphoneName;

	// Token: 0x04000137 RID: 311
	private float lastMicRestartTime;

	// Token: 0x04000138 RID: 312
	private float micRestartWait = 20f;

	// Token: 0x0200097E RID: 2430
	public enum SourceType
	{
		// Token: 0x040044A8 RID: 17576
		AudioSource,
		// Token: 0x040044A9 RID: 17577
		AudioListener,
		// Token: 0x040044AA RID: 17578
		MicrophoneInput,
		// Token: 0x040044AB RID: 17579
		StereoMix,
		// Token: 0x040044AC RID: 17580
		Custom
	}
}
