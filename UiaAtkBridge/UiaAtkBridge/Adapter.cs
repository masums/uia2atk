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
using System.Collections.Generic;
using System.Windows.Automation;
using System.Windows.Automation.Provider;

namespace UiaAtkBridge
{
	public abstract class Adapter : Atk.Object
	{

#region Constructors
		
		public Adapter (IRawElementProviderSimple provider)
		{
			Provider = provider;
		}
		
#endregion

#region Adapter Methods
	
		public IRawElementProviderSimple Provider { get; private set; }
		
		public virtual void RaiseAutomationEvent (AutomationEvent eventId, AutomationEventArgs e)
		{
			if (eventId != AutomationElementIdentifiers.AutomationFocusChangedEvent)
				return;

			Atk.Object parent = this; //in case this.GetType () == typeof(UiaAtkBridge.Window) 
			while (!(parent is UiaAtkBridge.Window)) {
				if (parent == null)
					throw new Exception ("Parent of an object should not be null");
				parent = parent.Parent;
			}
			TopLevelRootItem.Instance.CheckAndHandleNewActiveWindow ((UiaAtkBridge.Window)parent);
		}
		
		public virtual void RaiseAutomationPropertyChangedEvent (AutomationPropertyChangedEventArgs e)
		{
			if (e.Property == AutomationElementIdentifiers.HasKeyboardFocusProperty) {
				bool focused = (bool)e.NewValue;
				NotifyStateChange (Atk.StateType.Focused, focused);
				if (focused)
					Atk.Focus.TrackerNotify (this);
			} else if (e.Property == AutomationElementIdentifiers.IsOffscreenProperty) { 
				bool offscreen = (bool)e.NewValue;
				NotifyStateChange (Atk.StateType.Visible, !offscreen);
			} else if (e.Property == AutomationElementIdentifiers.IsEnabledProperty) {
				bool enabled = (bool) e.NewValue;
				NotifyStateChange (Atk.StateType.Enabled, enabled);
				NotifyStateChange (Atk.StateType.Sensitive, enabled);
			} else if (e.Property == AutomationElementIdentifiers.NameProperty) {
				Name = (string)e.NewValue;
			}
		}
		
#endregion
		
#region Overrides
		protected override Atk.StateSet OnRefStateSet ()
		{
			Atk.StateSet states = base.OnRefStateSet ();
			
			if (Provider != null) {
				bool enabled = 
				  (bool) Provider.GetPropertyValue (AutomationElementIdentifiers.IsEnabledProperty.Id);
				if (enabled) {
					states.AddState (Atk.StateType.Sensitive);
					states.AddState (Atk.StateType.Enabled);
				} else {
					states.RemoveState (Atk.StateType.Sensitive);
					states.RemoveState (Atk.StateType.Enabled);
				}
				
				bool showing = !(bool) Provider.GetPropertyValue (AutomationElementIdentifiers.IsOffscreenProperty.Id);
				if (showing)
				{
					states.AddState (Atk.StateType.Showing);
					states.AddState (Atk.StateType.Visible);
				}
				else
				{
					states.RemoveState (Atk.StateType.Showing);
				}
			}
			
			return states;
		}

		protected override int OnGetIndexInParent()
		{
			if (Parent == null)
				return -1;
			ParentAdapter parent = Parent as ParentAdapter;
			if (parent != null)
				return parent.GetIndexOfChild (this);
			for (int i = Parent.NAccessibleChildren - 1; i >= 0; i--) {
				if (Parent.RefAccessibleChild(i) == this)
					return i;
			}
			return -1;
		}
#endregion

		internal System.Windows.Rect BoundingRectangle
		{
			get {
				return (System.Windows.Rect) 
					Provider.GetPropertyValue (
					  AutomationElementIdentifiers.BoundingRectangleProperty.Id);
			}
		}

		// TODO: Figure out if this does what we want
		protected void NotifyPropertyChange (String name, double val)
		{
			Atk.PropertyValues v;
			v.PropertyName = name;
			v.OldValue = new GLib.Value ((int)0);
			v.NewValue = new GLib.Value (val);
			OnPropertyChange (v);
		}
	}
}