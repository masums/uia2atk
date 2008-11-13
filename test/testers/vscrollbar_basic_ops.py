#!/usr/bin/env python

##############################################################################
# Written by:  Cachen Chen <cachen@novell.com>
# Date:        08/04/2008
# Description: Test accessibility of vscrollbar widget 
#              Use the vscrollbarframe.py wrapper script
#              Test the samples/vscrollbar.py script
##############################################################################

# The docstring below  is used in the generated log file
"""
Test accessibility of vscrollbar widget
"""

# imports
import sys
import os

from strongwind import *
from vscrollbar import *
from helpers import *
from sys import argv
from os import path

app_path = None 
try:
  app_path = argv[1]
except IndexError:
  pass #expected

# open the vscrollbar sample application
try:
  app = launchVScrollBar(app_path)
except IOError, msg:
  print "ERROR:  %s" % msg
  exit(2)

# make sure we got the app back
if app is None:
  exit(4)

# just an alias to make things shorter
vsbFrame = app.vScrollBarFrame

#check vscrollbar's states list
statesCheck(vsbFrame.vscrollbar, "VScrollBar")

#mouse click scrollbar
vsbFrame.vscrollbar.mouseClick()
sleep(config.SHORT_DELAY)
vsbFrame.assertScrollbar(20)
#mouseClick action update label's text
vsbFrame.assertLabel(20)

#check hscrollbar's states list
statesCheck(vsbFrame.vscrollbar, "VScrollBar")

#keyCombo move scrollbar
vsbFrame.vscrollbar.keyCombo("Page_Down")
sleep(config.SHORT_DELAY)
vsbFrame.assertScrollbar(40)
#keyCombo action update label's text
vsbFrame.assertLabel(40)

#set value to 10
vsbFrame.valueScrollBar(10)
sleep(config.SHORT_DELAY)
vsbFrame.assertScrollbar(10)

#set value to 0
vsbFrame.valueScrollBar(0)
sleep(config.SHORT_DELAY)
vsbFrame.assertScrollbar(0)

#set value to 100
vsbFrame.valueScrollBar(100)
sleep(config.SHORT_DELAY)
vsbFrame.assertScrollbar(100)

#set value to -10, minimum value is 0
vsbFrame.valueScrollBar(-10)
sleep(config.SHORT_DELAY)
vsbFrame.assertScrollbar(-10)

#set value to 120, maximum value is 119
vsbFrame.valueScrollBar(120)
sleep(config.SHORT_DELAY)
vsbFrame.assertScrollbar(120)



print "INFO:  Log written to: %s" % config.OUTPUT_DIR

#close application frame window
vsbFrame.quit()
