local self = {}
self.__index = self


local UIName = {
	"txt_hp_value",
	"txt_mp_value",
	"slider_hp",
	"slider_mp",
	"img_avatar",
}

local function OnClickAvatar()
	GlobalHooks.openUI:OpenUIPanel("UIPanelRoleInfo", 2, {data = self.data})
end

local function FindUI()
	for i = 1, #UIName do
		self[UIName[i]] = self.ui:GetChild(UIName[i])
	end
	local id = self.id
	self["img_avatar"]:AddListener(function()
		GlobalHooks.openUI:OpenUIPanel("UIPanelRoleInfo", 2, {id = id})
	end)
end



local function UpdateInfo()
	self["img_avatar"]:SetSprite("Avatar/Avatar_"..self.data.m_ID)
	self["txt_hp_value"]:SetText(self.data.hp.."/"..self.data.maxHp)
	self["txt_mp_value"]:SetText(self.data.mp.."/"..self.data.maxMp)
	self["slider_hp"]:SetFilledValue(self.data.hp / self.data.maxHp)
	self["slider_mp"]:SetFilledValue(self.data.mp / self.data.maxMp)
end

function self:InitUI(name, obj, sort, params)
	self.params = params
	self.id = params.id
	self.data = params.data
	self.ui = CS.UIManager.m_instance:LoadComponent(name, obj, sort)
	FindUI()
	UpdateInfo()
end

return self