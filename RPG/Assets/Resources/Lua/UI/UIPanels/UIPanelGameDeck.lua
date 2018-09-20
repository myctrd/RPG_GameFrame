local self = {}
self.__index = self

local function OnClickAvatar()
	GlobalHooks.openUI:OpenUIPanel("UIPanelRoleInfo", 2)
end

local function OnClickQuit()
	self.ui:Close()
	GlobalHooks.openUI:OpenUIPanel("UIPanelMenu")
	CS.LuaCallCSUtils.UnloadGameScene()
end

local function OnClickBag()
	GlobalHooks.openUI:OpenUIPanel("UIPanelBag", 2)
end

local function OnClickStory()
	GlobalHooks.openUI:OpenUIPanel("UIPanelStory", 2)
end

local function OnClickBattle()
	self.ui:Close()
	GlobalHooks.openUI:OpenUIPanel("UIPanelBattle", 1, {enemyID = 20001})
end

local UIName = {
	"img_avatar",
	"btn_quit",
	"btn_bag",
	"btn_story",
}

local function FindUI()
	for i = 1, #UIName do
		self[UIName[i]] = self.ui:GetChild(UIName[i])
	end
	local playerData = CS.LuaCallCSUtils.GetPlayerData()
	self["img_avatar"]:SetSprite("Avatar/Avatar_"..playerData.m_ID)
	self["img_avatar"]:AddListener(OnClickAvatar)
	self["btn_quit"]:AddListener(OnClickQuit)
	self["btn_bag"]:AddListener(OnClickBag)
	self["btn_story"]:AddListener(OnClickStory)
end

function self:InitUI(name, sort, params)
	self.params = params
	self.ui = CS.UIManager.m_instance:ShowOrCreatePanel(name, sort)
	FindUI()
end

return self