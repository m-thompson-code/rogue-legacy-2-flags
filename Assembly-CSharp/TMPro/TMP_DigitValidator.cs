using System;

namespace TMPro
{
	// Token: 0x02000D50 RID: 3408
	[Serializable]
	public class TMP_DigitValidator : TMP_InputValidator
	{
		// Token: 0x06006165 RID: 24933 RVA: 0x00035AA6 File Offset: 0x00033CA6
		public override char Validate(ref string text, ref int pos, char ch)
		{
			if (ch >= '0' && ch <= '9')
			{
				pos++;
				return ch;
			}
			return '\0';
		}
	}
}
