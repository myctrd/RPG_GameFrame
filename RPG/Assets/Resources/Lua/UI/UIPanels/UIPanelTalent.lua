local self = {}
self.__index = self

local UIName = {
	"btn_back",
	"part_1",
	"part_2",
	"part_3",
	"txt_points",
	"img_frame",
}

local function UpdatePoints()
	self["txt_points"]:SetText(self.points)
end

local function DeactiveTalent(talent)
	talent.status = 0  --不可点的天赋
	talent:SetImageColor(0, 0, 0)
	local img = talent:GetChild("Img")
	img:SetActive(false)
end

local function CanClickTalent(talent)
	talent.status = 1  --可点的天赋
	talent:SetImageColor(0, 0, 0)
	local img = talent:GetChild("Img")
	img:SetActive(true)
	img:SetImageColor(1, 1, 1)
end

local function ActiveTalent(talent)
	talent.status = 2  --已点的天赋
	talent:SetImageColor(1, 1, 1)
	local img = talent:GetChild("Img")
	img:SetImageColor(1, 184/255, 32/255)
end

local function ClickTalent(i, j)
	if self["talent_"..i.."_"..j].status == 1 then
		if self.points > 0 then
			self.points = self.points - 1
			UpdatePoints()
		else
			GlobalHooks.uiUitls:ShowFloatingMsg(GetText("NoMoreTalentPoints"))
			return
		end
	
		ActiveTalent(self["talent_"..i.."_"..j])
		local data = GlobalHooks.dataReader:FindData(self.file, "talent_"..i.."_"..j)
		local linkedTanlent = GlobalHooks.uiUitls:StringSplit(data["LINKEDTALENT"], ';')
		for k, v in pairs(linkedTanlent)do
			if self[v].status == 0 then
				CanClickTalent(self[v])
			end
		end
	end
end

local function ShowTalentInfo()
	self["img_frame"]:SetActive(true)
end

local function HideTalentInfo()
	self["img_frame"]:SetActive(false)
end

local function OnExit()
	GlobalHooks.eventManager:RemoveListener("Hover.TalentInfo", ShowTalentInfo)
	GlobalHooks.eventManager:RemoveListener("Hover.HideTalentInfo", HideTalentInfo)
end

local function OnEnter()
	GlobalHooks.eventManager:AddListener("Hover.TalentInfo", ShowTalentInfo)
	GlobalHooks.eventManager:AddListener("Hover.HideTalentInfo", HideTalentInfo)
end

local function FindUI()
	for i = 1, #UIName do
		self[UIName[i]] = self.ui:GetChild(UIName[i])
	end
	
	for i = 1, 3 do
		for j = 1, 17 do
			self["talent_"..i.."_"..j] = self["part_"..i]:GetChild("talent_"..i.."_"..j)
			DeactiveTalent(self["talent_"..i.."_"..j])
			self["talent_"..i.."_"..j]:AddListener(function()
				ClickTalent(i, j)
			end)
		end
	end
	CanClickTalent(self["talent_1_1"])
	CanClickTalent(self["talent_2_1"])
	CanClickTalent(self["talent_3_1"])
	self["btn_back"]:AddListener(function()
		OnExit()
        self.ui:Close()
    end)
end

function self:InitUI(name, sort, params)
	self.params = params
	self.file = "talent_10001"
	self.points = 30
	self.ui = CS.UIManager.m_instance:ShowOrCreatePanel(name, sort)
	OnEnter()
	FindUI()
	UpdatePoints()
end

return self