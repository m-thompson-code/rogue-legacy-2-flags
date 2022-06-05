using System;
using Rewired;

// Token: 0x02000CC7 RID: 3271
public class KeycodeConverter
{
	// Token: 0x06005D75 RID: 23925 RVA: 0x0015DB34 File Offset: 0x0015BD34
	public static string GetKeycodeStringName(KeyboardKeyCode key, bool shift, bool capsLock, bool numLock)
	{
		if (key > KeyboardKeyCode.Tab)
		{
			if (key != KeyboardKeyCode.Return)
			{
				switch (key)
				{
				case KeyboardKeyCode.Escape:
					return "Esc";
				case (KeyboardKeyCode)28:
				case (KeyboardKeyCode)29:
				case (KeyboardKeyCode)30:
				case (KeyboardKeyCode)31:
				case KeyboardKeyCode.Exclaim:
				case KeyboardKeyCode.DoubleQuote:
				case KeyboardKeyCode.Hash:
				case KeyboardKeyCode.Dollar:
				case (KeyboardKeyCode)37:
				case KeyboardKeyCode.Ampersand:
				case KeyboardKeyCode.LeftParen:
				case KeyboardKeyCode.RightParen:
				case KeyboardKeyCode.Asterisk:
				case KeyboardKeyCode.Slash:
				case KeyboardKeyCode.Colon:
				case KeyboardKeyCode.Less:
				case KeyboardKeyCode.Greater:
				case KeyboardKeyCode.At:
				case (KeyboardKeyCode)65:
				case (KeyboardKeyCode)66:
				case (KeyboardKeyCode)67:
				case (KeyboardKeyCode)68:
				case (KeyboardKeyCode)69:
				case (KeyboardKeyCode)70:
				case (KeyboardKeyCode)71:
				case (KeyboardKeyCode)72:
				case (KeyboardKeyCode)73:
				case (KeyboardKeyCode)74:
				case (KeyboardKeyCode)75:
				case (KeyboardKeyCode)76:
				case (KeyboardKeyCode)77:
				case (KeyboardKeyCode)78:
				case (KeyboardKeyCode)79:
				case (KeyboardKeyCode)80:
				case (KeyboardKeyCode)81:
				case (KeyboardKeyCode)82:
				case (KeyboardKeyCode)83:
				case (KeyboardKeyCode)84:
				case (KeyboardKeyCode)85:
				case (KeyboardKeyCode)86:
				case (KeyboardKeyCode)87:
				case (KeyboardKeyCode)88:
				case (KeyboardKeyCode)89:
				case (KeyboardKeyCode)90:
				case KeyboardKeyCode.Caret:
				case KeyboardKeyCode.Underscore:
					goto IL_629;
				case KeyboardKeyCode.Space:
					return "Space";
				case KeyboardKeyCode.Quote:
					if (!shift)
					{
						return "\"";
					}
					return "'";
				case KeyboardKeyCode.Plus:
					if (!shift)
					{
						return "=";
					}
					return "+";
				case KeyboardKeyCode.Comma:
					if (!shift)
					{
						return ",";
					}
					return "<";
				case KeyboardKeyCode.Minus:
					if (!shift)
					{
						return "-";
					}
					return "_";
				case KeyboardKeyCode.Period:
					if (!shift)
					{
						return ".";
					}
					return ">";
				case KeyboardKeyCode.Alpha0:
					if (!shift)
					{
						return "0";
					}
					return ")";
				case KeyboardKeyCode.Alpha1:
					if (!shift)
					{
						return "1";
					}
					return "!";
				case KeyboardKeyCode.Alpha2:
					if (!shift)
					{
						return "2";
					}
					return "@";
				case KeyboardKeyCode.Alpha3:
					if (!shift)
					{
						return "3";
					}
					return "#";
				case KeyboardKeyCode.Alpha4:
					if (!shift)
					{
						return "4";
					}
					return "$";
				case KeyboardKeyCode.Alpha5:
					if (!shift)
					{
						return "5";
					}
					return "%";
				case KeyboardKeyCode.Alpha6:
					if (!shift)
					{
						return "6";
					}
					return "^";
				case KeyboardKeyCode.Alpha7:
					if (!shift)
					{
						return "7";
					}
					return "&";
				case KeyboardKeyCode.Alpha8:
					if (!shift)
					{
						return "8";
					}
					return "*";
				case KeyboardKeyCode.Alpha9:
					if (!shift)
					{
						return "9";
					}
					return "(";
				case KeyboardKeyCode.Semicolon:
					if (!shift)
					{
						return ";";
					}
					return ":";
				case KeyboardKeyCode.Equals:
					if (!shift)
					{
						return "=";
					}
					return "+";
				case KeyboardKeyCode.Question:
					if (!shift)
					{
						return "/";
					}
					return "?";
				case KeyboardKeyCode.LeftBracket:
					if (!shift)
					{
						return "[";
					}
					return "{";
				case KeyboardKeyCode.Backslash:
					if (!shift)
					{
						return "\\";
					}
					return "|";
				case KeyboardKeyCode.RightBracket:
					if (!shift)
					{
						return "]";
					}
					return "}";
				case KeyboardKeyCode.BackQuote:
					if (!shift)
					{
						return "`";
					}
					return "~";
				case KeyboardKeyCode.A:
					return KeycodeConverter.TranslateAlphabetic('a', shift, capsLock);
				case KeyboardKeyCode.B:
					return KeycodeConverter.TranslateAlphabetic('b', shift, capsLock);
				case KeyboardKeyCode.C:
					return KeycodeConverter.TranslateAlphabetic('c', shift, capsLock);
				case KeyboardKeyCode.D:
					return KeycodeConverter.TranslateAlphabetic('d', shift, capsLock);
				case KeyboardKeyCode.E:
					return KeycodeConverter.TranslateAlphabetic('e', shift, capsLock);
				case KeyboardKeyCode.F:
					return KeycodeConverter.TranslateAlphabetic('f', shift, capsLock);
				case KeyboardKeyCode.G:
					return KeycodeConverter.TranslateAlphabetic('g', shift, capsLock);
				case KeyboardKeyCode.H:
					return KeycodeConverter.TranslateAlphabetic('h', shift, capsLock);
				case KeyboardKeyCode.I:
					return KeycodeConverter.TranslateAlphabetic('i', shift, capsLock);
				case KeyboardKeyCode.J:
					return KeycodeConverter.TranslateAlphabetic('j', shift, capsLock);
				case KeyboardKeyCode.K:
					return KeycodeConverter.TranslateAlphabetic('k', shift, capsLock);
				case KeyboardKeyCode.L:
					return KeycodeConverter.TranslateAlphabetic('l', shift, capsLock);
				case KeyboardKeyCode.M:
					return KeycodeConverter.TranslateAlphabetic('m', shift, capsLock);
				case KeyboardKeyCode.N:
					return KeycodeConverter.TranslateAlphabetic('n', shift, capsLock);
				case KeyboardKeyCode.O:
					return KeycodeConverter.TranslateAlphabetic('o', shift, capsLock);
				case KeyboardKeyCode.P:
					return KeycodeConverter.TranslateAlphabetic('p', shift, capsLock);
				case KeyboardKeyCode.Q:
					return KeycodeConverter.TranslateAlphabetic('q', shift, capsLock);
				case KeyboardKeyCode.R:
					return KeycodeConverter.TranslateAlphabetic('r', shift, capsLock);
				case KeyboardKeyCode.S:
					return KeycodeConverter.TranslateAlphabetic('s', shift, capsLock);
				case KeyboardKeyCode.T:
					return KeycodeConverter.TranslateAlphabetic('t', shift, capsLock);
				case KeyboardKeyCode.U:
					return KeycodeConverter.TranslateAlphabetic('u', shift, capsLock);
				case KeyboardKeyCode.V:
					return KeycodeConverter.TranslateAlphabetic('v', shift, capsLock);
				case KeyboardKeyCode.W:
					return KeycodeConverter.TranslateAlphabetic('w', shift, capsLock);
				case KeyboardKeyCode.X:
					return KeycodeConverter.TranslateAlphabetic('x', shift, capsLock);
				case KeyboardKeyCode.Y:
					return KeycodeConverter.TranslateAlphabetic('y', shift, capsLock);
				case KeyboardKeyCode.Z:
					return KeycodeConverter.TranslateAlphabetic('z', shift, capsLock);
				default:
					switch (key)
					{
					case KeyboardKeyCode.Keypad0:
						return "NUM 0";
					case KeyboardKeyCode.Keypad1:
						return "NUM 1";
					case KeyboardKeyCode.Keypad2:
						return "NUM 2";
					case KeyboardKeyCode.Keypad3:
						return "NUM 3";
					case KeyboardKeyCode.Keypad4:
						return "NUM 4";
					case KeyboardKeyCode.Keypad5:
						return "NUM 5";
					case KeyboardKeyCode.Keypad6:
						return "NUM 6";
					case KeyboardKeyCode.Keypad7:
						return "NUM 7";
					case KeyboardKeyCode.Keypad8:
						return "NUM 8";
					case KeyboardKeyCode.Keypad9:
						return "NUM 9";
					case KeyboardKeyCode.KeypadPeriod:
						if (numLock && !shift)
						{
							return ".";
						}
						goto IL_629;
					case KeyboardKeyCode.KeypadDivide:
						return "/";
					case KeyboardKeyCode.KeypadMultiply:
						return "*";
					case KeyboardKeyCode.KeypadMinus:
						return "-";
					case KeyboardKeyCode.KeypadPlus:
						return "+";
					case KeyboardKeyCode.KeypadEnter:
						break;
					case KeyboardKeyCode.KeypadEquals:
					case KeyboardKeyCode.Insert:
					case KeyboardKeyCode.Home:
					case KeyboardKeyCode.End:
					case KeyboardKeyCode.PageUp:
					case KeyboardKeyCode.PageDown:
					case KeyboardKeyCode.F13:
					case KeyboardKeyCode.F14:
					case KeyboardKeyCode.F15:
					case (KeyboardKeyCode)297:
					case (KeyboardKeyCode)298:
					case (KeyboardKeyCode)299:
					case KeyboardKeyCode.Numlock:
					case KeyboardKeyCode.ScrollLock:
					case KeyboardKeyCode.LeftWindows:
					case KeyboardKeyCode.RightWindows:
					case KeyboardKeyCode.AltGr:
					case (KeyboardKeyCode)314:
					case KeyboardKeyCode.Help:
					case KeyboardKeyCode.Print:
					case KeyboardKeyCode.SysReq:
					case KeyboardKeyCode.Break:
						goto IL_629;
					case KeyboardKeyCode.UpArrow:
						return "";
					case KeyboardKeyCode.DownArrow:
						return "";
					case KeyboardKeyCode.RightArrow:
						return "";
					case KeyboardKeyCode.LeftArrow:
						return "";
					case KeyboardKeyCode.F1:
						return "F1";
					case KeyboardKeyCode.F2:
						return "F2";
					case KeyboardKeyCode.F3:
						return "F3";
					case KeyboardKeyCode.F4:
						return "F4";
					case KeyboardKeyCode.F5:
						return "F5";
					case KeyboardKeyCode.F6:
						return "F6";
					case KeyboardKeyCode.F7:
						return "F7";
					case KeyboardKeyCode.F8:
						return "F8";
					case KeyboardKeyCode.F9:
						return "F9";
					case KeyboardKeyCode.F10:
						return "F10";
					case KeyboardKeyCode.F11:
						return "F11";
					case KeyboardKeyCode.F12:
						return "F12";
					case KeyboardKeyCode.CapsLock:
						return "Caps";
					case KeyboardKeyCode.RightShift:
						return "RShift";
					case KeyboardKeyCode.LeftShift:
						return "LShift";
					case KeyboardKeyCode.RightControl:
						return "R-Ctrl";
					case KeyboardKeyCode.LeftControl:
						return "L-Ctrl";
					case KeyboardKeyCode.RightAlt:
						return "R-Alt";
					case KeyboardKeyCode.LeftAlt:
						return "L-Alt";
					case KeyboardKeyCode.RightCommand:
						return "R-Cmd";
					case KeyboardKeyCode.LeftCommand:
						return "L-Cmd";
					case KeyboardKeyCode.Menu:
						return "Menu";
					default:
						goto IL_629;
					}
					break;
				}
			}
			return "Enter";
		}
		if (key == KeyboardKeyCode.Backspace)
		{
			return "Back";
		}
		if (key == KeyboardKeyCode.Tab)
		{
			return "Tab";
		}
		IL_629:
		return "";
	}

	// Token: 0x06005D76 RID: 23926 RVA: 0x0015E170 File Offset: 0x0015C370
	public static string TranslateAlphabetic(char baseChar, bool shift, bool capsLock)
	{
		return ((capsLock ^ shift) ? char.ToUpper(baseChar) : baseChar).ToString();
	}
}
