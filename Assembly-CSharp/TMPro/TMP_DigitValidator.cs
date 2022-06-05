using System;

namespace TMPro
{
	// Token: 0x02000852 RID: 2130
	[Serializable]
	public class TMP_DigitValidator : TMP_InputValidator
	{
		// Token: 0x060046C9 RID: 18121 RVA: 0x000FCD4B File Offset: 0x000FAF4B
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
