local m_language

function InitLanguage(language)
	m_language = language
end

function GetText(id)
	if m_language then
		local data = GlobalHooks.dataReader:FindData("localization", id)
		return data[m_language]
	end
end
