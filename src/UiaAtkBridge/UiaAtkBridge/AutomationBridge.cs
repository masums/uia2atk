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
//      Andres G. Aragoneses <aaragoneses@novell.com>
// 

using System;
using System.Collections.Generic;

using System.Windows.Automation;
using System.Windows.Automation.Provider;

using Mono.UIAutomation.Bridge;

namespace UiaAtkBridge
{
	public class AutomationBridge : IAutomationBridge
	{
#region Private Fields
		
		private bool applicationStarted = false;
		private Monitor appMonitor = null;
		private Dictionary<IntPtr, IRawElementProviderSimple>
			pointerProviderMapping;
		static private Dictionary<IRawElementProviderSimple, Adapter>
			providerAdapterMapping;
		
		private int windowProviders;
		
#endregion

#region Public Constructor
		
		public AutomationBridge ()
		{
			bool newMonitor = false;
			if (appMonitor == null) {
				Console.WriteLine ("about to create monitor");
				appMonitor = new Monitor();
				Console.WriteLine ("just made monitor");
			}
			pointerProviderMapping =
				new Dictionary<IntPtr,IRawElementProviderSimple> ();
			providerAdapterMapping =
				new Dictionary<IRawElementProviderSimple, Adapter>();

			windowProviders = 0;
		}
		
#endregion
		
#region Public Methods
		
		public static Adapter GetAdapterForProviderLazy (IRawElementProviderSimple provider)
		{
			return GetAdapterForProvider (provider, false);
		}
		
		[Obsolete("Use GetAdapterForProviderLazy as it's more efficient")]
		public static Adapter GetAdapterForProvider (IRawElementProviderSimple provider)
		{
			return GetAdapterForProvider (provider, true);
		}
		
		private static Adapter GetAdapterForProvider (IRawElementProviderSimple provider, bool avoidLazyLoading)
		{
			if (avoidLazyLoading) {
				Console.WriteLine ("WARNING: obsolete non-lazy-loading GetAdapterForProvider method called.");
				
				alreadyRequestedChildren = new List <Atk.Object> ();
				List <IRawElementProviderSimple> initialProvs = new List<IRawElementProviderSimple> ();
				
				foreach (IRawElementProviderSimple providerReady in providerAdapterMapping.Keys) {
					initialProvs.Add (providerReady);
				}
				
				foreach (IRawElementProviderSimple providerReady in initialProvs) {
					RequestChildren (providerAdapterMapping [providerReady]);
				}
				
				alreadyRequestedChildren = null;
			}
			
			Adapter adapter = null;
			providerAdapterMapping.TryGetValue (provider, out adapter);
			return adapter;
		}
		
		private static List <Atk.Object> alreadyRequestedChildren = null;
		
		private static void RequestChildren (Atk.Object adapter) {
			if (alreadyRequestedChildren.Contains (adapter))
				return;
			
			int nChildren = adapter.NAccessibleChildren;
			
			if (nChildren > 0) {
				for (int i = 0; i < nChildren; i++) {
					Atk.Object adapterN = adapter.RefAccessibleChild (i);
					
					if (i == 0)
						alreadyRequestedChildren.Add (adapter);
					
					RequestChildren (adapterN);
				}
			}
			else
				alreadyRequestedChildren.Add (adapter);
		}
		
#endregion
		
#region IAutomationBridge Members
		
		public bool ClientsAreListening {
			get {
				// TODO: How with ATK?
				return true;
			}
		}
		
		public object HostProviderFromHandle (IntPtr hwnd)
		{
			if (!pointerProviderMapping.ContainsKey (hwnd))
				return null;
			return pointerProviderMapping [hwnd];
		}

		
		public void RaiseAutomationEvent (AutomationEvent eventId, object provider, AutomationEventArgs e)
		{
			// TODO: Find better way to pass PreRun event on to bridge
			//        (nullx3 is a magic value)
			//        (once bridge events are working, should be able to happen upon construction, right?)
			if (eventId == null && provider == null && e == null) {
				if (!applicationStarted && appMonitor != null)
					appMonitor.ApplicationStarts ();
				return;
			}
			
			IRawElementProviderSimple simpleProvider =
				(IRawElementProviderSimple) provider;
			if (!providerAdapterMapping.ContainsKey (simpleProvider))
				return;
			
			providerAdapterMapping [simpleProvider].RaiseAutomationEvent (eventId, e);
		}
		
