local self = {}
self.__index = self

local function OnClickPlay()
	GlobalHooks.openUI:OpenUIPanel("UIPanelRoleSelect")
	self.ui:Close()
end

local function OnClickSettings()
	GlobalHooks.openUI:OpenUIPanel("UIPanelSettings")
	self.ui:Close()
end

local UIName = {
	"btn_play",
	"btn_settings",
	"btn_quit",
}

local function FindUI()
	for i = 1, #UIName do
		self[UIName[i]] = self.ui:GetChild(UIName[i])
	end
	self["btn_play"]:AddListener(OnClickPlay)
	self["btn_settings"]:AddListener(OnClickSettings)
end

function self:InitUI(name, sort, params)
	self.ui = CS.UIManager.m_instance:ShowOrCreatePanel(name, sort)
	FindUI()
end

return self