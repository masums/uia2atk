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
//	Mario Carrion <mcarrion@novell.com>
// 
using System;
using System.Windows.Forms;
using SWFHelpProvider = System.Windows.Forms.HelpProvider;

namespace Mono.UIAutomation.Winforms
{

	internal static class HelpProviderListener
	{
		#region Constructors

		static HelpProviderListener ()
		{
			initialized = false;
		}
		
		#endregion
		
		#region Private Static Fields
		
		private static bool initialized;

		#endregion
		
		#region Public Static Methods
		
		//Method called by SWF.HelpProvider static constructor
		public static void Initialize ()
		{
			if (initialized == true)
				return;
			
			Helper.AddPrivateEvent (typeof (SWFHelpProvider),
			                        null,
			                        "UIAHelpRequested", 
			                        typeof (HelpProviderListener),
			                        "OnUIAHelpRequested");
			
			Helper.AddPrivateEvent (typeof (SWFHelpProvider),
			                        null,
			                        "UIAHelpUnRequested",
			                        typeof (HelpProviderListener),
			                        "OnUIAHelpUnRequested");

			initialized = true;
		}
		
		#endregion
		
		#region Private Static Methods
		
#pragma warning disable 169

		private static void OnUIAHelpRequested (object sender, ControlEventArgs args)
		{
			SWFHelpProvider helpProvider = (SWFHelpProvider) sender;
			HelpProvider provider 
				= (HelpProvider) ProviderFactory.GetProvider (helpProvider);
			provider.Show (args.Control);
		}
		
		private static void OnUIAHelpUnRequested (object sender, ControlEventArgs args)
		{
			SWFHelpProvider helpProvider = (SWFHelpProvider) sender;
			HelpProvider provider 
				= (HelpProvider) ProviderFactory.GetProvider (helpProvider);
			provider.Hide (args.Control);
			
			ProviderFactory.ReleaseProvider (helpProvider);
		}		
		
#pragma warning restore 169
		
		#endregion
	}
}