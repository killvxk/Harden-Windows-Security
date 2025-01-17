using System;
using System.Collections.Generic;

namespace HardeningModule
{
    public partial class MDMClassProcessor
    {
        /// [System.Collections.Generic.Dictionary[string, [System.Collections.Generic.List[System.Collections.Generic.Dictionary[string, object]]]]]$Output = [HardeningModule.MDM]::Get()
        /// class Result {
        ///     [string]$Name
        ///     [string]$Value
        ///     [string]$CimInstance
        ///
        ///     Result([string]$Name, [string]$Value, [string]$CimInstance) {
        ///         $this.Name = $Name
        ///         $this.Value = $Value
        ///         $this.CimInstance = $CimInstance
        ///     }
        /// }
        ///
        /// $ResultsList = [System.Collections.Generic.List[Result]]::new()
        ///
        /// foreach ($CimInstanceResult in $Output.GetEnumerator()) {
        ///
        ///     try {
        ///         # 2 GetEnumerator is necessary otherwise there won't be expected results
        ///         foreach ($Key in $CimInstanceResult.Value.GetEnumerator().GetEnumerator()) {
        ///
        ///             # Precise type of the $Key variable at this point is this
        ///             [System.Collections.Generic.KeyValuePair`2[[System.String], [System.Object]]]$Key = $Key
        ///
        ///             if ($Key.key -in ('Class', 'InstanceID', 'ParentID')) {
        ///                 continue
        ///             }
        ///             $ResultsList.Add([Result]::New(
        ///                     $Key.Key,
        ///                     $Key.Value,
        ///                     $CimInstanceResult.Key
        ///                 ))
        ///         }
        ///     }
        ///     catch {
        ///         Write-Host $_.Exception.Message
        ///     }
        /// }
        /// $ResultsList | Out-GridView -Title "$($ResultsList.Count)"
        /// Above is the PowerShell equivalent of the method below
        /// It gets the results of all of the MDM related CimInstances and processes them into a list of MDMClassProcessor objects
        public static List<HardeningModule.MDMClassProcessor> Process()
        {
            // Get the results of all of the Intune policies from the system
            var output = HardeningModule.MDM.Get();

            // Create a list to store the processed results and return at the end
            List<HardeningModule.MDMClassProcessor> resultsList = new List<HardeningModule.MDMClassProcessor>();

            // Loop over each data
            foreach (var cimInstanceResult in output)
            {
                try
                {
                    foreach (var dictionary in cimInstanceResult.Value)
                    {
                        foreach (var keyValuePair in dictionary)
                        {
                            // filter out the items we don't need
                            if (keyValuePair.Key == "Class" || keyValuePair.Key == "InstanceID" || keyValuePair.Key == "ParentID")
                            {
                                continue;
                            }

                            // Add the date to the list
                            resultsList.Add(new HardeningModule.MDMClassProcessor(
                                keyValuePair.Key,
                                keyValuePair.Value?.ToString(),
                                cimInstanceResult.Key
                            ));
                        }
                    }
                }
                catch (Exception ex)
                {
                    HardeningModule.VerboseLogger.Write(ex.Message);
                }
            }

            return resultsList;
        }
    }
}
