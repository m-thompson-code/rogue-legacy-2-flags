using System;
using System.Collections;
using System.IO;
using UnityEngine;

// Token: 0x0200081A RID: 2074
public class TransparentScreenShot : MonoBehaviour
{
	// Token: 0x0600448D RID: 17549 RVA: 0x000F3342 File Offset: 0x000F1542
	private void Awake()
	{
		this.mainCam = base.gameObject.GetComponent<Camera>();
		this.CreateBlackAndWhiteCameras();
		this.CacheAndInitialiseFields();
	}

	// Token: 0x0600448E RID: 17550 RVA: 0x000F3364 File Offset: 0x000F1564
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

	// Token: 0x0600448F RID: 17551 RVA: 0x000F347C File Offset: 0x000F167C
	public void TakeHiResShot()
	{
		this.takeHiResShot = true;
	}

	// Token: 0x06004490 RID: 17552 RVA: 0x000F3485 File Offset: 0x000F1685
	private void LateUpdate()
	{
		this.takeHiResShot |= Input.GetKeyDown("k");
		if (this.takeHiResShot)
		{
			Debug.Log("CAPTURE");
			base.StartCoroutine(this.CaptureFrame());
		}
	}

	// Token: 0x06004491 RID: 17553 RVA: 0x000F34BD File Offset: 0x000F16BD
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

	// Token: 0x06004492 RID: 17554 RVA: 0x000F34CC File Offset: 0x000F16CC
	private void RenderCamToTexture(Camera cam, Texture2D tex)
	{
		cam.enabled = true;
		cam.Render();
		this.WriteScreenImageToTexture(tex);
		cam.enabled = false;
	}

	// Token: 0x06004493 RID: 17555 RVA: 0x000F34EC File Offset: 0x000F16EC
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

	// Token: 0x06004494 RID: 17556 RVA: 0x000F35CC File Offset: 0x000F17CC
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

	// Token: 0x06004495 RID: 17557 RVA: 0x000F361E File Offset: 0x000F181E
	private void WriteScreenImageToTexture(Texture2D tex)
	{
		tex.ReadPixels(new Rect(0f, 0f, (float)this.resWidth, (float)this.resHeight), 0, 0);
		tex.Apply();
	}

	// Token: 0x06004496 RID: 17558 RVA: 0x000F364C File Offset: 0x000F184C
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

	// Token: 0x06004497 RID: 17559 RVA: 0x000F36F4 File Offset: 0x000F18F4
	private void SavePng()
	{
		Debug.Log("SAVING");
		string path = this.uniqueFilename(this.resWidth, this.resHeight);
		byte[] bytes = this.textureTransparentBackground.EncodeToPNG();
		File.WriteAllBytes(path, bytes);
	}

	// Token: 0x06004498 RID: 17560 RVA: 0x000F3730 File Offset: 0x000F1930
	private void CacheAndInitialiseFields()
	{
		this.originalTimescaleTime = Time.timeScale;
		this.resWidth = Screen.width;
		this.resHeight = Screen.height;
		this.textureBlack = new Texture2D(this.resWidth, this.resHeight, TextureFormat.RGB24, false);
		this.textureWhite = new Texture2D(this.resWidth, this.resHeight, TextureFormat.RGB24, false);
		this.textureTransparentBackground = new Texture2D(this.resWidth, this.resHeight, TextureFormat.ARGB32, false);
	}

	// Token: 0x04003A79 RID: 14969
	[Tooltip("Resolution of image width")]
	public int resWidth = 3840;

	// Token: 0x04003A7A RID: 14970
	[Tooltip("Resolution of the image height")]
	public int resHeight = 2160;

	// Token: 0x04003A7B RID: 14971
	[Tooltip("Image format")]
	public TransparentScreenShot.Format format = TransparentScreenShot.Format.PNG;

	// Token: 0x04003A7C RID: 14972
	[Tooltip("A folder will be created with this base name in your project root")]
	public string folderBaseName = "screenshots";

	// Token: 0x04003A7D RID: 14973
	[Tooltip("How many frames should be captured per second of game time")]
	public int frameRate = 60;

	// Token: 0x04003A7E RID: 14974
	[Tooltip("How many frames should be captured before quitting")]
	public int framesToCapture = 1;

	// Token: 0x04003A7F RID: 14975
	private string folderName = "";

	// Token: 0x04003A80 RID: 14976
	private GameObject whiteCamGameObject;

	// Token: 0x04003A81 RID: 14977
	private Camera whiteCam;

	// Token: 0x04003A82 RID: 14978
	private GameObject blackCamGameObject;

	// Token: 0x04003A83 RID: 14979
	private Camera blackCam;

	// Token: 0x04003A84 RID: 14980
	private Camera mainCam;

	// Token: 0x04003A85 RID: 14981
	private int videoFrame;

	// Token: 0x04003A86 RID: 14982
	private float originalTimescaleTime;

	// Token: 0x04003A87 RID: 14983
	private bool done;

	// Token: 0x04003A88 RID: 14984
	private Texture2D textureBlack;

	// Token: 0x04003A89 RID: 14985
	private Texture2D textureWhite;

	// Token: 0x04003A8A RID: 14986
	private Texture2D textureTransparentBackground;

	// Token: 0x04003A8B RID: 14987
	private bool takeHiResShot;

	// Token: 0x04003A8C RID: 14988
	private int counter;

	// Token: 0x02000E46 RID: 3654
	public enum Format
	{
		// Token: 0x0400576F RID: 22383
		RAW,
		// Token: 0x04005770 RID: 22384
		JPG,
		// Token: 0x04005771 RID: 22385
		PNG,
		// Token: 0x04005772 RID: 22386
		PPM
	}
}
