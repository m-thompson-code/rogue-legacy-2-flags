using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

// Token: 0x0200080E RID: 2062
public static class SaveFileSystem
{
	// Token: 0x0600442C RID: 17452 RVA: 0x000F1486 File Offset: 0x000EF686
	public static void Initialize()
	{
		SaveFileSystem.m_saveFileSystem = new SaveFileSystem.PCSaveFileSystem();
		SaveFileSystem.m_saveBatchPool = new List<SaveFileSystem.SaveBatch>(2)
		{
			new SaveFileSystem.SaveBatch(),
			new SaveFileSystem.SaveBatch()
		};
	}

	// Token: 0x170016F4 RID: 5876
	// (get) Token: 0x0600442D RID: 17453 RVA: 0x000F14B3 File Offset: 0x000EF6B3
	public static string PersistentDataPath
	{
		get
		{
			return SaveFileSystem.m_saveFileSystem.PersistentDataPath;
		}
	}

	// Token: 0x170016F5 RID: 5877
	// (get) Token: 0x0600442E RID: 17454 RVA: 0x000F14BF File Offset: 0x000EF6BF
	public static string BackupFolderName
	{
		get
		{
			return SaveFileSystem.m_saveFileSystem.BackupFolderName;
		}
	}

	// Token: 0x170016F6 RID: 5878
	// (get) Token: 0x0600442F RID: 17455 RVA: 0x000F14CB File Offset: 0x000EF6CB
	public static bool IsSaving
	{
		get
		{
			return SaveFileSystem.m_totalBatchRefCount > 0;
		}
	}

	// Token: 0x06004430 RID: 17456 RVA: 0x000F14D5 File Offset: 0x000EF6D5
	public static void MountSaveDirectory()
	{
		SaveFileSystem.m_saveFileSystem.MountSaveDirectory();
	}

	// Token: 0x06004431 RID: 17457 RVA: 0x000F14E1 File Offset: 0x000EF6E1
	public static bool DirectoryExists(string path)
	{
		return SaveFileSystem.m_saveFileSystem.DirectoryExists(path);
	}

	// Token: 0x06004432 RID: 17458 RVA: 0x000F14EE File Offset: 0x000EF6EE
	public static void CreateDirectory(string path)
	{
		SaveFileSystem.m_saveFileSystem.CreateDirectory(path);
	}

	// Token: 0x06004433 RID: 17459 RVA: 0x000F14FB File Offset: 0x000EF6FB
	public static List<string> GetBackupFilesInDirectory(string path)
	{
		return SaveFileSystem.m_saveFileSystem.GetBackupFilesInDirectory(path);
	}

	// Token: 0x06004434 RID: 17460 RVA: 0x000F1508 File Offset: 0x000EF708
	public static bool FileExists(string path)
	{
		return SaveFileSystem.m_saveFileSystem.FileExists(path);
	}

	// Token: 0x06004435 RID: 17461 RVA: 0x000F1515 File Offset: 0x000EF715
	public static byte[] ReadAllBytes(string path)
	{
		return SaveFileSystem.m_saveFileSystem.ReadAllBytes(path);
	}

	// Token: 0x06004436 RID: 17462 RVA: 0x000F1524 File Offset: 0x000EF724
	public static SaveFileSystem.SaveBatch BeginSaveBatch(int profile)
	{
		foreach (SaveFileSystem.SaveBatch saveBatch in SaveFileSystem.m_saveBatchPool)
		{
			if (saveBatch.IsAvailable)
			{
				saveBatch.Initialize(profile);
				return saveBatch;
			}
		}
		SaveFileSystem.SaveBatch saveBatch2 = new SaveFileSystem.SaveBatch();
		SaveFileSystem.m_saveBatchPool.Add(saveBatch2);
		saveBatch2.Initialize(profile);
		return saveBatch2;
	}

	// Token: 0x06004437 RID: 17463 RVA: 0x000F15A0 File Offset: 0x000EF7A0
	private static void SubmitBatchUpdates(SaveFileSystem.SaveBatch batch)
	{
		SaveFileSystem.m_saveFileSystem.SubmitBatchUpdates(batch);
	}

	// Token: 0x06004438 RID: 17464 RVA: 0x000F15AD File Offset: 0x000EF7AD
	public static void WriteAllBytes(SaveFileSystem.SaveBatch batch, string path, byte[] bytes)
	{
		batch.IncrementRefCount();
		SaveFileSystem.m_saveFileSystem.WriteAllBytes(batch, path, bytes);
		batch.DecrementRefCount();
	}

