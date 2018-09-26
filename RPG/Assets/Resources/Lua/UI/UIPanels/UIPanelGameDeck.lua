local self = {}
self.__index = self

local function UpdateGold()
	if self.gold ~= nil then
		local value = CS.LuaCallCSUtils.GetPlayerGold() - self.gold
		if value > 0 then
			GlobalHooks.uiUitls:ShowFloatingMsg(GetText("Gold").." +"..value)
		end
	end
	self.gold = CS.LuaCallCSUtils.GetPlayerGold()
	self["txt_gold"]:SetText(self.gold)
end

local function OnExit()
	GlobalHooks.eventManager:RemoveListener("Common.UpdateGold", UpdateGold)
end

local function OnEnter()
    GlobalHooks.eventManager:AddListener("Common.UpdateGold", UpdateGold)
end

local function OnClickAvatar()
	GlobalHooks.openUI:OpenUIPanel("UIPanelRoleInfo", 2)
end

local function OnClickQuit()
	OnExit()
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

local function OnClickNotes()
	GlobalHooks.openUI:OpenUIPanel("UIPanelNotes", 2)
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
	"btn_notes",
	"txt_gold",
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
	self["btn_notes"]:AddListener(OnClickNotes)
	UpdateGold()
end

function self:InitUI(name, sort, params)
	self.params = params
	self.ui = CS.UIManager.m_instance:ShowOrCreatePanel(name, sort)
	OnEnter()
	FindUI()
end

return self