function StartRenamer {
	[CmdletBinding()]
    param(
        [Parameter(Position = 0, Mandatory = 0)][string] $customer,
        [Parameter(Position = 1, Mandatory = 0)][string] $project
    )

	try { 
		.\IOC.FW.Renamer.exe $customer $project
	}
	catch {

	}
}
 
Export-ModuleMember -Function StartRenamer

