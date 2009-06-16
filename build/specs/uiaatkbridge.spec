#
# spec file for package UiaAtkBridge
#
# Copyright (c) 2008 SUSE Linux Products GmbH, Nuernberg, Germany.
# This file and all modifications and additions to the pristine
# package are under the same license as the package itself.
#       
# Please submit bugfixes or comments via http://bugs.opensuse.org/ 
#            
# norootforbuild 
# 

Name:           uiaatkbridge
Version:	1.0
Release:	1
License:        MIT
Group:          System/Libraries
URL:		http://www.mono-project.com/Accessibility
Source0:        %{name}-%{version}.tar.bz2
BuildRoot:      %{_tmppath}/%{name}-%{version}-build
Requires:	mono-core >= 2.4 gtk-sharp2 >= 2.12.8
Requires:	mono-uia mono-winfxcore at-spi >= 1.22.0 
BuildRequires:	mono-devel >= 2.4 gtk-sharp2 >= 2.12.8
BuildRequires:	mono-uia mono-uia-devel mono-winfxcore
BuildRequires:  atk-devel gtk2-devel

Summary:        Bridge between UIA providers and ATK

%description
The bridge contains adapter Atk.Objects that wrap UIA providers.  Adapter
behavior is determined by provider ControlType and supported pattern interfaces.
The bridge implements interfaces from UIAutomationBridge which allow the UI
Automation core to send it automation events and provider information.

%prep
%setup -q

%build
%configure --disable-tests
make

%install
make DESTDIR=%{buildroot} install

#file we don't care about
rm -f %{buildroot}/%_libdir/uiaatkbridge/libbridge-glue.a
rm -f %{buildroot}/%_libdir/uiaatkbridge/libbridge-glue.la

%clean
rm -rf %{buildroot}

%files
%defattr(-,root,root)
%doc COPYING README NEWS
%dir %_libdir/uiaatkbridge
%_libdir/uiaatkbridge/UiaAtkBridge.dll*
%_libdir/uiaatkbridge/libbridge-glue.so*
%_prefix/lib/mono/gac/UiaAtkBridge


%post -p /sbin/ldconfig

%postun -p /sbin/ldconfig

%changelog
