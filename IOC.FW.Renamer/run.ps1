Import-Module .\run.psm1

Write-Host "Nome do cliente: "
$customer = Read-Host

Write-Host "Nome do projeto: "
$project = Read-Host

if ($customer -ne "" -and $project -ne "") {
	Write-Host "Renomeando template para os dados fornecidos: '$customer.$project'"
	StartRenamer $customer $project
}
Remove-Module run
cd .build\

.\run-build.ps1

del **\*.pdb