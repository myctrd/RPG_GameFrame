local self = {}
self.__index = self

local UIName = {
	"txt_content",
}

local function UpdateNotesInfo()
	local data = GlobalHooks.dataReader:FindData("notes", self.params.id)
	if self["txt_content"] then
		self["txt_content"]:SetText(GetText(data["NAME"]))
	end
end

local function FindUI()
	for i = 1, #UIName do
		self[UIName[i]] = self.ui:GetChild(UIName[i])
	end
	UpdateNotesInfo()
end

function self:InitUI(name, obj, sort, params)
	self.params = params
	self.ui = CS.UIManager.m_instance:LoadComponent(name, obj, sort)
	FindUI()
end

return self