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
//	Brad Taylor <brad@getcoded.net>
// 

using System;
using System.Windows;
using System.Reflection;
using System.Windows.Automation;
using System.Windows.Automation.Text;
using System.Windows.Automation.Provider;
using SWF = System.Windows.Forms;

namespace Mono.UIAutomation.Winforms.Behaviors.TextBox
{
	// NOTE: This class also supports RichTextBox as they share pretty much
	// everything
	internal class TextProviderBehavior : ProviderBehavior, ITextProvider
	{
		#region Constructor
		
		public TextProviderBehavior (TextBoxProvider provider)
			: base (provider)
		{
		}

		#endregion
		
		#region ProviderBehavior: Specialization
		
		public override AutomationPattern ProviderPattern { 
			get { return TextPatternIdentifiers.Pattern; }
		}
		
		public override void Connect ()
		{
		}
		
		public override void Disconnect ()
		{
		}

		public override object GetPropertyValue (int propertyId)
		{
			if (propertyId == AutomationElementIdentifiers.IsPasswordProperty.Id) {
				if (Provider.Control is SWF.TextBox) {
					SWF.TextBox textbox = TextBox;
					return (textbox.UseSystemPasswordChar || (int) textbox.PasswordChar != 0);
				} else if (Provider.Control is SWF.RichTextBox) {
					return false;
				}
			}
			return base.GetPropertyValue (propertyId);
		}
		
		#endregion

		#region ITextProvider Members
		
		//TODO: We should connect the events to update this.text_range_provider?
		public ITextRangeProvider DocumentRange {
			get { 
				if (textRangeProvider == null) {
					textRangeProvider = new TextRangeProvider (this, 
					                                           (SWF.TextBoxBase) Provider.Control);
				}
				return textRangeProvider;
			}
		}
		
		public SupportedTextSelection SupportedTextSelection {
			get { return SupportedTextSelection.Single; }
		}

		public ITextRangeProvider[] GetSelection ()
		{
			if (SupportedTextSelection == SupportedTextSelection.None)
				throw new InvalidOperationException ();
				
			//TODO: Return null when system cursor is not present, how to?

			return new ITextRangeProvider [] { 
				new TextRangeProvider (this, TextBoxBase) };
		}
		
		public ITextRangeProvider[] GetVisibleRanges ()
		{
			Assembly asm = SwfAssembly;
			object doc = GetDocumentFromTextBoxBase (TextBoxBase);

			Type document_type = asm.GetType ("System.Windows.Forms.Document", false);
			if (document_type == null) {
				throw new Exception ("Internal Document class not found in System.Windows.Forms");
			}

			MethodInfo mi = document_type.GetMethod ("GetVisibleLineIndexes", BindingFlags.NonPublic | BindingFlags.Instance);
			if (mi == null) {
				throw new Exception ("GetVisibleLineIndexes method not found in Document class");
			}

			int start_line = -1, end_line = -1;
			object[] args = new object[] {
				TextBoxBase.Bounds, start_line, end_line
			};
			mi.Invoke (doc, args);
			start_line = (int)args[1];
			end_line = (int)args[2];

			ITextRangeProvider range = DocumentRange.Clone ();
			range.MoveEndpointByUnit (TextPatternRangeEndpoint.Start, TextUnit.Line, start_line);
			range.MoveEndpointByUnit (TextPatternRangeEndpoint.End, TextUnit.Line, end_line - start_line);

			return new ITextRangeProvider[] { range };
		}
		
		public ITextRangeProvider RangeFromChild (IRawElementProviderSimple childElement) 
		{
			if (childElement == null) {
				throw new ArgumentNullException ("childElement");
			}
			
			if (Provider.Control is SWF.TextBox) {
				// TextBox can't have children
				throw new InvalidOperationException ();
			}
			
			// TODO: RichTextBox code path
			return null;
		}
		
		public ITextRangeProvider RangeFromPoint (Point screenLocation)
		{
			Assembly asm = SwfAssembly;
			object doc = GetDocumentFromTextBoxBase (TextBoxBase);

			Type document_type = asm.GetType ("System.Windows.Forms.Document", false);
			if (document_type == null) {
				throw new Exception ("Internal Document class not found in System.Windows.Forms");
			}

			MethodInfo mi = document_type.GetMethod ("FindCursor", BindingFlags.NonPublic | BindingFlags.Instance);
			if (mi == null) {
				throw new Exception ("FindCursor method not found in Document class");
			}

			int index = -1;
			object[] args = new object[] {
				(int)screenLocation.X, (int)screenLocation.Y, index
			};

			mi.Invoke (doc, args);
			index = (int)args[2];

			// Return the degenerate range
			return (ITextRangeProvider) new TextRangeProvider (
				this, TextBoxBase, index, index);
		}
		
		#endregion

		private Assembly SwfAssembly {
			get {
				if (!attempted_swf_load) {
					swf_asm = Assembly.GetAssembly (typeof (SWF.TextBoxBase));
					attempted_swf_load = true;
				}
				return swf_asm;
			}
		}

		private SWF.TextBoxBase TextBoxBase {
			get { return (SWF.TextBoxBase)Provider.Control; }
		}

		// NOTE: If you use this, you need to check if it returns null,
		// as this class is reused for RichTextBox
		private SWF.TextBox TextBox {
			get { return Provider.Control as SWF.TextBox; }
		}
		
		private Assembly swf_asm = null;
		private bool attempted_swf_load = false;

		private ITextRangeProvider textRangeProvider;

		internal object GetDocumentFromTextBoxBase (SWF.TextBoxBase textbox)
		{
			// Copied from TextRangeProvider.cs
			// I couldn't think of a good way to share this without
			// introducing a cyclical dependency -- Brad
			// %-< -------------------------------------------------
			Type textbox_type = textbox.GetType ();
			FieldInfo textbox_fi = textbox_type.GetField ("document", BindingFlags.NonPublic | BindingFlags.Instance);
			if (textbox_fi == null) {
				throw new Exception ("document field not found in TextBoxBase");
			}

			return textbox_fi.GetValue (textbox);
			// ------------------------------------------------- >-%
		}
	}
}