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
using System.Windows.Automation;
using System.Windows.Automation.Provider;
using SWF = System.Windows.Forms;
using Mono.UIAutomation.Winforms.Events;

namespace Mono.UIAutomation.Winforms.Behaviors.ListItem
{

	internal abstract class ToggleProviderBehavior
		: ProviderBehavior, IToggleProvider
	{
		
		#region Constructors

		protected ToggleProviderBehavior (ListItemProvider provider)
			: base (provider)
		{
		}

		#endregion 
		
		#region IProviderBehavior specializations

		public override AutomationPattern ProviderPattern { 
			get { return TogglePatternIdentifiers.Pattern; }
		}
		
		public override object GetPropertyValue (int propertyId)
		{
			if (propertyId == TogglePatternIdentifiers.ToggleStateProperty.Id)
				return ToggleState;
			else
				return null;
		}
		
		public override void Disconnect ()
		{
			Provider.SetEvent (ProviderEventType.TogglePatternToggleStateProperty,
			                   null);
		}
		
		#endregion

		#region IToggleProvider specializations 
	
		public ToggleState ToggleState {
			get { 
				ListItemProvider provider = (ListItemProvider) Provider;
				return provider.ListProvider.GetItemToggleState (provider); 
			}
		}
		
		public void Toggle ()
		{
			ListItemProvider provider = (ListItemProvider) Provider;
			
			if (provider.ListProvider.Control.InvokeRequired == true) {
				provider.ListProvider.Control.BeginInvoke (new SWF.MethodInvoker (Toggle));
				return;
			}
			
			provider.ListProvider.ToggleItem (provider);
		}

		#endregion

	}
}
