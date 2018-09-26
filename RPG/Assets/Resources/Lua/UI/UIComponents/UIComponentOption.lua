local self = {}
self.__index = self

local UIName = {
	"txt",
}

local function FindUI()
	for i = 1, #UIName do
		self[UIName[i]] = self.ui:GetChild(UIName[i])
	end
	self["txt"]:SetText(self.params.id..". "..GetText(self.params.txt))
	local event = self.params.event
	self["txt"]:AddListener(function()
		self.params.dialog:Close()
		if event ~= "0" then
			CS.LuaCallCSUtils.ActivateEvent(event)
		end
	end)
end

function self:InitUI(name, obj, sort, params)
	self.params = params
	self.ui = CS.UIManager.m_instance:LoadComponent(name, obj, sort)
	-- self.ui:ResetPosition()
	FindUI()
end

return self