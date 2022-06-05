using System;
using System.Runtime.InteropServices;
using FMOD;
using FMODUnity;
using UnityEngine;

// Token: 0x0200064C RID: 1612
public class JukeboxSpectrumAnalyzer : MonoBehaviour
{
	// Token: 0x0600313A RID: 12602 RVA: 0x0001B04E File Offset: 0x0001924E
	private void Start()
	{
		this.InitializeSpectrum();
	}

	// Token: 0x0600313B RID: 12603 RVA: 0x000D2BD8 File Offset: 0x000D0DD8
	private void InitializeSpectrum()
	{
		RuntimeManager.CoreSystem.createDSPByType(DSP_TYPE.FFT, out this.m_fft);
		this.m_fft.setParameterInt(1, 3);
		this.m_fft.setParameterInt(0, 256);
		ChannelGroup channelGroup;
		RuntimeManager.CoreSystem.getMasterChannelGroup(out channelGroup);
		channelGroup.addDSP(-1, this.m_fft);
		this.m_spectrumInitialized = true;
	}

	// Token: 0x0600313C RID: 12604 RVA: 0x0001B056 File Offset: 0x00019256
	public void StartSpectrum()
	{
		if (this.m_isPlaying)
		{
			return;
		}
		this.m_isPlaying = true;
	}

	// Token: 0x0600313D RID: 12605 RVA: 0x000D2C44 File Offset: 0x000D0E44
	public void StopSpectrum()
	{
		if (!this.m_isPlaying)
		{
			return;
		}
		if (this.m_spectrumAnalyzer && this.m_spectrumInitialized)
		{
			for (int i = 0; i < this.m_spectrumAnalyzer.spectrumInputData.Length; i++)
			{
				this.m_spectrumAnalyzer.spectrumInputData[i] = 0f;
			}
		}
		this.m_isPlaying = false;
	}

	// Token: 0x0600313E RID: 12606 RVA: 0x000D2CA0 File Offset: 0x000D0EA0
	private void Update()
	{
		if (this.m_spectrumAnalyzer && this.m_spectrumInitialized && this.m_isPlaying)
		{
			IntPtr ptr;
			uint num;
			this.m_fft.getParameterData(2, out ptr, out num);
			float[][] spectrum = ((DSP_PARAMETER_FFT)Marshal.PtrToStructure(ptr, typeof(DSP_PARAMETER_FFT))).spectrum;
			if (spectrum.Length != 0)
			{
				this.m_spectrumAnalyzer.spectrumInputData = spectrum[0];
			}
		}
	}

	// Token: 0x0400282D RID: 10285
	[SerializeField]
	private SimpleSpectrum m_spectrumAnalyzer;

	// Token: 0x0400282E RID: 10286
	private DSP m_fft;

	// Token: 0x0400282F RID: 10287
	private bool m_spectrumInitialized;

	// Token: 0x04002830 RID: 10288
	private bool m_isPlaying;
}
