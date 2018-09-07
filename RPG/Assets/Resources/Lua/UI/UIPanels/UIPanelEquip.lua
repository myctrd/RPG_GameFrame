local self = {}
self.__index = self

local function ShowItemInfo(item, id)
	local img_icon = item:GetChild("img_icon")
	local txt_count = item:GetChild("txt_count")
	local data = GlobalHooks.dataReader:FindData("equip", id)
	img_icon:SetSprite("Item/"..data["ICON"])
	txt_count:SetText("")
	
	local frame = item:GetChild("frame")
	frame:AddListener(function()
        GlobalHooks.openUI:OpenUIPanel("UIPanelItemInfo", 2, {index = 2, itemData = data})
    end)
end

local function OnSelectCol(col)
	self["content_bag"]:ClearChild()
	for i = 1, 6 do
		self["check_"..tostring(i)]:SetActive(i == col)
	end
	local equipBag = GlobalHooks.item:GetEquipBag(col)
	for k, v in pairs(equipBag)do
		local item = CS.UIManager.m_instance:LoadComponent("UIComponentItem", self["content_bag"]:GetTransfrom())
		ShowItemInfo(item, v)
	end
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