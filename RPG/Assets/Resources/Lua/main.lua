
GlobalHooks = {
	dataReader = require 'Logic.dataReader',
	openUI = require 'Logic.OpenUI',
	uiUitls = require 'Logic.UIUtils',
	item = require 'Manager.item',
	eventManager = require 'Logic.EventManager',
	roleName = {
	"Rejec",
	"Echo",
	"Timon",
	"Chloe",},
}

function GetData(tb, id)
	return GlobalHooks.dataReader:FindData(tb, id)
end

function InitUI()
	GlobalHooks.item:Init()
	GlobalHooks.item:AddItem("10001")
	GlobalHooks.item:AddItem("10002")
	GlobalHooks.item:AddItem("10003")
	GlobalHooks.item:AddItem("20001", 3)
	GlobalHooks.item:AddEquip("10101")
	GlobalHooks.item:AddEquip("40201")
	GlobalHooks.openUI:OpenUIPanel("UIPanelMain", 0)
	GlobalHooks.openUI:OpenUIPanel("UIPanelMenu")
end
