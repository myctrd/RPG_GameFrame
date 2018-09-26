local self = {}
self.__index = self

local UIName = {
	"btn_back",
	"content_bag",
}

local function FindUI()
	for i = 1, #UIName do
		self[UIName[i]] = self.ui:GetChild(UIName[i])
	end
	self["btn_back"]:AddListener(function()
        self.ui:Close()
    end)
end

function self:UpdateShopItem(data)
	local items = GlobalHooks.uiUitls:StringSplit(data, '.')
	for k, v in pairs(items) do
		GlobalHooks.openUI:InitUIComponent("UIComponentShopItem", self["content_bag"]:GetTransfrom(), 0, {id = v})
	end
end

function self:InitUI(name, sort, params)
	self.params = params
	self.ui = CS.UIManager.m_instance:ShowOrCreatePanel(name, sort)
	FindUI()
end

return self