	// Token: 0x06004439 RID: 17465 RVA: 0x000F15C8 File Offset: 0x000EF7C8
	public static void DeleteFile(SaveFileSystem.SaveBatch batch, string path)
	{
		batch.IncrementRefCount();
		SaveFileSystem.m_saveFileSystem.DeleteFile(batch, path);
		batch.DecrementRefCount();
	}

	// Token: 0x04003A45 RID: 14917
	public const string SAVE_FILE_EXT = ".rc2dat";

	// Token: 0x04003A46 RID: 14918
	private static SaveFileSystem.ISaveFileSystem m_saveFileSystem;

	// Token: 0x04003A47 RID: 14919
	private static List<SaveFileSystem.SaveBatch> m_saveBatchPool;

	// Token: 0x04003A48 RID: 14920
	private static int m_totalBatchRefCount;

	// Token: 0x02000E3D RID: 3645
	public struct FileUpdateCommand
	{
		// Token: 0x06006BD5 RID: 27605 RVA: 0x00192D45 File Offset: 0x00190F45
		public FileUpdateCommand(int profile, string filename, byte[] bytes)
		{
			this.Profile = profile;
			this.BlobName = filename;
			this.Bytes = bytes;
		}

		// Token: 0x04005752 RID: 22354
		public int Profile;

		// Token: 0x04005753 RID: 22355
		public string BlobName;

		// Token: 0x04005754 RID: 22356
		public byte[] Bytes;
	}

	// Token: 0x02000E3E RID: 3646
	public class SaveBatch
	{
		// Token: 0x1700232B RID: 9003
		// (get) Token: 0x06006BD6 RID: 27606 RVA: 0x00192D5C File Offset: 0x00190F5C
		// (set) Token: 0x06006BD7 RID: 27607 RVA: 0x00192D64 File Offset: 0x00190F64
		public bool IsAvailable { get; private set; }

		// Token: 0x1700232C RID: 9004
		// (get) Token: 0x06006BD8 RID: 27608 RVA: 0x00192D6D File Offset: 0x00190F6D
		// (set) Token: 0x06006BD9 RID: 27609 RVA: 0x00192D75 File Offset: 0x00190F75
		public int RefCount { get; private set; }

		// Token: 0x06006BDA RID: 27610 RVA: 0x00192D7E File Offset: 0x00190F7E
		public SaveBatch()
		{
			this.m_batchEnded = false;
			this.m_didAnyWork = false;
			this.Profile = -1;
			this.RefCount = 0;
			this.CommandList = null;
			this.IsAvailable = true;
		}

		// Token: 0x06006BDB RID: 27611 RVA: 0x00192DB0 File Offset: 0x00190FB0
		public void Initialize(int profile)
		{
			this.m_batchEnded = false;
			this.m_didAnyWork = false;
			this.Profile = profile;
			this.RefCount = 0;
			if (this.CommandList != null)
			{
				this.CommandList.Clear();
			}
			this.IsAvailable = false;
			SaveFileSystem.m_totalBatchRefCount++;
		}

		// Token: 0x06006BDC RID: 27612 RVA: 0x00192DFF File Offset: 0x00190FFF
		public void End()
		{
			this.m_batchEnded = true;
			SaveFileSystem.m_totalBatchRefCount--;
			if (this.RefCount == 0)
			{
				if (this.m_didAnyWork)
				{
					SaveFileSystem.SubmitBatchUpdates(this);
				}
				this.IsAvailable = true;
			}
		}

		// Token: 0x06006BDD RID: 27613 RVA: 0x00192E34 File Offset: 0x00191034
		public void IncrementRefCount()
		{
			int refCount = this.RefCount;
			this.RefCount = refCount + 1;
			SaveFileSystem.m_totalBatchRefCount++;
			this.m_didAnyWork = true;
		}

		// Token: 0x06006BDE RID: 27614 RVA: 0x00192E64 File Offset: 0x00191064
		public void DecrementRefCount()
		{
			int refCount = this.RefCount;
			this.RefCount = refCount - 1;
			SaveFileSystem.m_totalBatchRefCount--;
			if (this.m_batchEnded && this.RefCount == 0)
			{
				if (this.m_didAnyWork)
				{
					SaveFileSystem.SubmitBatchUpdates(this);
				}
				this.IsAvailable = true;
			}
		}

