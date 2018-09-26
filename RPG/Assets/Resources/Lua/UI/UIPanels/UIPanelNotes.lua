local self = {}
self.__index = self

local UIName = {
	"btn_col_1",
	"btn_col_2",
	"check_1",
	"check_2",
	"btn_back",
	"content_bag"
}

local function UpdateNotes()
	self["content_bag"]:ClearChild()
	for i = 1, 2 do
		self["check_"..tostring(i)]:SetActive(i == self.selectedIndex)
	end
	local notes = GlobalHooks.event:GetNotes(self.selectedIndex)
	local playerData = CS.LuaCallCSUtils.GetPlayerData()
	for k, v in pairs(notes)do
		local data = GlobalHooks.dataReader:FindData("notes", k)
		if CS.LuaCallCSUtils.PlayerPrefsHasKey(playerData.m_Name.."_Event_"..data["EVENT"]) == false then
			GlobalHooks.openUI:InitUIComponent("UIComponentNotes", self["content_bag"]:GetTransfrom(), 0, {id = k})
		end
	end
end

local function OnSelectCol(col)
	self.selectedIndex = col
	UpdateNotes()
end

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
        self.ui:Close()
    end)
	OnSelectCol(1)
end

function self:InitUI(name, sort, params)
	self.params = params
	self.ui = CS.UIManager.m_instance:ShowOrCreatePanel(name, sort)
	FindUI()
end

return self