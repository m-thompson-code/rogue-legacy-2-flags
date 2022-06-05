using System;
using System.Collections;
using System.IO;
using UnityEngine;

// Token: 0x02000CEE RID: 3310
public class TransparentScreenShot : MonoBehaviour
{
	// Token: 0x06005E4D RID: 24141 RVA: 0x00033F84 File Offset: 0x00032184
	private void Awake()
	{
		this.mainCam = base.gameObject.GetComponent<Camera>();
		this.CreateBlackAndWhiteCameras();
		this.CacheAndInitialiseFields();
	}

	// Token: 0x06005E4E RID: 24142 RVA: 0x00160D64 File Offset: 0x0015EF64
	private string uniqueFilename(int width, int height)
	{
		if (this.folderName == null || this.folderName.Length == 0)
		{
			this.folderName = Application.dataPath;
			if (Application.isEditor)
			{
				string path = this.folderName + "/..";
				this.folderName = Path.GetFullPath(path);
			}
			this.folderName += "/screenshots";
			Directory.CreateDirectory(this.folderName);
			string searchPattern = string.Format("screen_{0}x{1}*.{2}", width, height, this.format.ToString().ToLower());
			this.counter = Directory.GetFiles(this.folderName, searchPattern, SearchOption.TopDirectoryOnly).Length;
		}
		string result = string.Format("{0}/screen_{1}x{2}_{3}.{4}", new object[]
		{
			this.folderName,
			width,
			height,
			this.counter,
			this.format.ToString().ToLower()
		});
		this.counter++;
		return result;
	}

	// Token: 0x06005E4F RID: 24143 RVA: 0x00033FA3 File Offset: 0x000321A3
	public void TakeHiResShot()
	{
		this.takeHiResShot = true;
	}

	// Token: 0x06005E50 RID: 24144 RVA: 0x00033FAC File Offset: 0x000321AC
	private void LateUpdate()
	{
		this.takeHiResShot |= Input.GetKeyDown("k");
		if (this.takeHiResShot)
		{
			Debug.Log("CAPTURE");
			base.StartCoroutine(this.CaptureFrame());
		}
	}

	// Token: 0x06005E51 RID: 24145 RVA: 0x00033FE4 File Offset: 0x000321E4
	private IEnumerator CaptureFrame()
	{
		yield return new WaitForEndOfFrame();
		this.RenderCamToTexture(this.blackCam, this.textureBlack);
		this.RenderCamToTexture(this.whiteCam, this.textureWhite);
		this.CalculateOutputTexture();
		this.SavePng();
		this.videoFrame++;
		Debug.Log("Rendered frame " + this.videoFrame.ToString());
		this.takeHiResShot = false;
		base.StopCoroutine("CaptureFrame");
		yield break;
	}

	// Token: 0x06005E52 RID: 24146 RVA: 0x00033FF3 File Offset: 0x000321F3
	private void RenderCamToTexture(Camera cam, Texture2D tex)
	{
		cam.enabled = true;
		cam.Render();
		this.WriteScreenImageToTexture(tex);
		cam.enabled = false;
	}

	// Token: 0x06005E53 RID: 24147 RVA: 0x00160E7C File Offset: 0x0015F07C
	private void CreateBlackAndWhiteCameras()
	{
		this.whiteCamGameObject = new GameObject();
		this.whiteCamGameObject.name = "White Background Camera";
		this.whiteCam = this.whiteCamGameObject.AddComponent<Camera>();
		this.whiteCam.CopyFrom(this.mainCam);
		this.whiteCam.backgroundColor = Color.white;
		this.whiteCamGameObject.transform.SetParent(base.gameObject.transform, true);
		this.blackCamGameObject = new GameObject();
		this.blackCamGameObject.name = "Black Background Camera";
		this.blackCam = this.blackCamGameObject.AddComponent<Camera>();
		this.blackCam.CopyFrom(this.mainCam);
		this.blackCam.backgroundColor = Color.black;
		this.blackCamGameObject.transform.SetParent(base.gameObject.transform, true);
	}

	// Token: 0x06005E54 RID: 24148 RVA: 0x00160F5C File Offset: 0x0015F15C
	private void CreateNewFolderForScreenshots()
	{
		this.folderName = this.folderBaseName;
		int num = 1;
		while (Directory.Exists(this.folderName))
		{
			this.folderName = this.folderBaseName + num.ToString();
			num++;
		}
		Directory.CreateDirectory(this.folderName);
	}

