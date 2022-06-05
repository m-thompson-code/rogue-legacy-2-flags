using System;
using System.Collections;

// Token: 0x020005BF RID: 1471
public interface ILoadable
{
	// Token: 0x06003660 RID: 13920
	void LoadSync();

	// Token: 0x06003661 RID: 13921
	IEnumerator LoadAsync();
}
