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
// Copyright (c) 2009 Novell, Inc. (http://www.novell.com) 
// 
// Authors: 
//      Andres G. Aragoneses <aaragoneses@novell.com>
// 

using System;

using System.Windows.Automation.Provider;

namespace UiaAtkBridge
{
	/// <summary>
	/// This empty class serves as just the needed bit for the parent menu
	/// (menu with children) to implement *officially* Atk.Selection. Its
	/// implementation is however inherited from Menu (but can't be exposed
	/// there because MenuItem should not expose it)
	/// </summary>
	
	public class ParentMenu : MenuItem, Atk.SelectionImplementor
	{
		public ParentMenu (IRawElementProviderSimple provider) : base (provider)
		{
			Role = Atk.Role.Menu;
		}

		internal override void AddOneChild (Atk.Object child)
		{
			base.AddChildToParent (child);
		}
	}
}
