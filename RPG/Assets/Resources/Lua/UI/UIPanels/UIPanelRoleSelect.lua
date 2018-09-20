local self = {}
self.__index = self

local function OnClickBack()
	self.ui:Close()
	GlobalHooks.openUI:OpenUIPanel("UIPanelMenu")
end

local UIName = {
	"pro_1",
	"pro_2",
	"pro_3",
	"pro_4",
	"btn_back",
}

local function FindUI()
	for i = 1, #UIName do
		self[UIName[i]] = self.ui:GetChild(UIName[i])
	end
	self.roleAvailable = CS.LuaCallCSUtils.GetRoleAvailable()
	for i = 1, 4 do
		local data = GlobalHooks.dataReader:FindData("role", "1000"..tostring(i))
		if data then
			self:InitProUI(i, data)
		end
	end
	self["btn_back"]:AddListener(OnClickBack)
end

function self:InitUI(name, sort)
	self.ui = CS.UIManager.m_instance:ShowOrCreatePanel(name, sort)
	FindUI()
	
end

local function OnClickRole(data)
	CS.LuaCallCSUtils.SetRolePlayer(tonumber(data["ID"]))
	GlobalHooks.openUI:OpenUIPanel("UIPanelGameDeck", 1, {roleData = data})
	self.ui:Close()
	CS.LuaCallCSUtils.LoadGameScene()
end

function self:InitProUI(i, data, isAvailable)
	local obj = self["pro_"..tostring(i)]
	local isAvailable = self.roleAvailable[i - 1] == 1
	local img_avatar = obj:GetChild("img_avatar")
	local txt_name = obj:GetChild("txt_name")
	local img_frame = obj:GetChild("img_frame")
	if isAvailable then
		img_avatar:SetSprite("Role/Role_"..data["ID"])
		img_frame:AddListener(function()
			OnClickRole(data)
		end)
		txt_name:SetText(GetText(data["ROLENAME"]))
	else
		img_avatar:SetSprite("Role/Gray_"..data["ID"])
	end
end

return self