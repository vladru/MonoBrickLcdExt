using System;
using MonoBrickFirmware.Display.Dialogs;

namespace LcdDraw
{
	public class Program
	{
		public static void Main (string[] args)
		{
			var examplesDialog = new ExamplesDialog ("DrawLine example");
			examplesDialog.Show ();
		}
	}
}