@echo off
pushd "%~dp0"

set build_path=.build
set disk_path=
set build_x64=""
set config="--config Debug"
set install_directory=""

if not "%1" == "" (set build_path=%1 
set disk_path=%~d1)

if not "%2" == "" (set install_directory="-DINSTALL_DIRECTORY="%2"")

if not "%3" == "" (set config="--config %3")

for %%i in (%*) do if /i "%%i"=="-x64" set build_x64="-A x64"

IF NOT EXIST %build_path% mkdir %build_path%

if not %disk_path% == "" (%disk_path%)

cd %build_path%

cmake %~dp0 %install_directory:~1,-1% %build_x64:~1,-1%

cmake --build . --target install %config:~1,-1%

popd 