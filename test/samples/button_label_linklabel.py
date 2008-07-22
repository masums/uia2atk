#!/usr/bin/env ipy

##############################################################################
# Written by:  Calen Chen <cachen@novell.com>
# Date:        03/11/2008
# Description: This is a test application sample for winforms control:
#              Button
#              Label
#              LinkLabel
##############################################################################

# The docstring below is used in the generated log file
"""
This sample will show "Button", "Label" and "LinkLabel" controls in the form.
It can be used for Autotest tools(e.g. Strongwind) to test the behaviors of controls.
"""


import clr

clr.AddReference('System.Windows.Forms')
clr.AddReference('System.Drawing')

from System.Drawing import *
from System.Windows.Forms import *
import System


class RunApp(Form):
    """Button, Label and LinkLabel controls class"""

    def __init__(self):
        """RunApp class init function."""

        self.count = 0
        self.Text = "Button_Label_LinkLabel controls"

        # set up Label control
        self.label = Label()
        self.label.Text = "there is nothing now."
        self.label.Location = Point(10,10)
        self.label.AutoSize = True

        # set up Button1 control
        self.button1 = Button()
        self.button1.Name = "button1"
        self.button1.Text = "button1"
        self.button1.Location = Point(10,40)
        self.button1.BackColor = Color.Green
        self.button1.ForeColor = Color.Red
        self.button1.Click += self.button1_click
        self.button1.Cursor = Cursors.Hand

        #set up Button2 control:
        self.button2 = Button()
        self.button2.Name = "button2"
        self.button2.Text = "button2"
        self.button2.Location = Point(10,70)
        self.button2.BackColor = Color.Green
        self.button2.ForeColor = Color.Red
        self.button2.Click += self.button2_click
        self.button1.Cursor = Cursors.Hand

        #set up Enabled Button3 control:
        self.button3 = Button()
        self.button3.Name = "button3"
        self.button3.Text = "button3"
        self.button3.Location = Point(10,100)
        self.button3.ForeColor = Color.Red
        self.button3.Enabled = False

        # set up LinkLabel control
        self.linklabel1 = LinkLabel()
        self.linklabel1.Location = Point(10,160)
        self.linklabel1.AutoSize = True
        self.linklabel1.DisabledLinkColor = Color.Red
        self.linklabel1.VisitedLinkColor = Color.Blue
        self.linklabel1.LinkBehavior = LinkBehavior.HoverUnderline
        self.linklabel1.LinkColor = Color.Navy
        self.linklabel1.TabIndex = 0
        self.linklabel1.TabStop = True
        self.linklabel1.Links[0].Visited = True
        self.linklabel1.Text = "openSUSE:www.opensuse.org \n\n webmail:gmail.novell.com"
        self.linklabel1.Links.Add(9, 16, "www.opensuse.org")
        self.linklabel1.Links.Add(35, 16, "gmail.novell.com")
        self.linklabel1.LinkClicked += self.linklabel_clicked
        self.linklabel1.Links[1].Enabled = False
        
        # add controls
        self.Controls.Add(self.label)
        self.Controls.Add(self.button1)
        self.Controls.Add(self.button2)
        self.Controls.Add(self.button3)
        self.Controls.Add(self.linklabel1)
    
    def button1_click(self, sender, event):
        MessageBox.Show("successful clicked me", "message")

    def button2_click(self, sender, event):
        self.count += 1
        self.label.Text = "You have clicked me %s times" % self.count

    def linklabel_clicked(self, sender, LinkClicked):
        self.linklabel1.Links[0].Visited = True
        self.linklabel1.Links[1].Visited = True
        target = self.linklabel1.Links[0].LinkData

        if (target.StartsWith("www")):
            System.Diagnostics.Process.Start(target)
        else:
            MessageBox.Show("Item clicked: " + target)

form = RunApp()
Application.Run(form)
