@echo off
cd .\tools

cmd /c excelToLua.py
if %errorlevel% equ 0 (
	echo "                       "
	echo "                       "
	echo " LUA全部生成成功 "
	echo " 按任意键自动完成LUA复制"
	echo " 注意：请勿直接关闭本窗口！！！！"
	pause>nil
	cmd /c copy_to_luadata.bat
	exit
)
color fc
title 错误
echo "             错误总是在这个时候发生了             "
echo "             错误总是在这个时候发生了             "
echo "             错误总是在这个时候发生了             "
echo "                 根据上方提示修复！               "
pause