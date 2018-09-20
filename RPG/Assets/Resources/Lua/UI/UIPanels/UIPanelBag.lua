local self = {}
self.__index = self

local function UpdateItem()
	self["content_bag"]:ClearChild()
	for i = 1, 2 do
		self["check_"..tostring(i)]:SetActive(i == self.selectedIndex)
	end
	local itemBag = GlobalHooks.item:GetItemBag(self.selectedIndex)
	for k, v in pairs(itemBag)do
		if v > 0 then
			GlobalHooks.openUI:InitUIComponent("UIComponentItem", self["content_bag"]:GetTransfrom(), 0, {itemType = 1, id = k, amount = v})
		end
	end
end

local function OnSelectCol(col)
	self.selectedIndex = col
	UpdateItem()
end

local function OnExit()
	GlobalHooks.eventManager:RemoveListener("Item.UpdateItemCount", UpdateItem)
end

local function OnEnter()
    GlobalHooks.eventManager:AddListener("Item.UpdateItemCount", UpdateItem)
end

local UIName = {
	"btn_col_1",
	"btn_col_2",
	"check_1",
	"check_2",
	"btn_back",
	"content_bag",
}

local function FindUI()
	for i = 1, #UIName do
		self[UIName[i]] = self.ui:GetChild(UIName[i])
	end
	for i = 1, 2 do
		self["btn_col_"..tostring(i)]:AddListener(function()
			OnSelectCol(i)
		end)
	end
	self["btn_back"]:AddListener(function()
		OnExit()
        self.ui:Close()
    end)
	
	OnSelectCol(1)
end

function self:InitUI(name, sort, params)
	self.params = params
	self.ui = CS.UIManager.m_instance:ShowOrCreatePanel(name, sort)
	OnEnter()
	FindUI()
end

return self