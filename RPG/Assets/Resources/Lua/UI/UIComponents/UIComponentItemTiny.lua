local self = {}
self.__index = self

local UIName = {
	"frame",
	"img_icon",
	"txt_count",
}

local function UpdateItemInfo()
	local data = GlobalHooks.dataReader:FindData("item", self.params.id)
	if self["img_icon"] then
		self["img_icon"]:SetSprite("Item/"..data["ICON"])
	end
	if self["txt_count"] then
		if self.params.txt then
			self["txt_count"]:SetText(self.params.txt)
		else
			self["txt_count"]:SetText("")
		end
	end
	if self["frame"] then
		self["frame"]:SetSprite("Common/Frame_"..data["QUALITY"])
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