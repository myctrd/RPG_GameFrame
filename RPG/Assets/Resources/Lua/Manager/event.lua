local self = {}
self.__index = self

self.notesList_1 = {}  --任务传闻
self.notesList_2 = {}  --技能传闻

local function ClearAllList()
	self.notesList_1 = {}
	self.notesList_2 = {}
end

function self:GetNotes(i)
	if i == 1 then
		return self.notesList_1
	elseif i == 2 then
		return self.notesList_2
	end
end

function self:Init()
	GlobalHooks.eventManager:AddListener("DataManager.AddNotes", function(name, params)
		self:AddNotes(params.id, tonumber(params.type))
	end)
	ClearAllList()
	CS.LuaCallCSUtils.LoadEventData()
end

function self:AddNotes(id, m_type)
	local data = GlobalHooks.dataReader:FindData("notes", id)
	if m_type == 1 then
		self.notesList_1[id] = 1
	elseif m_type == 2 then
		self.notesList_2[id] = 1
	end
end

return self