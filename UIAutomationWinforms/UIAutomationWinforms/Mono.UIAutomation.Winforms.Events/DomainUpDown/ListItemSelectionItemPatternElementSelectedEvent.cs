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
//	Brad Taylor <brad@getcoded.net>
// 

using System;
using System.ComponentModel;
using System.Windows.Automation;
using System.Windows.Automation.Provider;
using SWF = System.Windows.Forms;
using Mono.UIAutomation.Winforms;
using Mono.UIAutomation.Winforms.Events;

namespace Mono.UIAutomation.Winforms.Events.DomainUpDown
{
	internal class ListItemSelectionItemPatternElementSelectedEvent
		: BaseAutomationEvent
	{
#region Public Methods
		public ListItemSelectionItemPatternElementSelectedEvent (ListItemProvider provider)
			: base (provider, 
			        SelectionItemPatternIdentifiers.ElementSelectedEvent)
		{
		}
#endregion
		
#region ProviderEvent Methods
		public override void Connect ()
		{
			DomainUpDownControl.SelectedItemChanged += OnElementSelectedEvent;
		}

		public override void Disconnect ()
		{
			DomainUpDownControl.SelectedItemChanged -= OnElementSelectedEvent;
		}
#endregion 

#region Private Properties
		private SWF.DomainUpDown DomainUpDownControl {
			get { return (SWF.DomainUpDown) ((ListItemProvider) Provider).ListProvider.Control; }
		}
#endregion
	
#region Private Methods
		private void OnElementSelectedEvent (object sender, EventArgs args)
		{
			ListItemProvider prov = ((ListItemProvider)Provider);
			if (prov.ListProvider.IsItemSelected (prov)) {
				RaiseAutomationEvent ();
			}
		}
#endregion
	}
}
