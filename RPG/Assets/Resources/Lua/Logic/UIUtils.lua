local self = {}
self.__index = self

function self:StringSplit(s, sep)
  local sep, fields = sep or ",", {}
    local pos, startIdx, endIdx = 1
    local fields = {}
    startIdx, endIdx = string.find(s, sep, pos, true)
    while startIdx do
        table.insert(fields, string.sub(s, pos, startIdx - 1))
        pos = endIdx + 1
        startIdx, endIdx = string.find(s, sep, pos, true)
    end
    table.insert(fields, string.sub(s, pos))
    return fields
end

function self:Init()

	GlobalHooks.eventManager:AddListener("Common.FloatingMsg", function(name, params)
		self:ShowFloatingMsg(GetText(params.txt))
	end)
	
	GlobalHooks.eventManager:AddListener("Dialog.CommonDialog", function(name, params)
		GlobalHooks.openUI:OpenUIPanel("UIPanelDialog", 2, {d_type = 1, name = params.name ,txt = params.txt})
	end)
	
	GlobalHooks.eventManager:AddListener("Dialog.NPCDialog", function(name, params)
		GlobalHooks.openUI:OpenUIPanel("UIPanelDialog", 2, {d_type = 2, npcID = params.npcID})
	end)

	GlobalHooks.eventManager:AddListener("Battle.BattleTips", function(name, params)
		GlobalHooks.openUI:InitUIComponent("UIComponentBattleTips", nil, 4, {txt = params.txt})
	end)
end

function self:Test()
	print("test")
end

function self:ShowFloatingMsg(str)
	GlobalHooks.openUI:InitUIComponent("UIComponentFloatingMsg", nil, 4, {txt = str})
end

return self