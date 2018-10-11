local self = {}
self.__index = self

local UIName = {
	"btn_back",
	"btn_closeList",
	"btn_add",
	"btn_forge",
	"txt_add",
	"equip_1",
	"equip_2",
	"info_1",
	"info_2",
	"content_bag",
	"content_mat",
	"txt_price",
	"info_cost",
	"txt_gold",
	"txt_refined",
	"slider_refined",
	"txt_max",
}

local function UpdateGold()
	self.gold = CS.LuaCallCSUtils.GetPlayerGold()
	self["txt_gold"]:SetText(self.gold)
end

local function OnClickForge()
	if self.gold and self.price then
		if self.gold < self.price then
			GlobalHooks.uiUitls:ShowFloatingMsg(GetText("Tip_NotEnoughGold"))
			return
		end
	end
	if self.needMats and self.hasMats then
		for i = 1, #self.needMats do
			if self.hasMats[i] < self.needMats[i] then
				GlobalHooks.uiUitls:ShowFloatingMsg(GetText("Tip_NotEnoughMaterials"))
				return
			end
		end
	end
	if self.mats then
		for k, v in pairs(self.mats)do
			local mat = GlobalHooks.uiUitls:StringSplit(v, ',')
			GlobalHooks.item:AddItem(mat[1], -tonumber(mat[2]))
		end
	end
	CS.LuaCallCSUtils.AddGold(-self.price)
	GlobalHooks.uiUitls:ShowFloatingMsg(GetText("Tip_ForgeSuccess"))
	GlobalHooks.item:RefineEquip(self.equipId, 10)
end

local function OnExit()
	GlobalHooks.eventManager:RemoveListener("Common.UpdateGold", UpdateGold)
end

local function OnEnter()
	GlobalHooks.eventManager:AddListener("Common.UpdateGold", UpdateGold)
end

local function ShowEquipInfo(obj, data)
	obj:SetActive(true)
	local txt_name = obj:GetChild("txt_name")
	txt_name:SetText(GetText(data["NAME"]))
	local txt_pro = obj:GetChild("txt_pro")
	txt_pro:SetText(GetText(GlobalHooks.roleName[tonumber(data["PRO"])]))
	local txt_des = obj:GetChild("txt_des")
	LoadEquipDescAndBuff(txt_des, data)
	local star = tonumber(data["QUALITY"])
	for i = 1, 5 do
		local s = obj:GetChild("img_star_"..i)
		s:SetActive(i <= star)
	end
end

function self:LoadEquip(id)
	self.equipId = id
	self["btn_closeList"]:SetActive(false)
	self["txt_max"]:SetActive(false)
	self["info_cost"]:SetActive(true)
	local data = GlobalHooks.dataReader:FindData("equip", id)
	if data then
		self["txt_add"]:SetText(GetText("Change"))
		local icon = self["equip_1"]:GetChild("img_icon")
		icon:SetActive(true)
		icon:SetSprite("Item/"..data["ICON"])
		self.price = tonumber(data["COSTGOLD"])
		self["txt_price"]:SetText(data["COSTGOLD"])
		self["txt_refined"]:SetText(GlobalHooks.item:GetEquipRefinedByID(id).."/"..data["RPOINTS"])
		self["slider_refined"]:SetFilledValue(GlobalHooks.item:GetEquipRefinedByID(id) / tonumber(data["RPOINTS"]))
		self["content_mat"]:ClearChild()
		self.mats = GlobalHooks.uiUitls:StringSplit(data["MATERIALS"], ';')
		self.needMats = {}
		self.hasMats = {}
		if #self.mats > 1 then
			for k, v in pairs(self.mats)do
				local mat = GlobalHooks.uiUitls:StringSplit(v, ',')
				local num = GlobalHooks.item:GetItemCountByID(mat[1])
				table.insert(self.needMats, tonumber(mat[2]))
				table.insert(self.hasMats, num)
				GlobalHooks.openUI:InitUIComponent("UIComponentItemTiny", self["content_mat"]:GetTransfrom(), 0, {id = mat[1], txt = num.."/"..mat[2]})
			end
		end
		self["equip_1"]:SetSprite("Common/Frame_"..data["QUALITY"])
		ShowEquipInfo(self["info_1"], data)
		if data["REFINED"] ~= "0" then
			local data_refined = GlobalHooks.dataReader:FindData("equip", data["REFINED"])
			if data_refined then
				local icon = self["equip_2"]:GetChild("img_icon")
				icon:SetActive(true)
				icon:SetSprite("Item/"..data_refined["ICON"])
				self["equip_2"]:SetSprite("Common/Frame_"..data_refined["QUALITY"])
				ShowEquipInfo(self["info_2"], data_refined)
			end
		else
			self["txt_max"]:SetActive(true)
			self["info_cost"]:SetActive(false)
			local icon = self["equip_2"]:GetChild("img_icon")
			icon:SetActive(false)
			self["equip_2"]:SetSprite("Common/Frame_1")
			self["info_2"]:SetActive(false)
		end
	end
end

local function OpenEquipList()
	self["btn_closeList"]:SetActive(true)
	self["content_bag"]:ClearChild()
	local equipBag = GlobalHooks.item:GetAllEquip()
	for k, v in pairs(equipBag)do
		local info = GlobalHooks.uiUitls:StringSplit(v, ',')
		GlobalHooks.openUI:InitUIComponent("UIComponentForgeEquip", self["content_bag"]:GetTransfrom(), 0, {id = info[1]})
	end
end

local function FindUI()
	for i = 1, #UIName do
		self[UIName[i]] = self.ui:GetChild(UIName[i])
	end
	self["txt_add"]:SetText(GetText("Add"))
	self["btn_add"]:AddListener(function()
		OpenEquipList()
    end)
	self["btn_forge"]:AddListener(function()
		OnClickForge()
    end)
	self["btn_back"]:AddListener(function()
		OnExit()
        self.ui:Close()
    end)
	self["btn_closeList"]:AddListener(function()
		self["btn_closeList"]:SetActive(false)
    end)
	UpdateGold()
end

function self:InitUI(name, sort, params)
	self.params = params
	self.ui = CS.UIManager.m_instance:ShowOrCreatePanel(name, sort)
	OnEnter()
	FindUI()
end

return self