
EXTRA_DIST =  

# Warning: This is an automatically generated file, do not edit!

if ENABLE_DEBUG
ASSEMBLY_COMPILER_COMMAND = gmcs
ASSEMBLY_COMPILER_FLAGS = -keyfile:$(top_srcdir)/mono-uia.snk -noconfig -codepage:utf8 -warn:4 -warnaserror -optimize+ -debug "-define:DEBUG"

ASSEMBLY = bin/Debug/UIAutomationWinformsTests.dll
ASSEMBLY_MDB = $(ASSEMBLY).mdb
COMPILE_TARGET = library
PROJECT_REFERENCES =  \
	../UIAutomationWinforms/bin/Debug/UIAutomationWinforms.dll
BUILD_DIR = bin/Debug

UIAUTOMATIONWINFORMS_DLL_SOURCE=../UIAutomationWinforms/bin/Debug/UIAutomationWinforms.dll
UIAUTOMATIONWINFORMSTESTS_DLL_MDB_SOURCE=bin/Debug/UIAutomationWinformsTests.dll.mdb
UIAUTOMATIONWINFORMSTESTS_DLL_MDB=$(BUILD_DIR)/UIAutomationWinformsTests.dll.mdb
UIAUTOMATIONWINFORMS_DLL_MDB_SOURCE=../UIAutomationWinforms/bin/Debug/UIAutomationWinforms.dll.mdb
UIAUTOMATIONWINFORMS_DLL_MDB=$(BUILD_DIR)/UIAutomationWinforms.dll.mdb

endif

if ENABLE_RELEASE
ASSEMBLY_COMPILER_COMMAND = gmcs
ASSEMBLY_COMPILER_FLAGS = -keyfile:$(top_srcdir)/mono-uia.snk -noconfig -codepage:utf8 -warn:4 -warnaserror -optimize+
ASSEMBLY = bin/Release/UIAutomationWinformsTests.dll
ASSEMBLY_MDB = 
COMPILE_TARGET = library
PROJECT_REFERENCES =  \
	../UIAutomationWinforms/bin/Release/UIAutomationWinforms.dll
BUILD_DIR = bin/Release

UIAUTOMATIONWINFORMS_DLL_SOURCE=../UIAutomationWinforms/bin/Release/UIAutomationWinforms.dll
UIAUTOMATIONWINFORMSTESTS_DLL_MDB=
UIAUTOMATIONWINFORMS_DLL_MDB=

endif

AL=al2
SATELLITE_ASSEMBLY_NAME=MonoTests.Mono.UIAutomation.Winforms.resources.dll

PROGRAMFILES = \
	$(UIAUTOMATIONWINFORMS_DLL) \
	$(UIAUTOMATIONWINFORMSTESTS_DLL_MDB) \
	$(UIAUTOMATIONWINFORMS_DLL_MDB)  


RESGEN=resgen2
	
all: $(ASSEMBLY) $(PROGRAMFILES)

FILES =  \
	AssemblyInfo.cs \
	BaseColorControlProviderTest.cs \
	BaseProviderTest.cs \
	ButtonProviderTest.cs \
	CheckBoxProviderTest.cs \
	CheckedListBoxProviderTest.cs \
	ComboBoxProviderTest.cs \
	ContextMenuProviderTest.cs \
	ContextMenuStripProviderTest.cs \
	DataGridProviderTest.cs \
	DataGridViewProviderTest.cs \
	DateTimePickerProviderTest.cs \
	DomainUpDownProviderTest.cs \
	ErrorProviderTest.cs \
	FormProviderTest.cs \
	FragmentControlProviderWrapperTest.cs \
	GroupBoxProviderTest.cs \
	HelpProviderTest.cs \
	LabelProviderTest.cs \
	LinkLabelProviderTest.cs \
	ListBoxProviderTest.cs \
	ListViewProviderTest.cs \
	MainMenuProviderTest.cs \
	MaskedTextBoxProviderTest.cs \
	MenuItemProviderTest.cs \
	MenuStripProviderTest.cs \
	MiscHierarchyTest.cs \
	MockBridge.cs \
	MonthCalendarProviderTest.cs \
	NumericUpDownProviderTest.cs \
	PanelProviderTest.cs \
	PictureBoxProviderTest.cs \
	PopupButtonPanelTest.cs \
	PrintPreviewControlProviderTest.cs \
	ProgressBarProviderTest.cs \
	RadioButtonProviderTest.cs \
	RichTextBoxProviderTest.cs \
	ScrollBarProviderTest.cs \
	SplitContainerProviderTest.cs \
	SplitterProviderTest.cs \
	StatusBarProviderTest.cs \
	StatusStripProviderTest.cs \
	TabControlProviderTest.cs \
	TabPageProviderTest.cs \
	TestHelper.cs \
	TextBoxBaseProviderTest.cs \
	TextBoxProviderTest.cs \
	TextRangeProviderTest.cs \
	ToolBarButtonProviderTest.cs \
	ToolBarProviderTest.cs \
	ToolStripButtonProviderTest.cs \
	ToolStripComboBoxProviderTest.cs \
	ToolStripDropDownItemProviderTest.cs \
	ToolStripLabelProviderTest.cs \
	ToolStripProgressBarProviderTest.cs \
	ToolStripProviderTest.cs \
	ToolStripSeparatorProviderTest.cs \
	ToolStripTextBoxProviderTest.cs \
	ToolTipProviderTest.cs \
	TrackBarProviderTest.cs \
	TreeViewProviderTest.cs \
	WebBrowserProviderTest.cs \
	WordTokenizerTest.cs 

DATA_FILES = 

RESOURCES = 

EXTRAS = 

REFERENCES =  \
	Accessibility \
	System \
	System.Data \
	System.Drawing \
	System.Xml \
	Mono.Posix \
	-pkg:mono-nunit \
	System.Windows.Forms \
	../UIAutomationWinforms/bin/Debug/UIAutomationWinforms.dll \
	$(MONO_UIA_LIBS)

DLL_REFERENCES = 

CLEANFILES = $(PROGRAMFILES) \
	bin/Debug/TestResult.xml

include $(top_srcdir)/Makefile.include

UIAUTOMATIONWINFORMS_DLL = $(BUILD_DIR)/UIAutomationWinforms.dll

$(eval $(call emit-deploy-target,UIAUTOMATIONWINFORMS_DLL))
$(eval $(call emit-deploy-target,UIAUTOMATIONWINFORMS_DLL_MDB))


$(eval $(call emit_resgen_targets))
$(build_xamlg_list): %.xaml.g.cs: %.xaml
	xamlg '$<'

$(ASSEMBLY) $(ASSEMBLY_MDB): $(build_sources) $(build_resources) $(build_datafiles) $(DLL_REFERENCES) $(PROJECT_REFERENCES) $(build_xamlg_list) $(build_satellite_assembly_list)
	@mkdir -p $(shell dirname $(ASSEMBLY))
	$(ASSEMBLY_COMPILER_COMMAND) $(ASSEMBLY_COMPILER_FLAGS) -out:$(ASSEMBLY) -target:$(COMPILE_TARGET) $(build_sources_embed) $(build_resources_embed) $(build_references_ref)
	@chmod +x providertest.sh
