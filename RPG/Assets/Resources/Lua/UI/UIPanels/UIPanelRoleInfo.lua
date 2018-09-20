local self = {}
self.__index = self

local function ShowEquipInfo(slot, obj, id)
	local img_icon = self["img_icon_"..slot]
	local img_empty = self["img_empty_"..slot]
	img_empty:SetActive(id == 0)
	img_icon:SetActive(id ~= 0)
	obj:RemoveListener()
	if id ~= 0 then
		local data = GlobalHooks.dataReader:FindData("equip", tostring(id))
		img_icon:SetSprite("Item/"..data["ICON"])
		obj:SetSprite("Common/Frame_"..data["QUALITY"])
		obj:AddListener(function()
			GlobalHooks.openUI:OpenUIPanel("UIPanelItemInfo", 2, {index = 2, itemData = data})
		end)
	else
		obj:SetSprite("Common/Frame_1")
		obj:AddListener(function()
			self:Close()
			GlobalHooks.openUI:OpenUIPanel("UIPanelEquip", 2, {slot = slot})
		end)
	end
end

local function UpdateRoleInfo()
	local playerData = CS.LuaCallCSUtils.GetPlayerData()
	--角色属性
	self["img_avatar"]:SetSprite("Avatar/Avatar_"..playerData.m_ID)
	self["txt_name"]:SetText(GetText(playerData.m_Name))
	self["txt_hp_value"]:SetText(playerData.hp.."/"..playerData.maxHp)
	self["txt_mp_value"]:SetText(playerData.mp.."/"..playerData.maxMp)
	self["slider_hp"]:SetFilledValue(playerData.hp / playerData.maxHp)
	self["slider_mp"]:SetFilledValue(playerData.mp / playerData.maxMp)
	self["txt_atk_value"]:SetText(playerData.atk)
	self["txt_ap_value"]:SetText(playerData.ap)
	self["txt_amr_value"]:SetText(playerData.amr)
	self["txt_mr_value"]:SetText(playerData.mr)
	self["txt_cr_value"]:SetText(math.floor(playerData.cr * 100) .."%")
	self["txt_cv_value"]:SetText(string.format("%.1f", tostring(playerData.cv)))
	--角色装备
	for i = 0, 5 do
		ShowEquipInfo(i + 1, self["slot_"..(i + 1)], playerData.equip[i])
	end
end

local function OnExit()
	GlobalHooks.eventManager:RemoveListener("Role.UpdateRoleInfo", UpdateRoleInfo)
end

local function OnEnter()
    GlobalHooks.eventManager:AddListener("Role.UpdateRoleInfo", UpdateRoleInfo)
end

local UIName = {
	"img_avatar",
	"txt_name",
	"txt_hp_value",
	"txt_mp_value",
	"txt_atk_value",
	"txt_ap_value",
	"txt_amr_value",
	"txt_mr_value",
	"txt_cr_value",
	"txt_cv_value",
	"slider_hp",
	"slider_mp",
	"btn_back",
	"slot_1",
	"slot_2",
	"slot_3",
	"slot_4",
	"slot_5",
	"slot_6",
}

local function FindUI()
	for i = 1, #UIName do
		self[UIName[i]] = self.ui:GetChild(UIName[i])
	end
	for i = 1, 6 do
		self["img_icon_"..i] = self.ui:GetChild("img_icon_"..i)
		self["img_empty_"..i] = self.ui:GetChild("img_empty_"..i)
	end
	self["btn_back"]:AddListener(function()
        self:Close()
    end)
	UpdateRoleInfo()
end

function self:InitUI(name, sort, params)
	self.params = params
	self.ui = CS.UIManager.m_instance:ShowOrCreatePanel(name, sort)
	OnEnter()
	FindUI()
end

function self:Close()
	OnExit()
	self.ui:Close()
end

return self