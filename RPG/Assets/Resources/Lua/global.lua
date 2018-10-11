local m_language

function InitLanguage(language)
	m_language = language
end

function GetText(id)
	if m_language then
		local data = GlobalHooks.dataReader:FindData("localization", id)
		if data then
			return data[m_language]
		end
		return "";
	end
end

local function SubAttrStr(str, attr, value)
	if str == "" then
		str = GetText(attr).." + "..value
	else
		str = str .. "\n"..GetText(attr).." + "..value
	end
	return str
end

function LoadEquipDescAndBuff(txt, data)
	local buffs = GlobalHooks.uiUitls:StringSplit(data["BUFF"], ';')
	local str = ""
	for k, v in pairs(buffs)do
		local buff = GlobalHooks.uiUitls:StringSplit(v, ',')
		if(tonumber(buff[1]) == 1)then
			str = SubAttrStr(str, "Attr_MaxHp", buff[2])
		elseif(tonumber(buff[1]) == 2)then
			str = SubAttrStr(str, "Attr_MaxMp", buff[2])
		elseif(tonumber(buff[1]) == 3)then
			str = SubAttrStr(str, "Attr_Atk", buff[2])
		elseif(tonumber(buff[1]) == 4)then
			str = SubAttrStr(str, "Attr_Ap", buff[2])
		elseif(tonumber(buff[1]) == 5)then
			str = SubAttrStr(str, "Attr_Amr", buff[2])
		elseif(tonumber(buff[1]) == 6)then
			str = SubAttrStr(str, "Attr_Mr", buff[2])
		elseif(tonumber(buff[1]) == 7)then
			str = SubAttrStr(str, "Attr_Cr", math.floor(tonumber(buff[2]) * 100)).."%"
		elseif(tonumber(buff[1]) == 8)then
			str = SubAttrStr(str, "Attr_Cv", buff[2])
		end
	end
	if #buffs > 0 then
		txt:SetText(str.."\n"..GetText(data["DESC"]))
	else
		txt:SetText(GetText(data["DESC"]))
	end
end

