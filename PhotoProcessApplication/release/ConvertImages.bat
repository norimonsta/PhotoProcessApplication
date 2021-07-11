 @ECHO OFF
SETLOCAL ENABLEEXTENSIONS ENABLEDELAYEDEXPANSION

   if [%1]==[] GOTO USAGE
   if [%2]==[] GOTO USAGE
   if [%3]==[] GOTO USAGE

   set INPUTDIR=%1
   set OUTPUTDIR=%2
   set SIZE=%3


PUSHD %INPUTDIR%
IF %ERRORLEVEL% NEQ 0 ECHO ERROR 1
If %ERRORLEVEL% NEQ 0 GOTO:EOF

IF %SIZE% EQU 0 GOTO:LOWRES
IF %SIZE% EQU 1 GOTO:HIGHRES

:HIGHRES

FOR /F "delims=" %%A IN ('dir /b *.jpg') DO (
echo %%A
magick %%A -quality 95 %OUTPUTDIR%\%%A
) 

IF %ERRORLEVEL% NEQ 0 Echo ERROR 2
If %ERRORLEVEL% NEQ 0 goto:eof
@echo High Res Export Complete
POPD
GOTO:END

:LOWRES

FOR /F "delims=" %%A IN ('dir /b *.jpg') DO (
if exist %OUTPUTDIR%\%%A (echo ERROR %%A) ELSE (
magick %%A -resize "1000x1000>" %OUTPUTDIR%\%%A)
echo %%A
)



IF %ERRORLEVEL% NEQ 0 Echo ERROR 3
If %ERRORLEVEL% NEQ 0 goto:eof

@echo Low Res Export Complete
POPD
GOTO:END


:END
ECHO Process Complete
CMD /C START "" "%~2"
exit

:USAGE
   echo
   echo Usage:
   echo ConvertImage.bat SourceDirectory DestinationDirectory Size
exit /B 1

ENDLOCAL
ECHO ON
