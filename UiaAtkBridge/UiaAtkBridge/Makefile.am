
EXTRA_DIST =  UiaAtkBridge.dll.config.in

ASSEMBLY_COMPILER_COMMAND = gmcs
ASSEMBLY_COMPILER_FLAGS = -noconfig -codepage:utf8 -warn:4 -optimize+ -debug "-define:DEBUG" -keyfile:$(top_srcdir)/mono-uia.snk -warnaserror

ASSEMBLY = bin/UiaAtkBridge.dll
ASSEMBLY_MDB = $(ASSEMBLY).mdb
COMPILE_TARGET = library
PROJECT_REFERENCES = 
BUILD_DIR = bin

UIAATKBRIDGE_DLL_MDB_SOURCE=bin/UiaAtkBridge.dll.mdb
UIAATKBRIDGE_DLL_MDB=$(BUILD_DIR)/UiaAtkBridge.dll.mdb

AL=al2
SATELLITE_ASSEMBLY_NAME=.resources.dll

PROGRAMFILES = \
	$(UIAATKBRIDGE_DLL_MDB)  


RESGEN=resgen2
	
all: $(ASSEMBLY) $(PROGRAMFILES) $(ASSEMBLY).config

FILES =  \
	ActionImplementorHelper.cs \
	Adapter.cs \
	AmbiDictionary.cs \
	AssemblyInfo.cs \
	AutomationBridge.cs \
	BaseTextImplementor.cs \
	Button.cs \
	CheckBoxButton.cs \
	ComboBox.cs \
	ComboBoxDropDown.cs \
	ComboBoxItem.cs \
	ComboBoxOptions.cs \
	ComboBoxOptionsTable.cs \
	ComponentAdapter.cs \
	ComponentImplementorHelper.cs \
	ComponentParentAdapter.cs \
	Container.cs \
	ContextMenu.cs \
	ContextMenuWindow.cs \
	DataGrid.cs \
	DataGridGroup.cs \
	EditableTextImplementorHelper.cs \
	ExpandCollapseButton.cs \
	HeaderItem.cs \
	Hyperlink.cs \
	ICanHaveSelection.cs \
	Image.cs \
	ImageImplementorHelper.cs \
	ITextImplementor.cs \
	List.cs \
	ListGroup.cs \
	ListItem.cs \
	Menu.cs \
	MenuBar.cs \
	MenuItem.cs \
	Monitor.cs \
	NamePropertyTextImplementor.cs \
	Pane.cs \
	ParentAdapter.cs \
	ParentMenu.cs \
	ProgressBar.cs \
	RadioButton.cs \
	RangeValueProviderTextImplementor.cs \
	ScrollBar.cs \
	SelectionProviderUserHelper.cs \
	Separator.cs \
	SimpleTreeItem.cs \
	Slider.cs \
	Spinner.cs \
	SplitButton.cs \
	SplitContainer.cs \
	Splitter.cs \
	Tab.cs \
	Table.cs \
	TableImplementorHelper.cs \
	TabPage.cs \
	TextBoxEntryView.cs \
	TextContainer.cs \
	TextImageLabel.cs \
	TextImplementorFactory.cs \
	TextLabel.cs \
	TextProviderTextImplementor.cs \
	ToggleButton.cs \
	ToolBar.cs \
	ToolTip.cs \
	TopLevelRootItem.cs \
	Tree.cs \
	TreeItem.cs \
	ValueProviderTextImplementor.cs \
	Window.cs \
	WrapperPanel.cs

DATA_FILES = 

RESOURCES = 

EXTRAS = 

REFERENCES =  \
	System  \
	System.Drawing  \
	Mono.Posix  \
	$(MONO_UIA_LIBS) \
	$(GTK_SHARP_20_LIBS)

DLL_REFERENCES = 

CLEANFILES = $(PROGRAMFILES)

install-data-local: gac-install

uninstall-local: gac-uninstall

include $(top_srcdir)/Makefile.include


$(ASSEMBLY).config: $(srcdir)/UiaAtkBridge.dll.config.in Makefile
	sed -e "s|\@expanded_libdir\@|@expanded_libdir@|" \
	    -e "s|\@GTK_MODULES_DIR\@|@GTK_MODULES_DIR@|" \
	    < $< > $@


$(eval $(call emit_resgen_targets))
$(build_xamlg_list): %.xaml.g.cs: %.xaml
	xamlg '$<'

$(ASSEMBLY) $(ASSEMBLY_MDB): $(build_sources) $(build_resources) $(build_datafiles) $(DLL_REFERENCES) $(PROJECT_REFERENCES) $(build_xamlg_list) $(build_satellite_assembly_list)
	mkdir -p $(shell dirname $(ASSEMBLY))
	$(ASSEMBLY_COMPILER_COMMAND) $(ASSEMBLY_COMPILER_FLAGS) -out:$(ASSEMBLY) -target:$(COMPILE_TARGET) $(build_sources_embed) $(build_resources_embed) $(build_references_ref)