		// Token: 0x04005755 RID: 22357
		private bool m_didAnyWork;

		// Token: 0x04005756 RID: 22358
		private bool m_batchEnded;

		// Token: 0x04005757 RID: 22359
		public int Profile;

		// Token: 0x04005758 RID: 22360
		public List<SaveFileSystem.FileUpdateCommand> CommandList;
	}

	// Token: 0x02000E3F RID: 3647
	private interface ISaveFileSystem
	{
		// Token: 0x1700232D RID: 9005
		// (get) Token: 0x06006BDF RID: 27615
		string PersistentDataPath { get; }

		// Token: 0x1700232E RID: 9006
		// (get) Token: 0x06006BE0 RID: 27616
		string BackupFolderName { get; }

		// Token: 0x06006BE1 RID: 27617
		void MountSaveDirectory();

		// Token: 0x06006BE2 RID: 27618
		bool DirectoryExists(string path);

		// Token: 0x06006BE3 RID: 27619
		void CreateDirectory(string path);

		// Token: 0x06006BE4 RID: 27620
		List<string> GetBackupFilesInDirectory(string directoryPath);

		// Token: 0x06006BE5 RID: 27621
		bool FileExists(string path);

		// Token: 0x06006BE6 RID: 27622
		byte[] ReadAllBytes(string path);

		// Token: 0x06006BE7 RID: 27623
		void WriteAllBytes(SaveFileSystem.SaveBatch batch, string path, byte[] bytes);

		// Token: 0x06006BE8 RID: 27624
		void DeleteFile(SaveFileSystem.SaveBatch batch, string path);

		// Token: 0x06006BE9 RID: 27625
		void SubmitBatchUpdates(SaveFileSystem.SaveBatch batch);
	}

	// Token: 0x02000E40 RID: 3648
	private class PCSaveFileSystem : SaveFileSystem.ISaveFileSystem
	{
		// Token: 0x1700232F RID: 9007
		// (get) Token: 0x06006BEA RID: 27626 RVA: 0x00192EB2 File Offset: 0x001910B2
		public string PersistentDataPath
		{
			get
			{
				return Application.persistentDataPath;
			}
		}

		// Token: 0x17002330 RID: 9008
		// (get) Token: 0x06006BEB RID: 27627 RVA: 0x00192EB9 File Offset: 0x001910B9
		public string BackupFolderName
		{
			get
			{
				return "Backups";
			}
		}

		// Token: 0x06006BEC RID: 27628 RVA: 0x00192EC0 File Offset: 0x001910C0
		public void MountSaveDirectory()
		{
		}

		// Token: 0x06006BED RID: 27629 RVA: 0x00192EC2 File Offset: 0x001910C2
		public void CreateDirectory(string path)
		{
			Directory.CreateDirectory(path);
		}

		// Token: 0x06006BEE RID: 27630 RVA: 0x00192ECB File Offset: 0x001910CB
		public bool DirectoryExists(string path)
		{
			return Directory.Exists(path);
		}

		// Token: 0x06006BEF RID: 27631 RVA: 0x00192ED3 File Offset: 0x001910D3
		public List<string> GetBackupFilesInDirectory(string path)
		{
			return new List<string>(Directory.GetFiles(path, "*.rc2dat"));
		}

		// Token: 0x06006BF0 RID: 27632 RVA: 0x00192EE5 File Offset: 0x001910E5
		public bool FileExists(string path)
		{
			return File.Exists(path);
		}

		// Token: 0x06006BF1 RID: 27633 RVA: 0x00192EED File Offset: 0x001910ED
		public byte[] ReadAllBytes(string path)
		{
			return File.ReadAllBytes(path);
		}

		// Token: 0x06006BF2 RID: 27634 RVA: 0x00192EF5 File Offset: 0x001910F5
		public void WriteAllBytes(SaveFileSystem.SaveBatch batch, string path, byte[] bytes)
		{
			File.WriteAllBytes(path, bytes);
		}

		// Token: 0x06006BF3 RID: 27635 RVA: 0x00192EFE File Offset: 0x001910FE
		public void DeleteFile(SaveFileSystem.SaveBatch batch, string path)
		{
			File.Delete(path);
		}

		// Token: 0x06006BF4 RID: 27636 RVA: 0x00192F06 File Offset: 0x00191106
		public void SubmitBatchUpdates(SaveFileSystem.SaveBatch batch)
		{
		}
	}
}
