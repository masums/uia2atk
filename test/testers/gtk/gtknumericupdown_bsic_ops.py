#!/usr/bin/env python

##############################################################################
# Written by:  Cachen Chen <cachen@novell.com>
# Date:        09/08/2008
# Description: Test accessibility of numericupdown widget 
#              Use the numericupdownframe.py wrapper script
#              Test the samples/gtk/numericupdown.py script
##############################################################################

# The docstring below  is used in the generated log file
"""
Test accessibility of numericupdown widget
"""

# imports
import sys
import os

from strongwind import *
from gtknumericupdown import *
from helpers import *
from sys import argv
from os import path

app_path = None 
try:
  app_path = argv[1]
except IndexError:
  pass #expected

# open the numericupdown sample application
try:
  app = launchGtkNumericUpDown(app_path)
except IOError, msg:
  print "ERROR:  %s" % msg
  exit(2)

# make sure we got the app back
if app is None:
  exit(4)

# just an alias to make things shorter
nudFrame = app.gtkNumericUpDownFrame


#check numericupdown's states list
statesCheck(nudFrame.numericupdown0, "NumericUpDown", add_states=["focused"])

#move focused back to numericupdown0, type numerber into numericupdown0 which 
#is editable, check Value and Text
nudFrame.numericupdown0.mouseClick()
nudFrame.numericupdown0.typeText("20")
nudFrame.keyCombo("Return", grabFocus=False)
sleep(config.SHORT_DELAY)
nudFrame.assertValue(nudFrame.numericupdown0, 20)
nudFrame.assertText(nudFrame.numericupdown0, "20")
#enter text from accerciser, press 'return' in app widget to confirm enter 
nudFrame.enterTextValue(nudFrame.numericupdown0, "10")
nudFrame.keyCombo("Return", grabFocus=False)
sleep(config.SHORT_DELAY)
nudFrame.assertValue(nudFrame.numericupdown0, 10)
nudFrame.assertText(nudFrame.numericupdown0, "10")

#movo focused to numericupdown0, then set numericupdown's value to 0
nudFrame.numericupdown0.mouseClick()
nudFrame.valueNumericUpDown(nudFrame.numericupdown0, 0)
sleep(config.SHORT_DELAY)
nudFrame.assertValue(nudFrame.numericupdown0, 0)
nudFrame.assertText(nudFrame.numericupdown0, "0")

#set numericupdown's value to 100
nudFrame.valueNumericUpDown(nudFrame.numericupdown0, 100)
sleep(config.SHORT_DELAY)
nudFrame.assertValue(nudFrame.numericupdown0, 100)
nudFrame.assertText(nudFrame.numericupdown0, "100")

#set numericupdown's value to maximumValue
nudFrame.valueNumericUpDown(nudFrame.numericupdown0, nudFrame.maximumValue)
sleep(config.SHORT_DELAY)
nudFrame.assertValue(nudFrame.numericupdown0, nudFrame.maximumValue)
nudFrame.assertText(nudFrame.numericupdown0, "119")

#set numericupdown's value to maximumValue+1
nudFrame.valueNumericUpDown(nudFrame.numericupdown0, nudFrame.maximumValue + 1)
sleep(config.SHORT_DELAY)
nudFrame.assertValue(nudFrame.numericupdown0, nudFrame.maximumValue + 1)
nudFrame.assertText(nudFrame.numericupdown0, "119")

#set numericupdown's value to minimumValue
nudFrame.valueNumericUpDown(nudFrame.numericupdown0, nudFrame.minimumValue)
sleep(config.SHORT_DELAY)
nudFrame.assertValue(nudFrame.numericupdown0, nudFrame.minimumValue)
nudFrame.assertText(nudFrame.numericupdown0, "0")

#set numericupdown's value to minimumValue-1
nudFrame.valueNumericUpDown(nudFrame.numericupdown0, nudFrame.minimumValue - 1)
sleep(config.SHORT_DELAY)
nudFrame.assertValue(nudFrame.numericupdown0, nudFrame.minimumValue - 1)
nudFrame.assertText(nudFrame.numericupdown0, "0")

#test press Up/Down action to check Text and Value by keyCombo to 
#numericupdown0 which increment value is 20
nudFrame.keyCombo("Up", grabFocus=False)
sleep(config.SHORT_DELAY)
nudFrame.assertValue(nudFrame.numericupdown0, nudFrame.minimumValue + 10)
nudFrame.assertText(nudFrame.numericupdown0, "10")

nudFrame.keyCombo("Down", grabFocus=False)
sleep(config.SHORT_DELAY)
nudFrame.assertValue(nudFrame.numericupdown0, nudFrame.minimumValue)
nudFrame.assertText(nudFrame.numericupdown0, "0")

print "INFO:  Log written to: %s" % config.OUTPUT_DIR

#close application frame window
nudFrame.quit()
