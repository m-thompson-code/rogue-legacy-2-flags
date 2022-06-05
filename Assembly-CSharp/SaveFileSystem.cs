using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

// Token: 0x02000CD9 RID: 3289
public static class SaveFileSystem
{
	// Token: 0x06005DBD RID: 23997 RVA: 0x0003397F File Offset: 0x00031B7F
	public static void Initialize()
	{
		SaveFileSystem.m_saveFileSystem = new SaveFileSystem.PCSaveFileSystem();
		SaveFileSystem.m_saveBatchPool = new List<SaveFileSystem.SaveBatch>(2)
		{
			new SaveFileSystem.SaveBatch(),
			new SaveFileSystem.SaveBatch()
		};
	}

	// Token: 0x17001EF4 RID: 7924
	// (get) Token: 0x06005DBE RID: 23998 RVA: 0x000339AC File Offset: 0x00031BAC
	public static string PersistentDataPath
	{
		get
		{
			return SaveFileSystem.m_saveFileSystem.PersistentDataPath;
		}
	}

	// Token: 0x17001EF5 RID: 7925
	// (get) Token: 0x06005DBF RID: 23999 RVA: 0x000339B8 File Offset: 0x00031BB8
	public static string BackupFolderName
	{
		get
		{
			return SaveFileSystem.m_saveFileSystem.BackupFolderName;
		}
	}

	// Token: 0x17001EF6 RID: 7926
	// (get) Token: 0x06005DC0 RID: 24000 RVA: 0x000339C4 File Offset: 0x00031BC4
	public static bool IsSaving
	{
		get
		{
			return SaveFileSystem.m_totalBatchRefCount > 0;
		}
	}

	// Token: 0x06005DC1 RID: 24001 RVA: 0x000339CE File Offset: 0x00031BCE
	public static void MountSaveDirectory()
	{
		SaveFileSystem.m_saveFileSystem.MountSaveDirectory();
	}

	// Token: 0x06005DC2 RID: 24002 RVA: 0x000339DA File Offset: 0x00031BDA
	public static bool DirectoryExists(string path)
	{
		return SaveFileSystem.m_saveFileSystem.DirectoryExists(path);
	}

	// Token: 0x06005DC3 RID: 24003 RVA: 0x000339E7 File Offset: 0x00031BE7
	public static void CreateDirectory(string path)
	{
		SaveFileSystem.m_saveFileSystem.CreateDirectory(path);
	}

	// Token: 0x06005DC4 RID: 24004 RVA: 0x000339F4 File Offset: 0x00031BF4
	public static List<string> GetBackupFilesInDirectory(string path)
	{
		return SaveFileSystem.m_saveFileSystem.GetBackupFilesInDirectory(path);
	}

	// Token: 0x06005DC5 RID: 24005 RVA: 0x00033A01 File Offset: 0x00031C01
	public static bool FileExists(string path)
	{
		return SaveFileSystem.m_saveFileSystem.FileExists(path);
	}

	// Token: 0x06005DC6 RID: 24006 RVA: 0x00033A0E File Offset: 0x00031C0E
	public static byte[] ReadAllBytes(string path)
	{
		return SaveFileSystem.m_saveFileSystem.ReadAllBytes(path);
	}

	// Token: 0x06005DC7 RID: 24007 RVA: 0x0015F02C File Offset: 0x0015D22C
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

	// Token: 0x06005DC8 RID: 24008 RVA: 0x00033A1B File Offset: 0x00031C1B
	private static void SubmitBatchUpdates(SaveFileSystem.SaveBatch batch)
	{
		SaveFileSystem.m_saveFileSystem.SubmitBatchUpdates(batch);
	}

	// Token: 0x06005DC9 RID: 24009 RVA: 0x00033A28 File Offset: 0x00031C28
	public static void WriteAllBytes(SaveFileSystem.SaveBatch batch, string path, byte[] bytes)
	{
		batch.IncrementRefCount();
		SaveFileSystem.m_saveFileSystem.WriteAllBytes(batch, path, bytes);
		batch.DecrementRefCount();
	}

