local self = {}
self.__index = self

local function OnClickBack()
	self.ui:Close()
	GlobalHooks.openUI:OpenUIPanel("UIPanelMenu")
end

local function OnClickLanguage()
	CS.LocalizationManager.m_instance:SwitchLanguage();
end

local UIName = {
	"btn_language",
	"btn_back",
}

local function FindUI()
	for i = 1, #UIName do
		self[UIName[i]] = self.ui:GetChild(UIName[i])
	end
	self["btn_back"]:AddListener(OnClickBack)
	self["btn_language"]:AddListener(OnClickLanguage)
end

function self:InitUI(name, sort, params)
	self.ui = CS.UIManager.m_instance:ShowOrCreatePanel(name, sort)
	FindUI()
end

return self