		public void RaiseAutomationPropertyChangedEvent (object element, AutomationPropertyChangedEventArgs e)
		{
			IRawElementProviderSimple simpleProvider =
				(IRawElementProviderSimple) element;
			if (!providerAdapterMapping.ContainsKey (simpleProvider))
				return;
			
			providerAdapterMapping [simpleProvider].RaiseAutomationPropertyChangedEvent (e);
		}
		
		public void RaiseStructureChangedEvent (object provider, StructureChangedEventArgs e)
		{
			IRawElementProviderSimple simpleProvider =
				(IRawElementProviderSimple) provider;
			int controlTypeId = (int) simpleProvider.GetPropertyValue (AutomationElementIdentifiers.ControlTypeProperty.Id);
			// TODO: Handle ChildrenBulkAdded
			if (e.StructureChangeType == StructureChangeType.ChildAdded) {
				if (!providerAdapterMapping.ContainsKey (simpleProvider))
					HandleElementAddition (simpleProvider);
			} else if (e.StructureChangeType == StructureChangeType.ChildRemoved) {
				if (controlTypeId == ControlType.Window.Id)
					HandleWindowProviderRemoval ((IWindowProvider)provider);
				else
					// TODO: Handle proper documented args
					//       (see FragmentRootControlProvider)
					HandleElementRemoval (simpleProvider);
			} else if (e.StructureChangeType == StructureChangeType.ChildrenBulkRemoved) {
				HandleBulkRemoved (simpleProvider);
			}
			
			// TODO: Other structure changes
		}
		
#endregion
		
#region Private Methods
		
		private ParentAdapter GetParentAdapter (IRawElementProviderSimple provider)
		{
			IRawElementProviderFragment fragment = (IRawElementProviderFragment)provider;
			IRawElementProviderSimple parentProvider;

			parentProvider = fragment.Navigate (NavigateDirection.Parent);
			if (!providerAdapterMapping.ContainsKey (parentProvider))
			{
				HandleElementAddition (parentProvider);
			}
			return (ParentAdapter)providerAdapterMapping[parentProvider];
		}

		private void HandleElementAddition (IRawElementProviderSimple simpleProvider)
		{
			int controlTypeId = (int) simpleProvider.GetPropertyValue (AutomationElementIdentifiers.ControlTypeProperty.Id);

			if (controlTypeId == ControlType.Window.Id)
				HandleNewWindowControlType (simpleProvider);
			else if (controlTypeId == ControlType.Button.Id)
				// TODO: Consider generalizing...
				HandleNewButtonControlType (simpleProvider);
			else if (controlTypeId == ControlType.Text.Id)
				HandleNewLabelControlType (simpleProvider);
			else if (controlTypeId == ControlType.CheckBox.Id)
				HandleNewCheckBoxControlType (simpleProvider);
			else if (controlTypeId == ControlType.List.Id)
				HandleNewListControlType (simpleProvider);
			else if (controlTypeId == ControlType.ListItem.Id)
				HandleNewListItemControlType (simpleProvider);
			else if (controlTypeId == ControlType.ComboBox.Id)
				HandleNewComboBoxControlType (simpleProvider);
			else if (controlTypeId == ControlType.StatusBar.Id)
				HandleNewStatusBarControlType (simpleProvider);
			else if (controlTypeId == ControlType.ProgressBar.Id)
				HandleNewProgressBarControlType (simpleProvider);
			else if (controlTypeId == ControlType.ScrollBar.Id)
				HandleNewScrollBarControlType (simpleProvider);
			else if (controlTypeId == ControlType.Group.Id)
				HandleNewGroupControlType (simpleProvider);
			else if (controlTypeId == ControlType.RadioButton.Id){
				HandleNewRadioButtonControlType (simpleProvider);}
			else if (controlTypeId == ControlType.Spinner.Id)
				HandleNewSpinnerControlType (simpleProvider);
//			else if (controlTypeId == ControlType.Edit.Id)
//				HandleNewEditControlType (simpleProvider);
			// TODO: Other providers
			else
				Console.WriteLine ("AutomationBridge: Unhandled control: " +
				                   ControlType.LookupById (controlTypeId).ProgrammaticName);
		}