	// Token: 0x06005DCA RID: 24010 RVA: 0x00033A43 File Offset: 0x00031C43
	public static void DeleteFile(SaveFileSystem.SaveBatch batch, string path)
	{
		batch.IncrementRefCount();
		SaveFileSystem.m_saveFileSystem.DeleteFile(batch, path);
		batch.DecrementRefCount();
	}

	// Token: 0x04004D21 RID: 19745
	public const string SAVE_FILE_EXT = ".rc2dat";

	// Token: 0x04004D22 RID: 19746
	private static SaveFileSystem.ISaveFileSystem m_saveFileSystem;

	// Token: 0x04004D23 RID: 19747
	private static List<SaveFileSystem.SaveBatch> m_saveBatchPool;

	// Token: 0x04004D24 RID: 19748
	private static int m_totalBatchRefCount;

	// Token: 0x02000CDA RID: 3290
	public struct FileUpdateCommand
	{
		// Token: 0x06005DCB RID: 24011 RVA: 0x00033A5D File Offset: 0x00031C5D
		public FileUpdateCommand(int profile, string filename, byte[] bytes)
		{
			this.Profile = profile;
			this.BlobName = filename;
			this.Bytes = bytes;
		}

		// Token: 0x04004D25 RID: 19749
		public int Profile;

		// Token: 0x04004D26 RID: 19750
		public string BlobName;

		// Token: 0x04004D27 RID: 19751
		public byte[] Bytes;
	}

	// Token: 0x02000CDB RID: 3291
	public class SaveBatch
	{
		// Token: 0x17001EF7 RID: 7927
		// (get) Token: 0x06005DCC RID: 24012 RVA: 0x00033A74 File Offset: 0x00031C74
		// (set) Token: 0x06005DCD RID: 24013 RVA: 0x00033A7C File Offset: 0x00031C7C
		public bool IsAvailable { get; private set; }

		// Token: 0x17001EF8 RID: 7928
		// (get) Token: 0x06005DCE RID: 24014 RVA: 0x00033A85 File Offset: 0x00031C85
		// (set) Token: 0x06005DCF RID: 24015 RVA: 0x00033A8D File Offset: 0x00031C8D
		public int RefCount { get; private set; }

		// Token: 0x06005DD0 RID: 24016 RVA: 0x00033A96 File Offset: 0x00031C96
		public SaveBatch()
		{
			this.m_batchEnded = false;
			this.m_didAnyWork = false;
			this.Profile = -1;
			this.RefCount = 0;
			this.CommandList = null;
			this.IsAvailable = true;
		}

		// Token: 0x06005DD1 RID: 24017 RVA: 0x0015F0A8 File Offset: 0x0015D2A8
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

		// Token: 0x06005DD2 RID: 24018 RVA: 0x00033AC8 File Offset: 0x00031CC8
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

		// Token: 0x06005DD3 RID: 24019 RVA: 0x0015F0F8 File Offset: 0x0015D2F8
		public void IncrementRefCount()
		{
			int refCount = this.RefCount;
			this.RefCount = refCount + 1;
			SaveFileSystem.m_totalBatchRefCount++;
			this.m_didAnyWork = true;
		}

		// Token: 0x06005DD4 RID: 24020 RVA: 0x0015F128 File Offset: 0x0015D328
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

		// Token: 0x04004D28 RID: 19752
		private bool m_didAnyWork;

		// Token: 0x04004D29 RID: 19753
		private bool m_batchEnded;

		// Token: 0x04004D2A RID: 19754
		public int Profile;

		// Token: 0x04004D2B RID: 19755
		public List<SaveFileSystem.FileUpdateCommand> CommandList;
	}

	// Token: 0x02000CDC RID: 3292
	private interface ISaveFileSystem
	{
		// Token: 0x17001EF9 RID: 7929
		// (get) Token: 0x06005DD5 RID: 24021
		string PersistentDataPath { get; }

