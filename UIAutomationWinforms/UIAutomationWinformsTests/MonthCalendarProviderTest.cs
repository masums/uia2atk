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
//      Brad Taylor <brad@getcoded.net>
// 

using System;
using System.Threading;
using System.Windows.Forms;
using System.Globalization;
using System.Windows.Automation;
using System.Windows.Automation.Provider;

using Mono.UIAutomation.Winforms;

using NUnit.Framework;

namespace MonoTests.Mono.UIAutomation.Winforms
{
	[TestFixture]
	public class MonthCalendarProviderTest : BaseProviderTest
	{
		[SetUp]
		public override void SetUp ()
		{
			base.SetUp ();
			
			oldCulture = Thread.CurrentThread.CurrentCulture;

			// Ensure that we're in the US locale so that we can
			// test Gregorian calendars.
			//
			// Regardless, Mono doesn't support anything else at
			// the moment, but let's just be cautious.
			Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
			currentCalendar = Thread.CurrentThread.CurrentCulture.Calendar;

			daysInWeek = (currentCalendar.AddWeeks (anyGivenSunday, 1) - anyGivenSunday).Days;

			calendar = (MonthCalendar) GetControlInstance ();
			Form.Controls.Add (calendar);
			Form.Show ();

			calendarProvider
				= ProviderFactory.GetProvider (calendar);
		}

		[TearDown]
		public override void TearDown ()
		{
			base.TearDown ();	

			// Restore previously set culture
			Thread.CurrentThread.CurrentCulture = oldCulture;
		}

		[Test]
		public void BasicPropertiesTest ()
		{
			TestProperty (calendarProvider,
			              AutomationElementIdentifiers.ControlTypeProperty,
			              ControlType.Calendar.Id);
			
			TestProperty (calendarProvider,
			              AutomationElementIdentifiers.LocalizedControlTypeProperty,
			              "calendar");
		}

		[Test]
		public void NavigationTest ()
		{
			IRawElementProviderFragmentRoot rootProvider
				= (IRawElementProviderFragmentRoot) calendarProvider;

			IRawElementProviderSimple child
				= rootProvider.Navigate (NavigateDirection.FirstChild);
			Assert.IsNotNull (child, "MonthCalendar has no children");

			TestProperty (child,
			              AutomationElementIdentifiers.ControlTypeProperty,
			              ControlType.DataGrid.Id);
			TestProperty (child,
			              AutomationElementIdentifiers.LocalizedControlTypeProperty,
			              "data grid");

			IRawElementProviderSimple header
				= ((IRawElementProviderFragmentRoot) child).Navigate (
					NavigateDirection.FirstChild);
			Assert.IsNotNull (header, "MonthCalendarDataGrid has no children");

			TestProperty (header,
			              AutomationElementIdentifiers.ControlTypeProperty,
			              ControlType.Header.Id);
			TestProperty (header,
			              AutomationElementIdentifiers.LocalizedControlTypeProperty,
			              "header");

			int numChildren = 0;

			IRawElementProviderSimple headerItem
				= ((IRawElementProviderFragmentRoot) header).Navigate (
						NavigateDirection.FirstChild);
			while (headerItem != null) {
				TestProperty (headerItem,
					      AutomationElementIdentifiers.ControlTypeProperty,
					      ControlType.HeaderItem.Id);
				TestProperty (headerItem,
					      AutomationElementIdentifiers.LocalizedControlTypeProperty,
					      "header item");
				
				Assert.AreEqual (anyGivenSunday.AddDays (numChildren).ToString ("ddd"),
				                 headerItem.GetPropertyValue (AutomationElementIdentifiers.NameProperty.Id),
				                 "Day name in header is incorrect");
				
				numChildren++;
				headerItem = ((IRawElementProviderFragment) headerItem)
					.Navigate (NavigateDirection.NextSibling);
			}

			Assert.AreEqual (daysInWeek, numChildren, "Not returning the correct number of days in a week");
		}

		[Test]
		public void IGridProviderTest ()
		{
			TestGridProvider (calendarProvider);
		}

		[Test]
		public void DataGridIGridProviderTest ()
		{
			IRawElementProviderSimple provider
				= ((IRawElementProviderFragmentRoot) calendarProvider)
					.Navigate (NavigateDirection.FirstChild);
			TestGridProvider (provider);
		}

		public void TestGridProvider (IRawElementProviderSimple provider)
		{
			IGridProvider gridProvider = (IGridProvider)
				provider.GetPatternProvider (GridPatternIdentifiers.Pattern.Id);
			
			Assert.AreEqual (daysInWeek, gridProvider.ColumnCount);
			Assert.AreEqual (6, gridProvider.RowCount);

			DateTime date = calendar.GetDisplayRange (false).Start;
			for (int r = 0; r < 6; r++) {
				for (int c = 0; c < daysInWeek; c++) {
					IRawElementProviderSimple child
						= gridProvider.GetItem (r, c);
					
					Assert.AreEqual (date.Day.ToString (),
							 child.GetPropertyValue (AutomationElementIdentifiers.NameProperty.Id),
							 "Day name for grid item is incorrect");

					date = date.AddDays (1);
				}
			}
		}

		protected override Control GetControlInstance ()
		{
			return new MonthCalendar ();
		}

		private MonthCalendar calendar;
		private IRawElementProviderSimple calendarProvider;
		private Calendar currentCalendar;

		private CultureInfo oldCulture;
		private static int daysInWeek;

		private DateTime anyGivenSunday = new DateTime (2008, 12, 7);
	}
}
