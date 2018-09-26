local m_language

function InitLanguage(language)
	m_language = language
end

function GetText(id)
	if m_language then
		local data = GlobalHooks.dataReader:FindData("localization", id)
		if data then
			return data[m_language]
		end
		return "";
	end
end

