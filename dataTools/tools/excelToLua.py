import os
import os.path
import sys
import codecs
import xlrd
import shutil
lua_data_dir = "../lua"

def FloatToString (aFloat):
    if len(str(aFloat)) == 0:
        return '0';
    if type(aFloat) != float:
        print aFloat+" - 数字类型格式错误"
        os.system("pause")
        return ""
    strTemp = str(aFloat)
    strList = strTemp.split(".")
    if len(strList) == 1 :
        return strTemp
    else:
        if strList[1] == "0" :
            return strList[0]
        else:
            return strTemp

def StringToString (aStr):
    if type(aStr) != float:
        return str(aStr)
    strTemp = str(aStr)
    strList = strTemp.split(".")
    if len(strList) == 1 :
        return strTemp
    else:
        if strList[1] == "0" :
            return strList[0]
        else:
            return strTemp

def haveNext(table, col, row, max_cols):
    for c in range(col, max_cols):
        objType = str(table.cell_value(row, c));
        if(objType.find("@") == -1):
            return 1
    return 0

def table2lua(table, jsonfilename, main_key=None, con_str=None,uncon_str=None):

    con_array = []
    uncon_array = []
    if con_str:
        con_array = con_str.split('|')
    if uncon_str:
        uncon_array = uncon_str.split('|')

    nrows = table.nrows
    ncols = table.ncols
    if os.path.exists(jsonfilename):
        print 'error : ',jsonfilename,'already exist!'
        exit(1)
    f = codecs.open(jsonfilename,"w","utf-8")
    f.write(u"return {\n")
    for r in range(1,nrows):
        #if r == 2:
        #    continue
        line = u"{"
        #f.write(u"\t{ ")
        for c in range(ncols):
            strCellValue = u""
            objName = str(table.cell_value(1,c))
            objType = str(table.cell_value(2,c))
            objStr = table.cell_value(r,c)
            if(objName.find("@") >= 0):
                continue

            if len(con_array) > 0:
                check = False
                for it in con_array:
                    if it == objName:
                        check = True
                        break
                if not check:
                    continue       
            elif len(uncon_array) > 0:
                check = True
                for it in uncon_array:
                    if it == objName:
                        check = False
                        break
                if not check:
                    continue     
                     
            if r == 0 or r == 1:
                if type(objStr) == float:
                    firstCol = str(table.cell_value(r,0))
                    print 'Type Error: (row:%s,col:%s)' % (firstCol,objName)
                    exit(1)
                strCellValue = u"\""  + objStr + u"\""
            else:
                if cmp(objType, "NUMBER") == 0 or cmp(objType, "FLOAT") == 0:
                    strCellValue = FloatToString(objStr)
                    if strCellValue == '':
                        firstCol = str(table.cell_value(r,0))
                        print 'Type Error: (row:%s,col:%s)' % (firstCol,objName)
                        exit(1)
                else:
                    strvalue = StringToString(objStr).strip();
                    strvalue = "".join(strvalue.split("\n"))
                    strCellValue = u"\""  + strvalue + u"\""
            if main_key and main_key == objName:
                line = line + strCellValue
                #key row
                if r == 1:
                    line = u"[\"_key_\"] = " + line
                else:
                    line = u"[" + strCellValue + u"] = " + line
            else:
                line = line + strCellValue
            line += u","
            # if c < ncols-1:
            #     isNext = haveNext(table, c+1, 1, ncols)
            #     if isNext != 0:                   
            #f.write(strCellValue)
        line = line + u"}"
        f.write(line)
        # f.write(u" }")
        # if r < nrows-1:
        #     f.write(u",")
        # f.write(u"\n")
        f.write(u",\n")
    f.write(u"}")
    f.close()
    print "Create ",jsonfilename," OK"
    return


def excelTolua(key, main_key=None,con_str=None,uncon_str=None,dirstr=None):
    print 'begin trans excel:' + key
    data = xlrd.open_workbook(key.split(':')[0])
    if not dirstr:
        dirstr = lua_data_dir

    for table in data.sheets():
        if key.split(':')[1] == table.name:
            #do
            destfilename = os.path.join(dirstr, table.name + ".lua")
            table2lua(table,destfilename,main_key,con_str,uncon_str)
            return table.name
    print 'error : ',key,'not exist!'
    exit(1)

def tag2luaTable(tagname,taglist):
    ret = u"{"
    for name in taglist:
        v = u"\"" + tagname + u"." + name + u"\","
        ret = ret + v
    ret = ret + u"}"
    return ret

if __name__ == '__main__':
    reload(sys)
    sys.setdefaultencoding( "utf-8" )
    
    if os.path.isdir(lua_data_dir):
        shutil.rmtree(lua_data_dir)
    os.mkdir(lua_data_dir)

    data = xlrd.open_workbook("LuaConfig.xlsm")
    map_tag = dict()
    for table in data.sheets():
        if table.name == 'LuaConfig':
            #lua
            #Path Key Export Hide
            for r in range(3,table.nrows):
                key = table.cell_value(r,0)
                main_key = table.cell_value(r,1)
                con_str = table.cell_value(r,2)
                uncon_str = table.cell_value(r,3)
                tag_str = table.cell_value(r,4)

                dir_str = None 
                if tag_str != '':
                    dir_str = os.path.join(lua_data_dir, tag_str)
                    if not os.path.isdir(dir_str):
                        os.mkdir(dir_str)
                        map_tag[tag_str] = [] 
                tname = excelTolua(key,main_key,con_str,uncon_str,dir_str)
                if dir_str:
                    map_tag[tag_str].append(tname)
    if len(map_tag) > 0:
        mfilename = os.path.join(lua_data_dir, "_merge_.lua")
        f = codecs.open(mfilename,"w","utf-8")
        f.write(u"return {\n")

        for k,v in map_tag.items():
            line = k + u" = " + tag2luaTable(k,v) + u",\n"
            f.write(line)
        f.write(u"}")
        f.close()
    #os.system('echo Explorer to lua path')
    #os.system('exit')
    #os.system('explorer ' + lua_data_dir)