		// Token: 0x17001EFA RID: 7930
		// (get) Token: 0x06005DD6 RID: 24022
		string BackupFolderName { get; }

		// Token: 0x06005DD7 RID: 24023
		void MountSaveDirectory();

		// Token: 0x06005DD8 RID: 24024
		bool DirectoryExists(string path);

		// Token: 0x06005DD9 RID: 24025
		void CreateDirectory(string path);

		// Token: 0x06005DDA RID: 24026
		List<string> GetBackupFilesInDirectory(string directoryPath);

		// Token: 0x06005DDB RID: 24027
		bool FileExists(string path);

		// Token: 0x06005DDC RID: 24028
		byte[] ReadAllBytes(string path);

		// Token: 0x06005DDD RID: 24029
		void WriteAllBytes(SaveFileSystem.SaveBatch batch, string path, byte[] bytes);

		// Token: 0x06005DDE RID: 24030
		void DeleteFile(SaveFileSystem.SaveBatch batch, string path);

		// Token: 0x06005DDF RID: 24031
		void SubmitBatchUpdates(SaveFileSystem.SaveBatch batch);
	}

	// Token: 0x02000CDD RID: 3293
	private class PCSaveFileSystem : SaveFileSystem.ISaveFileSystem
	{
		// Token: 0x17001EFB RID: 7931
		// (get) Token: 0x06005DE0 RID: 24032 RVA: 0x00033AFA File Offset: 0x00031CFA
		public string PersistentDataPath
		{
			get
			{
				return Application.persistentDataPath;
			}
		}

		// Token: 0x17001EFC RID: 7932
		// (get) Token: 0x06005DE1 RID: 24033 RVA: 0x00033B01 File Offset: 0x00031D01
		public string BackupFolderName
		{
			get
			{
				return "Backups";
			}
		}

		// Token: 0x06005DE2 RID: 24034 RVA: 0x00002FCA File Offset: 0x000011CA
		public void MountSaveDirectory()
		{
		}

		// Token: 0x06005DE3 RID: 24035 RVA: 0x00033B08 File Offset: 0x00031D08
		public void CreateDirectory(string path)
		{
			Directory.CreateDirectory(path);
		}

		// Token: 0x06005DE4 RID: 24036 RVA: 0x00033B11 File Offset: 0x00031D11
		public bool DirectoryExists(string path)
		{
			return Directory.Exists(path);
		}

		// Token: 0x06005DE5 RID: 24037 RVA: 0x00033B19 File Offset: 0x00031D19
		public List<string> GetBackupFilesInDirectory(string path)
		{
			return new List<string>(Directory.GetFiles(path, "*.rc2dat"));
		}

		// Token: 0x06005DE6 RID: 24038 RVA: 0x00033B2B File Offset: 0x00031D2B
		public bool FileExists(string path)
		{
			return File.Exists(path);
		}

		// Token: 0x06005DE7 RID: 24039 RVA: 0x00033B33 File Offset: 0x00031D33
		public byte[] ReadAllBytes(string path)
		{
			return File.ReadAllBytes(path);
		}

		// Token: 0x06005DE8 RID: 24040 RVA: 0x00033B3B File Offset: 0x00031D3B
		public void WriteAllBytes(SaveFileSystem.SaveBatch batch, string path, byte[] bytes)
		{
			File.WriteAllBytes(path, bytes);
		}

		// Token: 0x06005DE9 RID: 24041 RVA: 0x00033B44 File Offset: 0x00031D44
		public void DeleteFile(SaveFileSystem.SaveBatch batch, string path)
		{
			File.Delete(path);
		}

		// Token: 0x06005DEA RID: 24042 RVA: 0x00002FCA File Offset: 0x000011CA
		public void SubmitBatchUpdates(SaveFileSystem.SaveBatch batch)
		{
		}
	}
}
