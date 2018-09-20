local self = {}
self.__index = self

local UIName = {
	"txt",
}

local function FindUI()
	for i = 1, #UIName do
		self[UIName[i]] = self.ui:GetChild(UIName[i])
	end
	self["txt"]:SetText(GetText(self.params.txt))
end

function self:InitUI(name, obj, sort, params)
	self.params = params
	self.ui = CS.UIManager.m_instance:LoadComponent(name, obj, sort)
	-- self.ui:ResetPosition()
	FindUI()
end

return self