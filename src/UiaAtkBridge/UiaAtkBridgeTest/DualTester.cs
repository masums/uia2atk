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

using NUnit.Framework;

namespace UiaAtkBridgeTest
{
	
	public interface IAtkTester
	{
		// how to do this interace method? invalid example:
		object GetAtkObjectThatImplementsInterface <I> ();
	}
	
	[TestFixture]
	public class DualTester {
		
		IAtkTester[] typesToTest =
			new IAtkTester[] { //new BridgeTester (), 
								new GailTester () };
		
		public static string Text {
			get { return "text_test"; }
		}
		
		[Test]
		public void AtkTextImplementor ()
		{
			foreach (IAtkTester tester in typesToTest)
			{
				Atk.Text atkText = (Atk.Text)
					tester.GetAtkObjectThatImplementsInterface <Atk.Text> ();
				Assert.AreEqual (0, atkText.CaretOffset, "CaretOffset");
				Assert.AreEqual (Text.Length, atkText.CharacterCount, "CharacterCount");
				Assert.AreEqual (Text[0], atkText.GetCharacterAtOffset (0), "GetCharacterAtOffset");
				Assert.AreEqual (Text, atkText.GetText (0, Text.Length), "GetText");
				
				//any value
				Assert.AreEqual (false, atkText.SetCaretOffset (-1));
				Assert.AreEqual (false, atkText.SetCaretOffset (0));
				Assert.AreEqual (false, atkText.SetCaretOffset (1));
				Assert.AreEqual (false, atkText.SetCaretOffset (15));
				
				// don't do this until bug#393565 is fixed:
				//Assert.AreEqual (typeof(Atk.TextAttribute), atkText.DefaultAttributes[0].GetType());
				
			}
		}
	}
}
