local self = {}
self.__index = self

local UIName = {
	"frame",
	"img_icon",
	"txt_name",
	"btn_select",
	"img_star_1",
	"img_star_2",
	"img_star_3",
	"img_star_4",
	"img_star_5",
}

local function UpdateItemInfo()
	local data = GlobalHooks.dataReader:FindData("equip", self.params.id)
	if self["img_icon"] then
		self["img_icon"]:SetSprite("Item/"..data["ICON"])
	end
	if self["txt_name"] then
		self["txt_name"]:SetText(GetText(data["NAME"]))
	end
	if self["frame"] then
		self["frame"]:SetSprite("Common/Frame_"..data["QUALITY"])
	end
	local star = tonumber(data["QUALITY"])
	for i = 1, 5 do
		self["img_star_"..i]:SetActive(i <= star)
	end
	
	self["btn_select"]:AddListener(function()
		GlobalHooks.eventManager:Broadcast("Forge.SelectEquip", {id = data["ID"]})
    end)
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