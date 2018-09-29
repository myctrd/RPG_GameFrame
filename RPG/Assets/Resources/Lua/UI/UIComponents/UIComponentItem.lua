local self = {}
self.__index = self

local UIName = {
	"frame",
	"img_icon",
	"txt_count",
	"img_equipped",
}

local function UpdateItemInfo()
	if self.params.itemType == 1 then
		local data = GlobalHooks.dataReader:FindData("item", self.params.id)
		if self["img_equipped"] then
			self["img_equipped"]:SetActive(false)
		end
		if self["img_icon"] then
			self["img_icon"]:SetSprite("Item/"..data["ICON"])
		end
		if self["txt_count"] then
			if self.params.amount then
				self["txt_count"]:SetText(tostring(self.params.amount))
			end
		end
		if self["frame"] then
			self["frame"]:SetSprite("Common/Frame_"..data["QUALITY"])
			self["frame"]:AddListener(function()
				GlobalHooks.openUI:InitUIComponent("UIComponentItemInfo", nil, 2, {index = 1, itemData = data})
			end)
		end
	else
		local data = GlobalHooks.dataReader:FindData("equip", self.params.id)
		local slot = tonumber(data["SLOT"])
		local playerData = CS.LuaCallCSUtils.GetPlayerData(GlobalHooks.openUI.roleInfo.id)
		if self["img_equipped"] then
			self["img_equipped"]:SetActive(playerData.equip[slot - 1] == tonumber(self.params.id))
		end
		if self["img_icon"] then
			self["img_icon"]:SetSprite("Item/"..data["ICON"])
		end
		if self["txt_count"] then
			self["txt_count"]:SetText("")
		end
		
		if self["frame"] then
			self["frame"]:SetSprite("Common/Frame_"..data["QUALITY"])
			self["frame"]:AddListener(function()
				GlobalHooks.openUI:InitUIComponent("UIComponentItemInfo", nil, 2, {index = 2, itemData = data})
			end)
		end
	end
end

local function FindUI()
	for i = 1, #UIName do
		self[UIName[i]] = self.ui:GetChild(UIName[i])
	end
	UpdateItemInfo()
end

function self:InitUI(name, obj, sort, params)
	self.params = params
	self.ui = CS.UIManager.m_instance:LoadComponent(name, obj, sort)
	FindUI()
end

return self