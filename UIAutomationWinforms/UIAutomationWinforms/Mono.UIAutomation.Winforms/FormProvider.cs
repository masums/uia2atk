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
//      Sandy Armstrong <sanfordarmstrong@gmail.com>
// 

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Automation;
using System.Windows.Automation.Provider;
using System.Windows.Forms;
using Mono.UIAutomation.Winforms.Events;
using Mono.UIAutomation.Winforms.Events.Form;
using Mono.UIAutomation.Winforms.Behaviors.Form;

namespace Mono.UIAutomation.Winforms
{
	internal class FormProvider : FragmentRootControlProvider
	{
#region Private Data
		
		private Form form;
		
#endregion
		
#region Constructors

		public FormProvider (Form form) : base (form)
		{
			this.form = form;
			
			form.Closed += OnClosed;
			form.Shown += OnShown;
		}
		
#endregion

		public override void Initialize ()
		{
			base.Initialize ();

			// Behaviors
			
			SetBehavior (WindowPatternIdentifiers.Pattern,
			             new WindowProviderBehavior (this));
			SetBehavior (TransformPatternIdentifiers.Pattern,
			             new TransformProviderBehavior (this));

			// Events
			
			SetEvent (ProviderEventType.AutomationFocusChangedEvent,
			          new FormAutomationFocusChangedEvent (this));
		}

		
		//FIXME: Revamp
		
#region Private Event Handlers
		
		private void OnClosed (object sender, EventArgs args)
		{
			FinalizeChildControlStructure ();

			if (!AutomationInteropProvider.ClientsAreListening)
				return;
			
			AutomationEventArgs eventArgs =
				new AutomationEventArgs (WindowPatternIdentifiers.WindowClosedEvent);
			AutomationInteropProvider.RaiseAutomationEvent (WindowPatternIdentifiers.WindowClosedEvent,
			                                                this,
			                                                eventArgs);
			// TODO: Fill in rest of eventargs			
			
			if (form.Owner == null)
				Helper.RaiseStructureChangedEvent (StructureChangeType.ChildRemoved,
				                                   this);
			else {
				FormProvider ownerProvider = 
					ProviderFactory.GetProvider (form.Owner, false, false) as FormProvider;
				ownerProvider.RemoveChildProvider (true, this);
			}
		}
		
		private void OnShown (object sender, EventArgs args)
		{
			if (!AutomationInteropProvider.ClientsAreListening)
				return;
			
			AutomationEventArgs eventArgs =
				new AutomationEventArgs (WindowPatternIdentifiers.WindowOpenedEvent);
			AutomationInteropProvider.RaiseAutomationEvent (WindowPatternIdentifiers.WindowOpenedEvent,
			                                                this,
			                                                eventArgs);
		}
		
#endregion
		

#region IRawElementProviderFragmentRoot Members

		public override IRawElementProviderSimple HostRawElementProvider {
			get {
				return this;
			}
		}

		protected override object GetProviderPropertyValue (int propertyId)
		{
			if (propertyId == AutomationElementIdentifiers.ControlTypeProperty.Id)
				return ControlType.Window.Id;
			else if (propertyId == AutomationElementIdentifiers.LabeledByProperty.Id)
				return null;
			else if (propertyId == AutomationElementIdentifiers.NameProperty.Id)
				return Control.Text;
			else if (propertyId == AutomationElementIdentifiers.LocalizedControlTypeProperty.Id)
				return "window";
			else if (propertyId == AutomationElementIdentifiers.NativeWindowHandleProperty.Id)
				return form.Handle; // TODO: Should be int, maybe?
			else if (propertyId == AutomationElementIdentifiers.HasKeyboardFocusProperty.Id)
				return false;
			else
				return base.GetProviderPropertyValue (propertyId);
		}
		
		public override IRawElementProviderFragment ElementProviderFromPoint (double x, double y)
		{
			if (x > form.Width || y > form.Height)
				return null;
			
			Control child = form.GetChildAtPoint (new Point ((int)x, (int)y));
			
			if (child != null) {
				Console.WriteLine (child);
				
				if (componentProviders.ContainsKey (child)) {
					IRawElementProviderSimple provider =
						componentProviders [child];
					IRawElementProviderFragment providerFragment =
						provider as IRawElementProviderFragment;
					if (providerFragment != null)
						return providerFragment;
				}
			} else
				Console.WriteLine ("ElementProviderFromPoint: Child is null");
			
			return this;
		}
		
		public override IRawElementProviderFragment GetFocus ()
		{
			foreach (Control control in form.Controls) {
				if (control.Focused) {
					// TODO: Necessary to delve into child control
					// for focused element?
					
					if (componentProviders.ContainsKey (control)) {
						IRawElementProviderSimple provider =
							componentProviders [control];
						IRawElementProviderFragment providerFragment =
							provider as IRawElementProviderFragment;
						if (providerFragment != null)
							return providerFragment;
					}
				}
			}
			
			return null;
		}
		
#endregion
	}
}