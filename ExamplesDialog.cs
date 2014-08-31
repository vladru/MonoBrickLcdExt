using System;
using MonoBrickFirmware.Display;
using MonoBrickFirmware.Display.Dialogs;

namespace LcdDraw
{
	public class ExamplesDialog: Dialog{

		public ExamplesDialog(string title) : 
			base(Font.MediumFont, title, Lcd.Width, Lcd.Height-(int)Font.MediumFont.maxHeight)
		{
		}

		protected override bool OnEnterAction ()
		{
			return true;//exit
		}

		protected override void OnDrawContent ()
		{
			Lcd.Instance.DrawLine( new Point (0, 0), new Point (Lcd.Width-1, Lcd.Height-1), true);
			Lcd.Instance.DrawLine( new Point (0, Lcd.Height-1), new Point (Lcd.Width-1, 0), true);
			Lcd.Instance.DrawLine( new Point (40, 40), new Point (Lcd.Width-40, Lcd.Height-40), true);
			Lcd.Instance.DrawLine( new Point (40, 100), new Point (Lcd.Width-20, Lcd.Height-60), true);
		}
	}
}

