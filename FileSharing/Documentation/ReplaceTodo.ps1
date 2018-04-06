$input=$args[0]


(get-content -Path $input) -replace '/// Todo:','/// \todo'