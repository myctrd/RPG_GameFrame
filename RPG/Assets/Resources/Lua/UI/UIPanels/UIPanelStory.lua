local self = {}
self.__index = self

local UIName = {
	"btn_back",
	"txt_name",
	"stroy_1",
	"stroy_2",
	"stroy_3",
	"stroy_4",
	"stroy_5",
}

local RoleID = 
{
	"10001",
	"10002",
	"10003",
	"10004",
}

local function LoadStory()
	local stroies = GlobalHooks.uiUitls:StringSplit(self.data["STORY"], ';')
	for i = 1, 5 do
		local txt_story = self["stroy_"..i]:GetChild("txt_story")
		local img_lock = self["stroy_"..i]:GetChild("img_lock")
		txt_story:SetActive(CS.LuaCallCSUtils.PlayerPrefsHasKey(self.data["ROLENAME"].."Story_"..i))
		img_lock:SetActive(not CS.LuaCallCSUtils.PlayerPrefsHasKey(self.data["ROLENAME"].."Story_"..i))
		txt_story:SetText(GetText("Story_"..stroies[i]))
	end
end

local function OnClickRole(id)
	self.data = GlobalHooks.dataReader:FindData("role", RoleID[id])
	if self.data then
		self["txt_name"]:SetText(GetText(self.data["ROLENAME"]))
		LoadStory()
	end
	for i = 1, #RoleID do
		local img_selected = self["role_"..i]:GetChild("img_selected")
		if img_selected then
			img_selected:SetActive(i == id)
		end
	end
end

local function InitRole(obj, id)
	local img_avatar = obj:GetChild("img_avatar")
	img_avatar:AddListener(function()
		OnClickRole(id)
    end)
end

local function FindUI()
	for i = 1, #UIName do
		self[UIName[i]] = self.ui:GetChild(UIName[i])
	end
	
	for i = 1, #RoleID do
		self["role_"..i] = self.ui:GetChild("role_"..i)
		if self["role_"..i] then
			InitRole(self["role_"..i], i)
		end
	end
	
	self["btn_back"]:AddListener(function()
        self.ui:Close()
    end)
	OnClickRole(1)
end

function self:InitUI(name, sort, params)
	self.params = params
	self.ui = CS.UIManager.m_instance:ShowOrCreatePanel(name, sort)
	FindUI()
end

return self