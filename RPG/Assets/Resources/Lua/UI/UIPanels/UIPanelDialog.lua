local self = {}
self.__index = self

local itemManager = require 'Manager.item'

local UIName = {
	"btn_back",
	"txt_name",
	"txt_content",
}

local function FindUI()
	for i = 1, #UIName do
		self[UIName[i]] = self.ui:GetChild(UIName[i])
	end
	if self.params.d_type == 1 then
		self["txt_name"]:SetText(GetText(self.params.name))
		self["txt_content"]:SetText(GetText(self.params.txt))
	elseif self.params.d_type == 2 then
		local data = GlobalHooks.dataReader:FindData("npc", tostring(self.params.npcID))
		self["txt_name"]:SetText(GetText(data["ROLENAME"]))
		self["txt_content"]:SetText(GetText(data["DIALOG"]))
	end
	self["btn_back"]:AddListener(function()
        self.ui:Close()
    end)
end

function self:InitUI(name, sort, params)
	self.params = params
	self.ui = CS.UIManager.m_instance:ShowOrCreatePanel(name, sort)
	
	FindUI()
end

return self