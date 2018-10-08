local self = {}
self.__index = self

self.itemBag_1 = {}  --任务道具背包(不可叠加，不可重复)
self.itemBag_2 = {}  --消耗道具背包(可叠加，不可重复)

self.equipBag_1 = {}
self.equipBag_2 = {}
self.equipBag_3 = {}
self.equipBag_4 = {}
self.equipBag_5 = {}
self.equipBag_6 = {}

local function ClearAllBag()
	self.itemBag_1 = {}
	self.itemBag_2 = {}
	self.equipBag_1 = {}
	self.equipBag_2 = {}
	self.equipBag_3 = {}
	self.equipBag_4 = {}
	self.equipBag_5 = {}
	self.equipBag_6 = {}
end

local function IsTableHasKey(tb, id)
	for k, v in pairs(tb)do
		if k == id then
			return true
		end
	end
	return false
end

function self:GetItemBag(i)
	if i == 1 then
		return self.itemBag_1
	elseif i == 2 then
		return self.itemBag_2
	end
end

function self:GetEquipBag(i)
	if i == 1 then
		return self.equipBag_1
	elseif i == 2 then
		return self.equipBag_2
	elseif i == 3 then
		return self.equipBag_3
	elseif i == 4 then
		return self.equipBag_4
	elseif i == 5 then
		return self.equipBag_5
	elseif i == 6 then
		return self.equipBag_6
	end
end

function self:Init()
	GlobalHooks.eventManager:AddListener("DataManager.ConsumeTaskItem", function(name, params)
		self:ConsumeTaskItem(params.id)
	end)
	GlobalHooks.eventManager:AddListener("DataManager.AddItem", function(name, params)
		self:AddItem(params.id, tonumber(params.count))
	end)
	GlobalHooks.eventManager:AddListener("DataManager.AddEquip", function(name, params)
		self:AddEquip(params.id)
	end)
	GlobalHooks.eventManager:AddListener("Common.GetEquip", function(name, params)
		self:GetNewEquip(params.id)
	end)
	GlobalHooks.eventManager:AddListener("Common.GetItem", function(name, params)
		self:GetNewItem(params.id, params.count)
	end)
	ClearAllBag()
	CS.LuaCallCSUtils.LoadItemData()
end

function self:AddEquip(id)
	local data = GlobalHooks.dataReader:FindData("equip", id)
	if data["SLOT"] == "1" then
		table.insert(self.equipBag_1, data["ID"])
	elseif data["SLOT"] == "2" then
		table.insert(self.equipBag_2, data["ID"])
	elseif data["SLOT"] == "3" then
		table.insert(self.equipBag_3, data["ID"])
	elseif data["SLOT"] == "4" then
		table.insert(self.equipBag_4, data["ID"])
	elseif data["SLOT"] == "5" then
		table.insert(self.equipBag_5, data["ID"])
	elseif data["SLOT"] == "6" then
		table.insert(self.equipBag_6, data["ID"])
	end
end

function self:GetNewEquip(id)
	local data = GlobalHooks.dataReader:FindData("equip", id)
	GlobalHooks.uiUitls:ShowFloatingMsg(GetText(data["NAME"]).." + 1")
	self:AddEquip(id);
	CS.LuaCallCSUtils.AddEquip(id, "1")
end

function self:AddItem(id, count)
	local data = GlobalHooks.dataReader:FindData("item", id)
	if data["ITEMTYPE"] == "1" then
		self.itemBag_1[data["ID"]] = 1
		CS.LuaCallCSUtils.UpdateItem(id, "1")
	elseif data["ITEMTYPE"] == "2" then
		if(IsTableHasKey(self.itemBag_2, data["ID"]))then
			self.itemBag_2[data["ID"]] = self.itemBag_2[data["ID"]] + count
		else
			self.itemBag_2[data["ID"]] = count
		end
		CS.LuaCallCSUtils.UpdateItem(id, tostring(self.itemBag_2[data["ID"]]))
	end
	GlobalHooks.eventManager:Broadcast("Item.UpdateItemCount", {})
end

function self:GetNewItem(id, count)
	local data = GlobalHooks.dataReader:FindData("item", id)
	if tonumber(count) > 0 then
		GlobalHooks.uiUitls:ShowFloatingMsg(GetText(data["NAME"]).." + "..count)
	end
	self:AddItem(id, count);
end

function self:ConsumeTaskItem(id)
	if(self.itemBag_1[id])then
		self.itemBag_1[id] = 0
		CS.LuaCallCSUtils.UpdateItem(id, "0")
	end
end

function self:ConsumeItem(id, count)  --使用消耗品
	if(self.itemBag_2[id] and self.itemBag_2[id] >= count)then
		local data = GlobalHooks.dataReader:FindData("item", id)
		self.itemBag_2[id] = self.itemBag_2[id] - count
		GlobalHooks.uiUitls:ShowFloatingMsg(GetText(data["NAME"]) .." - "..count)
		GlobalHooks.eventManager:Broadcast("Item.UpdateItemCount", {})
		CS.LuaCallCSUtils.UpdateItem(id, self.itemBag_2[id])
	else
		GlobalHooks.uiUitls:ShowFloatingMsg(GetText("Tip_NotEnoughItems"))
	end
end

return self