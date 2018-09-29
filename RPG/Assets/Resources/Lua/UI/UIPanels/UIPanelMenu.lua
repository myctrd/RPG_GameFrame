local self = {}
self.__index = self


local function OnClickPlay()
	GlobalHooks.openUI:OpenUIPanel("UIPanelGameDeck", 1)
	self.ui:Close()
	CS.LuaCallCSUtils.LoadGameScene()
end

local function OnClickSettings()
	self.ui:Close()
	GlobalHooks.openUI:OpenUIPanel("UIPanelSettings")
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