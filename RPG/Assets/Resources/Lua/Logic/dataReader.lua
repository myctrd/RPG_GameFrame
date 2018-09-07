local self = {}
self.__index = self

self.DB = {}

local function get_key(tb, index)
    if tb["_key_"] ~= nil then
        return tb._key_[index]
    else
        return tb[1][index]
    end
end

local function gen_table(tb, arr)
    local ret = {}
    for k, v in ipairs(arr) do
		ret[get_key(tb, k)] = v
    end
	return ret
end

function self:FindData(tb_name, find_key)	
	local tb = self:GetDataTable(tb_name)
	for k, v in pairs(tb) do
		if k == find_key then
			return gen_table(tb, v)
		end
	end
	return nil
end

function self:GetDataTable(tb_name)
	if not self.DB.data then
		self.DB.data = {}
	end
	local tb = self.DB.data[tb_name]
	if not tb then
		ok, ret = pcall(require, 'Data.'..tb_name)
		if ok then
			tb = ret
			self.DB.data[tb_name] = tb
		end	
	end
	return tb  
end

return self