local self = {}
self.__index = self

self.menu = require 'UI.UIPanels.UIPanelMenu'
self.settings = require 'UI.UIPanels.UIPanelSettings'
self.gameDeck = require 'UI.UIPanels.UIPanelGameDeck'
self.roleInfo = require 'UI.UIPanels.UIPanelRoleInfo'
self.bag = require 'UI.UIPanels.UIPanelBag'
self.equip = require 'UI.UIPanels.UIPanelEquip'
self.battle = require 'UI.UIPanels.UIPanelBattle'
self.dialog = require 'UI.UIPanels.UIPanelDialog'
self.story = require 'UI.UIPanels.UIPanelStory'
self.shop = require 'UI.UIPanels.UIPanelShop'
self.notes = require 'UI.UIPanels.UIPanelNotes'
self.battleResult = require 'UI.UIPanels.UIPanelBattleResult'
self.talent = require 'UI.UIPanels.UIPanelTalent'
self.forge = require 'UI.UIPanels.UIPanelForge'

self.itemInfo = require 'UI.UIComponents.UIComponentItemInfo'
self.floatingMsg = require 'UI.UIComponents.UIComponentFloatingMsg'
self.item = require 'UI.UIComponents.UIComponentItem'
self.itemTiny = require 'UI.UIComponents.UIComponentItemTiny'
self.dmgValue = require 'UI.UIComponents.UIComponentDamageValue'
self.battleTips = require 'UI.UIComponents.UIComponentBattleTips'
self.option = require 'UI.UIComponents.UIComponentOption'
self.shopItem = require 'UI.UIComponents.UIComponentShopItem'
self.forgeEquip = require 'UI.UIComponents.UIComponentForgeEquip'
self.notesCom = require 'UI.UIComponents.UIComponentNotes'
self.roleInfoCom = require 'UI.UIComponents.UIComponentRoleInfo'

function self:InitUIComponent(name, obj, sort, params)
	if name == nil then
		return
	end
	if name == "UIComponentFloatingMsg" then
		self.floatingMsg:InitUI(name, obj, sort, params)
	elseif name == "UIComponentItem" then
		self.item:InitUI(name, obj, sort, params)
	elseif name == "UIComponentItemTiny" then
		self.itemTiny:InitUI(name, obj, sort, params)
	elseif name == "UIComponentShopItem" then
		self.shopItem:InitUI(name, obj, sort, params)
	elseif name == "UIComponentDamageValue" then
		self.dmgValue:InitUI(name, obj, sort, params)
	elseif name == "UIComponentBattleTips" then
		self.battleTips:InitUI(name, obj, sort, params)
	elseif name == "UIComponentOption" then
		self.option:InitUI(name, obj, sort, params)
	elseif name == "UIComponentItemInfo" then
		self.itemInfo:InitUI(name, obj, sort, params)
	elseif name == "UIComponentNotes" then
		self.notesCom:InitUI(name, obj, sort, params)
	elseif name == "UIComponentRoleInfo" then
		self.roleInfoCom:InitUI(name, obj, sort, params)
	elseif name == "UIComponentForgeEquip" then
		self.forgeEquip:InitUI(name, obj, sort, params)
	end
end

function self:OpenUIPanel(name, sort, params)
	if name == nil then
		return
	end
	if sort == nil then
		sort = 1
	end
	CS.LuaCallCSUtils.SetInteraction(false)
	if name == "UIPanelMenu" then
		self.menu:InitUI(name, sort, params)
	elseif name == "UIPanelSettings" then
		self.settings:InitUI(name, sort, params)
	elseif name == "UIPanelGameDeck" then
		self.gameDeck:InitUI(name, sort, params)
	elseif name == "UIPanelRoleInfo" then
		self.roleInfo:InitUI(name, sort, params)
	elseif name == "UIPanelBag" then
		self.bag:InitUI(name, sort, params)
	elseif name == "UIPanelEquip" then
		self.equip:InitUI(name, sort, params)
	elseif name == "UIPanelBattle" then
		self.battle:InitUI(name, sort, params)
	elseif name == "UIPanelBattleResult" then
		self.battleResult:InitUI(name, sort, params)
	elseif name == "UIPanelDialog" then
		self.dialog:InitUI(name, sort, params)
	elseif name == "UIPanelStory" then
		self.story:InitUI(name, sort, params)
	elseif name == "UIPanelNotes" then
		self.notes:InitUI(name, sort, params)
	elseif name == "UIPanelShop" then
		self.shop:InitUI(name, sort, params)
	elseif name == "UIPanelTalent" then
		self.talent:InitUI(name, sort, params)
	elseif name == "UIPanelForge" then
		self.forge:InitUI(name, sort, params)
	end
end

return self