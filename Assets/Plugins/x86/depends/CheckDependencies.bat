set thisdir=%CD%
set PATH=%thisdir%\..\dependencies;%PATH%

depends.exe /c /f1 /ot:RenderDependencies.log ..\TrueSkyPluginRender_MT.dll
depends.exe /c /f1 /ot:UIDependencies.log ..\TrueSkyUI_MD.dll