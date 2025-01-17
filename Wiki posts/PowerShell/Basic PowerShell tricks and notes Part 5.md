# Basic PowerShell Tricks and Notes Part 5

The following PowerShell series is designed for newcomers to PowerShell who want to quickly learn the essential basics, the most frequently used syntaxes, elements and tricks. It can also be used by advanced users as a quick reference or those who want to sharpen their skills.

The main source for learning PowerShell is Microsoft Learn websites. There are extensive and complete guides about each command/cmdlet with examples.

[PowerShell core at Microsoft Learn](https://learn.microsoft.com/en-us/powershell/module/microsoft.powershell.core/)

You can also use the Windows Copilot for asking any PowerShell related questions, code examples etc.

This is part 5 of this series, find other parts here:

* [Part 1](https://github.com/HotCakeX/Harden-Windows-Security/wiki/Basic-PowerShell-tricks-and-notes)
* [Part 2](https://github.com/HotCakeX/Harden-Windows-Security/wiki/Basic-PowerShell-Tricks-and-Notes-Part-2)
* [Part 3](https://github.com/HotCakeX/Harden-Windows-Security/wiki/Basic-PowerShell-Tricks-and-Notes-Part-3)
* [Part 4](https://github.com/HotCakeX/Harden-Windows-Security/wiki/Basic-PowerShell-Tricks-and-Notes-Part-4)
* [Part 5](https://github.com/HotCakeX/Harden-Windows-Security/wiki/Basic-PowerShell-Tricks-and-Notes-Part-5)

<br>

## Never Use Relative Path When Working With .NET Methods In PowerShell

When you are working with .NET methods in PowerShell, you should never use relative paths. Always use the full path to the file or directory. The reason is that the following command:

```powershell
[System.Environment]::CurrentDirectory
```

Always remembers the first directory the PowerShell instance was started in. If you use `cd` or `Set-Location` to change the current working directory, it will not change that environment variable, which is what .NET methods use when you pass in a relative path such as `.\file.txt`. That means .NET methods always consider that environment variable when you pass in a relative path from PowerShell, not the current working directory in PowerShell.

<br>

## Downloading PowerShell Files From Low Integrity Untrusted Sources

If you download your PowerShell scripts or module files from a Low Integrity source, such as a sandboxed browser, they will be deemed Untrusted. These files will possess Mark Of The Web (MotW) Zone Identifiers, marking them as such. Consequently, you must unblock them before utilization.

Failure to unblock these files, thereby removing their MotW designation, can result in complications and errors within PowerShell. For instance, they may generate errors such as `AuthorizationManager check failed`, a situation particularly prevalent when incorporating C# code in PowerShell via `Add-Type`.

Another issue arising from executing PowerShell files from Untrusted sources is the necessity for a more permissive execution policy such as `Bypass`; otherwise, you will encounter incessant prompts for confirmation.

<br>

## Executing PowerShell Cmdlets in C# Within PowerShell

Indeed, you can execute PowerShell cmdlets within `C#` directly inside PowerShell. By leveraging `Add-Type`, you can seamlessly integrate `C#` code into PowerShell, enabling it to run PowerShell cmdlets. This can be particularly useful in various scenarios, so here is an illustrative example.

Consider the following code snippet, which demonstrates how to create a PowerShell instance:

```csharp
using (PowerShell powerShell = PowerShell.Create())
{
    powerShell.AddScript("Write-Verbose 'Hello World!'");
    var results = powerShell.Invoke();
}
```

The version of the PowerShell instance created will correspond to the version in which you run the `C#` code via `Add-Type`. For instance, if you execute the `C#` code within Windows PowerShell, the `.Create()` method will instantiate a PowerShell instance using the Windows PowerShell assemblies. Conversely, if you execute the same code within PowerShell Core (`pwsh.exe`), it will instantiate a PowerShell Core instance.

This behavior ensures that your PowerShell instance is consistent with the environment in which your `C#` code is executed, providing seamless integration and execution across different PowerShell versions.

<br>

## Make Regex Faster In PowerShell

The Compiled option in Regex is beneficial when you need to reuse the same pattern multiple times, especially within loops. This option improves performance by compiling the regex pattern into a more efficient, executable form. Here, we'll explore the technical advantages and provide a practical example in PowerShell to demonstrate its efficacy.

When working with regular expressions in tight loops, the overhead of interpreting the pattern each time can significantly impact performance. The Compiled option mitigates this by converting the regex pattern into an intermediate language, which the .NET runtime can execute more swiftly.

#### Best Practices

* Pattern Reuse: Utilize the Compiled option when the same regex pattern is used repeatedly.

* Defined Outside Loops: Ensure the regex pattern is defined outside the loop.

```powershell
$Pattern = [regex]::new('Insert Regex Pattern', [System.Text.RegularExpressions.RegexOptions]::Compiled)
$Pattern.IsMatch($Data)
```

<br>
