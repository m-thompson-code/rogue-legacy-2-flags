using System;
using System.Runtime.InteropServices;
using FMOD;
using FMODUnity;
using UnityEngine;

// Token: 0x020003B6 RID: 950
public class JukeboxSpectrumAnalyzer : MonoBehaviour
{
	// Token: 0x06002322 RID: 8994 RVA: 0x000725C7 File Offset: 0x000707C7
	private void Start()
	{
		this.InitializeSpectrum();
	}

	// Token: 0x06002323 RID: 8995 RVA: 0x000725D0 File Offset: 0x000707D0
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

	// Token: 0x06002324 RID: 8996 RVA: 0x00072639 File Offset: 0x00070839
	public void StartSpectrum()
	{
		if (this.m_isPlaying)
		{
			return;
		}
		this.m_isPlaying = true;
	}

	// Token: 0x06002325 RID: 8997 RVA: 0x0007264C File Offset: 0x0007084C
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

	// Token: 0x06002326 RID: 8998 RVA: 0x000726A8 File Offset: 0x000708A8
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

	// Token: 0x04001E00 RID: 7680
	[SerializeField]
	private SimpleSpectrum m_spectrumAnalyzer;

	// Token: 0x04001E01 RID: 7681
	private DSP m_fft;

	// Token: 0x04001E02 RID: 7682
	private bool m_spectrumInitialized;

	// Token: 0x04001E03 RID: 7683
	private bool m_isPlaying;
}
