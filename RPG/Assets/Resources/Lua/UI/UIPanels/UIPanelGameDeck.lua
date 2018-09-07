local self = {}
self.__index = self

local function OnClickAvatar()
	GlobalHooks.openUI:OpenUIPanel("UIPanelRoleInfo", 2)
end

local function OnClickQuit()
	GlobalHooks.openUI:OpenUIPanel("UIPanelMenu")
	self.ui:Close()
end

local function OnClickBag()
	GlobalHooks.openUI:OpenUIPanel("UIPanelBag", 2)
end

local function OnClickEquip()
	GlobalHooks.openUI:OpenUIPanel("UIPanelEquip", 2)
end

local UIName = {
	"img_avatar",
	"btn_quit",
	"btn_bag",
	"btn_equip",
}

local function FindUI()
	for i = 1, #UIName do
		self[UIName[i]] = self.ui:GetChild(UIName[i])
	end
	self["img_avatar"]:SetSprite("Avatar/Avatar_"..self.params.roleData["ID"])
	self["img_avatar"]:AddListener(OnClickAvatar)
	self["btn_quit"]:AddListener(OnClickQuit)
	self["btn_bag"]:AddListener(OnClickBag)
	self["btn_equip"]:AddListener(OnClickEquip)
end

function self:InitUI(name, sort, params)
	self.params = params
	self.ui = CS.UIManager.m_instance:ShowOrCreatePanel(name, sort)
	FindUI()
end

return self