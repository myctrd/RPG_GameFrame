local self = {}
self.__index = self

local itemManager = require 'Manager.item'

local UIName = {
	"btn_back",
	"img_icon",
	"img_frame",
	"txt_name",
	"txt_pro",
	"txt_des",
	"txt_buff",
	"btn_op",
	"txt_op",
	"img_gold",
	"txt_price",
	"btn_add",
	"btn_minus",
	"txt_count",
	"stars",
}

local function FindUI()
	for i = 1, #UIName do
		self[UIName[i]] = self.ui:GetChild(UIName[i])
	end
	self["btn_back"]:AddListener(function()
        self.ui:Close()
    end)
end

local function AddShopItemCount(count, maxCount)
	if maxCount == nil or maxCount > 50 then
		maxCount = 50
	end
	self.count = self.count + count
	if self.count < 1 then
		self.count = 1
	end
	if self.count > maxCount then
		self.count = maxCount
	end
	self.totalPrice = self.price * self.count
	self["txt_count"]:SetText(self.count)
	self["txt_price"]:SetText(self.totalPrice)
end

local function InitItemInfo(index, data)
	self["txt_name"]:SetText(GetText(data["NAME"]))
	self["img_icon"]:SetSprite("Item/"..data["ICON"])
	self["img_frame"]:SetSprite("Common/Frame_"..data["QUALITY"])
	self.price = 0
	
	if index == 1 then  --展示物品信息
		self["txt_des"]:SetText(GetText(data["DESC"]))
		self["txt_pro"]:SetText("")
		if(data["ITEMTYPE"] == "1")then  --出示任务物品
			self["txt_op"]:SetText(GetText("Show"))
			self["btn_op"]:SetActive(GlobalHooks.openUI.bag.bagType == 1)
			self["btn_op"]:AddListener(function()
				GlobalHooks.openUI.bag:Close()
				self.ui:Close()
				CS.LuaCallCSUtils.ShowItem(data["ID"])
			end)
		elseif(data["ITEMTYPE"] == "2")then
			self["btn_op"]:SetActive(GlobalHooks.openUI.bag.bagType == 2)
			if GlobalHooks.openUI.bag.bagType == 2 then  --出售物品
				self["img_gold"]:SetActive(true)
				self.price = tonumber(data["SELLPRICE"])
				self["btn_add"]:AddListener(function()
					AddShopItemCount(1, self.amount)
				end) 
				self["btn_minus"]:AddListener(function()
					AddShopItemCount(-1, self.amount)
				end) 
				self["txt_op"]:SetText(GetText("Sell"))
				self["btn_op"]:AddListener(function()
					CS.LuaCallCSUtils.AddGold(self.totalPrice)
					GlobalHooks.item:GetNewItem(data["ID"], -self.count)
					self.ui:Close()
				end)
			end
		end
	elseif index == 2 then  --展示装备信息
		local playerData = CS.LuaCallCSUtils.GetPlayerData(GlobalHooks.openUI.roleInfo.id)
		self["stars"]:SetActive(true)
		local star = tonumber(data["QUALITY"])
		for i = 1, 5 do
			local s = self["stars"]:GetChild("img_star_"..i)
			s:SetActive(i <= star)
		end
		if playerData.equip[tonumber(data["SLOT"]) - 1] == tonumber(data["ID"]) then
			self["txt_op"]:SetText(GetText("TakeOff"))
			self["btn_op"]:AddListener(function()
				CS.LuaCallCSUtils.SetPlayerEquip(GlobalHooks.openUI.roleInfo.id, tonumber(data["SLOT"]) - 1, 0)
				GlobalHooks.uiUitls:ShowFloatingMsg(GetText("Tip_EquipTakeOff"))
				self.ui:Close()
			end) 
		else
			self["txt_op"]:SetText(GetText("Equip"))
			if playerData.m_Pro == tonumber(data["PRO"]) then
				self["btn_op"]:AddListener(function()
					CS.LuaCallCSUtils.SetPlayerEquip(GlobalHooks.openUI.roleInfo.id, tonumber(data["SLOT"]) - 1, tonumber(data["ID"]))
					GlobalHooks.uiUitls:ShowFloatingMsg(GetText("Tip_EquipSuccess"))
					self.ui:Close()
				end) 
			else
				self["btn_op"]:AddListener(function()
					GlobalHooks.uiUitls:ShowFloatingMsg(GetText("Tip_EquipFail"))
					self.ui:Close()
				end) 
			end
		end
		self["txt_pro"]:SetText(GetText(GlobalHooks.roleName[tonumber(data["PRO"])]))
		LoadEquipDescAndBuff(self["txt_des"], data)
	elseif index == 3 then  --展示商店商品信息
		local gold = CS.LuaCallCSUtils.GetPlayerGold()
		self["txt_des"]:SetText(GetText(data["DESC"]))
		self["txt_pro"]:SetText("")
		self["txt_op"]:SetText(GetText("Buy"))
		self["img_gold"]:SetActive(true)
		self.price = tonumber(data["PRICE"])
		
		self["btn_add"]:AddListener(function()
			AddShopItemCount(1)
		end) 
		self["btn_minus"]:AddListener(function()
			AddShopItemCount(-1)
		end) 
		self["btn_op"]:AddListener(function()
			if gold < self.totalPrice then
				GlobalHooks.uiUitls:ShowFloatingMsg(GetText("Tip_NotEnoughGold"))
			else
				CS.LuaCallCSUtils.AddGold(-self.totalPrice)
				GlobalHooks.item:GetNewItem(data["ID"], self.count)
				self.ui:Close()
			end
		end) 
	end
	
	self.count = 0
	AddShopItemCount(1)
end


function self:InitUI(name, obj, sort, params)
	self.params = params
	self.data = params.itemData
	self.index = params.index
	if params.amount then
		self.amount = params.amount
	end
	self.ui = CS.UIManager.m_instance:LoadComponent(name, obj, sort)
	FindUI()
	InitItemInfo(self.index, self.data)
end

return self