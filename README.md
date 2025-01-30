# Getting Started app for Windows.
[![Version](https://img.shields.io/badge/1.0-passing?label=Release)](https://github.com/Gabee8/WelcomeCenter/releases/tag/release)
[![License](https://img.shields.io/github/license/Gabee8/WelcomeCenter)](https://github.com/Gabee8/WelcomeCenter/blob/main/LICENSE)

Simple Windows Getting Started app program written C# by Visual Studio 2010
![welc_1](https://github.com/user-attachments/assets/4b7ca33e-ead2-4992-b606-06e88501095e)
![welc_2](https://github.com/user-attachments/assets/a62f239d-2bb3-4075-b503-6e90af0c993f)
![welc_3](https://github.com/user-attachments/assets/281a22ea-299a-4fab-b637-fc1ef0208167)



##### The program futures:
- 3 style from header
- Add custom apps the list
- Multilanguage enable
##### Edit your language:
Language path: bin\debug\Languages or application root folder\Languages.

Create mylanguage.xaml file example:
```xaml
<ResourceDictionary xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
                    xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml" xmlns:system="clr-namespace:System;assembly=mscorlib">

    <system:String x:Key="maintitle">Getting Started</system:String>
    <system:String x:Key="welcome">welcome</system:String>
.....
</ResourceDictionary>
```
