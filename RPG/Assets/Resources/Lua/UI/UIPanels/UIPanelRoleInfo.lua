local self = {}
self.__index = self

local UIName = {
	"img_avatar",
	"txt_name",
	"txt_hp_value",
	"txt_mp_value",
	"txt_atk_value",
	"txt_ap_value",
	"txt_amr_value",
	"txt_mr_value",
	"txt_cr_value",
	"txt_cv_value",
	"slider_hp",
	"slider_mp",
	"btn_back",
}

local function FindUI()
	for i = 1, #UIName do
		self[UIName[i]] = self.ui:GetChild(UIName[i])
	end
	local playerData = CS.LuaCallCSUtils.GetPlayerData()
	self["img_avatar"]:SetSprite("Avatar/Avatar_"..playerData.m_ID)
	self["txt_name"]:SetText(GetText(playerData.m_Name))
	self["txt_hp_value"]:SetText(playerData.hp.."/"..playerData.maxHp)
	self["txt_mp_value"]:SetText(playerData.mp.."/"..playerData.maxMp)
	self["txt_atk_value"]:SetText(playerData.atk)
	self["txt_ap_value"]:SetText(playerData.ap)
	self["txt_amr_value"]:SetText(playerData.amr)
	self["txt_mr_value"]:SetText(playerData.mr)
	self["txt_cr_value"]:SetText(math.floor(playerData.cr * 100) .."%")
	self["txt_cv_value"]:SetText(string.format("%.1f", tostring(playerData.cv)))
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