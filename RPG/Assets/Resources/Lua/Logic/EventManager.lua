local self = {}
self.__index = self

local EventCallBacks = {}
local GlobalCallBacks = {}
local EventRecvs = {}

local function globle_callback(eventName, evtData, ...)
  local evs = EventCallBacks[eventName]
  -- print("globle_callback "..eventName)
  local params = nil
  if evtData~=nil then
    if type(evtData) ~= "table" then
      -- print("globle_callback "..eventName.." "..evtData:GetClassType())
      -- 只能使用Dictionary
      params = {}
      local iter = evtData:GetEnumerator()
      while iter:MoveNext() do
        local data = iter.Current
        params[data.Key] = data.Value
      end
    else
      params = evtData
    end
  end

  if evs then
    for name,val in pairs(evs) do
      val(eventName, params, ...) 
    end
  end
end

function self:AddListener(sEventName, fCallBack)
    -- print("[LUAEvent]Subscribe", sEventName)
	if not EventCallBacks[sEventName] then
		EventCallBacks[sEventName] = {}
		CS.EventManager.AddLuaListener(sEventName, globle_callback)
	end
	local evs = EventCallBacks[sEventName]
	table.insert(evs, fCallBack)
end

local function HasListener(sEventName, fCallBack)
	if EventCallBacks[sEventName] then
		for _,v in ipairs(EventCallBacks[sEventName]) do
		    if v == fCallBack then
			  return true
		    end
		end
	end
	return false
end

function self:RemoveListener(sEventName, fCallBack)
	local t = EventCallBacks[sEventName]
	if t then
		for k, fun in pairs(t) do
		    if fun == fCallBack then
			    table.remove(t, k)
		    end
		end
	end
end

function self:RemoveAllListener()
  -- print("EventManager.UnsubscribeAll")
	for name,val in pairs(EventCallBacks) do
		CS.EventManager.RemoveLuaListener(name)
	end
	EventCallBacks = {}
	GlobalCallBacks = {}
end

function self:Broadcast(...)
	CS.EventManager.LuaBroadcast((...))
	for _,v in ipairs(GlobalCallBacks) do
		v((...))
	end
end

local function ListenerGlobalCallBack(fn)
  table.insert(GlobalCallBacks,fn)
end

local function RemoveListenerGlobalCallBack(fn)
  table.insert(GlobalCallBacks,fn)
end

-- self.RemoveAllListener = RemoveAllListener
-- self.AddListener = AddListener
-- self.RemoveListener = RemoveListener
-- self.Broadcast = Broadcast
-- self.HasListener = HasListener
-- self.ListenerGlobalCallBack = ListenerGlobalCallBack
-- self.RemoveListenerGlobalCallBack = RemoveListenerGlobalCallBack

return self
