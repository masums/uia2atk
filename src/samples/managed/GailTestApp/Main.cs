// Permission is hereby granted, free of charge, to any person obtaining 
// a copy of this software and associated documentation files (the 
// "Software"), to deal in the Software without restriction, including 
// without limitation the rights to use, copy, modify, merge, publish, 
// distribute, sublicense, and/or sell copies of the Software, and to 
// permit persons to whom the Software is furnished to do so, subject to 
// the following conditions: 
//  
// The above copyright notice and this permission notice shall be 
// included in all copies or substantial portions of the Software. 
//  
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, 
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF 
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND 
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE 
// LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION 
// OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION 
// WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE. 
// 
// Copyright (c) 2008 Novell, Inc. (http://www.novell.com) 
// 
// Authors: 
//      Andres G. Aragoneses <aaragoneses@novell.com>
// 
using System;
using Gtk;

using System.Threading;

namespace GailTestApp {


	public class MainClass
	{
		static MainWindow win = null;
		
		private static void Main (string[] args)
		{
			Start ();
		}
		
		private static void Start () {
			Start (null);
		}
		
		public static void Start (MovingThread guiThread)
		{
			if ((guiThread != null) && (guiThread.ThreadState == ThreadState.Running))
				return;

			if (guiThread != null) {

				guiThread.Deleg = Run;
				guiThread.Start ();
				
				//little hack (it doesn't matter, it's just for the nunit tests) in
				//order to wait for Gtk initialization
				Thread.Sleep (5000);
			}
			else
			{
				Run ();
			}
		}
		
		private static void Run () {
			Application.Init ();
			win = new MainWindow ();
			win.Show ();
			Application.Run ();
		}
		
		public static Gtk.Label GiveMeARealLabel (MovingThread guiThread)
		{
			if (win == null)
				Start (guiThread);
			
			return win.GiveMeARealLabel ();
		}
		
		public static Gtk.Button GiveMeARealButton (MovingThread guiThread)
		{
			if (win == null)
				Start (guiThread);
			
			return win.GiveMeARealButton ();
		}
		
		public static Gtk.Button GiveMeARealCheckBox (MovingThread guiThread)
		{
			if (win == null)
				Start (guiThread);
			
			return win.GiveMeARealCheckBox ();
		}
		
		public static Gtk.ComboBox GiveMeARealComboBox (MovingThread guiThread)
		{
			if (win == null)
				Start (guiThread);
			
			return win.GiveMeARealComboBox ();
		}
		
		public static Gtk.RadioButton GiveMeARealRadioButton (MovingThread guiThread)
		{
			if (win == null)
				Start (guiThread);
			
			return win.GiveMeARealRadioButton ();
		}
		
		public static Gtk.HScrollbar GiveMeARealHScrollbar (MovingThread guiThread)
		{
			if (win == null)
				Start (guiThread);
			
			return win.GiveMeARealHScrollbar ();
		}
		
		public static Gtk.VScrollbar GiveMeARealVScrollbar (MovingThread guiThread)
		{
			if (win == null)
				Start (guiThread);
			
			return win.GiveMeARealVScrollbar ();
		}
		
		public static Gtk.Statusbar GiveMeARealStatusbar (MovingThread guiThread)
		{
			if (win == null)
				Start (guiThread);
			
			return win.GiveMeARealStatusbar ();
		}
		
		public static Gtk.ProgressBar GiveMeARealProgressBar (MovingThread guiThread)
		{
			if (win == null)
				Start (guiThread);
			
			return win.GiveMeARealProgressBar ();
		}
		
		public static Gtk.Entry GiveMeARealEntry (MovingThread guiThread)
		{
			if (win == null)
				Start (guiThread);
			
			return win.GiveMeARealEntry ();
		}
		
		public static Gtk.ImageMenuItem GiveMeARealParentMenu (MovingThread guiThread, string name)
		{
			if (win == null)
				Start (guiThread);
			
			return win.GiveMeARealParentMenu (name);
		}
		
		public static Gtk.Window GiveMeARealWindow (MovingThread guiThread)
		{
			if (win == null)
				Start (guiThread);
			
			return win;
		}
		
		public static void Kill (MovingThread thread) 
		{
			thread.GLibDeleg = KillMe;
		}
		
		private static bool KillMe () 
		{
			win.Destroy ();
			win.Dispose ();
			Application.Quit ();
			return true;
		}
	}
}
