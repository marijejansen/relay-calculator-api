$rgName = "RelayCalculator"
$location = "westeurope"
$bicepFile = "relay-calculator-api.bicep"
$templateFile = "relay-calculator-api.json"
$templateParameterFile = "relay-calculator-api.parameters.test.json"
$login = "marijejansen"
$password = "abc123ABC!"

New-AzResourceGroup -Name $rgName -Location $location
Build-Bicep -Path $bicepFile
New-AzResourceGroupDeployment `
    -ResourceGroupName $rgName `
    -TemplateFile $templateFile `
    -TemplateParameterFile $templateParameterFile `
    -sqlAdministratorLogin $login `
    -sqlAdministratorLoginPassword $password `
    # -WhatIf 