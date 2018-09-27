local self = {}
self.__index = self

local itemManager = require 'Manager.item'

local UIName = {
	"btn_back",
	"txt_result",
}

local function FindUI()
	for i = 1, #UIName do
		self[UIName[i]] = self.ui:GetChild(UIName[i])
	end
	self["txt_result"]:SetText(GetText(self.params.txt))
	self["btn_back"]:AddListener(function()
		self.ui:Close()
        GlobalHooks.openUI.battle:Close()
    end)
end

function self:InitUI(name, sort, params)
	self.params = params
	self.ui = CS.UIManager.m_instance:ShowOrCreatePanel(name, sort)
	
	FindUI()
end

return self