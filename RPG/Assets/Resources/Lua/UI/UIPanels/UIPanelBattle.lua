local self = {}
self.__index = self

local function UpdatePlayerInfo()
	self["RoleInfo"]:SetActive(true)
	self.playerData = CS.LuaCallCSUtils.GetBattlePlayerData()
	if self.playerHp and self.playerHp ~= self.playerData.hp then
		GlobalHooks.openUI:InitUIComponent("UIComponentDamageValue", self["value_pos_e"]:GetTransfrom(), 0, {value = self.playerData.hp - self.playerHp})
	end
	self["img_avatar"]:SetSprite("Avatar/Avatar_"..self.playerData.m_ID)
	self["txt_name"]:SetText(GetText(self.playerData.m_Name))
	self["txt_hp_value"]:SetText(self.playerData.hp.."/"..self.playerData.maxHp)
	self["txt_mp_value"]:SetText(self.playerData.mp.."/"..self.playerData.maxMp)
	self["slider_hp"]:SetFilledValue(self.playerData.hp / self.playerData.maxHp)
	self["slider_mp"]:SetFilledValue(self.playerData.mp / self.playerData.maxMp)
end

local function UpdateEnemyInfo()
	self["EnemyInfo"]:SetActive(true)
	self.enemyData = CS.LuaCallCSUtils.GetEnemyData()
	if self.enemyHp and self.enemyHp ~= self.enemyData.hp then
		GlobalHooks.openUI:InitUIComponent("UIComponentDamageValue", self["value_pos_e"]:GetTransfrom(), 0, {value = self.enemyData.hp - self.enemyHp})
	end
	self.enemyHp = self.enemyData.hp
	self["img_avatar_e"]:SetSprite("Avatar/Avatar_"..self.enemyData.m_ID)
	self["txt_name_e"]:SetText(GetText(self.enemyData.m_Name))
	self["txt_hp_value_e"]:SetText(self.enemyData.hp.."/"..self.enemyData.maxHp)
	self["txt_mp_value_e"]:SetText(self.enemyData.mp.."/"..self.enemyData.maxMp)
	self["slider_hp_e"]:SetFilledValue(self.enemyData.hp / self.enemyData.maxHp)
	self["slider_mp_e"]:SetFilledValue(self.enemyData.mp / self.enemyData.maxMp)
end

local function UpdateBattleInfo()
	UpdatePlayerInfo()
	UpdateEnemyInfo()
end

local function OnExit()
	GlobalHooks.eventManager:RemoveListener("Battle.UpdateBattleInfo", UpdateBattleInfo)
end

local function OnEnter()
	GlobalHooks.eventManager:AddListener("Battle.UpdateBattleInfo", UpdateBattleInfo)
end

function self:Close()
	OnExit()
	GlobalHooks.openUI:OpenUIPanel("UIPanelGameDeck")
	self.ui:Close()
	CS.LuaCallCSUtils.EndBattle()
end

local function OnClickQuit()
	self:Close()
end

local function OnClickAttack()
	CS.LuaCallCSUtils.RoleAttackRole(self.playerData, self.enemyData)
end

local UIName = {
	"img_avatar",
	"txt_name",
	"txt_hp_value",
	"txt_mp_value",
	"slider_hp",
	"slider_mp",
	"img_avatar_e",
	"txt_name_e",
	"txt_hp_value_e",
	"txt_mp_value_e",
	"slider_hp_e",
	"slider_mp_e",
	"btn_attack",
	"btn_quit",
	"RoleInfo",
	"EnemyInfo",
	"value_pos_e",
	"value_pos",
}

local function FindUI()
	for i = 1, #UIName do
		self[UIName[i]] = self.ui:GetChild(UIName[i])
	end
	self["RoleInfo"]:SetActive(false)
	self["EnemyInfo"]:SetActive(false)
	self["btn_attack"]:AddListener(OnClickAttack)
	self["btn_quit"]:AddListener(OnClickQuit)
end

function self:InitUI(name, sort, params)
	self.params = params
	self.enemyID = self.params.enemyID
	self.playerHp = nil
	self.enemyHp = nil
	self.ui = CS.UIManager.m_instance:ShowOrCreatePanel(name, sort)
	OnEnter()
	FindUI()
	CS.LuaCallCSUtils.StartBattle(self.enemyID)
end

return self