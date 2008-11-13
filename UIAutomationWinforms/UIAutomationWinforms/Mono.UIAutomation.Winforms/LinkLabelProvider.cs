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
using System.Windows.Forms;
using Mono.UIAutomation.Winforms.Behaviors;
using Mono.UIAutomation.Winforms.Behaviors.LinkLabel;

namespace Mono.UIAutomation.Winforms
{

	internal class LinkLabelProvider : FragmentControlProvider
	{

		#region Constructor

		public LinkLabelProvider (LinkLabel linkLabel) : base (linkLabel)
		{
		}

		#endregion
		
		#region SimpleControlProvider specialization

		public override void Initialize()
		{
			base.Initialize ();
			
			SetBehavior (InvokePatternIdentifiers.Pattern,
			             new InvokeProviderBehavior (this));
		}

		protected override object GetProviderPropertyValue (int propertyId)
		{
			if (propertyId == AutomationElementIdentifiers.ControlTypeProperty.Id)
				return ControlType.Hyperlink.Id;
			else if (propertyId == AutomationElementIdentifiers.LabeledByProperty.Id)
				return null;
			else if (propertyId == AutomationElementIdentifiers.NameProperty.Id)
				return Control.Text;
			else if (propertyId == AutomationElementIdentifiers.LocalizedControlTypeProperty.Id)
				return "hyperlink";
			else
				return base.GetProviderPropertyValue (propertyId);
		}		
		
		#endregion
	}
}