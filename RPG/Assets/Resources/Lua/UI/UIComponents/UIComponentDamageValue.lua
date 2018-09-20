local self = {}
self.__index = self

local UIName = {
	"txt_value",
}

local function FindUI()
	for i = 1, #UIName do
		self[UIName[i]] = self.ui:GetChild(UIName[i])
	end
	if self.params.value <= 0 then
		self["txt_value"]:SetText(self.params.value)
		self["txt_value"]:SetTextColor(255, 0, 0)
	else
		self["txt_value"]:SetText("+"..self.params.value)
		self["txt_value"]:SetTextColor(62, 255, 0)
	end
	
end

function self:InitUI(name, obj, sort, params)
	self.params = params
	self.ui = CS.UIManager.m_instance:LoadComponent(name, obj, sort)
	self.ui:ResetPosition()
	FindUI()
end

return self