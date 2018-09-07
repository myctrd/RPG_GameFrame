local self = {}
self.__index = self

local itemManager = require 'Manager.item'

local UIName = {
	"btn_back",
	"img_icon",
	"txt_name",
	"txt_pro",
	"txt_des",
	"txt_buff",
	"btn_op",
	"txt_op",
}

local function FindUI()
	for i = 1, #UIName do
		self[UIName[i]] = self.ui:GetChild(UIName[i])
	end
	self["btn_back"]:AddListener(function()
        self.ui:Close()
    end)
end

local function SubAttrStr(str, attr, value)
	if str == "" then
		str = GetText(attr).." + "..value
	else
		str = str .. "\n"..GetText(attr).." + "..value
	end
	return str
end

local function ShowEquipInfo(data)
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
		self["txt_des"]:SetText(str.."\n"..GetText(data["DESC"]))
	else
		self["txt_des"]:SetText(GetText(data["DESC"]))
	end
end

local function InitItemInfo(index, data)
	self["txt_name"]:SetText(GetText(data["NAME"]))
	self["img_icon"]:SetSprite("Item/"..data["ICON"])
	if index == 1 then
		self["txt_des"]:SetText(GetText(data["DESC"]))
		self["txt_pro"]:SetText("")
		if(data["ITEMTYPE"] == "1")then
			self["btn_op"]:SetActive(false)
		elseif(data["ITEMTYPE"] == "2")then
			self["txt_op"]:SetText(GetText("Use"))
			self["btn_op"]:AddListener(function()
				itemManager:ConsumeItem(data["ID"], 1)
				self.ui:Close()
			end)
			
		end
	elseif index == 2 then
		local playerData = CS.LuaCallCSUtils.GetPlayerData()
		if playerData.equip[tonumber(data["SLOT"]) - 1] == tonumber(data["ID"]) then
			self["txt_op"]:SetText("Take off")
		else
			self["txt_op"]:SetText(GetText("Equip"))
			if playerData.m_Pro == tonumber(data["PRO"]) then
				self["btn_op"]:AddListener(function()
					GlobalHooks.uiUitls:ShowFloatingMsg(GetText("Tip_EquipSuccess"))
					self.ui:Close()
				end) 
			else
				self["btn_op"]:AddListener(function()
					GlobalHooks.uiUitls:ShowFloatingMsg(GetText("Tip_EquipFail"))
					self.ui:Close()
				end) 
			end
		end
		self["txt_pro"]:SetText(GetText(GlobalHooks.roleName[tonumber(data["PRO"])]))
		ShowEquipInfo(data)
	end
end

function self:InitUI(name, sort, params)
	self.params = params
	self.data = params.itemData
	self.index = params.index
	self.ui = CS.UIManager.m_instance:ShowOrCreatePanel(name, sort)
	
	FindUI()
	InitItemInfo(self.index, self.data)
end

return self