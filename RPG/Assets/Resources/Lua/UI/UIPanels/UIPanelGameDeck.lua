local self = {}
self.__index = self

local function UpdateRoleInfo()
	self["roleInfo"]:ClearChild()
	self.playerList = CS.LuaCallCSUtils.GetPlayerList()
	for i = 0, 3 do
		if self.playerList[i] then
			GlobalHooks.openUI:InitUIComponent("UIComponentRoleInfo", self["roleInfo"]:GetTransfrom(), 0, {id = i, data = self.playerList[i]})
		end
	end
end

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
	GlobalHooks.eventManager:RemoveListener("Role.UpdateRoleInfo", UpdateRoleInfo)
end

local function OnEnter()
    GlobalHooks.eventManager:AddListener("Common.UpdateGold", UpdateGold)
	GlobalHooks.eventManager:AddListener("Role.UpdateRoleInfo", UpdateRoleInfo)
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
	"btn_quit",
	"btn_bag",
	"btn_story",
	"btn_notes",
	"txt_gold",
	"roleInfo",
}

local function FindUI()
	for i = 1, #UIName do
		self[UIName[i]] = self.ui:GetChild(UIName[i])
	end
	self["btn_quit"]:AddListener(OnClickQuit)
	self["btn_bag"]:AddListener(OnClickBag)
	self["btn_story"]:AddListener(OnClickStory)
	self["btn_notes"]:AddListener(OnClickNotes)
	UpdateGold()
	UpdateRoleInfo()
end

function self:InitUI(name, sort, params)
	self.params = params
	self.ui = CS.UIManager.m_instance:ShowOrCreatePanel(name, sort)
	OnEnter()
	FindUI()
end

return self