		private void HandleElementRemoval (IRawElementProviderSimple provider)
		{
			Adapter adapter;
			if (providerAdapterMapping .TryGetValue (provider, out adapter) == false)
				return;

			ParentAdapter parent = adapter.Parent as ParentAdapter;
			if (parent != null)
				parent.RemoveChild (adapter);
			
			providerAdapterMapping.Remove (provider);
			try {
				IntPtr providerHandle = (IntPtr) provider.GetPropertyValue (AutomationElementIdentifiers.NativeWindowHandleProperty.Id);
				pointerProviderMapping.Remove (providerHandle);
			//FIXME: why is this? we should have a specific exception here, or a comment explaining
			} catch {}
		}
		
		
		private void HandleBulkRemoved (IRawElementProviderSimple provider)
		{
			IRawElementProviderFragment fragment;
			if ((fragment = provider as IRawElementProviderFragment) == null)
				return;
			List<Adapter> keep = new List<Adapter> ();
			IRawElementProviderFragment child = fragment.Navigate(NavigateDirection.FirstChild);
			while (child != null) {
				if (providerAdapterMapping.ContainsKey (child))
					keep.Add (providerAdapterMapping[child]);
				child = child.Navigate (NavigateDirection.NextSibling);
			}
			Adapter adapter = providerAdapterMapping[provider];
			int index = adapter.NAccessibleChildren;
			while (--index >= 0) {
				Adapter childAdapter = adapter.RefAccessibleChild (index) as Adapter;
				if (!keep.Contains(childAdapter))
					HandleElementRemoval (childAdapter.Provider);
			}
		}

		private void HandleNewWindowControlType (IRawElementProviderSimple provider)
		{
			IRawElementProviderSimple simpleProvider =
				(IRawElementProviderSimple) provider;
			
			Window newWindow = new Window (provider);
			providerAdapterMapping [simpleProvider] = newWindow;
			
			TopLevelRootItem.Instance.AddOneChild (newWindow);
			
			IntPtr providerHandle = (IntPtr) simpleProvider.GetPropertyValue (AutomationElementIdentifiers.NativeWindowHandleProperty.Id);
			pointerProviderMapping [providerHandle] = simpleProvider;
			
			windowProviders++;
		}
		
		private void HandleWindowProviderRemoval (IWindowProvider provider)
		{
			Console.WriteLine ("FormIsRemoved");
			TopLevelRootItem.Instance.RemoveChild (providerAdapterMapping [(IRawElementProviderSimple) provider]);
			providerAdapterMapping.Remove ((IRawElementProviderSimple) provider);
			
			IRawElementProviderSimple simpleProvider =
				(IRawElementProviderSimple) provider;
			IntPtr providerHandle = (IntPtr) simpleProvider.GetPropertyValue (AutomationElementIdentifiers.NativeWindowHandleProperty.Id);
			pointerProviderMapping.Remove (providerHandle);
			
			windowProviders--;
			if (windowProviders == 0)
				appMonitor.Quit ();
		}
		
		private void HandleNewButtonControlType (IRawElementProviderSimple provider)
		{
			ParentAdapter parentObject = GetParentAdapter (provider);
			
			if (parentObject.Role == Atk.Role.ScrollBar)
				return;

			Button atkButton = new Button (provider);
			providerAdapterMapping [provider] = atkButton;
			
			parentObject.AddOneChild (atkButton);
			parentObject.AddRelationship (Atk.RelationType.Embeds,
			                              atkButton);
		}
		
		private void HandleNewLabelControlType (IRawElementProviderSimple provider)
		{
			ParentAdapter parentObject = GetParentAdapter (provider);
			
			TextLabel atkLabel = new TextLabel (provider);
			providerAdapterMapping [provider] = atkLabel;
			
			parentObject.AddOneChild (atkLabel);
			parentObject.AddRelationship (Atk.RelationType.Embeds,
			                              atkLabel);
		}
		
		private void HandleNewCheckBoxControlType (IRawElementProviderSimple provider)
		{
			ParentAdapter parentObject = GetParentAdapter (provider);
			
			CheckBox atkCheck = new CheckBox (provider);
			providerAdapterMapping [provider] = atkCheck;
			
			parentObject.AddOneChild (atkCheck);
			parentObject.AddRelationship (Atk.RelationType.Embeds,
			                              atkCheck);
		}
		
		private void HandleNewListControlType (IRawElementProviderSimple provider)
		{
			ParentAdapter parentObject = GetParentAdapter (provider);
			
			List atkList = new List ((IRawElementProviderFragmentRoot)provider);
			providerAdapterMapping [provider] = atkList;
			
			parentObject.AddOneChild (atkList);
			parentObject.AddRelationship (Atk.RelationType.Embeds,
			                              atkList);
		}

