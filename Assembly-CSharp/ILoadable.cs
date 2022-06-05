using System;
using System.Collections;

// Token: 0x020009C7 RID: 2503
public interface ILoadable
{
	// Token: 0x06004C72 RID: 19570
	void LoadSync();

	// Token: 0x06004C73 RID: 19571
	IEnumerator LoadAsync();
}
