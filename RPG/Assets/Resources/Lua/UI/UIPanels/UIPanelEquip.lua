local self = {}
self.__index = self

local function UpdateItem()
	self["content_bag"]:ClearChild()
	for i = 1, 6 do
		self["check_"..tostring(i)]:SetActive(i == self.selectedIndex)
	end
	local equipBag = GlobalHooks.item:GetEquipBag(self.selectedIndex)
	for k, v in pairs(equipBag)do
		local data = GlobalHooks.dataReader:FindData("equip", v)
		-- if tonumber(data["PRO"]) == CS.LuaCallCSUtils.GetPlayerData(GlobalHooks.openUI.roleInfo.id).m_Pro then
			GlobalHooks.openUI:InitUIComponent("UIComponentItem", self["content_bag"]:GetTransfrom(), 0, {itemType = 2, id = v})
		-- end
	end
end

local function OnSelectCol(col)
	self.selectedIndex = col
	UpdateItem()
end

local function OnExit()
	GlobalHooks.eventManager:RemoveListener("Role.UpdateRoleInfo", UpdateItem)
end

local function OnEnter()
    GlobalHooks.eventManager:AddListener("Role.UpdateRoleInfo", UpdateItem)
end

local UIName = {
	"btn_col_1",
	"btn_col_2",
	"btn_col_3",
	"btn_col_4",
	"btn_col_5",
	"btn_col_6",
	"check_1",
	"check_2",
	"check_3",
	"check_4",
	"check_5",
	"check_6",
	"btn_back",
	"content_bag",
}

local function FindUI()
	for i = 1, #UIName do
		self[UIName[i]] = self.ui:GetChild(UIName[i])
	end
	
	for i = 1, 6 do
		self["btn_col_"..tostring(i)]:AddListener(function()
			OnSelectCol(i)
		end)
	end
	self["btn_back"]:AddListener(function()
		OnExit()
        self.ui:Close()
		GlobalHooks.openUI:OpenUIPanel("UIPanelRoleInfo", 2, {id = self.id})
    end)
	OnSelectCol(self.params.slot)
end

function self:InitUI(name, sort, params)
	self.params = params
	self.id = params.id
	self.ui = CS.UIManager.m_instance:ShowOrCreatePanel(name, sort)
	OnEnter()
	FindUI()
end

return self