		private void HandleNewListItemControlType (IRawElementProviderSimple provider)
		{
			ParentAdapter parentObject = GetParentAdapter (provider);
			
			ListItem atkItem = new ListItem (provider);
			providerAdapterMapping [provider] = atkItem;
			
			parentObject.AddOneChild (atkItem);
			parentObject.AddRelationship (Atk.RelationType.Embeds,
			                              atkItem);
		}

		private void HandleNewComboBoxControlType (IRawElementProviderSimple provider)
		{
			ParentAdapter parentObject = GetParentAdapter (provider);

			ComboBox atkCombo = new ComboBox ((IRawElementProviderFragmentRoot)provider);
			providerAdapterMapping [provider] = atkCombo;
			
			parentObject.AddOneChild (atkCombo);
			parentObject.AddRelationship (Atk.RelationType.Embeds,
			                              atkCombo);
		}
		
		private void HandleNewStatusBarControlType (IRawElementProviderSimple provider)
		{
			ParentAdapter parentObject = GetParentAdapter (provider);
			
			StatusBar atkStatus;
			if (provider is IGridProvider)
				atkStatus = new StatusBarWithGrid (provider);
 			else atkStatus = new StatusBar (provider);
			providerAdapterMapping [provider] = atkStatus;
			
			parentObject.AddOneChild (atkStatus);
			parentObject.AddRelationship (Atk.RelationType.Embeds,
			                              atkStatus);
		}
		
		private void HandleNewProgressBarControlType (IRawElementProviderSimple provider)
		{
			ParentAdapter parentObject = GetParentAdapter (provider);
			
			ProgressBar atkProgress;
			if (provider is IGridProvider)
				atkProgress = new ProgressBar (provider);
 			else atkProgress = new ProgressBar (provider);
			providerAdapterMapping [provider] = atkProgress;
			
			parentObject.AddOneChild (atkProgress);
			parentObject.AddRelationship (Atk.RelationType.Embeds,
			                              atkProgress);
		}
		
		private void HandleNewScrollBarControlType (IRawElementProviderSimple provider)
		{
			ParentAdapter parentObject =
				GetParentAdapter (provider);
			
			ScrollBar atkScroll = new ScrollBar (provider);
			providerAdapterMapping [provider] = atkScroll;
			
			parentObject.AddOneChild (atkScroll);
			parentObject.AddRelationship (Atk.RelationType.Embeds,
			                              atkScroll);
		}
		
		private void HandleNewGroupControlType (IRawElementProviderSimple provider)
		{
			ParentAdapter parentObject =
				GetParentAdapter (provider);
			
			Pane atkPane = new Pane (provider);
			providerAdapterMapping [provider] = atkPane;
			
			parentObject.AddOneChild (atkPane);
			parentObject.AddRelationship (Atk.RelationType.Embeds,
			                              atkPane);
		}
		
		private void HandleNewRadioButtonControlType (IRawElementProviderSimple provider)
		{
			ParentAdapter parentObject = GetParentAdapter (provider);
			
			RadioButton atkRadio = new RadioButton (provider);
			providerAdapterMapping [provider] = atkRadio;
			
			parentObject.AddOneChild (atkRadio);
			parentObject.AddRelationship (Atk.RelationType.Embeds,
			                              atkRadio);
		}
		
		private void HandleNewSpinnerControlType (IRawElementProviderSimple provider)
		{
			ParentAdapter parentObject = GetParentAdapter (provider);
			
			Adapter atkSpinner;
			if (provider.GetPatternProvider (SelectionPatternIdentifiers.Pattern.Id) != null)
				atkSpinner = new List ((IRawElementProviderFragmentRoot)provider);
 			else
				atkSpinner = new Spinner (provider);
			providerAdapterMapping [provider] = atkSpinner;
			
			parentObject.AddOneChild (atkSpinner);
			parentObject.AddRelationship (Atk.RelationType.Embeds,
			                              atkSpinner);
		}
		
//		private void HandleNewEditControlType (IRawElementProviderSimple provider)
//		{
//			ParentAdapter parentObject = GetParentAdapter (provider);
//			
//			Adapter atkEdit=  new EditableTextBoxEntry ((IRawElementProviderFragmentRoot)provider);
//			providerAdapterMapping [provider] = atkEdit;
//			
//			parentObject.AddOneChild (atkEdit);
//			parentObject.AddRelationship (Atk.RelationType.Embeds,
//			                              atkEdit);
//		}
		
#endregion
	}
}