	// Token: 0x06005E55 RID: 24149 RVA: 0x00034010 File Offset: 0x00032210
	private void WriteScreenImageToTexture(Texture2D tex)
	{
		tex.ReadPixels(new Rect(0f, 0f, (float)this.resWidth, (float)this.resHeight), 0, 0);
		tex.Apply();
	}

	// Token: 0x06005E56 RID: 24150 RVA: 0x00160FB0 File Offset: 0x0015F1B0
	private void CalculateOutputTexture()
	{
		for (int i = 0; i < this.textureTransparentBackground.height; i++)
		{
			for (int j = 0; j < this.textureTransparentBackground.width; j++)
			{
				float num = this.textureWhite.GetPixel(j, i).r - this.textureBlack.GetPixel(j, i).r;
				num = 1f - num;
				Color color;
				if (num == 0f)
				{
					color = Color.clear;
				}
				else
				{
					color = this.textureBlack.GetPixel(j, i) / num;
				}
				color.a = num;
				this.textureTransparentBackground.SetPixel(j, i, color);
			}
		}
	}

	// Token: 0x06005E57 RID: 24151 RVA: 0x00161058 File Offset: 0x0015F258
	private void SavePng()
	{
		Debug.Log("SAVING");
		string path = this.uniqueFilename(this.resWidth, this.resHeight);
		byte[] bytes = this.textureTransparentBackground.EncodeToPNG();
		File.WriteAllBytes(path, bytes);
	}

	// Token: 0x06005E58 RID: 24152 RVA: 0x00161094 File Offset: 0x0015F294
	private void CacheAndInitialiseFields()
	{
		this.originalTimescaleTime = Time.timeScale;
		this.resWidth = Screen.width;
		this.resHeight = Screen.height;
		this.textureBlack = new Texture2D(this.resWidth, this.resHeight, TextureFormat.RGB24, false);
		this.textureWhite = new Texture2D(this.resWidth, this.resHeight, TextureFormat.RGB24, false);
		this.textureTransparentBackground = new Texture2D(this.resWidth, this.resHeight, TextureFormat.ARGB32, false);
	}

	// Token: 0x04004D71 RID: 19825
	[Tooltip("Resolution of image width")]
	public int resWidth = 3840;

	// Token: 0x04004D72 RID: 19826
	[Tooltip("Resolution of the image height")]
	public int resHeight = 2160;

	// Token: 0x04004D73 RID: 19827
	[Tooltip("Image format")]
	public TransparentScreenShot.Format format = TransparentScreenShot.Format.PNG;

	// Token: 0x04004D74 RID: 19828
	[Tooltip("A folder will be created with this base name in your project root")]
	public string folderBaseName = "screenshots";

	// Token: 0x04004D75 RID: 19829
	[Tooltip("How many frames should be captured per second of game time")]
	public int frameRate = 60;

	// Token: 0x04004D76 RID: 19830
	[Tooltip("How many frames should be captured before quitting")]
	public int framesToCapture = 1;

	// Token: 0x04004D77 RID: 19831
	private string folderName = "";

	// Token: 0x04004D78 RID: 19832
	private GameObject whiteCamGameObject;

	// Token: 0x04004D79 RID: 19833
	private Camera whiteCam;

	// Token: 0x04004D7A RID: 19834
	private GameObject blackCamGameObject;

	// Token: 0x04004D7B RID: 19835
	private Camera blackCam;

	// Token: 0x04004D7C RID: 19836
	private Camera mainCam;

	// Token: 0x04004D7D RID: 19837
	private int videoFrame;

	// Token: 0x04004D7E RID: 19838
	private float originalTimescaleTime;

	// Token: 0x04004D7F RID: 19839
	private bool done;

	// Token: 0x04004D80 RID: 19840
	private Texture2D textureBlack;

	// Token: 0x04004D81 RID: 19841
	private Texture2D textureWhite;

	// Token: 0x04004D82 RID: 19842
	private Texture2D textureTransparentBackground;

	// Token: 0x04004D83 RID: 19843
	private bool takeHiResShot;

	// Token: 0x04004D84 RID: 19844
	private int counter;

	// Token: 0x02000CEF RID: 3311
	public enum Format
	{
		// Token: 0x04004D86 RID: 19846
		RAW,
		// Token: 0x04004D87 RID: 19847
		JPG,
		// Token: 0x04004D88 RID: 19848
		PNG,
		// Token: 0x04004D89 RID: 19849
		PPM
	}
}
