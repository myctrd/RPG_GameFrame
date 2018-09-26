local self = {}
self.__index = self

local itemManager = require 'Manager.item'

local UIName = {
	"btn_back",
	"txt_name",
	"txt_content",
	"img_bg_options",
	"img_options",
}

local function FindUI()
	for i = 1, #UIName do
		self[UIName[i]] = self.ui:GetChild(UIName[i])
	end
	if self.params.d_type == 1 then  --传入名字、对话内容直接显示
		self["txt_name"]:SetText(GetText(self.params.name))
		self["txt_content"]:SetText(GetText(self.params.txt))
	elseif self.params.d_type == 2 then  --根据npc ID读取默认对话
		local data = GlobalHooks.dataReader:FindData("npc", tostring(self.params.npcID))
		self["txt_name"]:SetText(GetText(data["ROLENAME"]))
		self["txt_content"]:SetText(GetText(data["DIALOG"]))
	elseif self.params.d_type == 3 then  --显示对话选项
		self["img_bg_options"]:SetActive(true)
		self["txt_name"]:SetText(GetText(self.params.name))
		self["txt_content"]:SetText(GetText(self.params.txt))
		local options = GlobalHooks.uiUitls:StringSplit(self.params.options, '.')
		local events = GlobalHooks.uiUitls:StringSplit(self.params.events, '.')
		for i = 1, #options do	
			GlobalHooks.openUI:InitUIComponent("UIComponentOption", self["img_options"]:GetTransfrom(), 0, {id = i, txt = options[i], event = events[i], dialog = self.ui})
		end
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