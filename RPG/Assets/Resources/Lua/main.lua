
GlobalHooks = {
	dataReader = require 'Logic.dataReader',
	openUI = require 'Logic.OpenUI',
	uiUitls = require 'Logic.UIUtils',
	item = require 'Manager.item',
	event = require 'Manager.event',
	eventManager = require 'Logic.EventManager',
	roleName = {
	"Rejec",
	"Echo",
	"Timon",
	"Chloe",},
}

function OpenUIPanel(panel)
	GlobalHooks.openUI:OpenUIPanel(panel)
end

function GetData(tb, id)
	return GlobalHooks.dataReader:FindData(tb, id)
end

function InitUI()
	GlobalHooks.item:Init()
	GlobalHooks.event:Init()
	GlobalHooks.uiUitls:Init()
	GlobalHooks.openUI:OpenUIPanel("UIPanelMenu")
end
