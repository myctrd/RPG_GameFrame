local self = {}
self.__index = self

local main = require 'UI.UIPanels.UIPanelMain'
local menu = require 'UI.UIPanels.UIPanelMenu'
local settings = require 'UI.UIPanels.UIPanelSettings'
local roleSelect = require 'UI.UIPanels.UIPanelRoleSelect'
local gameDeck = require 'UI.UIPanels.UIPanelGameDeck'
local roleInfo = require 'UI.UIPanels.UIPanelRoleInfo'
local bag = require 'UI.UIPanels.UIPanelBag'
local equip = require 'UI.UIPanels.UIPanelEquip'
local itemInfo = require 'UI.UIPanels.UIPanelItemInfo'

local floatingMsg = require 'UI.UIComponents.UIComponentFloatingMsg'

function self:InitUIComponent(name, obj, sort, params)
	if name == nil then
		return
	end
	if name == "UIComponentFloatingMsg" then
		floatingMsg:InitUI(name, obj, sort, params)
	end
end

function self:OpenUIPanel(name, sort, params)
	if name == nil then
		return
	end
	if sort == nil then
		sort = 1
	end
	if name == "UIPanelMain" then
		main:InitUI(name, sort, params)
	elseif name == "UIPanelMenu" then
		menu:InitUI(name, sort, params)
	elseif name == "UIPanelSettings" then
		settings:InitUI(name, sort, params)
	elseif name == "UIPanelRoleSelect" then
		roleSelect:InitUI(name, sort, params)
	elseif name == "UIPanelGameDeck" then
		gameDeck:InitUI(name, sort, params)
	elseif name == "UIPanelRoleInfo" then
		roleInfo:InitUI(name, sort, params)
	elseif name == "UIPanelBag" then
		bag:InitUI(name, sort, params)
	elseif name == "UIPanelEquip" then
		equip:InitUI(name, sort, params)
	elseif name == "UIPanelItemInfo" then
		itemInfo:InitUI(name, sort, params)
	end
end

return self