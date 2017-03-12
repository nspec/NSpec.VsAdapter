@ECHO OFF
REM archive-branch.bat 
REM Usage: archive-branch <your-branch-name>
REM NOTE: Run this script from a copy outside of working repository.

:TopOfScript
ECHO.
ECHO Starting...

ECHO.
ECHO * 1/7. Get local branch from remote, if needed
ECHO.
git checkout %1
IF %ERRORLEVEL% NEQ 0 GOTO :Err

ECHO.
ECHO * 2/7. Log head commit
ECHO.
git log -1
IF %ERRORLEVEL% NEQ 0 GOTO :Err

ECHO.
ECHO * 3/7. Go back to master
ECHO.
git checkout master
IF %ERRORLEVEL% NEQ 0 GOTO :Err

ECHO.
ECHO * 4/7. Create local tag
ECHO.
git tag archive/%1 %1
git tag -l archive/%1
IF %ERRORLEVEL% NEQ 0 GOTO :Err

ECHO.
ECHO * 5/7. Create remote tag
ECHO.
git push origin archive/%1
IF %ERRORLEVEL% NEQ 0 GOTO :Err

ECHO.
ECHO * 6/7. Delete local branch
ECHO.
git branch -d %1
IF %ERRORLEVEL% NEQ 0 GOTO :Err

ECHO.
ECHO * 7/7. Delete remote branch
ECHO.
git push origin --delete %1
IF %ERRORLEVEL% NEQ 0 GOTO :Err

GOTO :EndOfScript

:Err
ECHO Errors encountered during execution.  Last command exited with status: %ERRORLEVEL%.

:EndOfScript
ECHO.
ECHO Finished
