local self = {}
self.__index = self

local UIName = {
	"frame",
	"img_icon",
	"txt_price",
	"txt_name",
	"btn_buy",
}

local function UpdateItemInfo()
	local data = GlobalHooks.dataReader:FindData("item", self.params.id)
	if self["img_icon"] then
		self["img_icon"]:SetSprite("Item/"..data["ICON"])
	end
	if self["txt_name"] then
		self["txt_name"]:SetText(GetText(data["NAME"]))
	end
	if self["txt_price"] then
		self["txt_price"]:SetText(data["PRICE"])
	end
	if self["frame"] then
		self["frame"]:SetSprite("Common/Frame_"..data["QUALITY"])
	end
	if self["btn_buy"] then
		self["btn_buy"]:AddListener(function()
			GlobalHooks.openUI:InitUIComponent("UIComponentItemInfo", nil, 2, {index = 3, itemData = data})
		end)
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